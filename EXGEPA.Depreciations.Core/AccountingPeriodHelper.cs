using CORESI.Data;
using CORESI.IoC;
using CORESI.Tools.Collections;
using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace EXGEPA.Depreciations.Core
{
    [Export]
    public class AccountingPeriodHelper : IAccountingPeriodHelper
    {

        private static IDataProvider<AccountingPeriod> AccountingPeriodService { get; set; }

        readonly int _Factor;
        public AccountingPeriodHelper(int factor = 1, bool loadHistory = true)
        {
            AccountingPeriodService = ServiceLocator.Resolve<IDataProvider<AccountingPeriod>>();
            this._Factor = 12 / factor;
            if (loadHistory)
            {
                AccountingPeriodService.SelectAll().ForEach(x =>
                    {
                        AccountingPeriods.Add(x.StartDate, x);
                        x.Key = x.StartDate.Year.ToString();
                    });
            }
        }
        Dictionary<DateTime, AccountingPeriod> AccountingPeriods = new Dictionary<DateTime, AccountingPeriod>();
        object locker = new object();
        public List<AccountingPeriod> GetAccountingPeriodToDate(DateTime startDate, DateTime endDate)
        {
            List<AccountingPeriod> result = new List<AccountingPeriod>();
            startDate = new DateTime(startDate.Year, 01, 01);
            do
            {
                if (AccountingPeriods.TryGetValue(startDate.Date, out AccountingPeriod currentPeriod))
                {
                    startDate = startDate.AddMonths(_Factor);
                }
                else
                {
                    lock (locker)
                    {
                        if (AccountingPeriods.TryGetValue(startDate.Date, out currentPeriod))
                        {
                            startDate = startDate.AddMonths(_Factor);
                        }
                        else
                        {
                            currentPeriod = new AccountingPeriod()
                            {
                                StartDate = startDate,
                                Key = startDate.Year.ToString()
                            };
                            startDate = startDate.AddMonths(_Factor);
                            currentPeriod.EndDate = startDate.AddDays(-1);
                            AccountingPeriods.Add(currentPeriod.StartDate, currentPeriod);
                        }
                    }
                }
                result.Add(currentPeriod);
            }
            while (result.Last().EndDate < endDate);
            return result;
        }

        public AccountingPeriod GetOpenPeriod()
        {
            return AccountingPeriods.Values.Where(x => !x.Approved).First();
        }



    }
}
