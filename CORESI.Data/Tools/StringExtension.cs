using System;
using System.Runtime.CompilerServices;

namespace CORESI.Data.Tools
{
    public static class StringExtension
    {
        public static bool IsValidData(this string data)
        {
            return !(string.IsNullOrEmpty(data) || string.IsNullOrWhiteSpace(data));
        }

        public static void CheckData(this string data, [CallerMemberName] string variableName = "")
        {
            if (!data.IsValidData())
            {
                throw new Exception(variableName + " is empty or not valide");
            }
        }

        public static bool EqualsTo(this string data, string toCompareWith)
        {
            return data.Equals(toCompareWith, StringComparison.InvariantCultureIgnoreCase);
        }

    }
}
