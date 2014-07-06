namespace MarkdownLog
{
    internal class EmptyTableCell : ITableCell
    {
        public int RequiredWidth { get { return 0; } }

        public string BuildCodeFormattedString(TableCellRenderSpecification spec)
        {
            return "";
        }
    }
}