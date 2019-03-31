using CORESI.Data;
using System.Collections.Generic;

namespace EXGEPA.Model
{
    public class Level : ALocalization
    {
        [DataAttribute(IsList = true)]
        public virtual IList<Office> Offices { get; set; }

        
        public virtual Building Building { get; set; }
    }
}
