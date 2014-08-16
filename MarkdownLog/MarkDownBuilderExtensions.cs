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
using System.Reflection;

namespace MarkdownLog
{
    public static class MarkDownBuilderExtensions
    {
        public static Header ToMarkdownHeader(this string text)
        {
            return new Header(text);
        }

        public static SubHeader ToMarkdownSubHeader(this string text)
        {
            return new SubHeader(text);
        }

        public static Paragraph ToMarkdownParagraph(this string text)
        {
            return new Paragraph(text);
        }

        public static Blockquote ToMarkdownBlockquote(this string text)
        {
            var blockquote = new Blockquote();
            blockquote.Append(text);
            return blockquote;
        }

        public static Blockquote ToMarkdownBlockquote(this IMarkdownElement element)
        {
            var blockquote = new Blockquote();
            blockquote.Append(element);
            return blockquote;
        }

        public static CodeBlock ToMarkdownCodeBlock(this string code)
        {
            return new CodeBlock(code);
        }


        public static NumberedList ToMarkdownNumberedList<T>(this IEnumerable<T> items)
        {
            return items.ToMarkdownNumberedList(i => i.ToString());
        }

        public static NumberedList ToMarkdownNumberedList<T>(this IEnumerable<T> items, Func<T,string> toText)
        {
            return new NumberedList(items.Select(toText));
        }

        public static BulletedList ToMarkdownBulletedList<T>(this IEnumerable<T> items)
        {
            return items.ToMarkdownBulletedList(i => i.ToString());
        }

        public static BulletedList ToMarkdownBulletedList<T>(this IEnumerable<T> items, Func<T, string> toText)
        {
            return new BulletedList(items.Select(toText));
        }

        public static BarChart ToMarkdownBarChart(this IEnumerable<KeyValuePair<string, int>> series)
        {
            return ToMarkdownBarChart(series, i => i.Key, i => i.Value);
        }

        public static BarChart ToMarkdownBarChart(this IEnumerable<KeyValuePair<string, float>> series)
        {
            return ToMarkdownBarChart(series, i => i.Key, i => i.Value);
        }

        public static BarChart ToMarkdownBarChart(this IEnumerable<KeyValuePair<string, double>> series)
        {
            return ToMarkdownBarChart(series, i => i.Key, i => i.Value);
        }

        public static BarChart ToMarkdownBarChart(this IEnumerable<Tuple<string, int>> series)
        {
            return ToMarkdownBarChart(series, i => i.Item1, i => i.Item2);
        }

        public static BarChart ToMarkdownBarChart(this IEnumerable<Tuple<string, float>> series)
        {
            return ToMarkdownBarChart(series, i => i.Item1, i => i.Item2);
        }

        public static BarChart ToMarkdownBarChart(this IEnumerable<Tuple<string, double>> series)
        {
            return ToMarkdownBarChart(series, i => i.Item1, i => i.Item2);
        }

        public static BarChart ToMarkdownBarChart<T>(this IEnumerable<T> dataPoints, Func<T, string> getCategoryName, Func<T, double> getValue)
        {
            return new BarChart {DataPoints = dataPoints.Select(i => new BarChartDataPoint {CategoryName = getCategoryName(i), Value = getValue(i)})};
        }

        public static IDictionary<string,object> ToPropertyValues<T>(this T obj) 
        {
            var properties = typeof(T).GetProperties().ToList();
            return properties.ToDictionary(i => i.Name, i => i.GetValue(obj, null));
        }

        public static Table ToMarkdownTable<T>(this IEnumerable<T> rows)
        {
            var properties = typeof (T).GetProperties().ToList();

            return ToMarkdownTable(rows, properties.Select(property => (Func<T, object>) (r => r.GetFormattedValue(property))).ToArray())
                .WithHeaders(properties.Select(i=>i.Name).ToArray());
        }

        private static string GetFormattedValue<T>(this T obj, PropertyInfo property)
        {
            try
            {
                var value = property.GetValue(obj, null);
                
                if (value == null)
                    return "";
                
                if(value.GetType().IsWholeNumber())
                    return value.ToString();
                
                if (value is float)
                    return ((float)value).ToString("0.00");

                if (value is double)
                    return ((double)value).ToString("0.00");

                if (value is decimal)
                    return ((decimal)value).ToString("0.00");

                if (value is DateTime)
                    return ((DateTime)value).ToString("r");

                return value.ToString();
            }
            catch (Exception exception)
            {
                return "{" +exception.GetType().Name + "}";
            }
        }


        public static Table WithHeaders(this Table table, params string[] titles)
        {
            var newColumns = new List<TableColumn>();
            for (int i = 0; i < titles.Length; i++)
            {
                var newTitle = titles[i];
                var column = table.Columns.ElementAtOrDefault(i) ?? new TableColumn();
                column.HeaderCell = new TableCell{Text = newTitle};
                newColumns.Add(column);
            }

            table.Columns = newColumns;
            return table;
        }

        public static Table ToMarkdownTable<T>(this IEnumerable<T> rows, params Func<T, object>[] getCellValueFuncs)
        {
            var columnCount = getCellValueFuncs.Count();
            List<List<object>> rowValues = rows.Select(r => getCellValueFuncs.Select(i => i(r)).ToList()).ToList();

            return new Table
            {
                Columns = Enumerable.Range(0, columnCount).Select(i => new TableColumn {Alignment = Alignment(rowValues, i)}),
                Rows = rowValues.Select(r => new TableRow {Cells = Enumerable.Range(0, columnCount).Select(i => new TableCell {Text = r[i].ToString()})})
            };
        }

        private static TableColumnAlignment Alignment(IEnumerable<List<object>> rowValues, int columnIndex)
        {
            return EntireColumnIsNumeric(rowValues, columnIndex) ? TableColumnAlignment.Right : TableColumnAlignment.Unspecified;
        }

        private static bool EntireColumnIsNumeric(IEnumerable<List<object>> rowValues, int column)
        {
            return rowValues.All(r => IsNumeric(r[column]));
        }

        private static bool IsNumeric(object obj)
        {
            decimal decimalValue;

            var isNumeric = obj.GetType().IsNumeric();
            var isBlank = obj.ToString().Trim() == "";
            var isDecimal = decimal.TryParse(obj.ToString(), out decimalValue);
            
            return isNumeric || isBlank || isDecimal;
        }

        public static Table ToMarkdownTable<T>(this IEnumerable<T> rows, params Func<T, string>[] getCellValueFuncs)
        {
            return new Table {Rows = rows.Select(row => new TableRow {Cells = getCellValueFuncs.Select(i => new TableCell {Text = i(row)})})};
        }
    }
}
