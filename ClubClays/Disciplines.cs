using System.Collections.Generic;

namespace ClubClays
{
    static class Disciplines
    {
        static private List<Stand> FiveStandOfFiveSingles()
        {
            return new List<Stand>()
            {
                new Stand(new List<string>() { "Single", "Single", "Single", "Single", "Single" }),
                new Stand(new List<string>() { "Single", "Single", "Single", "Single", "Single" }),
                new Stand(new List<string>() { "Single", "Single", "Single", "Single", "Single" }),
                new Stand(new List<string>() { "Single", "Single", "Single", "Single", "Single" }),
                new Stand(new List<string>() { "Single", "Single", "Single", "Single", "Single" })
            };
        }

        static private List<Stand> FiveStandOfFivePairs()
        {
            return new List<Stand>()
            { 
                new Stand(new List<string>() { "Pair", "Pair", "Pair", "Pair", "Pair" }),
                new Stand(new List<string>() { "Pair", "Pair", "Pair", "Pair", "Pair" }),
                new Stand(new List<string>() { "Pair", "Pair", "Pair", "Pair", "Pair" }),
                new Stand(new List<string>() { "Pair", "Pair", "Pair", "Pair", "Pair" }),
                new Stand(new List<string>() { "Pair", "Pair", "Pair", "Pair", "Pair" })
            };
        }

        //static public List<Stand> Discipline()
        //{
        //    return new List<Stand>() 
        //    { 
        //        new Stand(new List<string>() {})
        //    };
        //}

        // Trap
        public static class Trap
        {
            static public List<Stand> AmericanTrap() //Singles
            {
                return FiveStandOfFiveSingles();
            }
            static public List<Stand> AmericanTrapDoubles()
            {
                return FiveStandOfFivePairs();
            }
            static public List<Stand> DoubleRise()
            {
                return FiveStandOfFivePairs();
            }
            static public List<Stand> DTL() //Down The Line - !Different Scoring System!
            {
                return FiveStandOfFiveSingles();
            }
            static public List<Stand> OlympicDoubleTrap()
            {
                return FiveStandOfFivePairs();
            }
            static public List<Stand> OlympicTrap()
            {
                return FiveStandOfFiveSingles();
            }
            static public List<Stand> SingleBarrel()
            {
                return FiveStandOfFiveSingles();
            }
            static public List<Stand> AutomaticBallTrap()
            {
                return FiveStandOfFiveSingles();
            }
            static public List<Stand> Helice() //Unsure of standard number of "stands"
            {
                return FiveStandOfFiveSingles();
            }
            static public List<Stand> UniversalTrench()  //Unsure of usual number of singles per stand and standard number of "stands"
            {
                return null;
            }
            static public List<Stand> WobbleTrap() //Unsure of standard number of "stands"
            {
                return FiveStandOfFiveSingles();
            }
            static public List<Stand> NordicTrap()
            {
                return FiveStandOfFiveSingles();
            }
        }
        // Skeet
        public static class Skeet
        {
            static public List<Stand> AmericanSkeet()
            {
                return new List<Stand>()
                {
                    new Stand(new List<string>() {})
                };
            }
            static public List<Stand> EnglishSkeet()
            {
                return new List<Stand>()
                {
                    new Stand(new List<string>() {})
                };
            }
            static public List<Stand> OlympicSkeet() // This is for 1 round (usually 5) ? station 4 occurs twice
            {
                return new List<Stand>()
                {
                    new Stand(new List<string>() { "Single", "Pair" }), // 1
                    new Stand(new List<string>() { "Single", "Pair" }), // 2
                    new Stand(new List<string>() { "Single", "Pair" }), // 3
                    new Stand(new List<string>() { "Single", "Single" }), // 4
                    new Stand(new List<string>() { "Single", "Pair" }), //5
                    new Stand(new List<string>() { "Single", "Pair" }), //6
                    new Stand(new List<string>() { "Pair" }), //7
                    new Stand(new List<string>() { "Pair", "Pair" }), //4
                    new Stand(new List<string>() { "Single", "Single" }) // 8
                };
            }
            static public List<Stand> OlympicSkeetFinal()//?
            {
                return new List<Stand>()
                {
                    new Stand(new List<string>() { "Pair", "Pair" }),
                    new Stand(new List<string>() { "Pair" }),
                    new Stand(new List<string>() { "Pair", "Pair" })
                };
            }
            static public List<Stand> OlympicSkeetPre2012() //?
            {
                return new List<Stand>()
                {
                    new Stand(new List<string>() {})
                };
            }
            static public List<Stand> SkeetDoubles()
            {
                return new List<Stand>()
                {
                    new Stand(new List<string>() {})
                };
            }
            static public List<Stand> SkeetShootOff() // Repeats till someone misses
            {
                return new List<Stand>()
                {
                    new Stand(new List<string>() { "Pair", "Pair" }), //3
                    new Stand(new List<string>() { "Pair", "Pair" }), //4
                    new Stand(new List<string>() { "Pair", "Pair" }), //5
                };
            }
            static public List<Stand> WobbleSkeet()
            {
                return new List<Stand>()
                {
                    new Stand(new List<string>() {})
                };
            }
        }
        // Sporting Clays
        public static class Sporting
        {
            static public List<Stand> CompakSporting() //custom layouts
            {
                return null;
            }
            static public List<Stand> EnglishSporting() //custom layouts
            {
                return null;
            }
            static public List<Stand> FITASCSporting() //custom layouts
            {
                return null;
            }
            static public List<Stand> FiveStand()
            {
                return new List<Stand>()
                {
                    new Stand(new List<string>() { "Single", "Pair", "Pair" }),
                    new Stand(new List<string>() { "Single", "Pair", "Pair" }),
                    new Stand(new List<string>() { "Single", "Pair", "Pair" }),
                    new Stand(new List<string>() { "Single", "Pair", "Pair" }),
                    new Stand(new List<string>() { "Single", "Pair", "Pair" })
                };
            }
            static public List<Stand> Sportrap()
            {
                return new List<Stand>()
                {
                    new Stand(new List<string>() { "Single", "Pair", "Pair" }),
                    new Stand(new List<string>() { "Single", "Pair", "Pair" }),
                    new Stand(new List<string>() { "Single", "Pair", "Pair" }),
                    new Stand(new List<string>() { "Single", "Pair", "Pair" }),
                    new Stand(new List<string>() { "Single", "Pair", "Pair" })
                };
            }
            static public List<Stand> SuperSporting() //custom layouts
            {
                return null;
            }
        }
        // Other
        //public static class Other
        //{
            //static public List<Stand> AllRound()
            //{
                //return new List<Stand>()
                //{
                    //new Stand(new List<string>() {})
                //};
            //}
        //}
    }
}