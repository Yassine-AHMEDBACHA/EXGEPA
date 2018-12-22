using CORESI.Data;
using System;

namespace EXGEPA.Model
{
    public class AccountingPeriod : KeyRow
    {
        public bool Approved { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PeriodModel PeriodModel { get; set; }

        public override string ToString()
        {
            return $"Periode :{this.Key}|-StartDate :{StartDate.ToShortDateString()}|-End Date :{EndDate.ToShortDateString()}";
        }
    }

    public enum PeriodModel
    {
        Yearly = 1,
        Quarterly,
        HalfYearly,
        Custom
    }
}
