namespace MarkdownLog
{
    public class SubHeader : HeaderBase
    {
        public SubHeader(string format, params object[] args) : this(string.Format(format, args)) { }

        public SubHeader(string text) : base(text, '-')
        {
        }
    }
}