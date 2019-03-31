using System;

namespace CORESI.Tools.DateTimeTools
{
    public static class DateTimeHelper
    {
        public static bool Between(this DateTime input, DateTime startDate, DateTime endDate, bool includEndDate = false)
        {
            if (includEndDate)
                return (input >= startDate && input <= endDate);
            else
                return (input >= startDate && input < endDate);
        }
    }
}
