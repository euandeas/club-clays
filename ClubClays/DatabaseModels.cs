namespace ClubClays.DatabaseModels;

using SQLite;

/// <summary>
/// The main database tables for storing shoot data
/// </summary>
public class Shooter
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public bool PrimaryUser { get; set; }
    public string Name { get; set; }
    public string Class { get; set; }
}

public class Shoot
{

}

public class Round
{

}


public class StandFormat
{

}

public class StandClayFormat
{

}

public class OverallScore
{

}

public class StandScore
{

}

public class Shots
{

}

/// <summary>
/// The tables used for saving formats that can be used again
/// </summary>
public class SavedRoundFormat
{

}

public class SavedStandFormat 
{ 

}

public class SavedClayFormat
{

}

