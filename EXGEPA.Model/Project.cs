using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORESI.Data;

namespace EXGEPA.Model
{
    public class Project : NamedKeyRow
    {
        public AnalyticalAccount AnalyticalAccount { get; set; }
        public GeneralAccount GeneralAccount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
