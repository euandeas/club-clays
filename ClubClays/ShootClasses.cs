using System;
using System.Collections.Generic;
using System.Linq;
using AndroidX.Lifecycle;
using ClubClays.DatabaseModels;

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

        public void SaveShootData() { }
        public void SaveFormat() { }
    }

    class ShootScoreManagement : Shoot
    {
        protected int currentStand = 1;
        protected int currentPair = 1;
        protected int currentShooterIndex;
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
                    return $"{ShootersByOriginalPos.ElementAt(currentShooterIndex).Value.StandScoresByStandNum[currentStand].standTotal}/{(StandsByNum[currentStand].numOfPairs)*2}";
                }               
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

        }

        private int CalculateHits(int val1, int val2)
        {
            int total = 0;
            if (val1 == 1) { total++; }
            if (val2 == 1) { total++; }
            return total;
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