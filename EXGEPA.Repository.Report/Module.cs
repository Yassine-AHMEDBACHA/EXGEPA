using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORESI.WPF.Core;
using EXGEPA.Report.Tva;

namespace EXGEPA.Repository.Report
{
    public class Module : AModule
    {
        public override int Priority
        {
            get
            {
                return 0;
            }
        }

        public override void AddGroups()
        {
            return;
        }

        public override void InitializeModule()
        {
            var report = new TvaSheet();
            report.CreateDocument();
        }
    }
}
