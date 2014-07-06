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