namespace MarkdownLog
{
    // Custom cell
    public class IosTableViewCheckmarkCell : IIosTableViewCell
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value ?? ""; }
        }

        public bool IsChecked { get; set; }

        public int RequiredWidth
        {
            get { return BuildCodeFormattedString(0).Length; }
        }

        public string BuildCodeFormattedString(int maximumWidth)
        {
            return string.Format(" [{0}] {1} ", CheckCharacter, Text);
        }

        private char CheckCharacter
        {
            get { return IsChecked ? 'X' : ' '; }
        }
    }
}