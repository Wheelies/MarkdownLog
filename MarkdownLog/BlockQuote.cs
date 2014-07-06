using System;
using System.Text;

namespace MarkdownLog
{
    public class Blockquote : IMarkdownElement
    {
        private readonly StringBuilder _builder = new StringBuilder();

        public Blockquote()
        {
        }

        public Blockquote(string text)
        {
            Append(text ?? "");
        }

        public void AppendLine()
        {
            AppendLine("");
        }

        public void AppendLine(string text)
        {
            Append(text ?? "");
            Append(Environment.NewLine);
        }

        public void Append(string text)
        {
            text = text ?? "";

            if (_builder.Length > 0)
                _builder.AppendLine();

            _builder.Append(text.EscapeMarkdownCharacters().PrependAllLines("> "));
        }

        public void Append(IMarkdownElement element)
        {
            if (_builder.Length > 0)
                _builder.AppendLine();

            _builder.Append(element.ToMarkdown().PrependAllLines("> "));
        }

        public string ToMarkdown()
        {
            return _builder + Environment.NewLine;
        }
    }
}
