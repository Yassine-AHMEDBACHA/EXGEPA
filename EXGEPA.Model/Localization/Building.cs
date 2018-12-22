using System.Collections.Generic;

namespace EXGEPA.Model
{
    public class Building: ALocalization
    {
        public IList<Level> Levels { get; set; }
        public Site Site { get; set; }
    }
}
