namespace MarkdownLog
{
    public abstract class MarkdownElement : IMarkdownElement
    {
        public abstract string ToMarkdown();

        public override string ToString()
        {
            return ToMarkdown();
        }
    }
}