using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownLog
{
    public abstract class ListBase : IMarkdownElement
    {
        private readonly IEnumerable<ListItem> _items = new List<ListItem>();
        private int _wordWrapColumn;

        protected ListBase(params string[] items) : this((IEnumerable<string>) items)
        {
        }

        protected ListBase(IEnumerable<string> items)
        {
            items = items ?? Enumerable.Empty<string>();

            _items = items.Select(ListItem.FromText);
            WordWrap = true;
            WordWrapColumn = 80;
        }

        public bool WordWrap { get; set; }

        public int WordWrapColumn
        {
            get { return _wordWrapColumn; }
            set { _wordWrapColumn = Math.Max(0, value); }
        }

        public string ToMarkdown()
        {
            var builder = new StringBuilder();

            var number = 1;
            foreach (var item in _items)
            {
                var firstLinePrefix = GetListItemFirstLinePrefix(number);

                var markdown = item.ToMarkdown();
                var wrappedMarkdown = WordWrap ? markdown.WrapAt(WordWrapColumn - firstLinePrefix.Length) : markdown;

                builder.AppendFormat("{0}{1}", firstLinePrefix, wrappedMarkdown.IndentAllExceptFirst(firstLinePrefix.Length));
                builder.AppendLine();
                number ++;
            }
            return builder.ToString();
        }

        protected abstract string GetListItemFirstLinePrefix(int itemNumber);
    }
}