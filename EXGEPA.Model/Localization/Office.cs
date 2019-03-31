
using CORESI.Data;
using System.Collections.Generic;

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
