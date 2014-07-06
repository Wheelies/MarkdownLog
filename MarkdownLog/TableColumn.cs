namespace MarkdownLog
{
    public class TableColumn
    {
        private ITableCell _headerCell;

        public TableColumn()
        {
            _headerCell = new EmptyTableCell();
            Alignment = TableColumnAlignment.Unspecified;
        }

        public ITableCell HeaderCell
        {
            get { return _headerCell; }
            set { _headerCell = value ?? new EmptyTableCell(); }
        }

        public TableColumnAlignment Alignment { get; set; }
    }
}