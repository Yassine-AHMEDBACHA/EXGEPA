using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.Data
{
    public abstract class ExtensibleDataRow : KeyRow
    {
        public string GenericFields { get; set; }
        
    }
}
