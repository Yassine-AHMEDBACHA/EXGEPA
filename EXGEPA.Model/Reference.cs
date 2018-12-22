using CORESI.Data;
using System.Collections.Generic;

namespace EXGEPA.Model
{
    public class Reference : NamedKeyRow
    {
        public ReferenceType ReferenceType { get; set; }
        public virtual GeneralAccount InvestmentAccount { get; set; }
        public virtual GeneralAccount ChargeAccount { get; set; }
        public string ImagePath { get; set; }

        [DataAttribute(IsList = true)]
        public virtual List<Item> Items { get; set; }
    }
}
