using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AndroidX.Lifecycle;
using ClubClays.DatabaseModels;
using SQLite;

namespace ClubClays
{
    public class ShooterStandData : ViewModel 
    {
        public List<Shooters> selectedShooters;
        public List<Shooters> allShooters;
        public List<StandFormats> standFormats;
    }

    class Shoot : ViewModel
    {
        protected DateTime date;
        protected string location;
        protected string discipline;
        protected int numOfStands;
        protected int numOfClays;
        protected int startingStand;
        protected string userNotes;
        protected bool rotateShooters;

        protected Dictionary<int, Shooter> ShootersByOriginalPos = new Dictionary<int, Shooter>();
        protected SortedList<int, Stand> StandsByNum = new SortedList<int, Stand>();

        public int CurrentNumStands
        {
            get
            {
                return StandsByNum.Count();
            }
        }

        public int NumberOfShooters
        {
            get
            {
                return ShootersByOriginalPos.Count();
            }
        }

        public int PairsInStand(int StandNum)
        {
            return StandsByNum[StandNum].numOfPairs;
        }

        public List<string> ShooterOverallData(int shooterOriginalNum)
        {
            List<string> overallData = new List<string>();
            overallData.Add(ShootersByOriginalPos[shooterOriginalNum].name);
            for (int x = 1; x <= StandsByNum.Count(); x++)
            {
                if(x <= ShootersByOriginalPos[shooterOriginalNum].StandScoresByStandNum.Count)
                {
                    overallData.Add($"{ShootersByOriginalPos[shooterOriginalNum].StandScoresByStandNum[x].standTotal}");
                }
                else
                {
                    overallData.Add("0");
                }
            }
            overallData.Add($"{ShootersByOriginalPos[shooterOriginalNum].overallTotal}");
            return overallData;
        }

        public void ShooterStandData(int shooterOriginalNum, int StandNum, out string name, out string total, out SortedList<int, int[]> hits)
        {
            name = ShootersByOriginalPos[shooterOriginalNum].name;
            if (StandNum <= ShootersByOriginalPos[shooterOriginalNum].StandScoresByStandNum.Count)
            {
                total = $"{ShootersByOriginalPos[shooterOriginalNum].StandScoresByStandNum[StandNum].standTotal}";
                hits = ShootersByOriginalPos[shooterOriginalNum].StandScoresByStandNum[StandNum].ShotsByPairNum;
            }
            else
            {
                int[] standardHitMiss = { 0, 0 };
                SortedList<int, int[]> hitsForStand = new SortedList<int, int[]>();
                total = "0";
                for (int x = 1; x <= StandsByNum[StandNum].numOfPairs; x++)
                {
                    hitsForStand.Add(x, standardHitMiss);
                }
                hits = hitsForStand;
            }
        }

        protected struct Stand
        {
            public string standType;
            public string standFormat;
            public int numOfPairs;

            public Stand(string standType, string standFormat, int numOfPairs)
            {
                this.standType = standType;
                this.standFormat = standFormat;
                this.numOfPairs = numOfPairs;
            }
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
                public int runningTotalAtStand;
                public SortedList<int, int[]> ShotsByPairNum;

                public StandScore(int standTotal, int standPercentage, int runningTotalAtStand)
                {
                    this.standTotal = standTotal;
                    this.standPercentage = standPercentage;
                    this.runningTotalAtStand = runningTotalAtStand;
                    ShotsByPairNum = new SortedList<int, int[]>();
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

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            using (var db = new SQLiteConnection(dbPath))
            {
                Shoots newShoot = new Shoots() { Date = date, Location = location, EventType = discipline, NumStands = numOfStands, ClayAmount = numOfClays, StartingStand = startingStand, Notes = userNotes };
                db.Insert(newShoot);

                for (int x = 1; x <= StandsByNum.Count; x++)
                {
                    // need to check if this obeys unique parameters
                    StandFormats standFormat = new StandFormats() { StandType = StandsByNum[x].standType, StandFormat = StandsByNum[x].standFormat, NumPairs = StandsByNum[x].numOfPairs };
                    
                    Stands newStand = new Stands() { ShootId = newShoot.Id, StandFormatId = standFormat.Id, StandNum = x };
                    db.Insert(newStand);
                    
                    for (int y = 1; y <= ShootersByOriginalPos.Count; y++)
                    {
                        StandScores newStandScore = new StandScores() { StandId = newStand.Id, ShooterId = ShootersByOriginalPos[y].id, StandTotal = ShootersByOriginalPos[y].StandScoresByStandNum[x].standTotal, StandPercentageHit = ShootersByOriginalPos[y].StandScoresByStandNum[x].standPercentage, RunningTotal = ShootersByOriginalPos[y].StandScoresByStandNum[x].runningTotalAtStand };
                        db.Insert(newStandScore);

                        for (int z = 1; z <= ShootersByOriginalPos[y].StandScoresByStandNum[x].ShotsByPairNum.Count; z++)
                        {
                            int id = 0;

                            switch ($"{ShootersByOriginalPos[y].StandScoresByStandNum[x].ShotsByPairNum[z][0]}{ShootersByOriginalPos[y].StandScoresByStandNum[x].ShotsByPairNum[z][1]}")
                            {
                                case "00":
                                    id = 0;
                                    break;
                                case "11":
                                    id = 1;
                                    break;
                                case "22":
                                    id = 2;
                                    break;
                                case "01":
                                    id = 3;
                                    break;
                                case "02":
                                    id = 4;
                                    break;
                                case "10":
                                    id = 5;
                                    break;
                                case "20":
                                    id = 6;
                                    break;
                                case "12":
                                    id = 7;
                                    break;
                                case "21":
                                    id = 8;
                                    break;
                            }

                            db.Insert(new StandShotsLink() { StandScoresId = newStandScore.Id, shotsId = id, PairNum = z });
                        }
                    }
                }

                for (int w = 1; w <= ShootersByOriginalPos.Count; w++)
                {
                    OverallScores newOverallScore = new OverallScores() { ShootId = newShoot.Id, ShooterId = ShootersByOriginalPos[w].id, OverallTotal = ShootersByOriginalPos[w].overallTotal, OverallPercentage = ShootersByOriginalPos[w].overallPercentage };
                    db.Insert(newOverallScore);
                }
            }
        }
        public void SaveFormat() { }
        public void CalculateStats() 
        {
            // Calculate total number of clays
            for (int x = 1; x <= StandsByNum.Count; x++)
            {
                numOfClays += StandsByNum[x].numOfPairs;
            }

            for (int y = 1; y <= ShootersByOriginalPos.Count; y++)
            {
                // Calculate their overall percentage 
                for (int x = 1; x <= StandsByNum.Count; x++)
                {
                    // calculate each stand percentage
                }
            }
        }
    }

    class ShootScoreManagement : Shoot
    {
        protected int currentStand = 1;
        protected int currentPair = 1;
        protected int currentShooterIndex = 0;
        protected string trackingType;

        public int CurrentStand { get => currentStand; }
        public int CurrentPair { get => currentPair; }
        public string CurrentShooterName { get => ShootersByOriginalPos.ElementAt(currentShooterIndex).Value.name; }
        public string TrackingType { get => trackingType; }

        public string CurrentStandScore { get
            {
                if (currentPair == 1)
                {
                    return $"0/{(StandsByNum[currentStand].numOfPairs)*2}";
                }
                else
                {
                    return $"{ShootersByOriginalPos.ElementAt(currentShooterIndex).Value.StandScoresByStandNum[currentStand].standTotal}/{StandsByNum[currentStand].numOfPairs*2}";
                }               
            }
        }

        public bool LastPair { get
            {
                if (currentPair == StandsByNum[currentStand].numOfPairs)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool LastShooter{ get
            {
                if (currentShooterIndex == ShootersByOriginalPos.Count - 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool LastStand{ get
            {
                if (currentStand == StandsByNum.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string UserNotes{set 
            {
                userNotes = value;
            }
        }

        public void InitialiseBasics(List<Shooters> shooters, DateTime date, string location, bool rotateShooters, string discipline, int startingStand)
        {
            this.date = date;
            this.location = location;
            this.rotateShooters = rotateShooters;
            this.discipline = discipline;
            this.startingStand = startingStand;


            currentStand = startingStand;
            trackingType = "Unknown";

            int counter = 1;
            foreach (Shooters shooter in shooters)
            {
                ShootersByOriginalPos.Add(counter++, new Shooter(shooter.Id, shooter.Name, shooter.Class));
            }

        }

        public void InitialiseStands(List<StandFormats> stands)
        {
            trackingType = "Known";
            int standNum = 1;
            foreach (StandFormats stand in stands)
            {
                StandsByNum.Add(standNum++, new Stand(stand.StandType, stand.StandFormat, stand.NumPairs));
            }
        }

        public void AddScore(int shot1Val, int shot2Val)
        {
            int hits = CalculateHits(shot1Val, shot2Val);

            Shooter shooter = ShootersByOriginalPos.ElementAt(currentShooterIndex).Value;
            if (!shooter.StandScoresByStandNum.ContainsKey(currentStand))
            {
                shooter.StandScoresByStandNum.Add(currentStand, new Shooter.StandScore(0, 0, shooter.overallTotal));
            }

            shooter.StandScoresByStandNum[currentStand].ShotsByPairNum.Add(currentPair, new int[] { shot1Val, shot2Val });
            shooter.StandScoresByStandNum[currentStand].standTotal += hits;
            shooter.overallTotal += hits;
            shooter.StandScoresByStandNum[currentStand].runningTotalAtStand = shooter.overallTotal;

            currentPair += 1;
        }

        private int CalculateHits(int val1, int val2)
        {
            int total = 0;
            if (val1 == 1) { total++; }
            if (val2 == 1) { total++; }
            return total;
        }

        public void NextShooter()
        {
            currentPair = 1;
            currentShooterIndex += 1;
        }
        public void NextStand()
        {
            currentStand += 1;
            currentPair = 1;
            currentShooterIndex = 0;

        }
        public void UndoScore() 
        {

        }
    }

    //for adding previous shoots
    class AddShoot : Shoot
    {

    }

    //for viewing shoot history
    class PreviousShoot : Shoot 
    { 

    }
}