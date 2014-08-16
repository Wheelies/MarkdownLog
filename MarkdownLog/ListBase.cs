#region Copyright and license
// /*
// The MIT License (MIT)
// 
// Copyright (c) 2014 BlackJet Software Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownLog
{
    public abstract class ListBase : MarkdownElement
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

        public override string ToMarkdown()
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