namespace MarkdownLog
{
    public class Header : HeaderBase
    {
        public Header(string format, params object[] args) : this(string.Format(format, args)) { }

        public Header(string text) : base(text, '=')
        {
        }
    }
}
