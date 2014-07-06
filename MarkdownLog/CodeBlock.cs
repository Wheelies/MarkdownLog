using System;
using System.Text;

namespace MarkdownLog
{
    public class CodeBlock : IMarkdownElement
    {
        private readonly StringBuilder _builder = new StringBuilder();

        public CodeBlock()
        {
        }

        public CodeBlock(string text)
        {
            Append(text);
        }

        public void AppendLine()
        {
            AppendLine("");
        }

        public void AppendLine(string text)
        {
            Append(text);
            Append(Environment.NewLine);
        }

        public void Append(string text)
        {
            text = text ?? "";
            _builder.Append(text);
        }
        
        public string ToMarkdown()
        {
            return _builder.ToString().PrependAllLines("    ").TrimEnd(' ');
        }
    }
}
