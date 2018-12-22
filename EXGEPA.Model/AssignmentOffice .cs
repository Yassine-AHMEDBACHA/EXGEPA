using CORESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXGEPA.Model
{
    public class AssignmentOffice : KeyRow
    {
        public Office Office { get; set; }
        public DateTime OfficeAssignmentStartDate { get; set; }
        public DateTime OfficeAssignmentEndDate { get; set; }
    }
}
