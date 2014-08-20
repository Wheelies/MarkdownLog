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
    public class TableView : MarkdownElement
    {
        private IEnumerable<TableViewSection> _sections = new List<TableViewSection>();

        public TableView()
        {
        }

        public TableView(IEnumerable<IosTableViewCell> cells)
        {
            Sections = new[] {new TableViewSection {Cells = cells}};
        }

        public IEnumerable<TableViewSection> Sections
        {
            get { return _sections; }
            set { _sections = value ?? Enumerable.Empty<TableViewSection>(); }
        }

        public override string ToMarkdown()
        {
            var builder = new StringBuilder();
            var indent = new string(' ', 4);

            var rows = Sections
                .Where(i => i.Header != null).Select(i => i.Header.RequiredWidth)
                .Concat(Sections.SelectMany(i => i.Cells.Select(j => j.RequiredWidth)))
                .ToList();

            if (!rows.Any()) return "";

            var widestCell = rows.Max();

            var horizontalLine = " " + new String('_', widestCell);
            var containedHorizontalLine = "|" + new String('_', widestCell) + "|";

            builder.Append(indent);
            builder.AppendLine(horizontalLine);

            foreach (var section in Sections)
            {
                var isFirstSection = Sections.ElementAt(0) == section;
                if (!isFirstSection)
                {
                    builder.Append(indent);
                    builder.AppendLine(containedHorizontalLine);
                }

                if (section.Header != null)
                {
                    builder.Append(indent);
                    builder.AppendFormat("|{0}|", section.Header.BuildCodeFormattedString(widestCell).PadRight(widestCell));
                    builder.AppendLine();
                    builder.Append(indent);
                    builder.AppendLine(containedHorizontalLine);
                }

                foreach (var cell in section.Cells)
                {
                    builder.Append(indent);
                    builder.AppendFormat("|{0}|", cell.BuildCodeFormattedString(widestCell).PadRight(widestCell));
                    builder.AppendLine();
                }
            }

            builder.Append(indent);
            builder.AppendLine(containedHorizontalLine);

            return builder.ToString();
        }
    }
}