namespace MarkdownLog
{
    public class RawMarkdown : IMarkdownElement
    {
        private readonly string _markdown;

        public RawMarkdown(string markdown)
        {
            _markdown = markdown ?? "";
        }

        public string ToMarkdown()
        {
            return _markdown;
        }
    }
}
