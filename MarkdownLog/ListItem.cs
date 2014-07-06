namespace MarkdownLog
{
    internal class ListItem : IMarkdownElement
    {
        private string _markdown = "";

        public static ListItem FromText(string text)
        {
            text = text ?? "";
            return new ListItem{_markdown = text.EscapeMarkdownCharacters()};
        }

        public string ToMarkdown()
        {
            return _markdown;
        }
    }
}