using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXGEPA.Report.InvestismentRecap
{
    public class RecapRow
    {
        public RecapRow(GeneralAccount generalAccount)
        {
            this.GeneralAccount = generalAccount;
        }
        public GeneralAccount GeneralAccount { get; set; }
        public decimal InitialAmount { get; set; }

        public decimal aquisitionAmount { get; set; }

        public decimal OutputAmount { get; set; }

        public decimal FinalAmount
        {
            get { return this.InitialAmount + this.aquisitionAmount - this.OutputAmount; }
        }
        public decimal PreviousDepreciation { get; set; }
        public decimal Depreciation { get; set; }
        public decimal Cumulated
        {
            get
            {
                return this.Depreciation + this.PreviousDepreciation;
            }
        }
        public decimal VNC
        {
            get
            {
                return this.FinalAmount - this.Cumulated;
            }
        }
    }
}
