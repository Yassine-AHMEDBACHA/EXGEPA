using CORESI.Data;

using System;

namespace EXGEPA.Model
{
    public class Depreciation : KeyRow
    {
        public AccountingPeriod AccountingPeriod { get; set; }

        public Item Item { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Rate { get; set; }
        public int Period { get; set; }
        public decimal InitialValue { get; set; }
        public decimal PreviousDepreciation { get; set; }
        public decimal Annuity { get; set; }
        public decimal AccountingNetValue { get; set; }

        public DepreciationType DepreciationType { get; set; }

    }
    public enum DepreciationType
    {
        AcceleratedDepreciation = 1,
        LinearDepreciation
    }
}
