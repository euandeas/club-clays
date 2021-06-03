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
        public List<Shooters> selectedShooters;
        public List<Shooters> allShooters;
        public List<Stand> standFormats;
    }

    public class Stand
    {
        public string standType;
        public List<string> shotFormat;
        public int numClays;

        public Stand(string standType, List<string> shotFormat)
        {
            this.standType = standType;
            this.shotFormat = shotFormat;
            foreach (string format in shotFormat)
            {
                if (format == "Pair")
                {
                    numClays += 2;
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
        protected int numOfClays = 0;
        protected string userNotes;

        protected List<Shooter> Shooters = new List<Shooter>();
        protected SortedList<int, Stand> StandsByNum = new SortedList<int, Stand>();

        public int NumStands
        {
            get
            {
                return StandsByNum.Count();
            }
        }

        public int NumClays
        {
            get
            {
                return numOfClays;
            }
        }

        public int NumberOfShooters
        {
            get
            {
                return Shooters.Count();
            }
        }


        public void ShooterOverallData(int position, out string name, out int overallTotal, out List<int> totals)
        {
            totals = new List<int>();
            name = Shooters[position].name;
            overallTotal = Shooters[position].overallTotal;

            for (int x = 1; x <= StandsByNum.Count(); x++)
            {
                if (Shooters[position].StandScoresByStandNum.Count() >= x)
                {
                    totals.Add(Shooters[position].StandScoresByStandNum[x].standTotal);
                }
                else
                {
                    totals.Add(0);
                }
            }
        }

        public void ShooterStandData(int position, int standNum, out string name, out int standTotal, out List<Tuple<string, int[]>> totals)
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
                public List<Tuple<string, int[]>> shots;

                public StandScore(int standTotal, int standPercentage)
                {
                    this.standTotal = standTotal;
                    this.standPercentage = standPercentage;
                    shots = new List<Tuple<string, int[]>>();
                }
            }

            public Shooter(int id,string name, string shooterClass)
            {
                this.id = id;
                this.name = name;
                this.shooterClass = shooterClass;
            }
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
                    Stands newStand = new Stands() { ShootId = newShoot.Id, StandNum = x, StandType = StandsByNum[x].standType };
                    db.Insert(newStand);

                    for (int a = 0; a <= StandsByNum[x].shotFormat.Count-1; a++)
                    {
                        StandShots newStandShot = new StandShots() { ShotNum = a + 1, StandId = newStand.Id, Type = StandsByNum[x].shotFormat[a] };
                    }
                    
                    for (int y = 0; y <= Shooters.Count-1; y++)
                    {
                        StandScores newStandScore = new StandScores() { StandId = newStand.Id, ShooterId = Shooters[y].id, StandTotal = Shooters[y].StandScoresByStandNum[x].standTotal};
                        db.Insert(newStandScore);

                        for (int z = 0; z <= Shooters[y].StandScoresByStandNum[x].shots.Count-1; z++)
                        {
                            int id = 0;

                            switch (Shooters[y].StandScoresByStandNum[x].shots[z].Item1)
                            {
                                case "Pair":
                                    if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 0) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 0)) { id = 3; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 1) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 1)) { id = 4; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 2) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 2)) { id = 5; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 0) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 1)) { id = 6; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 0) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 2)) { id = 7; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 1) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 0)) { id = 8; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 2) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 0)) { id = 9; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 1) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 2)) { id = 10; }
                                    else if ((Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 2) && (Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0] == 1)) { id = 11; }
                                    break;
                                case "Single":
                                    switch (Shooters[y].StandScoresByStandNum[x].shots[z].Item2[0])
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

                            db.Insert(new Shots() { StandScoreId = newStandScore.Id, Num = z + 1, ShotCode = id});
                        }
                    }
                }

                for (int w = 0; w <= Shooters.Count-1; w++)
                {
                    OverallScores newOverallScore = new OverallScores() { ShootId = newShoot.Id, ShooterId = Shooters[w].id, OverallTotal = Shooters[w].overallTotal, OverallPercentage = Shooters[w].overallPercentage };
                    db.Insert(newOverallScore);
                }
            }
        }
        public void SaveFormat() { }
        public void CalculateStats() 
        {
            for (int y = 0; y <= Shooters.Count-1; y++)
            {
                Shooters[y].overallPercentage = (int)Math.Round((double)Shooters[y].overallTotal / numOfClays * 100);
                for (int x = 1; x <= StandsByNum.Count; x++)
                {
                    Shooters[y].StandScoresByStandNum[x].standPercentage = (int)Math.Round((double)Shooters[y].StandScoresByStandNum[x].standTotal / (StandsByNum[x].numClays*2) * 100);
                }
            }
        }
        //public Java.IO.File ShootToCSV()
        //{
        //    CalculateStats();
        //    string csvPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), $"{discipline.Replace(" ","")}{date:yyyyMMdd}.csv");
        //    StringBuilder sb = new StringBuilder();

        //    sb.AppendLine("OVERALL");
        //    sb.Append("NAME,");
        //    for (int x = 1; x <= StandsByNum.Count; x++)
        //    {
        //        sb.Append($"STAND {x},");
        //    }
        //    sb.Append($"TOTAL /{numOfClays},");
        //    sb.AppendLine("%");

        //    for (int x = 1; x <= ShootersByOriginalPos.Count(); x++)
        //    {
        //        List<string> shooterData = ShooterOverallData(x);
        //        sb.Append(string.Join(',', shooterData));
        //        sb.AppendLine($",{ShootersByOriginalPos[x].overallPercentage}");
        //    }
        //    File.WriteAllText(csvPath, sb.ToString());
        //    sb.Clear();

        //    for (int x = 1; x <= StandsByNum.Count; x++)
        //    {
        //        sb.AppendLine("");
        //        sb.AppendLine($"STAND {x}");
        //        sb.Append("NAME,");
        //        for (int z = 1; z <= StandsByNum[x].numOfPairs; z++)
        //        {
        //            sb.Append($"PAIR {z},");
        //        }
        //        sb.Append($"TOTAL /{StandsByNum[x].numOfPairs*2},");
        //        sb.AppendLine("%");

        //        for (int y = 1; y <= ShootersByOriginalPos.Count(); y++)
        //        {
        //            ShooterStandData(y, x, out string name, out string total, out SortedList<int, int[]> hits);
        //            sb.Append($"{name},");
        //            for (int z = 1; z <= hits.Count; z++)
        //            {
        //                sb.Append($"{TranslateHitMiss(hits[z][0])}{TranslateHitMiss(hits[z][1])},");
        //            }
        //            sb.Append($"{total}");
        //            sb.AppendLine($",{ShootersByOriginalPos[y].StandScoresByStandNum[x].standPercentage}");
        //        }
        //        File.AppendAllText(csvPath, sb.ToString());
        //        sb.Clear();
        //    }

        //    var file = new Java.IO.File(csvPath);

        //    return file;
        //}
        public string TranslateHitMiss(int value)
        {
            switch (value)
            {
                default:
                    return " ";

                case 1:
                    return "X";

                case 2:
                    return "O";

            }
        }
    }

    class ShootScoreManagement : Shoot
    {
        public string UserNotes{ set 
            {
                userNotes = value;
            }
        }

        public void Initialise(List<Shooters> shooters, List<Stand> stands, DateTime date, string location, string discipline)
        {
            this.date = date;
            this.location = location;
            this.discipline = discipline;

            foreach (Shooters shooter in shooters)
            {
                Shooters.Add(new Shooter(shooter.Id, shooter.Name, shooter.Class));
            }

            foreach (Stand stand in stands)
            {
                AddStand(stand);
            }
        }

        public void AddStand(Stand stand)
        {
            StandsByNum.Add(StandsByNum.Count + 1, stand);
            numOfClays += StandsByNum[StandsByNum.Count].numClays;

            for (int x = 0; x <= Shooters.Count() - 1; x++)
            {
                Shooters[x].StandScoresByStandNum.Add(StandsByNum.Count, new Shooter.StandScore(0, 0));
                foreach (string format in stand.shotFormat)
                {
                    if (format == "Pair")
                    {
                        Shooters[x].StandScoresByStandNum[StandsByNum.Count].shots.Add(new Tuple<string, int[]>(format, new int[] { 0, 0 }));
                    }
                }
            }
        }

        public int UpdateScore(int position, int standNum, int shotsNum, int shotNum)
        {
            int updatedTo = 0;
            int currentValue = Shooters[position].StandScoresByStandNum[standNum].shots[shotsNum].Item2[shotNum];
            switch (currentValue)
            {
                case 0: //not take -> hit
                    Shooters[position].StandScoresByStandNum[standNum].shots[shotsNum].Item2[shotNum] = updatedTo = 1;
                    Shooters[position].StandScoresByStandNum[standNum].standTotal += 1;
                    Shooters[position].overallTotal += 1;
                    break;
                case 1: //hit -> miss
                    Shooters[position].StandScoresByStandNum[standNum].shots[shotsNum].Item2[shotNum] = updatedTo = 2;
                    Shooters[position].StandScoresByStandNum[standNum].standTotal -= 1;
                    Shooters[position].overallTotal -= 1;
                    break;
                case 2: //miss -> not taken
                    Shooters[position].StandScoresByStandNum[standNum].shots[shotsNum].Item2[shotNum] = updatedTo = 0;
                    break;
            }

            return updatedTo;
        }

        public int ShooterStandTotal(int position, int standNum)
        {
            return Shooters[position].StandScoresByStandNum[standNum].standTotal;
        }

        public List<string> StandShots(int standNum)
        {
            return StandsByNum[standNum].shotFormat;
        }
    }

    //for viewing shoot history
    class PreviousShoot : Shoot 
    {
        protected Dictionary<int,int> shooterOriginalPosByID = new Dictionary<int, int>();
        
        public DateTime Date
        {
            get { return date; }   
        }

        public string EventType
        {
            get { return discipline; }
        }

        //public void InitialisePreviousShoot(int shootID)
        //{
        //    string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
        //    using (var db = new SQLiteConnection(dbPath))
        //    {
        //        var shoot = db.Get<Shoots>(shootID);
        //        date = shoot.Date;
        //        location = shoot.Location;
        //        discipline = shoot.EventType;
        //        startingStand = shoot.StartingStand;
        //        numOfClays = shoot.ClayAmount;

        //        int counter = 1;
        //        var overallScores = db.Table<OverallScores>().Where<OverallScores>(s => s.ShootId == shoot.Id).ToList();
        //        foreach (OverallScores overallScore in overallScores)
        //        {
        //            var shooter = db.Get<Shooters>(overallScore.ShooterId);
        //            shooterOriginalPosByID.Add(overallScore.ShooterId, counter);
        //            ShootersByOriginalPos.Add(counter, new Shooter(shooter.Id, shooter.Name, shooter.Class));
        //            ShootersByOriginalPos[counter].overallPercentage = overallScore.OverallPercentage;
        //            ShootersByOriginalPos[counter].overallTotal = overallScore.OverallTotal;
        //            counter++;
        //        }

        //        var stands = db.Table<Stands>().Where<Stands>(s => s.ShootId == shoot.Id).OrderBy(s => s.StandNum).ToList();
        //        foreach (Stands stand in stands)
        //        {
        //            var standFormats = db.Get<StandFormats>(stand.StandFormatId);
        //            StandsByNum.Add(stand.StandNum, new Stand(standFormats.StandType, standFormats.StandFormat, standFormats.NumPairs));

        //            var standScores = db.Table<StandScores>().Where<StandScores>(s => s.StandId == stand.Id).ToList();
        //            foreach (StandScores standScore in standScores)
        //            {
        //                var shooterStandScore = new Shooter.StandScore(standScore.StandTotal, standScore.StandPercentageHit, standScore.RunningTotal);

        //                var shotPairs = db.Table<StandShotsLink>().Where<StandShotsLink>(s => s.StandScoresId == standScore.Id).ToList();
        //                foreach (StandShotsLink shotPair in shotPairs)
        //                {
        //                    shooterStandScore.ShotsByPairNum.Add(shotPair.PairNum, new int[] { db.Get<Shots>(shotPair.shotsId).FirstShot, db.Get<Shots>(shotPair.shotsId).SecondShot });
        //                }

        //                ShootersByOriginalPos[shooterOriginalPosByID[standScore.ShooterId]].StandScoresByStandNum.Add(stand.StandNum, shooterStandScore);
        //            }
        //        }
        //    }
        //}
    }
}