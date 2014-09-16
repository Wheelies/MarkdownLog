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
        private const string ZeroLengthActivityIndicator = "+";
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

        internal class Range
        {
            private static readonly Range NullRange = new Range(Double.NaN, Double.NaN);

            private readonly double _start;
            private readonly double _end;

            internal Range(double start, double end)
            {
                _start = Math.Min(start, end);
                _end = Math.Max(start, end);
            }

            public double Start
            {
                get { return _start; }
            }

            public double End
            {
                get { return _end; }
            }

            public double Length
            {
                get { return _end - _start; }
            }

            public Range Scale(double unitLength)
            {
                return new Range(Scale(_start, unitLength), Scale(_end, unitLength));
            }

            private static int Scale(double value, double unitLength)
            {
                return (int)Math.Round(unitLength * value);
            }

            private Range GetIntersect(Range range)
            {
                if (End < range.Start || range.End < Start)
                    return NullRange;

                var max = Math.Min(End, range.End);
                var min = Math.Max(Start, range.Start);

                return new Range(min, max);
            }

            public double GetOverlap(Range range)
            {
                var intersect = GetIntersect(range);

                return (intersect == NullRange) ? 0 : intersect.Length;
            }

            public double GetStartOffset(Range range)
            {
                var intersect = GetIntersect(range);
                return intersect == NullRange ? 0 : intersect.Start - Start;
            }

            public double GetEndOffset(Range range)
            {
                var intersect = GetIntersect(range);
                return intersect == NullRange ? Length : End - intersect.End;
            }

            public override string ToString()
            {
                return string.Format("{0} -> {1}", _start, _end);
            }
        }

        private class PreparedActivity
        {
            public string Name { get; set; }
            public string StartText { get; set; }
            public string EndText { get; set; }
            public string LengthText { get; set; }
            public Range Range { get; set; }
        }

        public override string ToMarkdown()
        {
            if (!_activities.Any()) return "";

            var activities = (from i in _activities
                let range = new Range(i.StartValue, i.EndValue)
                select new PreparedActivity
                {
                    Name = i.Name.EscapeCSharpString(),
                    StartText = FormatValue(i.StartValue),
                    EndText = FormatValue(i.EndValue),
                    LengthText = string.Format("({0})", FormatValue(range.End - range.Start)),
                    Range = range,
                }).ToList();

            var builder = new StringBuilder();
            var indent = new string(' ', 4);

            var maxEnd = activities.Max(i => i.Range.End);
            var minStart = activities.Min(i => i.Range.Start);
            var maxActivityRangeWidth = ( maxEnd - minStart ) + "|".Length;

            var minimumChartWidth = ScaleAlways || maxActivityRangeWidth < 1 ? _maximumChartWidth : 0;
            var chartWidth = (int)Math.Max(Math.Min(maxActivityRangeWidth, _maximumChartWidth), minimumChartWidth);
            var unitLength = chartWidth / maxActivityRangeWidth;

            var longestName = activities.Max(i => i.Name.Length);
            var longestStartText = activities.Max(i => i.StartText.Length);
            var longestEndText = activities.Max(i => i.EndText.Length);
            var longestLengthText = activities.Max(i => i.LengthText.Length);

            var scaledNegativeRange = new Range(Math.Min(0, minStart), 0).Scale(unitLength);
            var scaledPositiveRange = new Range(0, Math.Max(0, maxEnd)).Scale(unitLength);

            foreach (var activity in activities)
            {
                builder.Append(indent);
                builder.Append(activity.Name.PadRight(longestName));

                builder.Append(" ");

                var scaledRange = activity.Range.Scale(unitLength);
                bool isZeroLengthActivity = scaledRange.Length < unitLength;
                bool isZeroLengthNegative = isZeroLengthActivity && scaledRange.Start < 0;
                

                builder.Append(Spaces(scaledNegativeRange.GetStartOffset(scaledRange)));
                builder.Append(isZeroLengthNegative ? ZeroLengthActivityIndicator : Bar(scaledNegativeRange.GetOverlap(scaledRange)));
                
                double spacesAfterNegative = scaledNegativeRange.GetEndOffset(scaledRange);
                if (isZeroLengthNegative) spacesAfterNegative--;

                builder.Append(Spaces(spacesAfterNegative));
                builder.Append(isZeroLengthActivity && Math.Abs(scaledRange.Start) < unitLength ? ZeroLengthActivityIndicator : "|");

                bool isZeroLengthPositive = isZeroLengthActivity && scaledRange.Start > 0;
                double spacesBeforePositive = scaledPositiveRange.GetStartOffset(scaledRange);
                if (isZeroLengthPositive) spacesBeforePositive--;

                builder.Append(Spaces(spacesBeforePositive));

                builder.Append(isZeroLengthPositive ? ZeroLengthActivityIndicator : Bar(scaledPositiveRange.GetOverlap(scaledRange)));
                builder.Append(Spaces(scaledPositiveRange.GetEndOffset(scaledRange)));
                builder.Append(Spaces(2));

                builder.Append(activity.StartText.PadLeft(longestStartText));
                builder.Append(" -> ");
                builder.Append(activity.EndText.PadLeft(longestEndText));
                builder.Append(" ");
                builder.Append(activity.LengthText.PadLeft(longestLengthText));
                builder.AppendLine();
            }

            builder.Append(indent);
            builder.Append(new string(' ', longestName + 1));

            builder.Append(new string('-', (int) Math.Round(scaledNegativeRange.Length + 1 + scaledPositiveRange.Length)));
            builder.AppendLine();
            
            return builder.ToString();
        }

        private string Spaces(double count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", string.Format("Cannot be negative (value was {0})", count));

            return new string(' ', (int) Math.Round(count));
        }

        private string Bar(double length)
        {
            return new string('=', (int)Math.Round(length));
        }

        private string FormatValue(double value)
        {
            var format = "{0:0." + new string('#', _maximumDecimalPlaces) + "}";
            return string.Format(format, value);
        }
    }
}