namespace CORESI.Data.Tools
{
    using System;
    using System.Runtime.CompilerServices;

    public static class StringExtension
    {
        public static bool IsValid(this string data) => !data.IsNotValid();

        public static bool IsNotValid(this string data) => string.IsNullOrEmpty(data) || string.IsNullOrWhiteSpace(data);

        public static void CheckData(this string data, [CallerMemberName] string variableName = "")
        {
            if (!data.IsValid())
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
