﻿using CORESI.Data;
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
            Item item = new Item
            {
                Amount = 1000,
                FiscalRate = 100,
                AquisitionDate = new DateTime(2015, 01, 01)
            };
            DailyCalculator calculator = new DailyCalculator(new AccountingPeriodHelper(loadHistory: false));
            System.Collections.Generic.List<Depreciation> result = calculator.GetDepriciations(item, new DateTime(2010, 01, 01), new DateTime(2015, 12, 31));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
            Depreciation depreciation = result.First();
            Assert.AreEqual(0, depreciation.AccountingNetValue);
        }

        [Test]
        public void GetDepreciationsTwoYears()
        {
            Item item = new Item() { Amount = 1000, FiscalRate = 50, AquisitionDate = new DateTime(2014, 01, 1), LimiteDate = new DateTime(2015, 12, 31) };
            DailyCalculator calculator = new DailyCalculator(new AccountingPeriodHelper(loadHistory: false));
            System.Collections.Generic.List<Depreciation> result = calculator.GetDepriciations(item, new DateTime(2010, 01, 01), new DateTime(2020, 12, 31));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            Depreciation depreciation = result.First();
            Assert.AreEqual(500, depreciation.AccountingNetValue);
            depreciation = result.Last();
            Assert.AreEqual(0, depreciation.AccountingNetValue);
        }


        [Test]
        public void GetDepreciationsForMonthely()
        {
            Item item = new Item()
            {
                Amount = 1000,
                AquisitionDate = new DateTime(2014, 01, 6),
                LimiteDate = new DateTime(2015, 12, 24)
            };
            DailyCalculator calculator = new DailyCalculator(new AccountingPeriodHelper(12, loadHistory: false));
            System.Collections.Generic.List<Depreciation> result = calculator.GetDepriciations(item, new DateTime(2010, 01, 01), new DateTime(2015, 12, 31));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 24);
            Depreciation depreciation = result.Last();
            Assert.AreEqual(0, depreciation.AccountingNetValue);
            Assert.AreEqual(1000, result.Sum(x => x.Annuity));
        }

        [Test]
        public void GetMonthelyDepreciationsWithDefaultValue()
        {
            Item item = new Item();
            ICalculator calculator = new DailyCalculator(new AccountingPeriodHelper(loadHistory: false));
            System.Collections.Generic.List<Depreciation> result = calculator.GetDepriciations(item, new DateTime(1, 01, 01), new DateTime(1, 1, 1));
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        //  [Ignore("Long time to Run")]
        public void AllItemsTest()
        {
            System.Collections.Generic.List<Item> Items = (ServiceLocator.Resolve<IDataProvider<Item>>()).SelectAll().ToList();
            DailyCalculator calculator = new DailyCalculator(new AccountingPeriodHelper(loadHistory: false));
            System.Collections.Generic.Dictionary<Item, System.Collections.Generic.List<Depreciation>> result = calculator.GetDepriciation(Items, new DateTime(1950, 01, 01), new DateTime(2080, 12, 31));
            Assert.AreNotEqual(null, result);
            Assert.IsTrue(result.Count > 0);

        }

    }
}
