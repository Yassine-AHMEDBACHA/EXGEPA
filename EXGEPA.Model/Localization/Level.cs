using CORESI.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXGEPA.Model
{
    public class Level : ALocalization
    {
        [DataAttribute(IsList = true)]
        public virtual IList<Office> Offices { get; set; }

        
        public virtual Building Building { get; set; }
    }
}
