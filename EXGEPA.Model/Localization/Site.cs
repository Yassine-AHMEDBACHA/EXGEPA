using CORESI.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXGEPA.Model
{
    public class Site : ALocalization
    {  
        public IList<Building> Buildings { get; set; }
        public Region Region { get; set; }
    }
}
