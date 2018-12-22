using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORESI.Data;
using CORESI.IoC;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Depreciations.Core
{
    public abstract class Calculator : ICalculator
    {

        protected readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected IParameterProvider parameterProvider;

        //protected bool UsePreviouseDepreciation;

        Dictionary<Item, List<Depreciation>> Stock { get; set; }
        protected IAccountingPeriodHelper AccountingPeriodHelper;

        public Calculator()
        {
            ServiceLocator.Resolve(out this.parameterProvider);
            Stock = new Dictionary<Item, List<Depreciation>>();
            //this.UsePreviouseDepreciation = this.parameterProvider.GetAndSetIfMissing("UsePreviouseDepreciation", false);
        }

        protected List<Item> GetConcernedItem(IEnumerable<Item> source, DateTime startDate, DateTime EndDate)
        {
            var result = source.Where(x => Tools.GetEndComputationDate(x) >= startDate).ToList();
            logger.Debug("Filtring Items that limite date is after " + startDate.ToShortDateString() + "  =  " + result.Count);
            result = result.Where(x => Tools.GetStartComputationDate(x) <= EndDate).ToList();
            Parallel.ForEach(source.Except(result), item => item.Depreciations = new List<Depreciation>());
            logger.Debug("Filtring Items that start date is befor " + EndDate.ToShortDateString() + "  =  " + result.Count);
            return result;
        }

        protected Calculator(IAccountingPeriodHelper accountingPeriodHelper)
            : this()
        {
            this.AccountingPeriodHelper = accountingPeriodHelper ?? throw new Exception("accountingPeriodHelper must not be null");
        }

        public Dictionary<Item, List<Depreciation>> GetDepriciation(List<Item> source, DateTime startDate, DateTime endDate)
        {
            logger.Debug("Start computing for : " + source.Count() + "Item(s) and period between  " + startDate.ToShortDateString() + " and " + endDate.ToShortDateString());
            var target = GetConcernedItem(source, startDate, endDate);
            return target.ToDictionary(x => x, x => this.GetDepriciations(x, startDate, endDate));
        }

        protected List<Depreciation> GenerateDepriciationFromAccountingPeriod(Item item, DateTime startComputationDate, DateTime endComputationDate, List<AccountingPeriod> periods)
        {
            var result = periods.Select(x => new Depreciation()
            {
                Item = item,
                Rate = item.FiscalRate,
                StartDate = (startComputationDate >= x.StartDate) ? startComputationDate : x.StartDate,
                EndDate = (endComputationDate <= x.EndDate) ? endComputationDate : x.EndDate,
                AccountingPeriod = x
            }).ToList();
            return result;
        }

        protected void ComputeDep(List<Depreciation> result, Depreciation first)
        {
            SetDepriciationValues(first);
            for (int i = 1; i < result.Count; i++)
            {
                result[i].InitialValue = result[i - 1].AccountingNetValue;
                result[i].PreviousDepreciation = result[i - 1].PreviousDepreciation + result[i - 1].Annuity;
                SetDepriciationValues(result[i]);
            }
        }

        public List<Depreciation> GetDepriciations(Item item, DateTime startDate, DateTime endDate)
        {
            logger.Debug($"Starting computing depreciation for :{item.Key}|-Start Date :{startDate.ToString("yyyy/MM/dd")}|-End date :{endDate.ToString("yyyy/MM/dd")}");

            var startComputationDate = Tools.GetStartComputationDate(item, startDate);
            var endComputationDate = Tools.GetEndComputationDate(item, endDate);

            var periods = AccountingPeriodHelper.GetAccountingPeriodToDate(startComputationDate, endComputationDate);
            var result = GenerateDepriciationFromAccountingPeriod(item, startComputationDate, endComputationDate, periods);
            var firstPeriod = result.FirstOrDefault();
            SetStartingComputeParameters(item, startComputationDate, firstPeriod);
            ComputeDep(result, firstPeriod);
            item.Depreciations = result;
            return result;
        }

        private void SetStartingComputeParameters(Item item, DateTime startComputationDate, Depreciation firstPeriod)
        {
            if (Tools.GetDefaultStartDate(item) < startComputationDate && item.PreviousDepreciation == 0)
            {
                var previous = LoadPrevieousDepriciation(item, startComputationDate.AddDays(-1));
                var lastComputationResult = previous.LastOrDefault();

                if (lastComputationResult != null)
                {
                    firstPeriod.InitialValue = lastComputationResult.AccountingNetValue;
                    firstPeriod.PreviousDepreciation = lastComputationResult.PreviousDepreciation + lastComputationResult.Annuity;
                }
                else
                {
                    firstPeriod.InitialValue = GetInitialValue(item);
                    firstPeriod.PreviousDepreciation = item.PreviousDepreciation;
                }
            }
            else
            {
                firstPeriod.InitialValue = GetInitialValue(item);
                firstPeriod.PreviousDepreciation = item.PreviousDepreciation;
            }
        }

        private List<Depreciation> LoadPrevieousDepriciation(Item item, DateTime targetDate)
        {
            var startDate = Tools.GetDefaultStartDate(item);
            if (startDate > targetDate)
                return new List<Depreciation>();
            var periods = AccountingPeriodHelper.GetAccountingPeriodToDate(startDate, targetDate);
            var result = GenerateDepriciationFromAccountingPeriod(item, startDate, targetDate, periods);

            result.First().InitialValue = this.GetInitialValue(item);
            ComputeDep(result, result.First());
            return result;
        }

        private decimal GetInitialValue(Item item)
        {
            if (item.DepreciationBase == 0 && item.Amount != 0 && item.PreviousDepreciation == 0)
            {
                return item.Amount;
            }
            if (item.DepreciationBase == 0 && item.Amount != 0 && item.PreviousDepreciation != 0)
            {
                item.DepreciationBase = item.Amount - item.PreviousDepreciation;
                return item.DepreciationBase;
            }
            if (item.DepreciationBase != 0)
            {
                return item.DepreciationBase;
            }

            return item.Amount;
        }

        protected abstract void SetDepriciationValues(Depreciation depreciation);

        public AccountingPeriod GetCurrentAccountingPeriod()
        {
            return AccountingPeriodHelper.GetOpenPeriod();
        }
    }
}
