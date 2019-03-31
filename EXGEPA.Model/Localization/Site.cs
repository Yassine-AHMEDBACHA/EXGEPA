using System.Collections.Generic;

namespace EXGEPA.Model
{
    public class Site : ALocalization
    {  
        public IList<Building> Buildings { get; set; }
        public Region Region { get; set; }
    }
}
