using System;

namespace MarkdownLog
{
    public class HorizontalRule : MarkdownElement
    {
        public override string ToMarkdown()
        {
            return new string('-', 80) + Environment.NewLine;
        }
    }
}