using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ClubClays.DatabaseModels
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

        [OneToMany] 
        public List<Stands> Stands { get; set; }

        [OneToMany]
        public List<OverallScores> OverallScores { get; set; }
    }

    public class Stands
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(Shoots))]
        public int ShootId { get; set; }
        [ForeignKey(typeof(StandFormats))]
        public int StandFormatId { get; set; }
        public int StandNum { get; set; }

        [OneToMany]
        public List<StandScores> StandScores { get; set; }
    }

    public class StandScores
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(Stands))]
        public int StandId { get; set; }
        [ForeignKey(typeof(Shooters))]
        public int ShooterId { get; set; }
        public int StandTotal { get; set; }
        public int StandPercentageHit { get; set; }
        public int RunningTotal { get; set; }

        [OneToMany]
        public List<StandShotsLink> StandsShotsLink { get; set; }
    }

    public class StandShotsLink
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(StandScores))]
        public int StandScoresId { get; set; }
        [ForeignKey(typeof(Shots))]
        public int shotsId { get; set; }
        public int PairNum { get; set; }
    }

    public class Shots
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public bool FirstShot { get; set; }
        public bool SecondShot { get; set; }

        [OneToMany]
        public List<StandShotsLink> StandsShotsLink { get; set; }
    }
    public class OverallScores
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(Shoots))]
        public int ShootId { get; set; }
        [ForeignKey(typeof(Shooters))]
        public int ShooterId { get; set; }
        public int OverallTotal { get; set; }
        public int OverallPercentage { get; set; }
    }
    public class Shooters
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        [MaxLength(1)]
        public string Class { get; set; }

        [OneToMany]
        public List<StandScores> StandScores { get; set; }

        [OneToMany]
        public List<OverallScores> OverallScores { get; set; }
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

        [OneToMany]
        public List<SavedFormatsLink> SavedFormatsLink { get; set; }
    }

    public class SavedFormatsLink
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(ShootFormats))]
        public int ShootFormatId { get; set; }
        [ForeignKey(typeof(StandFormats))]
        public int StandFormatId { get; set; }
        public int StandNum { get; set; }
    }

    public class StandFormats
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string StandType { get; set; }
        public string StandFormat { get; set; }
        public int NumPairs { get; set; }

        [OneToMany]
        public List<SavedFormatsLink> SavedFormatsLink { get; set; }

        [OneToMany]
        public List<Stands> Stands { get; set; }
    }
}