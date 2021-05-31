using System;
using SQLite;

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
        public int NumClays { get; set; }
        [MaxLength(255)]
        public string Notes { get; set; }
    }

    public class Stands
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ShootId { get; set; }
        public int StandNum { get; set; }
        public string StandType { get; set; }
        public string NumClays { get; set; }
    }

    public class StandScores
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int StandId { get; set; }
        public int ShooterId { get; set; }
        public int StandTotal { get; set; }
    }

    public class Shots
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int StandScoreId { get; set; }
        public string Num { get; set; }
        public int ShotCode { get; set; }
    }

    public class OverallScores
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ShootId { get; set; }
        public int ShooterId { get; set; }
        public int OverallTotal { get; set; }
        public int OverallPercentage { get; set; }
    }
    public class Shooters
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed(Name = "Name", Order = 1, Unique = true)]
        public string Name { get; set; }
        [MaxLength(1)]
        public string Class { get; set; }
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
        [Indexed(Name = "StandFormatData", Order = 1, Unique = true)]
        public string StandType { get; set; }
        [Indexed(Name = "StandFormatData", Order = 2, Unique = true)]
        public int NumClays { get; set; }
    }
    public class StandShotsFormats
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int StandFormatId { get; set; }
        public int ShotNum { get; set; }
        public string Type { get; set; }
    }
}