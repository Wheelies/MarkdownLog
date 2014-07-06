using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownLog
{
    public class BarChart : IMarkdownElement
    {
        private IEnumerable<BarChartDataPoint> _dataPoints = new List<BarChartDataPoint>();
        private int _maximumChartWidth = 80;
        private int _maximumDecimalPlaces = 2;

        public bool ScaleAlways { get; set; }

        public int MaximumChartWidth
        {
            get { return _maximumChartWidth; }
            set { _maximumChartWidth = Math.Max(1, value); }
        }

        public int MaximumDecimalPlaces
        {
            get { return _maximumDecimalPlaces; }
            set { _maximumDecimalPlaces = Math.Max(0, value); }
        }

        public IEnumerable<BarChartDataPoint> DataPoints
        {
            get { return _dataPoints; }
            set { _dataPoints = value ?? Enumerable.Empty<BarChartDataPoint>(); }
        }

        public string ToMarkdown()
        {
            if (!_dataPoints.Any()) return "";

            var builder = new StringBuilder();
            var indent = new string(' ', 4);

            var maxValue = _dataPoints.Max(i => i.Value);
            var minValue = Math.Min(0, _dataPoints.Min(i => i.Value));

            var width = Math.Max(Math.Min(maxValue - minValue, _maximumChartWidth), ScaleAlways || maxValue < 1 ? _maximumChartWidth : 0);
            var unitLength = width / (maxValue - minValue);

            var longestCategoryName = _dataPoints.Max(i => i.CategoryName.EscapeCSharpString().Length);
            
            var longestNegativeBar = GetLongestNegativeBar(unitLength);
            var longestPositiveBar = GetLongestPositiveBar(unitLength);

            foreach (var dataPoint in _dataPoints)
            {
                var barLength = GetBarLength(dataPoint, unitLength);
                builder.Append(indent);
                builder.Append(dataPoint.CategoryName.EscapeCSharpString().PadRight(longestCategoryName));

                builder.Append(" ");

                builder.Append(new string('#', Math.Max(0, -barLength)).PadLeft(-longestNegativeBar));
                builder.Append("|");
                builder.Append(new string('#', Math.Max(0, barLength)));
                builder.Append("  ");
                builder.Append(FormatValue(dataPoint.Value));
                builder.AppendLine();
            }

            builder.Append(indent);
            builder.Append(new string(' ', longestCategoryName + 1));
            builder.Append(new string('-', -longestNegativeBar + 1 + longestPositiveBar));
            builder.AppendLine();
            
            return builder.ToString();
        }

        private int GetLongestPositiveBar(double unitLength)
        {
            var positiveValues = _dataPoints.Where(i => i.Value > 0).ToList();
            return positiveValues.Any() ? positiveValues.Max(i => GetBarLength(i, unitLength)) : 0;
        }

        private int GetLongestNegativeBar(double unitLength)
        {
            var negativeValues = _dataPoints.Where(i => i.Value < 0).ToList();
            var longestNegativeBar = negativeValues.Any() ? negativeValues.Min(i => GetBarLength(i, unitLength)) : 0;
            return longestNegativeBar;
        }

        private string FormatValue(double value)
        {
            var format = "{0:0." + new string('#', _maximumDecimalPlaces) + "}";
            return string.Format(format, value);
        }

        private static int GetBarLength(BarChartDataPoint dataPoint, double unitLength)
        {
            return (int) Math.Round(unitLength * dataPoint.Value);
        }
    }
}