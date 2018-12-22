using System;
using CORESI.Data;

namespace EXGEPA.Model
{
    public class Assignment : KeyRow
    {
        public Item Item { get; set; }
        public Person Person { get; set; }
        public Office Office { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
