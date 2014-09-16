using MarkdownLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                "    Boil Kettle       |=====                       0 ->  5  (5)\r\n" +
                "    Find Mugs         | ==                         1 ->  3  (2)\r\n" +
                "    Add Tea Bag       |   =                        3 ->  4  (1)\r\n" +
                "    Pour Water in Mug |     =                      5 ->  6  (1)\r\n" +
                "    Add Milk          |        =                   8 ->  9  (1)\r\n" +
                "    Remove Tea Bag    |         =                  9 -> 10  (1)\r\n" +
                "    Drink Tea         |               ==========  15 -> 25 (10)\r\n" +
                "                      --------------------------\r\n"
                ,
                "<pre><code>Boil Kettle       |=====                       0 -&gt;  5  (5)\n" +
                "Find Mugs         | ==                         1 -&gt;  3  (2)\n" +
                "Add Tea Bag       |   =                        3 -&gt;  4  (1)\n" +
                "Pour Water in Mug |     =                      5 -&gt;  6  (1)\n" +
                "Add Milk          |        =                   8 -&gt;  9  (1)\n" +
                "Remove Tea Bag    |         =                  9 -&gt; 10  (1)\n" +
                "Drink Tea         |               ==========  15 -&gt; 25 (10)\n" +
                "                  --------------------------\n" +
                "</code></pre>\n\n");
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
                "    Backwards Activity |   ======  9 -> 3 (6)\r\n" +
                "                       ----------\r\n",
                "<pre><code>Backwards Activity |   ======  9 -&gt; 3 (6)\n" +
                "                   ----------\n" +
                "</code></pre>\n\n");
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
                "    Both Negative            ==== |     -5 -> -1 (4)\r\n" +
                "    Negative (zero length)        |     -4 -> -4 (0)\r\n" +
                "    Negative to Zero          ====|     -4 ->  0 (4)\r\n" +
                "    Negative to Positive       ===|==   -3 ->  2 (5)\r\n" +
                "    Zero to Positive              |===   0 ->  3 (3)\r\n" +
                "    Zero to Zero                  |      0 ->  0 (0)\r\n" +
                "    Both Positive                 | ==   1 ->  3 (2)\r\n" +
                "    Both Negative (inverted) ==== |     -1 -> -5 (4)\r\n" +
                "    Zero to Negative          ====|      0 -> -4 (4)\r\n" +
                "    Positive to Negative       ===|==    2 -> -3 (5)\r\n" +
                "    Positive to Zero              |===   3 ->  0 (3)\r\n" +
                "    Both Positive (inverted)      | ==   3 ->  1 (2)\r\n" +
                "                             ---------\r\n",
                "<pre><code>Both Negative            ==== |     -5 -&gt; -1 (4)\n" +
                "Negative (zero length)        |     -4 -&gt; -4 (0)\n" +
                "Negative to Zero          ====|     -4 -&gt;  0 (4)\n" +
                "Negative to Positive       ===|==   -3 -&gt;  2 (5)\n" +
                "Zero to Positive              |===   0 -&gt;  3 (3)\n" +
                "Zero to Zero                  |      0 -&gt;  0 (0)\n" +
                "Both Positive                 | ==   1 -&gt;  3 (2)\n" +
                "Both Negative (inverted) ==== |     -1 -&gt; -5 (4)\n" +
                "Zero to Negative          ====|      0 -&gt; -4 (4)\n" +
                "Positive to Negative       ===|==    2 -&gt; -3 (5)\n" +
                "Positive to Zero              |===   3 -&gt;  0 (3)\n" +
                "Both Positive (inverted)      | ==   3 -&gt;  1 (2)\n" +
                "                         ---------\n" +
                "</code></pre>\n\n");
        }

    }
}