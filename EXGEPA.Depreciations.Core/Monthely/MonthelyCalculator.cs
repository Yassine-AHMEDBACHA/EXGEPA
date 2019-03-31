using EXGEPA.Core.Interfaces;
using EXGEPA.Model;
using System;
using System.ComponentModel.Composition;

namespace EXGEPA.Depreciations.Core
{
    [Export(typeof(ICalculator))]
    public class MonthelyCalculator : Calculator
    {
        private readonly int monthSplit;

        [ImportingConstructor]
        public MonthelyCalculator(IAccountingPeriodHelper accountingPeriodHelper)
            : base(accountingPeriodHelper)
        {
            this.monthSplit = this.parameterProvider.GetAndSetIfMissing("MonthlyDepreciationDateSplie", 15);
        }


        public MonthelyCalculator()
        {
            monthSplit = 15;
        }

        protected override void SetDepriciationValues(Depreciation depreciation)
        {
            try
            {
                int monthCount = GetMonthCount(depreciation.StartDate, depreciation.Item.LimiteDate);
                if (monthCount == 0)
                {
                    depreciation.Annuity = depreciation.Period = 0;
                }
                else
                {
                    decimal monthelyAnnuity = (depreciation.InitialValue / monthCount);
                    depreciation.Period = GetMonthCount(depreciation.StartDate, depreciation.EndDate);
                    depreciation.Annuity = Math.Round(monthelyAnnuity * depreciation.Period, 2);
                }
                depreciation.AccountingNetValue = depreciation.InitialValue - depreciation.Annuity;
                //logger.Info($"Article:{depreciation.Item?.Key}| Exercice = {depreciation.AccountingPeriod?.Key}| Date Debut de calcul:{depreciation.StartDate}| EndLife:{depreciation.Item.LimiteDate}| Debut de periode:{depreciation.StartDate}| fin de periode:{depreciation.EndDate}| Duree de vie restante pour l'article:{monthCount}| Periode en mois :{depreciation.Period}");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw ex;
            }
        }

        public int GetMonthCount(DateTime startDate, DateTime endDate)
        {
            if (IsSameDate(startDate, endDate))
            {
                return 0;
            }
            if (endDate > startDate)
            {
                DateTime targetStartDate = ComputeStartDate(startDate);
                DateTime targeEndDate = ComputeEndDate(endDate);
                int count = ((targeEndDate.Year - targetStartDate.Year) * 12) + (targeEndDate.Month + 1 - targetStartDate.Month);
                return count;
            }

            throw new ArgumentException($"the end date of calculation must be after the start date! start date = {startDate} | end date = {endDate}");
        }

        private bool IsSameDate(DateTime startDate, DateTime endDate)
        {
            bool sameYear = endDate.Year == startDate.Year;
            bool sameMonth = endDate.Month == startDate.Month;
            bool sameMonthBegining = endDate.Day <= this.monthSplit && startDate.Day <= this.monthSplit;
            bool sameMonthEnding = endDate.Day > this.monthSplit && startDate.Day > this.monthSplit;
            return sameYear && sameMonth && (sameMonthBegining || sameMonthEnding);

        }

        private DateTime ComputeEndDate(DateTime startDate)
        {
            if (startDate.Day < this.monthSplit)
            {
                return startDate.AddMonths(-1);
            }

            return startDate;
        }

        private DateTime ComputeStartDate(DateTime startDate)
        {
            if (startDate.Day <= this.monthSplit)
            {
                return startDate;
            }

            return startDate.AddMonths(1);
        }
    }
}
