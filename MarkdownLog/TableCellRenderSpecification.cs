namespace MarkdownLog
{
    public class TableCellRenderSpecification
    {
        private readonly TableColumnAlignment _alignment;
        private readonly int _maximumWidth;

        internal TableCellRenderSpecification(TableColumnAlignment alignment, int maximumWidth)
        {
            _alignment = alignment;
            _maximumWidth = maximumWidth;
        }

        public TableColumnAlignment Alignment
        {
            get { return _alignment; }
        }

        public int MaximumWidth
        {
            get { return _maximumWidth; }
        }
    }
}