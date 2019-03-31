using EXGEPA.Model;
using System.Collections.Generic;

namespace EXGEPA.Depreciations.Model
{
    public class Wrapper
    {
        public Item Item { get; set; }
        public List<Depreciation> Depreciations { get; set; }
        public decimal Annuity { get; set; }

        public decimal PreviouseDepreciation { get; set; }
        public decimal Cumuled { get { return PreviouseDepreciation + Annuity; } }
        public decimal NetAccountingValue { get; set; }
        public int Duration { get; set; }
    }
}
