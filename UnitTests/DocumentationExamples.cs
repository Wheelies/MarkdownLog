using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            Console.Write(data.ToMarkdownTable());

            // Produces:
            //
            //     Year | Album                    | Songs | Rating   
            //     ---- | ------------------------ | ----- | --------- 
            //     1991 | Out of Time              | 11    | * * * *  
            //     1992 | Automatic for the People | 12    | * * * * *
            //     1994 | Monster                  | 12    | * * *   
        }

        [TestMethod]
        public void ParagraphExample()
        {
            var text = "Lolita, light of my life, fire of my loins. My sin, my soul. Lo-lee-ta: the tip of the tongue taking a trip of three steps down the palate to tap, at three, on the teeth. Lo. Lee. Ta.";
            Console.Write(text.ToMarkdownParagraph());
        }

        [TestMethod]
        public void ParagraphWithCustomWordWrapColumn()
        {
            var text = "Most people die of a sort of creeping common sense, and discover when it is too late that the only things one never regrets are one's mistakes.";
            var paragraph = new Paragraph(text) { WordWrapColumn = 30 };
            Console.Write(paragraph);
        }

        [TestMethod]
        public void NumberedListExample()
        {
            var planets = new[] {"Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune"};
            Console.Write(planets.ToMarkdownNumberedList());

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

        [TestMethod]
        public void BulletedListExample()
        {
            var beatles = new[] { "John", "Paul", "Ringo", "George" };
            Console.Write(beatles.ToMarkdownBulletedList());
        }

        [TestMethod]
        public void BarChartExample()
        {
            var worldCup = new Dictionary<string, int>
            {
                {"Brazil", 5},
                {"Italy", 4},
                {"Germany", 4},
                {"Argentina", 2},
                {"Uruguay", 2},
                {"France", 1},
                {"Spain", 1},
                {"England", 1}
            };

            Console.Write(worldCup.ToMarkdownBarChart());
        }

        [TestMethod]
        public void BarChartWithNegativeAndFloatingPointValues()
        {
            const int valueCount = 20;
            var chart = new BarChart
            {
                ScaleAlways = true,
                MaximumChartWidth = 40,
                DataPoints = from i in Enumerable.Range(0, valueCount)
                    let rad = (i * 2.0 * Math.PI) / valueCount
                    select new BarChartDataPoint
                    {
                        CategoryName = string.Format("Cos({0:0.0})", rad),
                        Value = Math.Cos(rad)
                    }
            };

            chart.WriteToTrace();
        }

        [TestMethod]
        public void TableExample2()
        {
            var data = new[]
            {
                new {Name = "Meryl Streep", Nominations = 18, Awards = 3},
                new {Name = "Katharine Hepburn", Nominations = 12, Awards = 4},
                new {Name = "Jack Nicholson", Nominations = 12, Awards = 3}
            };

            Console.Write(data.ToMarkdownTable());

            var tableWithHeaders = data
                .ToMarkdownTable(i => i.Name, i => i.Nominations + i.Awards)
                .WithHeaders("Name", "Total");

            Console.Write(tableWithHeaders);
        }

        [TestMethod]
        public void HeaderExamples()
        {
            Console.Write("The Origin of the Species".ToMarkdownHeader());
            Console.Write("By Means of Natural Selection".ToMarkdownSubHeader());

			for (int headerLevel = 1; headerLevel <= 6; headerLevel++)
			{
				Console.Write(("This should be a header " + headerLevel).ToMarkdownHeader(headerLevel));
			}
        }

        [TestMethod]
        public void BlockquoteExample()
        {
            const string text = "There are only two hard things in computer science:\n" +
                                "cache invalidation,\n" +
                                "naming things,\n" +
                                "and off-by-one errors.";

            Console.Write(text.ToMarkdownBlockquote());
        }

        [TestMethod]
        public void ContainerExample()
        {
            var log = new MarkdownContainer();

            var countries = new[]{"Zimbabwe", "Italy", "Bolivia", "Finland", "Australia"};

            log.Append("Countries (unsorted)".ToMarkdownHeader());
            log.Append(countries.ToMarkdownNumberedList());

            var sorted = countries.OrderBy(i => i);

            log.Append("Countries (sorted)".ToMarkdownHeader());
            log.Append(sorted.ToMarkdownNumberedList());

            Console.Write(log);
        }
    }
}