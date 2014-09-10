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
                "    Backwards Activity |   ======  3 -> 9 (6)\r\n" +
                "                       ----------\r\n",
                "<pre><code>Backwards Activity |   ======  3 -&gt; 9 (6)\n" +
                "                   ----------\n" +
                "</code></pre>\n\n");
        }
        [TestMethod]
        public void TestCanPlotGanttChartWithNegativeActivityStart()
        {
            var chart = new GanttChart
            {
                Activities = new[]
                {
                    new GanttChartActivity {Name = "Negative Start", StartValue = -2, EndValue = 3}
                }
            };

            chart.AssertOutputEquals(
                "    Negative Start ==|===  -2 -> 3 (5)\r\n" +
                "                       ----------\r\n",
                "<pre><code>>Negative Start |===  -2 -&gt; 3 (5)\n" +
                "                   ----------\n" +
                "</code></pre>\n\n");
        }

    }
}