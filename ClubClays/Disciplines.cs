using System;
using System.Collections.Generic;

namespace ClubClays
{
    class Disciplines
    {
        public interface IBaseDiscipline
        {
            public string name { get; }
            public int scorePerHit { get; }
            public bool showDetails { get; }

            public bool differentiateFirstAndSecondShot { get; }
            public bool differentiateFirstAndSecondShotScore { get; }
            public int scorePerSecondHit { get; }

            public List<Stand> RoundLayout();
        }
        // Trap
        static public class Trap
        {
            static private List<Stand> FiveStandOfFiveSingles()
            {
                return new List<Stand>()
                {
                    new Stand(1, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null) }),
                    new Stand(2, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null) }),
                    new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null) }),
                    new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null) }),
                    new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Single", null) })
                };
            }

            static private List<Stand> FiveStandOfFivePairs()
            {
                return new List<Stand>()
                {
                    new Stand(1, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null) }),
                    new Stand(2, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null) }),
                    new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null) }),
                    new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null)}),
                    new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null) })
                };
            }
            public class AmericanTrap : IBaseDiscipline
            {
                public string name => "American Trap";
                public int scorePerHit => 1;
                public bool showDetails => false;
                public bool differentiateFirstAndSecondShot => false;
                public bool differentiateFirstAndSecondShotScore => false;
                public int scorePerSecondHit => 1;

                public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            public class AmericanTrapDoubles : IBaseDiscipline
            {
                public string name => "American Trap Doubles";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                public List<Stand> RoundLayout()
                {
                    return FiveStandOfFivePairs();
                }
            }
            public class DoubleRise : IBaseDiscipline
            {
                public string name => "Double Rise";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                public List<Stand> RoundLayout()
                {
                    return FiveStandOfFivePairs();
                }
            }
            public class DTL : IBaseDiscipline
            {
                public string name => "Down The Line";

                public int scorePerHit => 3;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => true;

                public bool differentiateFirstAndSecondShotScore => true;

                public int scorePerSecondHit => 2;

                public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            public class OlympicDoubleTrap : IBaseDiscipline
            {
                public string name => "Olympic Double Trap";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                public List<Stand> RoundLayout()
                {
                    return FiveStandOfFivePairs();
                }
            }
            public class OlympicTrap : IBaseDiscipline
            {
                public string name => "Olympic Trap";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            public class SingleBarrel : IBaseDiscipline
            {
                public string name => "Single Barrel";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            public class AutomaticBallTrap : IBaseDiscipline
            {
                public string name => "Automatic Ball Trap";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            public class Helice : IBaseDiscipline
            {
                public string name => "Helice";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                //Unsure of standard number of "stands"

                public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            public class UniversalTrench : IBaseDiscipline
            {
                public string name => "Universal Trench";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => true;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                //25 single targets with two shots allowed at each one. Scoring is based on a single point being awarded for each hit, regardless of whether it was the first or second shot.

                public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            public class WobbleTrap : IBaseDiscipline
            {
                public string name => "Wobble Trap";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                //Unsure of standard number of "stands"

                public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            public class NordicTrap : IBaseDiscipline
            {
                public string name => "Nordic Trap";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
        }
        // Skeet
        static public class Skeet
        {
            public class AmericanSkeet : IBaseDiscipline
            {
                public string name => "American Skeet";

                public int scorePerHit => 1;

                public bool showDetails => true;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                //The first miss is repeated immediately and is called an option. If no targets are missed during the round, the last or 25th target is shot at the last station.

                public List<Stand> RoundLayout()
                {
                    return new List<Stand>()
                    {
                        new Stand(1, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 1
                        new Stand(2, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 2
                        new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }) }), // 3
                        new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }) }), // 4
                        new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }) }), //5
                        new Stand(6, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //6
                        new Stand(7, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }),  //7
                        new Stand(8, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }) })  //8
                    };
                }
            }
            public class EnglishSkeet : IBaseDiscipline
            {
                public string name => "English Skeet";

                public int scorePerHit => 1;

                public bool showDetails => true;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                // The first target that the shooter misses is immediately reshot. If a shooter hits the first 24 targets without missing, they get the option on stand 7 to shoot either the low or high target again for the 25th shot.

                public List<Stand> RoundLayout()
                {
                    return new List<Stand>()
                    {
                        new Stand(1, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 1
                        new Stand(2, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 2
                        new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }) }), // 3
                        new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Optional", "Optional" }) }), // 4
                        new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }) }), //5
                        new Stand(6, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //6
                        new Stand(7, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }), new Tuple<string, string[]>("Single", new string[] { "Optional" }) }) //7
                    };
                }
            }
            static public class OlympicSkeet
            {
                public class OlympicSkeetVar1 : IBaseDiscipline
                {
                    public string name => "Olympic Skeet";

                    public int scorePerHit => 1;

                    public bool showDetails => true;

                    public bool differentiateFirstAndSecondShot => false;

                    public bool differentiateFirstAndSecondShotScore => false;

                    public int scorePerSecondHit => 1;

                    // This is for 1 round (usually 5) ? station 4 occurs twice

                    public List<Stand> RoundLayout()
                    {
                        return new List<Stand>()
                        {
                            new Stand(1, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 1
                            new Stand(2, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 2
                            new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 3
                            new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" })}), // 4
                            new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //5
                            new Stand(6, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //6
                            new Stand(7, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //7
                            new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //4
                            new Stand(8, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }) }) // 8
                        };

                    }
                }

                public class OlympicSkeetVar2 : IBaseDiscipline
                {
                    public string name => "Olympic Skeet";

                    public int scorePerHit => 1;

                    public bool showDetails => true;

                    public bool differentiateFirstAndSecondShot => false;

                    public bool differentiateFirstAndSecondShotScore => false;

                    public int scorePerSecondHit => 1;

                    // This is for 1 round (usually 5) ? station 4 occurs twice

                    public List<Stand> RoundLayout()
                    {
                        return new List<Stand>()
                        {
                            new Stand(1, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 1
                            new Stand(2, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 2
                            new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 3
                            new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" })}), // 4
                            new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //5
                            new Stand(6, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //6
                            new Stand(7, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //7
                            new Stand(8, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }) }) // 8
                        };
                    }
                }
            }

            static public class OlympicSkeetFinal
            {
                public class OlympicSkeetFinalVar1 : IBaseDiscipline
                {
                    public string name => "Olympic Skeet Final";

                    public int scorePerHit => 1;

                    public bool showDetails => true;

                    public bool differentiateFirstAndSecondShot => false;

                    public bool differentiateFirstAndSecondShotScore => false;

                    public int scorePerSecondHit => 1;

                    // Repeats eliminating after 20, 30, 40, 50, 60 clays

                    public List<Stand> RoundLayout()
                    {
                        return new List<Stand>()
                        {
                            new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }),
                            new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }),
                            new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) })
                        };
                    }
                }
                public class OlympicSkeetFinalVar2 : IBaseDiscipline
                {
                    public string name => "Olympic Skeet Final";

                    public int scorePerHit => 1;

                    public bool showDetails => true;

                    public bool differentiateFirstAndSecondShot => false;

                    public bool differentiateFirstAndSecondShotScore => false;

                    public int scorePerSecondHit => 1;

                    // Repeats eliminating after 20, 30, 40, 50, 60 clays

                    public List<Stand> RoundLayout()
                    {
                        return new List<Stand>()
                        {
                            new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }),
                            new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }),
                            new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) })
                        };
                    }
                }
            }
            public class OlympicSkeetPre2012 : IBaseDiscipline
            {
                public string name => "Olympic Skeet (Pre 2012)";

                public int scorePerHit => 1;

                public bool showDetails => true;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                // from hotclays

                public List<Stand> RoundLayout()
                {
                    return new List<Stand>()
                    {
                        new Stand(1, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 1
                        new Stand(2, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 2
                        new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 3
                        new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 4
                        new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //5
                        new Stand(6, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "Low" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //6
                        new Stand(7, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //7
                        new Stand(8, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Single", new string[] { "Low" }) }) // 8
                    };
                }
            }
            static public class SkeetDoubles
            {
                public class SkeetDoublesVar1 : IBaseDiscipline
                {
                    public string name => "Skeet Doubles";

                    public int scorePerHit => 1;

                    public bool showDetails => true;

                    public bool differentiateFirstAndSecondShot => false;

                    public bool differentiateFirstAndSecondShotScore => false;

                    public int scorePerSecondHit => 1;

                    //In the first round, shooters move from stand one to stand seven then inreverse back to stand two. In the second round they shoot one to seven, then seven back to one.

                    public List<Stand> RoundLayout()
                    {
                        return new List<Stand>()
                        {
                            new Stand(1, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //1
                            new Stand(2, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //2
                            new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //3
                            new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //4
                            new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //5
                            new Stand(6, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //6
                            new Stand(7, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //7
                            new Stand(6, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //6
                            new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //5
                            new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //4
                            new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //3
                            new Stand(2, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }) //2
                        };
                    }
                }
                public class SkeetDoublesVar2 : IBaseDiscipline
                {
                    public string name => "Skeet Doubles";

                    public int scorePerHit => 1;

                    public bool showDetails => true;

                    public bool differentiateFirstAndSecondShot => false;

                    public bool differentiateFirstAndSecondShotScore => false;

                    public int scorePerSecondHit => 1;

                    //In the first round, shooters move from stand one to stand seven then inreverse back to stand two. In the second round they shoot one to seven, then seven back to one.

                    public List<Stand> RoundLayout()
                    {
                        return new List<Stand>()
                        {
                            new Stand(1, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //1
                            new Stand(2, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //2
                            new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //3
                            new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //4
                            new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //5
                            new Stand(6, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //6
                            new Stand(7, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //7
                            new Stand(6, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //6
                            new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //5
                            new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //4
                            new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //3
                            new Stand(2, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //2
                            new Stand(1, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }) //1
                        };
                    }
                }
            }
            public class SkeetShootOff : IBaseDiscipline
            {
                public string name => "Skeet Shoot Off";

                public int scorePerHit => 1;

                public bool showDetails => true;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                // Repeats till someone misses

                public List<Stand> RoundLayout()
                {
                    return new List<Stand>()
                    {
                        new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[]{ "Low", "High" }) }), //3
                        new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //4
                        new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }) //5
                    };
                }
            }
            public class WobbleSkeet : IBaseDiscipline
            {
                public string name => "Wobble Skeet";

                public int scorePerHit => 1;

                public bool showDetails => true;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                public List<Stand> RoundLayout()
                {
                    return new List<Stand>()
                    {
                        new Stand(1, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 1
                        new Stand(2, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 2
                        new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 3
                        new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), // 4
                        new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //5
                        new Stand(6, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }), //6
                        new Stand(7, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }),  //7
                        new Stand(8, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", new string[] { "High" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" })  })  //8
                    };
                }
            }
        }
        // Sporting Clays
        static public class Sporting
        {
            public class CompakSporting : IBaseDiscipline
            {
                public string name => "Compak Sporting";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                //custom layouts

                public List<Stand> RoundLayout()
                {
                    return null;
                }
            }
            public class EnglishSporting : IBaseDiscipline
            {
                public string name => "English Sporting";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                //custom layouts

                public List<Stand> RoundLayout()
                {
                    return null;
                }
            }
            public class FITASCSporting : IBaseDiscipline
            {
                public string name => "FITASC Sporting";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                //custom layouts

                public List<Stand> RoundLayout()
                {
                    return null;
                }
            }
            public class FiveStand : IBaseDiscipline
            {
                public string name => "Five Stand";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                public List<Stand> RoundLayout()
                {
                    return new List<Stand>()
                    {
                        new Stand(1, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null) }),
                        new Stand(2, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null) }),
                        new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null) }),
                        new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null) }),
                        new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null) })
                    };
                }
            }
            public class Sportrap : IBaseDiscipline
            {
                public string name => "Sportrap";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                public List<Stand> RoundLayout()
                {
                    return new List<Stand>()
                    {
                        new Stand(1, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null) }),
                        new Stand(2, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null) }),
                        new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null) }),
                        new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null) }),
                        new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Single", null), new Tuple<string, string[]>("Pair", null), new Tuple<string, string[]>("Pair", null) })
                    };
                }
            }
            public class SuperSporting : IBaseDiscipline
            {
                public string name => "Super Sporting";

                public int scorePerHit => 1;

                public bool showDetails => false;

                public bool differentiateFirstAndSecondShot => false;

                public bool differentiateFirstAndSecondShotScore => false;

                public int scorePerSecondHit => 1;

                //custom layouts

                public List<Stand> RoundLayout()
                {
                    return null;
                }
            }
        }
    }
}