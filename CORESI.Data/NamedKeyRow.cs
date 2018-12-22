using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.Data
{
    public abstract class NamedKeyRow : KeyRow, INamedKey
    {
        [DataAttribute(Ordinal = 5)]
        public string Caption { get; set; }
    }
}
