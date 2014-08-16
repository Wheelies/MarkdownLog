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
using System.Text;

namespace MarkdownLog
{
    public class IosTableViewCell : IIosTableViewCell
    {
        private const int SpacesBeforeText = 1;
        private const int SpacesBetweenTextAndAccessory = 1;
        private const int SpacesAfterAccessory = 1;
        private string _text;

        public IosTableViewCell() : this("", TableViewCellAccessory.None)
        {
        }

        public IosTableViewCell(string text)
            : this(text, TableViewCellAccessory.None)
        {
        }

        public IosTableViewCell(string text, TableViewCellAccessory accessory)
        {
            _text = text;
            Accessory = accessory;
        }

        public string Text
        {
            get { return _text; }
            set { _text = value ?? ""; }
        }

        public TableViewCellAccessory Accessory { get; set; }

        public int RequiredWidth
        {
            get
            {
                return SpacesBeforeText +
                       Text.Length +
                       SpacesBetweenTextAndAccessory +
                       GetAccessorySymbol().Length +
                       SpacesAfterAccessory;
            }
        }

        public string BuildCodeFormattedString(int maximumWidth)
        {
            var sb = new StringBuilder();
            var accessoryText = GetAccessorySymbol();
            var textPadding = maximumWidth - SpacesBeforeText - SpacesBetweenTextAndAccessory - accessoryText.Length - SpacesAfterAccessory;

            sb.Append(new String(' ', SpacesBeforeText));
            sb.Append(Text.PadRight(textPadding));
            sb.Append(new String(' ', SpacesBetweenTextAndAccessory));
            sb.Append(accessoryText);
            sb.Append(new String(' ', SpacesAfterAccessory));

            return sb.ToString();
        }

        private string GetAccessorySymbol()
        {
            switch (Accessory)
            {
                case TableViewCellAccessory.None:
                    return "";
                case TableViewCellAccessory.DisclosureIndicator:
                    return ">";
                case TableViewCellAccessory.DetailDisclosureButton:
                    return "(>)";
                case TableViewCellAccessory.Checkmark:
                    return "/";
                case TableViewCellAccessory.DetailButton:
                    return "(i)";
                default:
                    throw new NotSupportedException("Unsupported TableViewCellAccessory: " + Accessory);
            }
        }
    }
}