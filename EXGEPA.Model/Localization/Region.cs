using CORESI.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXGEPA.Model
{
    public class Region : NamedKeyRow
    {

        [DataAttribute(IsList=true)]
        public virtual IList<Site> Sites { get; set; }

    }
}
