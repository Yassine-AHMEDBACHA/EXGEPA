using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EXGEPA.Depreciations.Core
{
    public class DailyCalculator : Calculator
    {

        public DailyCalculator(AccountingPeriodHelper accountingPeriodHelper)
            : base(accountingPeriodHelper)
        { }

       

        
        protected void ComputeDep(List<Depreciation> result, Depreciation first)
        {
            SetDepriciationValues(first);
            for (int i = 1; i < result.Count; i++)
            {
                result[i].InitialValue = result[i - 1].AccountingNetValue;
                SetDepriciationValues(result[i]);
            }
        }

      

        private List<Depreciation> LoadPrevieousDepriciation(Item item, DateTime targetDate)
        {
            var startDate = Tools.GetDefaultStartDate(item);
            if (startDate > targetDate)
                return new List<Depreciation>();
            var periods = AccountingPeriodHelper.GetAccountingPeriodToDate(startDate, targetDate);
            var result = GenerateDepriciationFromAccountingPeriod(item, startDate, targetDate, periods);
            result.First().InitialValue = item.Amount;
            ComputeDep(result, result.First());
            return result;
        }


        protected override void SetDepriciationValues(Depreciation depriciation)
        {
            var totalDayesByPeriod = (int)(depriciation.Item.LimiteDate - depriciation.StartDate).TotalDays + 1;
            var dailyAnnuity = (depriciation.InitialValue / totalDayesByPeriod);
            depriciation.Period = (int)(depriciation.EndDate - depriciation.StartDate).TotalDays + 1;
            depriciation.Annuity = Math.Round(dailyAnnuity * depriciation.Period, 2);
            depriciation.AccountingNetValue = depriciation.InitialValue - depriciation.Annuity;
        }
    }
}
