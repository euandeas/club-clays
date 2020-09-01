using System;
using SQLite;

namespace ClubClays
{
    //Main tables
    public class Shoots
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string EventType { get; set; }
        public int NumStands { get; set; }
        public int ClayAmount { get; set; }
        public int StartingStand { get; set; }
        public string DataCollectionType { get; set; }
        [MaxLength(250)]
        public string Notes { get; set; }
    }

    public class Stands
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ShootId { get; set; }
        public int StandNum { get; set; }
        public string StandType { get; set; }
        public string StandFormat { get; set; }
        public int NumPairs { get; set; }
    }

    public class StandScores
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int StandId { get; set; }
        public int ShooterId { get; set; }
        public int StandTotal { get; set; }
        public int StandPercentageHit { get; set; }
        public int RunningTotal { get; set; }
    }

    public class Shots
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int StandScoreId { get; set; }
        public int PairNumber { get; set; }
        public bool FirstShot { get; set; }
        public bool SecondShot { get; set; }
    }
    public class OverallScores
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ShootId { get; set; }
        public int ShootersId { get; set; }
        public int OverallTotal { get; set; }
        public int OverallPercentage { get; set; }
    }
    public class Shooters
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public char Class { get; set; }
    }

    //Shoot Format tables
    public class ShootFormats
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FormatName { get; set; }
        public string EventType { get; set; }
        public string NumStands { get; set; }
        public string ClayAmount { get; set; }
    }
    public class StandFormats
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ShootFormatId { get; set; }
        public string Symbol { get; set; }
        public int StandNum { get; set; }
        public string StandType { get; set; }
        public string StandFormat { get; set; }
        public int NumPairs { get; set; }
    }
}