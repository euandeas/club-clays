using System;
using System.Collections.Generic;

namespace ClubClays
{
    static class Disciplines
    {
        //ALL CURRENTLY ONLY REPRESENT 1 ROUND OF EACH DISCIPLINE

        //static public class Discipline
        //{
        //    static string name = "";
        //    static int scorePerHit = 1;
        //
        //    static bool differentiateFirstAndSecondShot = false;
        //    static bool differentiateFirstAndSecondShotScore = false;
        //    static int scorePerSecondHit = 1;
        //
        //   static public List<Stand> RoundLayout()
        //    {
        //        return null;
        //    }
        //}

        static public class Discipline
        {
            static public string name = "";
            static public int scorePerHit = 1;

            static public bool differentiateFirstAndSecondShot = false;
            static public bool differentiateFirstAndSecondShotScore = false;
            static public int scorePerSecondHit = 1;

            static public List<Stand> RoundLayout()
            {
                return null;
            }
        }

        static private List<Stand> FiveStandOfFiveSingles()
        {
            return new List<Stand>()
            {
                new Stand(1, new List<Tuple<string,string[]>>() { "Single", "Single", "Single", "Single", "Single" }),
                new Stand(2, new List<Tuple<string,string[]>>() { "Single", "Single", "Single", "Single", "Single" }),
                new Stand(3, new List<Tuple<string,string[]>>() { "Single", "Single", "Single", "Single", "Single" }),
                new Stand(4, new List<Tuple<string,string[]>>() { "Single", "Single", "Single", "Single", "Single" }),
                new Stand(5, new List<Tuple<string,string[]>>() { "Single", "Single", "Single", "Single", "Single" })
            };
        }

        static private List<Stand> FiveStandOfFivePairs()
        {
            return new List<Stand>()
            { 
                new Stand(1, new List<Tuple<string,string[]>>() { "Pair", "Pair", "Pair", "Pair", "Pair" }),
                new Stand(2, new List<Tuple<string,string[]>>() { "Pair", "Pair", "Pair", "Pair", "Pair" }),
                new Stand(3, new List<Tuple<string,string[]>>() { "Pair", "Pair", "Pair", "Pair", "Pair" }),
                new Stand(4, new List<Tuple<string,string[]>>() { "Pair", "Pair", "Pair", "Pair", "Pair" }),
                new Stand(5, new List<Tuple<string,string[]>>() { "Pair", "Pair", "Pair", "Pair", "Pair" })
            };
        }

        // Trap
        public static class Trap
        {
            static public class AmericanTrap
            {
                static public string name = "American Trap";
                static public int scorePerHit = 1;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            static public class AmericanTrapDoubles
            {
                static public string name = "American Trap Doubles";
                static public int scorePerHit = 1;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return FiveStandOfFivePairs();
                }
            }
            static public class DoubleRise
            {
                static public string name = "Double Rise";
                static public int scorePerHit = 1;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return FiveStandOfFivePairs();
                }
            }
            static public class DTL
            {
                static public string name = "Down The Line";
                static public int scorePerHit = 3;

                static public bool differentiateFirstAndSecondShot = true;
                static public bool differentiateFirstAndSecondShotScore = true;
                static public int scorePerSecondHit = 2;

                static public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            static public class OlympicDoubleTrap
            {
                static public string name = "Olympic Double Trap";
                static public int scorePerHit = 1;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return FiveStandOfFivePairs();
                }
            }
            static public class OlympicTrap
            {
                static public string name = "Olympic Trap";
                static public int scorePerHit = 1;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            static public class SingleBarrel
            {
                static public string name = "Single Barrel";
                static public int scorePerHit = 1;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            static public class AutomaticBallTrap
            {
                static public string name = "Automatic Ball Trap";
                static public int scorePerHit = 1;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            static public class Helice
            {
                //Unsure of standard number of "stands"
                static public string name = "Helice";
                static public int scorePerHit = 1;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            static public class UniversalTrench
            {
                //25 single targets with two shots allowed at each one. Scoring is based on a single point being awarded for each hit, regardless of whether it was the first or second shot.
                static public string name = "Universal Trench";
                static public int scorePerHit = 1;

                static public bool differentiateFirstAndSecondShot = true;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            static public class WobbleTrap
            {
                //Unsure of standard number of "stands"
                static public string name = "Wobble Trap";
                static public int scorePerHit = 1;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
            static public class NordicTrap
            {
                static public string name = "Nordic Trap";
                static public int scorePerHit = 1;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return FiveStandOfFiveSingles();
                }
            }
        }
        // Skeet
        public static class Skeet
        {
            static public class AmericanSkeet
            {
                //The first miss is repeated immediately and is called an option. If no targets are missed during the round, the last or 25th target is shot at the last station.
                static public string name = "American Skeet";
                static public int scorePerHit = 1;
                static public bool showDetails = true;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
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
            static public class EnglishSkeet
            {
                // The first target that the shooter misses is immediately reshot. If a shooter hits the first 24 targets without missing, they get the option on stand 7 to shoot either the low or high target again for the 25th shot.
                static public string name = "English Skeet";
                static public int scorePerHit = 1;
                static public bool showDetails = true;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
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
                // This is for 1 round (usually 5) ? station 4 occurs twice
                static public string name = "Olympic Skeet";
                static public int scorePerHit = 1;
                static public bool showDetails = true;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout(int layout)
                {
                    switch (layout)
                    {
                        default:
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
                        case 2:
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
                // Repeats eliminating after 20, 30, 40, 50, 60 clays
                static public string name = "Olympic Skeet Final";
                static public int scorePerHit = 1;
                static public bool showDetails = true;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout(int layout)
                {
                    switch (layout)
                    {
                        default:
                            return new List<Stand>()
                            {
                                new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }),
                                new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }) }),
                                new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) })
                            };
                        case 2:
                            return new List<Stand>()
                            {
                                new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }),
                                new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }),
                                new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) })
                            };
                    }
                }
            }
            static public class OlympicSkeetPre2012
            {
                // from hotclays
                static public string name = "Olympic Skeet (Pre 2012)";
                static public int scorePerHit = 1;
                static public bool showDetails = true;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
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
                //In the first round, shooters move from stand one to stand seven then inreverse back to stand two. In the second round they shoot one to seven, then seven back to one.
                static public string name = "Skeet Doubles";
                static public int scorePerHit = 1;
                static public bool showDetails = true;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout(int layout)
                {
                    switch (layout)
                    {
                        default:
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
                        case 2:
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
            static public class SkeetShootOff
            {
                // Repeats till someone misses
                static public string name = "Skeet Shoot Off";
                static public int scorePerHit = 1;
                static public bool showDetails = true;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout(int layout)
                {
                    return new List<Stand>()
                    {
                        new Stand(3, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[]{ "Low", "High" }) }), //3
                        new Stand(4, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }), //4
                        new Stand(5, new List<Tuple<string,string[]>>() { new Tuple<string, string[]>("Pair", new string[] { "High", "Low" }), new Tuple<string, string[]>("Pair", new string[] { "Low", "High" }) }) //5
                    };
                }
            }
            static public class WobbleSkeet
            {
                static public string name = "Wobble Skeet";
                static public int scorePerHit = 1;
                static public bool showDetails = true;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
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
        public static class Sporting
        {
            static public class CompakSporting
            {
                //custom layouts
                static public string name = "Compak Sporting";
                static public int scorePerHit = 1;
                static public bool showDetails = false;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return null;
                }
            }
            static public class EnglishSporting
            {
                //custom layouts
                static public string name = "English Sporting";
                static public int scorePerHit = 1;
                static public bool showDetails = false;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return null;
                }
            }
            static public class FITASCSporting
            {
                //custom layouts
                static public string name = "FITASC Sporting";
                static public int scorePerHit = 1;
                static public bool showDetails = false;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return null;
                }
            }
            static public class FiveStand
            {
                static public string name = "Five Stand";
                static public int scorePerHit = 1;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
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
            static public class Sportrap
            {
                static public string name = "Sportrap";
                static public int scorePerHit = 1;
                static public bool showDetails = false;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
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
            static public class SuperSporting
            {
                //custom layouts
                static public string name = "Super Sporting";
                static public int scorePerHit = 1;
                static public bool showDetails = false;

                static public bool differentiateFirstAndSecondShot = false;
                static public bool differentiateFirstAndSecondShotScore = false;
                static public int scorePerSecondHit = 1;

                static public List<Stand> RoundLayout()
                {
                    return null;
                }
            }
        }
    }
}