using EXGEPA.Model;
using System;
using System.Collections.Generic;
namespace EXGEPA.Core.Interfaces
{
    public interface ICalculator
    {
        AccountingPeriod GetCurrentAccountingPeriod();
        Dictionary<Item, List<Depreciation>> GetDepriciation(List<Item> source, DateTime startDate, DateTime endDate);
        List<Depreciation> GetDepriciations(Item item, DateTime startDate, DateTime endDate);


    }
}
