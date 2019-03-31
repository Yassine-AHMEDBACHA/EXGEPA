using EXGEPA.Core.Interfaces;
using EXGEPA.Depreciations.Core;
using EXGEPA.Model;
using NUnit.Framework;
using System;
using System.Linq;

namespace EXGEPA.Depriciations.Tests
{
    [TestFixture]
    public class MonthelyCalculatorTest
    {
        [Test]
        public void GetMonthelyDepreciationsStandardTest()
        {
            var item = new Item();
            item.Amount = 1000;
            item.FiscalRate = 100;
            item.AquisitionDate = new DateTime(2015, 01, 01);
            item.LimiteDate = new DateTime(2015, 12, 31);
            ICalculator calculator = new MonthelyCalculator(new AccountingPeriodHelper(loadHistory: false));
            var result = calculator.GetDepriciations(item, new DateTime(2010, 01, 01), new DateTime(2015, 12, 31));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
            var depreciation = result.First();
            Assert.AreEqual(0, depreciation.AccountingNetValue);
        }


        [Test]
        public void GetMonthelyDepreciationsTwoYears()
        {
            var item = new Item() { Amount = 1000, FiscalRate = 50, AquisitionDate = new DateTime(2014, 01, 1), LimiteDate = new DateTime(2015, 12, 31) };
            ICalculator calculator = new MonthelyCalculator(new AccountingPeriodHelper(loadHistory: false));
            var result = calculator.GetDepriciations(item, new DateTime(2010, 01, 01), new DateTime(2020, 12, 31));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            var depreciation = result.First();
            Assert.AreEqual(500, depreciation.AccountingNetValue);
            depreciation = result.Last();
            Assert.AreEqual(0, depreciation.AccountingNetValue);
        }

        [Test]
        public void GetMonthelyDepreciations()
        {
            var item = new Item()
            {
                Amount = 1000,
                AquisitionDate = new DateTime(2014, 01, 6),
                LimiteDate = new DateTime(2015, 12, 24)
            };
            ICalculator calculator = new MonthelyCalculator(new AccountingPeriodHelper(loadHistory: false));
            var result = calculator.GetDepriciations(item, new DateTime(2010, 01, 01), new DateTime(2015, 12, 31));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            var depreciation = result.Last();
            Assert.AreEqual(0, depreciation.AccountingNetValue);
            Assert.AreEqual(1000, result.Sum(x => x.Annuity));
        }

        [Test]
        public void GetMonthelyDepreciationsWithDefaultValue()
        {
            var item = new Item();
            ICalculator calculator = new MonthelyCalculator(new AccountingPeriodHelper(loadHistory: false));
            var result = calculator.GetDepriciations(item, new DateTime(1, 01, 01), new DateTime(1, 1, 1));
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestCase("2018/01/01", "2018/12/31", 12)]
        [TestCase("2018/01/01", "2018/12/01", 11)]
        [TestCase("2018/01/01", "2019/01/01", 12)]
        [TestCase("2018/01/01", "2019/01/31", 13)]
        [TestCase("2018/11/16", "2018/12/31", 1)]
        [TestCase("2016/11/16", "2036/11/15", 240)]
        [TestCase("2016/11/15", "2036/11/14", 240)]
        [TestCase("2016/11/14", "2036/11/13", 240)]
        [TestCase("2018/12/01", "2018/12/31", 1)]
        [TestCase("2018/12/15", "2018/12/31", 1)]
        [TestCase("2018/12/01", "2018/12/01", 0)]
        [TestCase("2018/12/20", "2018/12/21", 0)]
        [TestCase("2018/12/15", "2018/12/16", 1)]
        public void GetDuration(string start, string end, int expectedResult)
        {
            var startDate = DateTime.Parse(start);
            var endDate = DateTime.Parse(end);
            var monthlyCalculator = new MonthelyCalculator();
            var actual = monthlyCalculator.GetMonthCount(startDate, endDate);
            Assert.AreEqual(expectedResult, actual);
        }

        [Test]
        public void Should_Compute_Depreciation_when_got_Previouse_Depreciation_And_TransferOrder_Date()
        {
            var item = new Item()
            {
                Amount = 10000,
                AquisitionDate = new DateTime(2010, 01, 01),
                LimiteDate = new DateTime(2019, 12, 31),
                TransferOrder = new TransferOrder { Date = new DateTime(2018, 7, 01) },
                PreviousDepreciation = 8000,
                FiscalRate = 10,
                StartDepreciationDate = StartDepreciationDate.AqusitionDate,
            };
            var calculator = new MonthelyCalculator(new AccountingPeriodHelper(loadHistory: false));
            var result = calculator.GetDepriciations(item, new DateTime(2010, 01, 01), new DateTime(2019, 12, 31));
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void Should_Compute_Depreciation_when()
        {
            var item = new Item()
            {
                Amount = 10000,
                AquisitionDate = new DateTime(2010, 01, 01),
                LimiteDate = new DateTime(2019, 12, 31),
                TransferOrder = new TransferOrder { Date = new DateTime(2018, 7, 01) },
                FiscalRate = 10,
                StartDepreciationDate = StartDepreciationDate.AqusitionDate,
            };

            var calculator = new MonthelyCalculator(new AccountingPeriodHelper(loadHistory: false));
            var result = calculator.GetDepriciations(item, new DateTime(2010, 01, 01), new DateTime(2019, 12, 31));
            Assert.AreEqual(10, result.Count);
        }
    }
}
