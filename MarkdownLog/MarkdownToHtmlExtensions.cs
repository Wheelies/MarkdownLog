namespace MarkdownLog
{
    public static class MarkdownToHtmlExtensions
    {
        public static string ToHtml(this IMarkdownElement markdownElement)
        {
            var md = markdownElement.ToMarkdown();

            return MarkdownToHtml(md);
        }

        public static string MarkdownToHtml(this string markdown)
        {
            var converter = new MarkdownToHtmlConverter();
            return converter.Transform(markdown);
        }
    }
}