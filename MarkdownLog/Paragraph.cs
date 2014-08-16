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
using System.Linq;

namespace MarkdownLog
{
    public class Paragraph : MarkdownElement
    {
        private readonly string _text;
        private int _wordWrapColumn;

        public Paragraph(string format, params object[] args) : this(string.Format(format, args))
        {
        }

        public Paragraph(string text)
        {
            _text = text;
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
            var originalLines = _text.SplitByLine();
            var linesWithoutFinalEmptyLine = (originalLines.Last() == "")
                ? originalLines.Take(originalLines.Count - 1)
                : originalLines;

            var lines = linesWithoutFinalEmptyLine.Select(line =>
            {
                var escapedLine = line.EscapeMarkdownCharacters();
                var wrapped = WordWrap ? escapedLine.WrapAt(WordWrapColumn) : escapedLine;
                return string.Join(Environment.NewLine, wrapped.SplitByLine());
            });

            return string.Join("  " + Environment.NewLine, lines) + Environment.NewLine;
        }
    }
}
