using System;
using System.Collections.Generic;
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

        protected Dictionary<int, Shooter> ShootersById;
        protected SortedList<int, Stand> StandsByNum;

        protected struct Stand
        {
            string standType;
            string standFormat;
            int numOfPairs;

            public Stand(string standType, string standFormat, int numOfPairs)
            {
                this.standType = standType;
                this.standFormat = standFormat;
                this.numOfPairs = numOfPairs;
            }
        }

        protected class Shooter
        {
            public string name;
            public string shooterClass;
            public int overallTotal;
            public int overallPercentage;
            public SortedList<int, StandScore> StandScoresByStandNum;

            public struct StandScore
            {
                int standTotal;
                int standPercentage;
                int runningTotalAtStand;
                SortedList<int, bool[]> ShotsByPairNum;
            }

            public Shooter(string name, string shooterClass)
            {
                this.name = name;
                this.shooterClass = shooterClass;
            }
        }

        public void SaveShootData() { }
        public void SaveFormat() { }
    }

    class ShootScoreManagement : Shoot
    {
        protected int currentStand;
        protected int currentPair;
        protected string currentShooterName;
        protected string trackingType;

        protected void InitialiseBasics(List<Shooters> shooters, DateTime date, string location, bool rotateShooters, string discipline, int startingStand)
        {
            this.date = date;
            this.location = location;
            this.rotateShooters = rotateShooters;
            this.discipline = discipline;
            this.startingStand = startingStand;

            foreach (Shooters shooter in shooters)
            {
                ShootersById.Add(shooter.Id, new Shooter(shooter.Name, shooter.Class));
            }
        }

        public void AddScore() 
        {
         
        }

        public void UndoScore() 
        {

        }

        public bool GetCurrentData(out int Stand, out int Pair, out string ShooterName)
        {
            Stand = currentStand;
            Pair = currentPair;
            ShooterName = currentShooterName;

            bool isLastPair = true;

            return isLastPair;
        }
    }

    class UnknownFormatShoot : ShootScoreManagement
    {
        public void Initialise(List<Shooters> shooters, DateTime date, string location, bool rotateShooters, string discipline, int startingStand)
        {
            InitialiseBasics(shooters, date, location, rotateShooters, discipline, startingStand);
            trackingType = "Unknown";
        }
    }

    class KnownFormatShoot : ShootScoreManagement
    {
        public void Initialise(List<Shooters> shooters, List<StandFormats> stands, DateTime date, string location, bool rotateShooters, string discipline, int startingStand)
        {
            // Simple data initialisation
            InitialiseBasics(shooters, date, location, rotateShooters, discipline, startingStand);
            trackingType = "Known";

            int standNum = 1;
            foreach (StandFormats stand in stands)
            {
                StandsByNum.Add(standNum, new Stand(stand.StandType, stand.StandFormat, stand.NumPairs));
                standNum++;
            }
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