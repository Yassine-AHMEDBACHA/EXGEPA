using CORESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.DataAccess.Core
{
    public class Parameter : KeyRow
    {
        [DataAttribute(Ordinal = 10)]
        public string Value { get; set; }
    }
}
