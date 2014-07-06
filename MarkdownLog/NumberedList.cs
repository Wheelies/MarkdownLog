using System.Collections.Generic;

namespace MarkdownLog
{
    public class NumberedList : ListBase
    {
        public NumberedList(params string[] items) : base(items)
        {
        }

        public NumberedList(IEnumerable<string> items) : base(items)
        {
        }

        protected override string GetListItemFirstLinePrefix(int itemNumber)
        {
            return string.Format("{0,4}. ", itemNumber);
        }
    }
}