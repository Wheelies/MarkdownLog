using System.Collections.Generic;
using System.Linq;

namespace MarkdownLog
{
    public class TableRow
    {
        private IEnumerable<ITableCell> _cells = Enumerable.Empty<ITableCell>();

        public IEnumerable<ITableCell> Cells
        {
            get { return _cells; }
            set { _cells = value ?? Enumerable.Empty<ITableCell>(); }
        }
    }
}