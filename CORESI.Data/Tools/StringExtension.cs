using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.Data.Tools
{
    public static class StringExtension
    {
        public static bool IsValidData(this string data)
        {
            return !(string.IsNullOrEmpty(data) || string.IsNullOrWhiteSpace(data));
        }

        public static void CheckData(this string data, string variableName)
        {
            if (!data.IsValidData())
            {
                throw new Exception(variableName + " is empty or not valide");
            }
        }


    }
}
