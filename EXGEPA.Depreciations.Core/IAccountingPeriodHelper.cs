using System;
using System.Collections.Generic;
using EXGEPA.Model;

namespace EXGEPA.Depreciations.Core
{
    public interface IAccountingPeriodHelper
    {
        List<AccountingPeriod> GetAccountingPeriodToDate(DateTime startDate, DateTime endDate);
        AccountingPeriod GetOpenPeriod();
    }
}