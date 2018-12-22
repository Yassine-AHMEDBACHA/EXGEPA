using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.Tools.StringTools
{
    public static class StringHelper
    {
        public static string Align(this string str, int lenght, string ch)
        {
            if (str.Length > lenght)
            {
                return str.Substring(0, lenght);
            }

            while (str.Length < lenght)
            {
                    str = ch + str;
            }
            
            return str;
        }
    }
}
