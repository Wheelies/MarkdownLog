using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownLog
{
    public class TableView : IMarkdownElement
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

        public string ToMarkdown()
        {
            var builder = new StringBuilder();
            var indent = new string(' ', 4);

            var rows = Sections
                .Where(i => i.Header != null).Select(i => i.Header.RequiredWidth)
                .Concat(Sections.SelectMany(i => i.Cells.Select(j => j.RequiredWidth)))
                .ToList();

            if (!rows.Any()) return "";

            var widestCell = rows.Max();

            var horizontalLine = " " + new String('-', widestCell);
            var containedHorizontalLine = "|" + new String('-', widestCell) + "|";

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
            builder.AppendLine(horizontalLine);

            return builder.ToString();
        }
    }
}