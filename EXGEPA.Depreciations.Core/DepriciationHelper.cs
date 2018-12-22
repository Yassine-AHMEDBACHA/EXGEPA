using System;

namespace EXGEPA.Depreciations.Core
{
    public class DepriciationHelper
    {

        public static DateTime GetLimiteDate(decimal rate, DateTime startDate)
        {
            var result = startDate;
            if (rate != 0)
            {
                var monthcount = (int)Math.Round((100 / rate) * 12, 0);
                result = result.AddMonths(monthcount);
                result = result.AddDays(-1);
            }

            return result;
        }
    }
}
