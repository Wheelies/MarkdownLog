using System.Collections.Generic;
using System.Linq;

namespace MarkdownLog
{
    public class TableViewSection
    {
        private IEnumerable<IIosTableViewCell> _cells = new List<IIosTableViewCell>();

        public IIosTableViewHeaderCell Header { get; set; }

        public IEnumerable<IIosTableViewCell> Cells
        {
            get { return _cells; }
            set { _cells = value ?? Enumerable.Empty<IIosTableViewCell>(); }
        }
    }
}