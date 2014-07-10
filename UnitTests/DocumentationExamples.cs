using System;
using System.Collections.Generic;
using MarkdownLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.MarkdownLog
{
    [TestClass]
    public class DocumentationExamples
    {
        [TestMethod]
        public void TableExample()
        {
            var data = new[]
            {
                new {Year = 1991, Album = "Out of Time", Songs = 11, Rating = "* * * *"},
                new {Year = 1992, Album = "Automatic for the People", Songs = 12, Rating = "* * * * *"},
                new {Year = 1994, Album = "Monster", Songs = 12, Rating = "* * *"}
            };

            Console.Write(data.ToMarkdownTable().ToMarkdown());

            // Produces:
            //
            //     Year | Album                    | Songs | Rating   
            //     ---- | ------------------------ | ----- | --------- 
            //     1991 | Out of Time              | 11    | * * * *  
            //     1992 | Automatic for the People | 12    | * * * * *
            //     1994 | Monster                  | 12    | * * *   
        }

        [TestMethod]
        public void BarChartExample()
        {
            var worldCup = new Dictionary<string, int>
            {
                {"Brazil", 5},
                {"Italy", 4},
                {"Germany", 3},
                {"Argentina", 2},
                {"Uruguay", 2},
                {"France", 1},
                {"Spain", 1},
                {"England", 1}
            };

            Console.Write(worldCup.ToMarkdownBarChart().ToMarkdown());
        }
        [TestMethod]
        public void NumberedListExample()
        {
            var planets = new[] {"Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune"};
            Console.Write(planets.ToMarkdownNumberedList().ToMarkdown());

            // Produces:
            //
            //    1. Mercury
            //    2. Venus
            //    3. Earth
            //    4. Mars
            //    5. Jupiter
            //    6. Saturn
            //    7. Uranus
            //    8. Neptune
        }
    }
}