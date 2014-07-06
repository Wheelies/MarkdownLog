using System;

namespace MarkdownLog
{
    public class TableCell : ITableCell
    {
        private string _text;
        
        public string Text
        {
            get { return _text; }
            set { _text = value ?? ""; }
        }

        public int RequiredWidth
        {
            get { return GetEncodedText().Length; }
        }

        public string BuildCodeFormattedString(TableCellRenderSpecification spec)
        {
            var alignment = spec.Alignment;
            var maximumWidth = spec.MaximumWidth;
            var encodedText = GetEncodedText();

            return encodedText.Align(alignment, maximumWidth);
        }

        private string GetEncodedText()
        {
            return _text.Trim().EscapeCSharpString();
        }
    }
}