using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobReader
{
    class LineItem
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public decimal TotalCost { get; set; }
        public bool Exempt { get; set; }
    }
}
