using System.Text;

namespace MarkdownLog
{
    public abstract class HeaderBase : IMarkdownElement
    {
        private readonly char _underlineChar;
        private readonly string _text = "";

        protected HeaderBase(string text, char underlineChar)
        {
            _text = text ?? "";
            _underlineChar = underlineChar;
        }

        public string ToMarkdown()
        {
            var builder = new StringBuilder();

            var textLines = _text.SplitByLine();

            foreach(var textLine in textLines)
            {
                if (builder.Length > 0)
                    builder.AppendLine();

                var markdown = textLine.EscapeMarkdownCharacters();
                builder.Append(markdown);
                builder.AppendLine();
                builder.Append(new string(_underlineChar, markdown.Length));
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}