namespace MarkdownLog
{
    public class RawMarkdown : MarkdownElement
    {
        private readonly string _markdown;

        public RawMarkdown(string markdown)
        {
            _markdown = markdown ?? "";
        }

        public override string ToMarkdown()
        {
            return _markdown;
        }
    }
}
