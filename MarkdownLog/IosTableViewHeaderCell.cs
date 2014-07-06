namespace MarkdownLog
{
    public class IosTableViewHeaderCell : IIosTableViewHeaderCell
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value ?? ""; }
        }

        public int RequiredWidth
        {
            get { return Text.Length; }
        }

        public string BuildCodeFormattedString(int maximumWidth)
        {
            return Text;
        }
    }
}