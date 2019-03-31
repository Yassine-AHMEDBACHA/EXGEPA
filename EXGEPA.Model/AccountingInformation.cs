
using CORESI.Data;

using System;
using System.Collections.Generic;

namespace EXGEPA.Model.Model
{
    public class AccountingInformations
    {
        public int Id { get; set; }
        public virtual Item Item { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        [DataAttribute(IsList = true)]
        public virtual List<Depreciation> Depreciations { get; set; }

    }
}
