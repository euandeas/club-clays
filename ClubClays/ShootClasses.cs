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
        protected string eventType;
        protected string trackingType;
        protected int numOfStands;
        protected int numOfClays;
        protected int startingStand;
        protected string userNotes;
        protected bool rotateShooters;

        protected int currentStand;
        protected int currentPair;
        protected string currentShooterName;

        protected Dictionary<int, Shooter> ShootersById;
        protected SortedList<int, Stand> StandsByNum;

        protected struct Stand
        {
            string standType;
            string standFormat;
            int numOfPairs;
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
        }

        public void SaveShootData() { }
        public void SaveFormat() { }
    }

    class ShootScoreManagement : Shoot
    {
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

    class UndeterminedShoot : ShootScoreManagement
    {

    }

    class DeterminedShoot : ShootScoreManagement
    {

    }

    class AddShoot : Shoot
    {

    }

    class PreviousShoot : Shoot 
    { 

    }
}