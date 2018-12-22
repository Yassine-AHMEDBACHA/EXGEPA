using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORESI.Tools.StringTools;

namespace EXGEPA.Core.Tools
{
    public class KeyLengthNormalizer
    {
        public static string Normalize(string code, int lenght = 12, string ch = "0")
        {
            return code.Align(lenght, ch);
        }
    }
}
