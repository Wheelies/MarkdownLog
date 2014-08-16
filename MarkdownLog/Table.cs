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
    public class Table : MarkdownElement
    {
        private static readonly EmptyTableCell EmptyCell = new EmptyTableCell();

        private IEnumerable<TableRow> _rows = new List<TableRow>();
        private IEnumerable<TableColumn> _columns = new List<TableColumn>();

        public IEnumerable<TableRow> Rows
        {
            get { return _rows; }
            set { _rows = value ?? Enumerable.Empty<TableRow>(); }
        }

        public IEnumerable<TableColumn> Columns
        {
            get { return _columns; }
            set { _columns = value ?? new List<TableColumn>(); }
        }

        public override string ToMarkdown()
        {
            var markdownBuilder = new MarkdownBuilder(this);
            return markdownBuilder.Build();
        }

        private class MarkdownBuilder
        {
            private class Row
            {
                public IList<ITableCell> Cells { get; set; }
            }
            
            private readonly List<Row> _rows;
            private readonly List<TableColumn> _columns;
            private readonly StringBuilder _builder = new StringBuilder();
            private readonly IList<TableCellRenderSpecification> _columnRenderSpecs;

            internal MarkdownBuilder(Table table)
            {
                _columns = table.Columns.ToList();
                _rows = table.Rows.Select(row => new Row {Cells = row.Cells.ToList()}).ToList();

                var columnCount = Math.Max(_columns.Count, _rows.Any() ? _rows.Max(r => r.Cells.Count) : 0);
                _columnRenderSpecs = Enumerable.Range(0, columnCount).Select(BuildColumnSpecification).ToList();
            }

            private TableCellRenderSpecification BuildColumnSpecification(int column)
            {
                return new TableCellRenderSpecification(GetColumnAt(column).Alignment, GetMaximumCellWidth(column));
            }

            internal string Build()
            {
                BuildHeaderRow();
                BuildDividerRow();

                foreach (var row in _rows)
                {
                    BuildBodyRow(row);
                }

                return _builder.ToString();
            }

            private void BuildHeaderRow()
            {
                var headerCells = (from column in Enumerable.Range(0, _columnRenderSpecs.Count)
                    let cell = GetColumnAt(column).HeaderCell
                    let text = BuildCellMarkdownCode(column, cell)
                    select text).ToList();

                _builder.Append("    ");
                _builder.AppendLine(" " + string.Join(" | ", headerCells));
            }

            private void BuildDividerRow()
            {
                _builder.Append("    ");
                _builder.AppendLine(string.Join("|", _columnRenderSpecs.Select(BuildDividerCell)));
            }

            private static string BuildDividerCell(TableCellRenderSpecification spec)
            {
                var dashes = new string('-', spec.MaximumWidth);

                switch (spec.Alignment)
                {
                    case TableColumnAlignment.Left:
                        return ":" + dashes + " ";
                    case TableColumnAlignment.Center:
                        return ":" + dashes + ":";
                    case TableColumnAlignment.Right:
                        return " " + dashes + ":";
                    default:
                        return " " + dashes + " ";

                }
            }

            private void BuildBodyRow(Row row)
            {
                var rowCells = (from column in Enumerable.Range(0, _columnRenderSpecs.Count)
                    let cell = GetCellAt(row.Cells, column)
                    select BuildCellMarkdownCode(column, cell)).ToList();

                _builder.Append("    ");
                _builder.AppendLine(" " + string.Join(" | ", rowCells));
            }

            private string BuildCellMarkdownCode(int column, ITableCell cell)
            {
                var columnSpec = _columnRenderSpecs[column];
                var maximumWidth = columnSpec.MaximumWidth;
                var cellText = cell.BuildCodeFormattedString(new TableCellRenderSpecification(columnSpec.Alignment, maximumWidth));
                var truncatedCellText = cellText.Length > maximumWidth ? cellText.Substring(0, maximumWidth) : cellText.PadRight(maximumWidth);
                
                return truncatedCellText;
            }

            private int GetMaximumCellWidth(int column)
            {
                var headerCells = new[] {GetColumnAt(column).HeaderCell};
                var bodyCells = _rows.Select(row => GetCellAt(row.Cells, column));
                var columnCells = headerCells.Concat(bodyCells);
                return columnCells.Max(i => i.RequiredWidth);
            }

            private TableColumn GetColumnAt(int index)
            {
                return index < _columns.Count
                    ? _columns[index]
                    : CreateDefaultHeaderCell(index);
            }

            private static TableColumn CreateDefaultHeaderCell(int columnIndex)
            {
                // GitHub Flavoured Markdown requires a header cell. If header text isn't provided
                // use an Excel-like naming scheme (e.g. A, B, C, .., AA, AB, etc)

                return new TableColumn {HeaderCell = new TableCell {Text = columnIndex.ToColumnTitle()}};
            }

            private ITableCell GetCellAt(IList<ITableCell> cells, int index)
            {
                return index < cells.Count ? cells[index] : EmptyCell;
            }
        }
    }
}
