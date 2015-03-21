using MarkdownLog;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using NUnit.Framework;
using System;
namespace UnitTests.MarkdownLog
{
    [TestClass]
    public class GanttChartTests
    {
        [TestMethod]
        public void TestCanPlotGanttChart()
        {
            var chart = new GanttChart
            {
                Activities = new[]
                {
                    new GanttChartActivity {Name = "Boil Kettle", StartValue = 0, EndValue = 5},
                    new GanttChartActivity {Name = "Find Mugs", StartValue = 1, EndValue = 3},
                    new GanttChartActivity {Name = "Add Tea Bag", StartValue = 3, EndValue = 4},
                    new GanttChartActivity {Name = "Pour Water in Mug", StartValue = 5, EndValue = 6},
                    new GanttChartActivity {Name = "Add Milk", StartValue = 8, EndValue = 9},
                    new GanttChartActivity {Name = "Remove Tea Bag", StartValue = 9, EndValue = 10},
                    new GanttChartActivity {Name = "Drink Tea", StartValue = 15, EndValue = 25},

                }
            };

            chart.AssertOutputEquals(
                "    Boil Kettle       |=====                       0 ->  5  (5)" +Environment.NewLine+
                "    Find Mugs         | ==                         1 ->  3  (2)" +Environment.NewLine+
                "    Add Tea Bag       |   =                        3 ->  4  (1)" +Environment.NewLine+
                "    Pour Water in Mug |     =                      5 ->  6  (1)" +Environment.NewLine+
                "    Add Milk          |        =                   8 ->  9  (1)" +Environment.NewLine+
                "    Remove Tea Bag    |         =                  9 -> 10  (1)" +Environment.NewLine+
                "    Drink Tea         |               ==========  15 -> 25 (10)" +Environment.NewLine+
                "                      --------------------------"+Environment.NewLine
                ,
                "<pre><code>Boil Kettle       |=====                       0 -&gt;  5  (5)"+Environment.NewLine +
                "Find Mugs         | ==                         1 -&gt;  3  (2)"+Environment.NewLine +
                "Add Tea Bag       |   =                        3 -&gt;  4  (1)"+Environment.NewLine +
                "Pour Water in Mug |     =                      5 -&gt;  6  (1)"+Environment.NewLine +
                "Add Milk          |        =                   8 -&gt;  9  (1)"+Environment.NewLine +
                "Remove Tea Bag    |         =                  9 -&gt; 10  (1)"+Environment.NewLine +
                "Drink Tea         |               ==========  15 -&gt; 25 (10)"+Environment.NewLine +
                "                  --------------------------"+Environment.NewLine +
                "</code></pre>"+Environment.NewLine+Environment.NewLine);
        }

        [TestMethod]
        public void TestCanPlotEmptyGanttChart()
        {
            var chart = new GanttChart();

            chart.AssertOutputEquals("", "");
        }

        [TestMethod]
        public void TestCanPlotGanttChartWithBackwardsActivities()
        {
            var chart = new GanttChart
            {
                Activities = new[]
                {
                    new GanttChartActivity {Name = "Backwards Activity", StartValue = 9, EndValue = 3}
                }
            };

            chart.AssertOutputEquals(
                "    Backwards Activity |   ======  9 -> 3 (6)"+Environment.NewLine +
                "                       ----------"+Environment.NewLine,
                "<pre><code>Backwards Activity |   ======  9 -&gt; 3 (6)"+Environment.NewLine +
                "                   ----------" +Environment.NewLine+
                "</code></pre>"+Environment.NewLine+Environment.NewLine);
        }

        [TestMethod]
        public void TestGanttChartRangeGetOverlap()
        {
            Assert.AreEqual(4, new GanttChart.Range(0, 8).GetOverlap(new GanttChart.Range(3, 7)));
            Assert.AreEqual(1, new GanttChart.Range(0, 8).GetOverlap(new GanttChart.Range(7, 10)));
            Assert.AreEqual(0, new GanttChart.Range(0, 8).GetOverlap(new GanttChart.Range(8, 10)));
            Assert.AreEqual(0, new GanttChart.Range(0, 8).GetOverlap(new GanttChart.Range(9, 10)));
        }

        [TestMethod]
        public void TestGanttChartRangeGetStartOffset()
        {
            Assert.AreEqual(3, new GanttChart.Range(0, 8).GetStartOffset(new GanttChart.Range(3, 8)));
            Assert.AreEqual(0, new GanttChart.Range(0, 8).GetStartOffset(new GanttChart.Range(0, 4)));
            Assert.AreEqual(0, new GanttChart.Range(2, 8).GetStartOffset(new GanttChart.Range(0, 5)));
            Assert.AreEqual(1, new GanttChart.Range(2, 6).GetStartOffset(new GanttChart.Range(3, 8)));
            Assert.AreEqual(0, new GanttChart.Range(0, 8).GetStartOffset(new GanttChart.Range(9, 10)));
        }

        [TestMethod]
        public void TestGanttChartRangeGetEndOffset()
        {
            Assert.AreEqual(0, new GanttChart.Range(0, 8).GetEndOffset(new GanttChart.Range(3, 8)));
            Assert.AreEqual(4, new GanttChart.Range(0, 8).GetEndOffset(new GanttChart.Range(0, 4)));
            Assert.AreEqual(3, new GanttChart.Range(2, 8).GetEndOffset(new GanttChart.Range(0, 5)));
            Assert.AreEqual(0, new GanttChart.Range(2, 6).GetEndOffset(new GanttChart.Range(3, 8)));
            Assert.AreEqual(8, new GanttChart.Range(0, 8).GetEndOffset(new GanttChart.Range(9, 10)));
            Assert.AreEqual(3, new GanttChart.Range(0, 3).GetEndOffset(new GanttChart.Range(-5, -4)));
        }

        [TestMethod]
        public void TestCanPlotGanttChartWithNegativeActivities()
        {
            var chart = new GanttChart
            {
                Activities = new[]
                {
                    new GanttChartActivity {Name = "Both Negative", StartValue = -5, EndValue = -1},
                    new GanttChartActivity {Name = "Negative (zero length)", StartValue = -4, EndValue = -4},
                    new GanttChartActivity {Name = "Negative to Zero", StartValue = -4, EndValue = 0},
                    new GanttChartActivity {Name = "Negative to Positive", StartValue = -3, EndValue = 2},
                    new GanttChartActivity {Name = "Zero to Positive", StartValue = 0, EndValue = 3},
                    new GanttChartActivity {Name = "Zero to Zero", StartValue = 0, EndValue = 0},
                    new GanttChartActivity {Name = "Both Positive", StartValue = 1, EndValue = 3},
                    new GanttChartActivity {Name = "Both Negative (inverted)", StartValue = -1, EndValue = -5},
                    new GanttChartActivity {Name = "Zero to Negative", StartValue = 0, EndValue = -4},
                    new GanttChartActivity {Name = "Positive to Negative", StartValue = 2, EndValue = -3},
                    new GanttChartActivity {Name = "Positive to Zero", StartValue = 3, EndValue = 0},
                    new GanttChartActivity {Name = "Both Positive (inverted)", StartValue = 3, EndValue = 1}
                    
                }
            };

            chart.AssertOutputEquals(
                "    Both Negative            ==== |     -5 -> -1 (4)"+Environment.NewLine +
                "    Negative (zero length)    +   |     -4 -> -4 (0)"+Environment.NewLine +
                "    Negative to Zero          ====|     -4 ->  0 (4)"+Environment.NewLine +
                "    Negative to Positive       ===|==   -3 ->  2 (5)"+Environment.NewLine +
                "    Zero to Positive              |===   0 ->  3 (3)"+Environment.NewLine +
                "    Zero to Zero                  +      0 ->  0 (0)"+Environment.NewLine +
                "    Both Positive                 | ==   1 ->  3 (2)"+Environment.NewLine +
                "    Both Negative (inverted) ==== |     -1 -> -5 (4)"+Environment.NewLine +
                "    Zero to Negative          ====|      0 -> -4 (4)"+Environment.NewLine +
                "    Positive to Negative       ===|==    2 -> -3 (5)"+Environment.NewLine +
                "    Positive to Zero              |===   3 ->  0 (3)"+Environment.NewLine +
                "    Both Positive (inverted)      | ==   3 ->  1 (2)"+Environment.NewLine +
                "                             ---------"+Environment.NewLine,
                "<pre><code>Both Negative            ==== |     -5 -&gt; -1 (4)\n" +
                "Negative (zero length)    +   |     -4 -&gt; -4 (0)\n" +
                "Negative to Zero          ====|     -4 -&gt;  0 (4)\n" +
                "Negative to Positive       ===|==   -3 -&gt;  2 (5)\n" +
                "Zero to Positive              |===   0 -&gt;  3 (3)\n" +
                "Zero to Zero                  +      0 -&gt;  0 (0)\n" +
                "Both Positive                 | ==   1 -&gt;  3 (2)\n" +
                "Both Negative (inverted) ==== |     -1 -&gt; -5 (4)\n" +
                "Zero to Negative          ====|      0 -&gt; -4 (4)\n" +
                "Positive to Negative       ===|==    2 -&gt; -3 (5)\n" +
                "Positive to Zero              |===   3 -&gt;  0 (3)\n" +
                "Both Positive (inverted)      | ==   3 -&gt;  1 (2)\n" +
                "                         ---------\n" +
                "</code></pre>\n\n");
        }

        [TestMethod]
        public void TestCanPlotScaledGanttChart()
        {
            var chart = new GanttChart
            {
                MaximumChartWidth = 60,
                Activities = new[]
                {
                    new GanttChartActivity {Name = "Site Preparation", StartValue = 1, EndValue = 10},
                    new GanttChartActivity {Name = "Dig Trenches", StartValue = 12, EndValue = 16},
                    new GanttChartActivity {Name = "Drains", StartValue = 16, EndValue = 22},
                    new GanttChartActivity {Name = "Foundations", StartValue = 24, EndValue = 30},
                    new GanttChartActivity {Name = "Brickwork", StartValue = 30, EndValue = 54},
                    new GanttChartActivity {Name = "Roof Timber", StartValue = 54, EndValue = 60},
                    new GanttChartActivity {Name = "Partitions", StartValue = 54, EndValue = 60},
                    new GanttChartActivity {Name = "Frames", StartValue = 54, EndValue = 64},
                    new GanttChartActivity {Name = "Roof Tiling", StartValue = 60, EndValue = 72},
                    new GanttChartActivity {Name = "First Fix", StartValue = 72, EndValue = 82},
                    new GanttChartActivity {Name = "Glazing", StartValue = 60, EndValue = 66},
                    new GanttChartActivity {Name = "Plaster", StartValue = 68, EndValue = 80},
                    new GanttChartActivity {Name = "Second Fix", StartValue = 80, EndValue = 92},
                    new GanttChartActivity {Name = "Interior Finish", StartValue = 88, EndValue = 100},
                    new GanttChartActivity {Name = "Cleanup", StartValue = 100, EndValue = 108}
                }
            };

            chart.AssertOutputEquals(
                "    Site Preparation | =====                                                          1 ->  10  (9)"+Environment.NewLine +
                "    Dig Trenches     |       ==                                                      12 ->  16  (4)"+Environment.NewLine +  
                "    Drains           |         ===                                                   16 ->  22  (6)"+Environment.NewLine +
                "    Foundations      |             ====                                              24 ->  30  (6)"+Environment.NewLine +
                "    Brickwork        |                 =============                                 30 ->  54 (24)"+Environment.NewLine +
                "    Roof Timber      |                              ===                              54 ->  60  (6)"+Environment.NewLine +
                "    Partitions       |                              ===                              54 ->  60  (6)"+Environment.NewLine +
                "    Frames           |                              ======                           54 ->  64 (10)"+Environment.NewLine +
                "    Roof Tiling      |                                 =======                       60 ->  72 (12)"+Environment.NewLine +
                "    First Fix        |                                        ======                 72 ->  82 (10)"+Environment.NewLine +
                "    Glazing          |                                 ====                          60 ->  66  (6)"+Environment.NewLine +
                "    Plaster          |                                      ======                   68 ->  80 (12)"+Environment.NewLine +
                "    Second Fix       |                                            =======            80 ->  92 (12)"+Environment.NewLine +
                "    Interior Finish  |                                                 =======       88 -> 100 (12)"+Environment.NewLine +
                "    Cleanup          |                                                        ====  100 -> 108  (8)"+Environment.NewLine +
                "                     -------------------------------------------------------------"+Environment.NewLine,
                "<pre><code>Site Preparation | =====                                                          1 -&gt;  10  (9)\n" +
                "Dig Trenches     |       ==                                                      12 -&gt;  16  (4)\n" +
                "Drains           |         ===                                                   16 -&gt;  22  (6)\n" +
                "Foundations      |             ====                                              24 -&gt;  30  (6)\n" +
                "Brickwork        |                 =============                                 30 -&gt;  54 (24)\n" +
                "Roof Timber      |                              ===                              54 -&gt;  60  (6)\n" +
                "Partitions       |                              ===                              54 -&gt;  60  (6)\n" +
                "Frames           |                              ======                           54 -&gt;  64 (10)\n" +
                "Roof Tiling      |                                 =======                       60 -&gt;  72 (12)\n" +
                "First Fix        |                                        ======                 72 -&gt;  82 (10)\n" +
                "Glazing          |                                 ====                          60 -&gt;  66  (6)\n" +
                "Plaster          |                                      ======                   68 -&gt;  80 (12)\n" +
                "Second Fix       |                                            =======            80 -&gt;  92 (12)\n" +
                "Interior Finish  |                                                 =======       88 -&gt; 100 (12)\n" +
                "Cleanup          |                                                        ====  100 -&gt; 108  (8)\n" +
                "                 -------------------------------------------------------------\n" +
                "</code></pre>\n\n"

                );
        }

        [TestMethod]
        public void TestDecimalValuesAreRounded()
        {
            var chart = new GanttChart
            {
                MaximumChartWidth = 60,
                Activities = new[]
                {
                    new GanttChartActivity {Name = "Fetch HTML", StartValue = 0, EndValue = 1.2},
                    new GanttChartActivity {Name = "Fetch Images", StartValue = 0, EndValue = 1.8},
                    new GanttChartActivity {Name = "Render Page", StartValue = 1.2, EndValue = 3.6},
                }
            };

            chart.AssertOutputEquals(
                "    Fetch HTML   |=      0 -> 1.2 (1.2)"+Environment.NewLine +
                "    Fetch Images |==     0 -> 1.8 (1.8)"+Environment.NewLine +
                "    Render Page  | ==  1.2 -> 3.6 (2.4)"+Environment.NewLine +
                "                 ----"+Environment.NewLine,
                "<pre><code>Fetch HTML   |=      0 -&gt; 1.2 (1.2)\n" +
                "Fetch Images |==     0 -&gt; 1.8 (1.8)\n" +
                "Render Page  | ==  1.2 -&gt; 3.6 (2.4)\n" +
                "             ----\n" +
                "</code></pre>\n\n"
                );
        }

        [TestMethod]
        public void TestZeroLengthActivitiesArePlotted()
        {
            var chart = new GanttChart
            {
                MaximumChartWidth = 60,
                Activities = new[]
                {
                    new GanttChartActivity {Name = "v1.0 Release Date", StartValue = -10, EndValue = -10},
                    new GanttChartActivity {Name = "Today", StartValue = 0, EndValue = 0},
                    new GanttChartActivity {Name = "Feasibility Study", StartValue = 0, EndValue = 5},
                    new GanttChartActivity {Name = "Requirements Gathering", StartValue = 4, EndValue = 10},
                    new GanttChartActivity {Name = "Development", StartValue = 8, EndValue = 15},
                    new GanttChartActivity {Name = "Testing", StartValue = 11, EndValue = 18},
                    new GanttChartActivity {Name = "v2.0 Release Date", StartValue = 20, EndValue = 20},
                }
            };

            chart.AssertOutputEquals(
                "    v1.0 Release Date      +         |                      -10 -> -10 (0)"+Environment.NewLine +
                "    Today                            +                        0 ->   0 (0)"+Environment.NewLine +
                "    Feasibility Study                |=====                   0 ->   5 (5)"+Environment.NewLine +
                "    Requirements Gathering           |    ======              4 ->  10 (6)"+Environment.NewLine +
                "    Development                      |        =======         8 ->  15 (7)"+Environment.NewLine +
                "    Testing                          |           =======     11 ->  18 (7)"+Environment.NewLine +
                "    v2.0 Release Date                |                   +   20 ->  20 (0)"+Environment.NewLine +
                "                           -------------------------------"+Environment.NewLine,
                "<pre><code>v1.0 Release Date      +         |                      -10 -&gt; -10 (0)\n" +
                "Today                            +                        0 -&gt;   0 (0)\n" +
                "Feasibility Study                |=====                   0 -&gt;   5 (5)\n" +
                "Requirements Gathering           |    ======              4 -&gt;  10 (6)\n" +
                "Development                      |        =======         8 -&gt;  15 (7)\n" +
                "Testing                          |           =======     11 -&gt;  18 (7)\n" +
                "v2.0 Release Date                |                   +   20 -&gt;  20 (0)\n" +
                "                       -------------------------------\n" +
                "</code></pre>\n\n"
                );
        }

    }
}