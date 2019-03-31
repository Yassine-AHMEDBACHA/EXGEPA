using CORESI.Data;
using System.Collections.Generic;

namespace EXGEPA.Model
{
    public class Region : NamedKeyRow
    {

        [DataAttribute(IsList=true)]
        public virtual IList<Site> Sites { get; set; }

    }
}
