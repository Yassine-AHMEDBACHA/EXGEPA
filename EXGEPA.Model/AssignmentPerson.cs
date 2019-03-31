using CORESI.Data;
using System;

namespace EXGEPA.Model
{
    public class AssignmentPerson :KeyRow
    {
        public Person Person { get; set; }
        public DateTime UserAssignmentStartDate { get; set; }
        public DateTime UserAssignmentEndDate { get; set; }
    }
}
