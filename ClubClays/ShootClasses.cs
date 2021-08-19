using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AndroidX.Lifecycle;
using ClubClays.DatabaseModels;
using SQLite;

namespace ClubClays
{
    public class ShooterStandData : ViewModel
    {
        public List<Shooters> selectedShooters = new List<Shooters>();
        public List<Shooters> allShooters = new List<Shooters>();
        public ShootFormats selectedFormat = null;
    }

    public class ShootFormat : ViewModel
    {
        public List<Stand> originalStands;
        public List<Stand> stands;
    }

    public class Stand
    {
        public int id;
        public int standNum;
        // List<Tuple<type, details>>
        public List<Tuple<string, string[]>> shotFormat;
        public int numClays;

        public Stand(int standNum, List<Tuple<string, string[]>> shotFormat)
        {
            this.standNum = standNum;
            this.shotFormat = shotFormat;
            foreach (Tuple<string, string[]> format in shotFormat)
            {
                if (format.Item1 == "Pair")
                {
                    numClays += 2;
                }

                if (format.Item1 == "Single")
                {
                    numClays += 1;
                }
            }
        }
    }

    class Shoot : ViewModel
    {
        protected string title;
        protected DateTime date;
        protected string location;
        protected string discipline;
        protected int numOfClays;
        protected string userNotes;
        protected int shootID;

        protected List<Shooter> Shooters = new List<Shooter>();
        protected SortedList<int, Stand> StandsByNum = new SortedList<int, Stand>();

        public int NumStands => StandsByNum.Count;

        public int NumberOfShooters => Shooters.Count;

        public string UserNotes
        {
            set => userNotes = value;
        }

        public DateTime Date => date;

        public string Title => title;

        public int ShootID => shootID;

        public void ShooterOverallData(int position, out string name, out int overallTotal, out List<int> totals)
        {
            totals = new List<int>();
            name = Shooters[position].name;
            overallTotal = Shooters[position].overallTotal;

            for (int x = 1; x <= StandsByNum.Count; x++)
            {
                totals.Add((Shooters[position].StandScoresByStandNum.Count >= x) ? Shooters[position].StandScoresByStandNum[x].standTotal : 0);
            }
        }

        public List<Tuple<string, string[]>> StandFormat(int standNum)
        {
            return StandsByNum[standNum].shotFormat;
        }

        public void ShooterStandData(int position, int standNum, out string name, out int standTotal, out List<Tuple<int[], string[]>> totals)
        {
            totals = Shooters[position].StandScoresByStandNum[standNum].shots;
            name = Shooters[position].name;
            standTotal = Shooters[position].StandScoresByStandNum[standNum].standTotal;
        }

        protected class Shooter
        {
            public int id;
            public string name;
            public string shooterClass;
            public int overallTotal;
            public int overallPercentage;
            public SortedList<int, StandScore> StandScoresByStandNum = new SortedList<int, StandScore>();

            public class StandScore
            {
                public int standTotal;
                public int standPercentage;
                // List<Tuple<hit miss, details>>
                public List<Tuple<int[], string[]>> shots;

                public StandScore(int standTotal)
                {
                    this.standTotal = standTotal;
                    shots = new List<Tuple<int[], string[]>>();
                }
            }

            public Shooter(int id, string name, string shooterClass)
            {
                this.id = id;
                this.name = name;
                this.shooterClass = shooterClass;
            }
        }

        public void Initialise(List<Shooters> shooters, ShootFormats shootFormat, DateTime date, string location, string discipline, string title)
        {
            this.date = date;
            this.location = location;
            this.discipline = discipline;
            this.title = title;

            foreach (Shooters shooter in shooters)
            {
                Shooters.Add(new Shooter(shooter.Id, shooter.Name, shooter.Class));
            }

            if (shootFormat != null)
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ClubClaysData.db3");
                using (var db = new SQLiteConnection(dbPath))
                {
                    var standFormats = db.Table<StandFormats>().Where(s => s.ShootFormatId == shootFormat.Id).OrderBy(s => s.StandNum).ToList();
                    int x = 1;
                    foreach (StandFormats stand in standFormats)
                    {
                        List<Tuple<string, string[]>> shotsLayout = new List<Tuple<string, string[]>>();
                        var shotsFormats = db.Table<StandShotsFormats>().Where(s => s.StandFormatId == stand.Id).OrderBy(s => s.ShotNum).ToList();
                        foreach (StandShotsFormats shot in shotsFormats)
                        {
                            shotsLayout.Add(new Tuple<string, string[]>(shot.Type, null));
                        }
                        AddStand(new Stand(x++, shotsLayout));
                    }
                }
            }
        }

        public void AddStand(Stand stand)
        {
            StandsByNum.Add(StandsByNum.Count + 1, stand);
            numOfClays += StandsByNum[StandsByNum.Count].numClays;

            for (int x = 0; x <= Shooters.Count - 1; x++)
            {
                Shooters[x].StandScoresByStandNum.Add(StandsByNum.Count, new Shooter.StandScore(0));
                foreach (Tuple<string, string[]> format in stand.shotFormat)
                {
                    if (format.Item1 == "Pair")
                    {
                        Shooters[x].StandScoresByStandNum[StandsByNum.Count].shots.Add(new Tuple<int[], string[]>(new int[] { 0, 0 }, null));
                    }
                    if (format.Item1 == "Single")
                    {
                        Shooters[x].StandScoresByStandNum[StandsByNum.Count].shots.Add(new Tuple<int[], string[]>(new int[] { 0 }, null));
                    }
                }
            }
        }

        public int UpdateScore(int position, int standNum, int shotsNum, int shotNum)
        {
            int updatedTo = 0;
            int currentValue = Shooters[position].StandScoresByStandNum[standNum].shots[shotsNum].Item1[shotNum];
            switch (currentValue)
            {
                case 0: //not take -> hit
                    Shooters[position].StandScoresByStandNum[standNum].shots[shotsNum].Item1[shotNum] = updatedTo = 1;
                    Shooters[position].StandScoresByStandNum[standNum].standTotal += 1;
                    Shooters[position].overallTotal += 1;
                    break;
                case 1: //hit -> miss
                    Shooters[position].StandScoresByStandNum[standNum].shots[shotsNum].Item1[shotNum] = updatedTo = 2;
                    Shooters[position].StandScoresByStandNum[standNum].standTotal -= 1;
                    Shooters[position].overallTotal -= 1;
                    break;
                case 2: //miss -> not taken
                    Shooters[position].StandScoresByStandNum[standNum].shots[shotsNum].Item1[shotNum] = updatedTo = 0;
                    break;
            }

            return updatedTo;
        }

        public int ShooterStandTotal(int position, int standNum)
        {
            return Shooters[position].StandScoresByStandNum[standNum].standTotal;
        }

        public List<Tuple<string, string[]>> StandShots(int standNum)
        {
            return StandsByNum[standNum].shotFormat;
        }

        public void InitialisePreviousShoot(int shootID)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            using (var db = new SQLiteConnection(dbPath))
            {
                var shoot = db.Get<Shoots>(shootID);
                this.shootID = shootID; 
                title = shoot.Title;
                date = shoot.Date;
                location = shoot.Location;
                discipline = shoot.EventType;
                numOfClays = shoot.NumClays;
                userNotes = shoot.Notes;

                var overallScores = db.Table<OverallScores>().Where<OverallScores>(s => s.ShootId == shoot.Id).ToList();
                foreach (OverallScores overallScore in overallScores)
                {
                    var shooter = db.Get<Shooters>(overallScore.ShooterId);
                    Shooter shooterObject = new Shooter(shooter.Id, shooter.Name, shooter.Class)
                    {
                        overallPercentage = overallScore.OverallPercentage,
                        overallTotal = overallScore.OverallTotal
                    };
                    Shooters.Add(shooterObject);
                }

                var stands = db.Table<Stands>().Where<Stands>(s => s.ShootId == shoot.Id).OrderBy(s => s.StandNum).ToList();
                int x = 1;
                foreach (Stands stand in stands)
                {
                    List<Tuple<string, string[]>> shotFormat = new List<Tuple<string, string[]>>();

                    var standShots = db.Table<StandShots>().Where<StandShots>(s => s.StandId == stand.Id).OrderBy(s => s.ShotNum).ToList();

                    foreach (StandShots standShot in standShots)
                    {
                        shotFormat.Add(new Tuple<string, string[]>(standShot.Type, null));
                    }

                    Stand standObject = new Stand(x++, shotFormat);
                    StandsByNum.Add(stand.StandNum, standObject);

                    var standScores = db.Table<StandScores>().Where<StandScores>(s => s.StandId == stand.Id).ToList();
                    foreach (StandScores standScore in standScores)
                    {
                        var shooterStandScore = new Shooter.StandScore(standScore.StandTotal);

                        var shots = db.Table<Shots>().Where<Shots>(s => s.StandScoreId == standScore.Id).OrderBy(s => s.Num).ToList();
                        foreach (Shots shot in shots)
                        {

                            switch (shot.ShotCode)
                            {
                                case 0:
                                    AddShotScore(ref shooterStandScore.shots, new int[] { 0 });
                                    break;
                                case 1:
                                    AddShotScore(ref shooterStandScore.shots, new int[] { 1 });
                                    break;
                                case 2:
                                    AddShotScore(ref shooterStandScore.shots, new int[] { 2 });
                                    break;
                                case 3:
                                    AddShotScore(ref shooterStandScore.shots, new int[] { 0, 0 });
                                    break;
                                case 4:
                                    AddShotScore(ref shooterStandScore.shots, new int[] { 1, 1 });
                                    break;
                                case 5:
                                    AddShotScore(ref shooterStandScore.shots, new int[] { 2, 2 });
                                    break;
                                case 6:
                                    AddShotScore(ref shooterStandScore.shots, new int[] { 0, 1 });
                                    break;
                                case 7:
                                    AddShotScore(ref shooterStandScore.shots, new int[] { 0, 2 });
                                    break;
                                case 8:
                                    AddShotScore(ref shooterStandScore.shots, new int[] { 1, 0 });
                                    break;
                                case 9:
                                    AddShotScore(ref shooterStandScore.shots, new int[] { 2, 0 });
                                    break;
                                case 10:
                                    AddShotScore(ref shooterStandScore.shots, new int[] { 1, 2 });
                                    break;
                                case 11:
                                    AddShotScore(ref shooterStandScore.shots, new int[] { 2, 1 });
                                    break;
                            }
                        }

                        Shooters.Find(s => s.id == standScore.ShooterId).StandScoresByStandNum.Add(stand.StandNum, shooterStandScore);
                    }
                }
            }
        }

        public void AddShotScore(ref List<Tuple<int[], string[]>> shots, int[] score)
        {
            shots.Add(new Tuple<int[], string[]>(score, null));
        }


        public void SaveShootData()
        {
            CalculateStats();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            using (var db = new SQLiteConnection(dbPath))
            {
                Shoots newShoot = new Shoots() { Title = title, Date = date, Location = location, EventType = discipline, NumStands = StandsByNum.Count, NumClays = numOfClays, Notes = userNotes };
                db.Insert(newShoot);

                for (int x = 1; x <= StandsByNum.Count; x++)
                {
                    Stands newStand = new Stands() { ShootId = newShoot.Id, StandNum = x };
                    db.Insert(newStand);

                    for (int a = 0; a <= StandsByNum[x].shotFormat.Count - 1; a++)
                    {
                        StandShots newStandShot = new StandShots() { ShotNum = a + 1, StandId = newStand.Id, Type = StandsByNum[x].shotFormat[a].Item1 };
                        db.Insert(newStandShot);
                    }

                    for (int y = 0; y <= Shooters.Count - 1; y++)
                    {
                        StandScores newStandScore = new StandScores() { StandId = newStand.Id, ShooterId = Shooters[y].id, StandTotal = Shooters[y].StandScoresByStandNum[x].standTotal };
                        db.Insert(newStandScore);

                        for (int z = 0; z <= Shooters[y].StandScoresByStandNum[x].shots.Count - 1; z++)
                        {
                            int id = 0;

                            switch (StandsByNum[x].shotFormat[z].Item1)
                            {
                                case "Pair":
                                    if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item1[0] == 0) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item1[0] == 0)) { id = 3; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item1[0] == 1) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item1[1] == 1)) { id = 4; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item1[0] == 2) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item1[1] == 2)) { id = 5; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item1[0] == 0) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item1[1] == 1)) { id = 6; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item1[0] == 0) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item1[1] == 2)) { id = 7; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item1[0] == 1) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item1[1] == 0)) { id = 8; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item1[0] == 2) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item1[1] == 0)) { id = 9; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item1[0] == 1) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item1[1] == 2)) { id = 10; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item1[0] == 2) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item1[1] == 1)) { id = 11; }
                                    break;
                                case "Single":
                                    switch (Shooters[y].StandScoresByStandNum[x].shots[z].Item1[0])
                                    {
                                        case 0:
                                            id = 0;
                                            break;
                                        case 1:
                                            id = 1;
                                            break;
                                        case 2:
                                            id = 2;
                                            break;
                                    }
                                    break;
                            }

                            db.Insert(new Shots() { StandScoreId = newStandScore.Id, Num = z + 1, ShotCode = id });
                        }
                    }
                }

                for (int w = 0; w <= Shooters.Count - 1; w++)
                {
                    OverallScores newOverallScore = new OverallScores() { ShootId = newShoot.Id, ShooterId = Shooters[w].id, OverallTotal = Shooters[w].overallTotal, OverallPercentage = Shooters[w].overallPercentage };
                    db.Insert(newOverallScore);
                }
            }
        }
        public void CalculateStats()
        {
            for (int y = 0; y <= Shooters.Count - 1; y++)
            {
                Shooters[y].overallPercentage = (int)Math.Round((double)Shooters[y].overallTotal / numOfClays * 100);
                for (int x = 1; x <= StandsByNum.Count; x++)
                {
                    Shooters[y].StandScoresByStandNum[x].standPercentage = (int)Math.Round((double)Shooters[y].StandScoresByStandNum[x].standTotal / (StandsByNum[x].numClays) * 100);
                }
            }
        }
        public Java.IO.File ShootToCSV()
        {
            CalculateStats();
            string csvPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), $"{discipline.Replace(" ", "")}{date:yyyyMMdd}.csv");
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("OVERALL");
            sb.Append("NAME,");
            for (int x = 1; x <= StandsByNum.Count; x++)
            {
                sb.Append($"STAND {x},");
            }
            sb.Append($"TOTAL /{numOfClays},");
            sb.AppendLine("%");

            for (int x = 0; x <= Shooters.Count - 1; x++)
            {
                ShooterOverallData(x, out string name, out int overallTotal, out List<int> totals);
                sb.Append($"{name},");
                sb.Append(string.Join(",", totals.Select(total => total.ToString()).ToArray()));
                sb.Append($",{overallTotal},");
                sb.AppendLine($"{Shooters[x].overallPercentage}");
            }
            File.WriteAllText(csvPath, sb.ToString());
            sb.Clear();

            for (int x = 1; x <= StandsByNum.Count; x++)
            {
                sb.AppendLine("");
                sb.AppendLine($"STAND {x}");
                sb.Append("NAME,");
                for (int z = 0; z <= StandsByNum[x].shotFormat.Count - 1; z++)
                {
                    sb.Append($"{StandsByNum[x].shotFormat[z]},");
                }
                sb.Append($"TOTAL /{StandsByNum[x].numClays},");
                sb.AppendLine("%");

                for (int y = 0; y <= Shooters.Count - 1; y++)
                {
                    ShooterStandData(y, x, out string name, out int total, out List<Tuple<int[], string[]>> hits);
                    sb.Append($"{name},");
                    for (int z = 0; z <= hits.Count - 1; z++)
                    {
                        if (StandsByNum[x].shotFormat[z].Item1 == "Pair") { sb.Append($"{TranslateHitMiss(hits[z].Item1[0])}{TranslateHitMiss(hits[z].Item1[1])},"); }
                        else if (StandsByNum[x].shotFormat[z].Item1 == "Single") { sb.Append($"{TranslateHitMiss(hits[z].Item1[0])},"); }
                    }
                    sb.Append($"{total}");
                    sb.AppendLine($",{Shooters[y].StandScoresByStandNum[x].standPercentage}");
                }
                File.AppendAllText(csvPath, sb.ToString());
                sb.Clear();
            }

            var file = new Java.IO.File(csvPath);

            return file;
        }
        public string TranslateHitMiss(int value)
        {
            return value switch
            {
                1 => "X",
                2 => "O",
                _ => " ",
            };
        }
    }
}