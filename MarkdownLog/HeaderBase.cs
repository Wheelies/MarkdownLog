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
using System.Text;

namespace MarkdownLog
{
    public abstract class HeaderBase : MarkdownElement
    {
        private readonly char _underlineChar;
        private readonly string _text = "";

        protected HeaderBase(string text, char underlineChar)
        {
            _text = text ?? "";
            _underlineChar = underlineChar;
        }

        public override string ToMarkdown()
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