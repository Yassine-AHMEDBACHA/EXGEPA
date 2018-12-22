
using CORESI.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXGEPA.Model
{
    public class Office : ALocalization
    {
        public Level Level { get; set; }
        public AnalyticalAccount AnalyticalAccount { get; set; }
        [DataAttribute(IsList = true)]
        public List<Item> Items { get; set; }
        public bool PrintLabel { get; set; }
    }
}
