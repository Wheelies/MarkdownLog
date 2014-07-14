namespace MarkdownLog
{
    internal class ListItem : MarkdownElement
    {
        private string _markdown = "";

        public static ListItem FromText(string text)
        {
            text = text ?? "";
            return new ListItem{_markdown = text.EscapeMarkdownCharacters()};
        }

        public override string ToMarkdown()
        {
            return _markdown;
        }
    }
}