using CORESI.Data;
using System;

namespace EXGEPA.Model
{
    public class AssignmentOffice : KeyRow
    {
        public Office Office { get; set; }
        public DateTime OfficeAssignmentStartDate { get; set; }
        public DateTime OfficeAssignmentEndDate { get; set; }
    }
}
