using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownLog;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;

namespace UnitTests.MarkdownLog
{
    [TestClass]
    public class BarChartTests
    {
        [TestMethod]
        public void TestCanPlotBarChart()
        {
            var chart = new BarChart
            {
                DataPoints = new[]
                {
                    new BarChartDataPoint {CategoryName = "Brazil", Value = 5},
                    new BarChartDataPoint {CategoryName = "Italy", Value = 4},
                    new BarChartDataPoint {CategoryName = "Germany", Value = 3},
                    new BarChartDataPoint {CategoryName = "Argentina", Value = 2},
                    new BarChartDataPoint {CategoryName = "Uruguay", Value = 2},
                    new BarChartDataPoint {CategoryName = "France", Value = 1},
                    new BarChartDataPoint {CategoryName = "Spain", Value = 1},
                    new BarChartDataPoint {CategoryName = "England", Value = 1}

                }
            };

            chart.AssertOutputEquals(
                "    Brazil    |#####  5"+Environment.NewLine +
                "    Italy     |####  4"+Environment.NewLine +
                "    Germany   |###  3"+Environment.NewLine +
                "    Argentina |##  2" +Environment.NewLine+
                "    Uruguay   |##  2" +Environment.NewLine+
                "    France    |#  1" +Environment.NewLine+
                "    Spain     |#  1" +Environment.NewLine+
                "    England   |#  1" +Environment.NewLine+
                "              ------"+Environment.NewLine
                ,
                "<pre><code>Brazil    |#####  5\n" +
                "Italy     |####  4\n" +
                "Germany   |###  3\n" +
                "Argentina |##  2\n" +
                "Uruguay   |##  2\n" +
                "France    |#  1\n" +
                "Spain     |#  1\n" +
                "England   |#  1\n" +
                "          ------\n" +
                "</code></pre>\n\n");
        }

        [TestMethod]
        public void TestCanPlotBarChartFromDictionary()
        {
            var data = new Dictionary<string, double>
            {

                {"Russia", 17.1},
                {"Antartica", 14.0},
                {"Canada", 10.0},
                {"China", 9.7},
                {"United States", 9.6}
            };

            data.ToMarkdownBarChart().WriteToTrace();
        }

        [TestMethod]
        public void TestDecimalValuesAreRoundedToNearestWhole()
        {
            var chart = new BarChart
            {
                DataPoints = new[]
                {
                    new BarChartDataPoint {CategoryName = "United States", Value = 16.2},
                    new BarChartDataPoint {CategoryName = "China", Value = 8.4},
                    new BarChartDataPoint {CategoryName = "Japan", Value = 6},
                    new BarChartDataPoint {CategoryName = "Germany", Value = 3.4},
                    new BarChartDataPoint {CategoryName = "France", Value = 2.6},
                    new BarChartDataPoint {CategoryName = "United Kingdom", Value = 2.5}

                }
            };

            chart.WriteToTrace();
        }

        [TestMethod]
        public void TestVeryLargeValuesAreScaled()
        {
            var chart = new BarChart
            {
                DataPoints = new[]
                {
                    new BarChartDataPoint {CategoryName = "China", Value = 1364},
                    new BarChartDataPoint {CategoryName = "India", Value = 1244},
                    new BarChartDataPoint {CategoryName = "United States", Value = 318},
                    new BarChartDataPoint {CategoryName = "Indonesia", Value = 247},
                    new BarChartDataPoint {CategoryName = "Brazil", Value = 203}
                }
            };

            chart.WriteToTrace();
        }

        [TestMethod]
        public void TestVerySmallValuesAreScaled()
        {
            var chart = new BarChart
            {
                MaximumDecimalPlaces = 8,
                DataPoints = new[]
                {
                    new BarChartDataPoint {CategoryName = "Length of a mosquito", Value = 0.015},
                    new BarChartDataPoint {CategoryName = "Length of a red ant ", Value = 0.005},
                    new BarChartDataPoint {CategoryName = "Human Hair thickness", Value = 0.0001},
                    new BarChartDataPoint {CategoryName = "Length of red blood cell", Value = 0.000008}
                }
            };

            chart.WriteToTrace();
        }

        [TestMethod]
        public void TestNegativeValuesCanBePlotted()
        {
            var chart = new BarChart
            {
                DataPoints = new[]
                {
                    new BarChartDataPoint {CategoryName = "United States", Value = -17.3},
                    new BarChartDataPoint {CategoryName = "United Kingdom ", Value = -10.1},
                    new BarChartDataPoint {CategoryName = "Germany", Value = -5.7},
                    new BarChartDataPoint {CategoryName = "France", Value = -5.3},
                    new BarChartDataPoint {CategoryName = "Japan", Value = -3}
                }
            };

            chart.WriteToTrace();
        }


        [TestMethod]
        public void TestChartWidthCanBeRestricted()
        {
            var chart = new BarChart
            {
                MaximumChartWidth = 10,
                DataPoints = new[]
                {
                    new BarChartDataPoint {CategoryName = "Elvis Presley", Value = 21},
                    new BarChartDataPoint {CategoryName = "The Beatles ", Value = 17},
                    new BarChartDataPoint {CategoryName = "Westlife", Value = 14},
                    new BarChartDataPoint {CategoryName = "Cliff Richard", Value = 14},
                    new BarChartDataPoint {CategoryName = "Madonna", Value = 13}
                }
            };

            chart.WriteToTrace();
        }

        [TestMethod]
        public void TestScalingCanBeSpecified()
        {
            const int valueCount = 20;
            var chart = new BarChart
            {
                ScaleAlways = true,
                MaximumChartWidth = 40,
                DataPoints = from i in Enumerable.Range(0, valueCount)
                    let rad = (i*2.0*Math.PI)/valueCount
                    select new BarChartDataPoint
                    {
                        CategoryName = string.Format("Cos({0:0.0})", rad),
                        Value = Math.Cos(rad)
                    }
            };

            chart.WriteToTrace();
        }

        [TestMethod]
        public void TestCanPlotBarChartWithNoData()
        {
            var chart = new BarChart();
            chart.WriteToTrace();
        }

        [TestMethod]
        public void TestSpecialCharactersInCategoryNameAreEscaped()
        {
            var chart = new BarChart{DataPoints = new[]
            {
                new BarChartDataPoint{CategoryName = "\tLine1\rLine2", Value = 10}
            }};

            chart.AssertOutputEquals(
                "    \\tLine1\\rLine2 |##########  10"+Environment.NewLine +
                "                   -----------"+Environment.NewLine,
                "<pre><code>\\tLine1\\rLine2 |##########  10\n" +
                "               -----------\n" +
                "</code></pre>\n\n");
        }

    }
}
