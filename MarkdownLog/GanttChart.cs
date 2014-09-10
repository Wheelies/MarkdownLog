#region Copyright and license
// /*
// The MIT License (MIT)
// 
// Copyright (c) 2014 BlackJet Software Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownLog
{
    public class GanttChart : MarkdownElement
    {
        private IEnumerable<GanttChartActivity> _activities = new List<GanttChartActivity>();
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

        public IEnumerable<GanttChartActivity> Activities
        {
            get { return _activities; }
            set { _activities = value ?? Enumerable.Empty<GanttChartActivity>(); }
        }

        public override string ToMarkdown()
        {
            if (!_activities.Any()) return "";

            var builder = new StringBuilder();
            var indent = new string(' ', 4);

            const double overallMinValue = 0f;
            double overallMaxValue = Math.Max(_activities.Max(i => i.StartValue), _activities.Max(i => i.EndValue));

            var width = Math.Max(Math.Min(overallMaxValue - overallMinValue, _maximumChartWidth), ScaleAlways || overallMaxValue < 1 ? _maximumChartWidth : 0);
            var unitLength = width / (overallMaxValue - overallMinValue);

            var longestActivityName = _activities.Max(i => i.Name.EscapeCSharpString().Length);
            var longestFormattedStartValue = _activities.Max(i => FormatValue(GetMinValue(i)).Length);
            var longestFormattedEndValue = _activities.Max(i => FormatValue(GetMaxValue(i)).Length);
            var longestFormattedLengthValue = _activities.Max(i => string.Format("({0})", FormatValue(GetMaxValue(i) - GetMinValue(i))).Length);

            var maxEndColumn = (int) (overallMaxValue*unitLength);

            foreach (var dataPoint in _activities)
            {
                var minValue = GetMinValue(dataPoint);
                var maxValue = GetMaxValue(dataPoint);

                var startColumn = GetColumnNumber(minValue, unitLength);
                var endColumn = GetColumnNumber(maxValue, unitLength);

                var barLength = endColumn - startColumn;

                builder.Append(indent);
                builder.Append(dataPoint.Name.EscapeCSharpString().PadRight(longestActivityName));

                builder.Append(" ");

                builder.Append("|");
                builder.Append(new string(' ', startColumn));
                builder.Append(new string('=', barLength));
                builder.Append("  ");
                builder.Append(new string(' ', maxEndColumn - endColumn));
                builder.Append(FormatValue(minValue).PadLeft(longestFormattedStartValue));
                builder.Append(" -> ");
                builder.Append(FormatValue(maxValue).PadLeft(longestFormattedEndValue));
                builder.Append(" ");
                builder.Append(string.Format("({0})", FormatValue(maxValue - minValue)).PadLeft(longestFormattedLengthValue));
                builder.AppendLine();
            }

            builder.Append(indent);
            builder.Append(new string(' ', longestActivityName + 1));
            
            builder.Append(new string('-', 1 + maxEndColumn));
            builder.AppendLine();
            
            return builder.ToString();
        }

        private static double GetMinValue(GanttChartActivity activity)
        {
            return Math.Min(activity.StartValue, activity.EndValue);
        }

        private static double GetMaxValue(GanttChartActivity activity)
        {
            return Math.Max(activity.StartValue, activity.EndValue);
        }

        private string FormatValue(double value)
        {
            var format = "{0:0." + new string('#', _maximumDecimalPlaces) + "}";
            return string.Format(format, value);
        }

        private static int GetColumnNumber(double value, double unitLength)
        {
            return (int) Math.Max(0, Math.Round(unitLength*value));
        }
    }
}