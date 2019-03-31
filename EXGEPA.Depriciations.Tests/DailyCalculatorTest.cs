using CORESI.Data;
using CORESI.IoC;
using EXGEPA.Core.Interfaces;
using EXGEPA.Depreciations.Core;
using EXGEPA.Model;
using NUnit.Framework;
using System;
using System.Linq;

namespace EXGEPA.Depriciations.Tests
{
    [TestFixture]
    public class DailyCalculatorTest
    {
        [Test]
        public void GetDepreciationsStandardTest()
        {
            var item = new Item();
            item.Amount = 1000;
            item.FiscalRate = 100;
            item.AquisitionDate = new DateTime(2015, 01, 01);
            DailyCalculator calculator = new DailyCalculator(new AccountingPeriodHelper(loadHistory: false));
            var result = calculator.GetDepriciations(item, new DateTime(2010, 01, 01), new DateTime(2015, 12, 31));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
            var depreciation = result.First();
            Assert.AreEqual(0, depreciation.AccountingNetValue);
        }

        [Test]
        public void GetDepreciationsTwoYears()
        {
            var item = new Item() { Amount = 1000, FiscalRate = 50, AquisitionDate = new DateTime(2014, 01, 1), LimiteDate = new DateTime(2015, 12, 31) };
            DailyCalculator calculator = new DailyCalculator(new AccountingPeriodHelper(loadHistory: false));
            var result = calculator.GetDepriciations(item, new DateTime(2010, 01, 01), new DateTime(2020, 12, 31));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            var depreciation = result.First();
            Assert.AreEqual(500, depreciation.AccountingNetValue);
            depreciation = result.Last();
            Assert.AreEqual(0, depreciation.AccountingNetValue);
        }


        [Test]
        public void GetDepreciationsForMonthely()
        {
            var item = new Item()
            {
                Amount = 1000,
                AquisitionDate = new DateTime(2014, 01, 6),
                LimiteDate = new DateTime(2015, 12, 24)
            };
            DailyCalculator calculator = new DailyCalculator(new AccountingPeriodHelper(12, loadHistory: false));
            var result = calculator.GetDepriciations(item, new DateTime(2010, 01, 01), new DateTime(2015, 12, 31));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 24);
            var depreciation = result.Last();
            Assert.AreEqual(0, depreciation.AccountingNetValue);
            Assert.AreEqual(1000, result.Sum(x => x.Annuity));
        }

        [Test]
        public void GetMonthelyDepreciationsWithDefaultValue()
        {
            var item = new Item();
            ICalculator calculator = new DailyCalculator(new AccountingPeriodHelper(loadHistory: false));
            var result = calculator.GetDepriciations(item, new DateTime(1, 01, 01), new DateTime(1, 1, 1));
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
      //  [Ignore("Long time to Run")]
        public void AllItemsTest()
        {
            var Items = (ServiceLocator.Resolve<IDataProvider<Item>>()).SelectAll().ToList();
            var calculator = new DailyCalculator(new AccountingPeriodHelper(loadHistory:false));
            var result = calculator.GetDepriciation(Items, new DateTime(1950, 01, 01), new DateTime(2080, 12, 31));
            Assert.AreNotEqual(null, result);
            Assert.IsTrue(result.Count>0);

        }

    }
}
