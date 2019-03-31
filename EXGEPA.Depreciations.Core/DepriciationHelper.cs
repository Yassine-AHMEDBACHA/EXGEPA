using System;

namespace EXGEPA.Depreciations.Core
{
    public class DepriciationHelper
    {

        public static DateTime GetLimiteDate(decimal rate, DateTime startDate)
        {
            DateTime result = startDate;
            if (rate != 0)
            {
                int monthcount = (int)Math.Round((100 / rate) * 12, 0);
                result = result.AddMonths(monthcount);
                result = result.AddDays(-1);
            }

            return result;
        }
    }
}
