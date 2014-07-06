using System;

namespace MarkdownLog
{
    public class HorizontalRule : IMarkdownElement
    {
        public string ToMarkdown()
        {
            return new string('-', 80) + Environment.NewLine;
        }
    }
}