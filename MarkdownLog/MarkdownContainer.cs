using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkdownLog
{
    public class MarkdownContainer : MarkdownElement
    {
        private readonly List<IMarkdownElement> _elements = new List<IMarkdownElement>();

        public void Append(IMarkdownElement element)
        {
            _elements.Add(element);
        }

        public override string ToMarkdown()
        {
            return string.Join(Environment.NewLine, _elements.Select(i => i.ToMarkdown()));
        }

        public override string ToString()
        {
            return ToMarkdown();
        }
    }
}
