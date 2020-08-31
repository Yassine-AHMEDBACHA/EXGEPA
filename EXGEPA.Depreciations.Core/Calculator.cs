using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORESI.Data;
using CORESI.IoC;
using CORESI.Tools.Collections;
using CORESI.Tools.DateTimeTools;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Depreciations.Core
{
    public abstract class Calculator : ICalculator
    {

        protected readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected IParameterProvider parameterProvider;

        Dictionary<Item, List<Depreciation>> Stock { get; set; }

        protected IAccountingPeriodHelper AccountingPeriodHelper;

        public Calculator()
        {
            ServiceLocator.Resolve(out this.parameterProvider);
            Stock = new Dictionary<Item, List<Depreciation>>();
        }

        protected Calculator(IAccountingPeriodHelper accountingPeriodHelper)
            : this()
        {
            this.AccountingPeriodHelper = accountingPeriodHelper ?? new AccountingPeriodHelper();
        }

        public Dictionary<Item, List<Depreciation>> GetDepriciation(List<Item> source, DateTime startDate, DateTime endDate)
        {
            logger.Debug("Start computing for : " + source.Count() + "Item(s) and period between  " + startDate.ToShortDateString() + " and " + endDate.ToShortDateString());
            List<Item> target = GetConcernedItem(source, startDate, endDate);
            return target.ToDictionary(x => x, x => this.GetDepriciations(x, startDate, endDate));
        }

        protected List<Depreciation> GenerateDepriciationFromAccountingPeriod(Item item, DateTime startComputationDate, DateTime endComputationDate, List<AccountingPeriod> periods)
        {
            List<Depreciation> result = periods.Select(x => new Depreciation()
            {
                Item = item,
                Rate = item.FiscalRate,
                StartDate = (startComputationDate >= x.StartDate) ? startComputationDate : x.StartDate,
                EndDate = (endComputationDate <= x.EndDate) ? endComputationDate : x.EndDate,
                AccountingPeriod = x
            }).ToList();
            return result;
        }

        public virtual void ComputeDep(List<Depreciation> result, Depreciation first)
        {
            SetDepriciationValues(first);
            for (int i = 1; i < result.Count; i++)
            {
                result[i].InitialValue = result[i - 1].AccountingNetValue;
                result[i].PreviousDepreciation = result[i - 1].PreviousDepreciation + result[i - 1].Annuity;
                SetDepriciationValues(result[i]);
            }
        }

        public List<Depreciation> GetDepriciations(Item item, DateTime periodeStartDate, DateTime periodeEndDate)
        {
            var startDate = Tools.GetDefaultStartDate(item);
            var defaultEndDate = Tools.GetEndComputationDate(item);
            var endDate = periodeEndDate > defaultEndDate ? defaultEndDate : periodeEndDate;
            var periods = this.AccountingPeriodHelper.GetAccountingPeriodToDate(startDate, endDate);
            var depreciations = periods.Select(p => new Depreciation()
            {
                Item = item,
                Rate = item.FiscalRate,
                StartDate = p.StartDate > startDate ? p.StartDate : startDate,
                EndDate = (p.EndDate > endDate) ? endDate : p.EndDate,
                AccountingPeriod = p
            }).ToLinkedList();

            this.SetAskedDepreciations(depreciations, periodeStartDate, periodeEndDate);
            this.SetUserPreviousDepreciation(item, depreciations);
            this.ComputeDepreciations(depreciations.ToQueue());
            var result = depreciations.Where(d => periodeStartDate <= d.StartDate && endDate >= d.EndDate).ToList();
            return result;
        }

        private void SetAskedDepreciations(LinkedList<Depreciation> depreciations, DateTime periodeStartDate, DateTime periodeEndDate)
        {
            Depreciation next = null;
            Depreciation befor = null;
            var first = depreciations.FirstOrDefault(x => periodeStartDate.Between(x.StartDate, x.EndDate));
            if (first != null)
            {
                if (first.StartDate != periodeStartDate)
                {
                    next = new Depreciation
                    {
                        Item = first.Item,
                        PreviousDepreciation = first.PreviousDepreciation,
                        StartDate = periodeStartDate,
                        EndDate = first.EndDate,
                        AccountingPeriod = first.AccountingPeriod
                    };

                    depreciations.AddAfter(depreciations.Find(first), next);
                    first.EndDate = periodeStartDate.AddDays(-1);
                }
                else
                {
                    next = first;
                }
            }

            var last = depreciations.FirstOrDefault(x => periodeEndDate.Between(x.StartDate, x.EndDate));
            if (last != null)
            {
                if (last.EndDate != periodeEndDate)
                {
                    befor = new Depreciation
                    {
                        Item = first.Item,
                        PreviousDepreciation = first.PreviousDepreciation,
                        StartDate = periodeEndDate.AddDays(1),
                        EndDate = last.EndDate,
                        AccountingPeriod = last.AccountingPeriod
                    };

                    depreciations.AddAfter(depreciations.Find(last), befor);
                    befor.EndDate = periodeEndDate;
                }
                else
                {
                    befor = last;
                }
            }
        }

        protected List<Item> GetConcernedItem(IEnumerable<Item> source, DateTime startDate, DateTime EndDate)
        {
            List<Item> result = source.Where(x => Tools.GetEndComputationDate(x) >= startDate).ToList();
            logger.Debug("Filtring Items that limite date is after " + startDate.ToShortDateString() + "  =  " + result.Count);
            result = result.Where(x => Tools.GetStartComputationDate(x) <= EndDate).ToList();
            Parallel.ForEach(source.Except(result), item => item.Depreciations = new List<Depreciation>());
            logger.Debug("Filtring Items that start date is befor " + EndDate.ToShortDateString() + "  =  " + result.Count);
            return result;
        }



        private void SetUserPreviousDepreciation(Item item, LinkedList<Depreciation> depreciations)
        {
            if (item.ExtendedProperties?.PreviouseDepreciationDate > DateTime.MinValue && item.PreviousDepreciation > 0)
            {
                var previouseDepreciationDate = item.ExtendedProperties.PreviouseDepreciationDate;
                var current = depreciations.FirstOrDefault(x => previouseDepreciationDate.Between(x.StartDate, x.EndDate));
                if (current != null)
                {
                    if (current.StartDate == previouseDepreciationDate)
                    {
                        current.PreviousDepreciation = item.PreviousDepreciation;
                        current.IsPreviouseDepreciationOverided = true;
                        return;
                    }

                    var next = new Depreciation
                    {
                        Item = current.Item,
                        PreviousDepreciation = item.PreviousDepreciation,
                        StartDate = previouseDepreciationDate,
                        EndDate = current.EndDate,
                        AccountingPeriod = current.AccountingPeriod,
                        IsPreviouseDepreciationOverided = true

                    };
                    depreciations.AddAfter(depreciations.Find(current), next);
                    current.EndDate = previouseDepreciationDate.AddDays(-1);
                }
            }
        }

        private void ComputeDepreciations(Queue<Depreciation> depreciations)
        {
            Depreciation last = null;

            while (depreciations.Count > 0)
            {
                var current = depreciations.Dequeue();
                if (current.IsPreviouseDepreciationOverided == true)
                {
                    current.InitialValue = current.Item.Amount - current.PreviousDepreciation;
                }
                else
                {
                    current.PreviousDepreciation = last?.CumulativeDepreciation ?? 0;


                    current.InitialValue = last?.AccountingNetValue ?? current.Item.Amount;
                }

                SetDepriciationValues(current);
                last = current;
            }
        }

        protected abstract void SetDepriciationValues(Depreciation depreciation);

        public AccountingPeriod GetCurrentAccountingPeriod()
        {
            return AccountingPeriodHelper.GetOpenPeriod();
        }
    }
}
