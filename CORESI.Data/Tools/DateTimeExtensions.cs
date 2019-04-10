namespace CORESI.Data.Tools
{
    using System;

    public static class DateTimeExtensions
    {
        public static bool IsBetween(this DateTime date, DateTime minDateTime, DateTime maxDateTime)
        {
            return date >= minDateTime && date <= maxDateTime;
        }
    }
}
