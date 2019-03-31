using System;
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
