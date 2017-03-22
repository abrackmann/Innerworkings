using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobReader
{
    class Job
    {
        public int Id { get; set; }
        public List<LineItem> LineItems { get; set; }
        public bool ExtraMargin { get; set; }
        public decimal Total { get; set; }
    }
}
