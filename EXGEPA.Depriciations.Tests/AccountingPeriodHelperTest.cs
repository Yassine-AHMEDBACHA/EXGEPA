using EXGEPA.Depreciations.Core;
using NUnit.Framework;
using System;
using System.Linq;

namespace EXGEPA.Depriciations.Tests
{
    [TestFixture]
    public class AccountingPeriodHelperTest
    {
        [Test]
        public void YearlyAccountPeriod()
        {
            System.Collections.Generic.List<Model.AccountingPeriod> result = (new AccountingPeriodHelper()).GetAccountingPeriodToDate(new DateTime(2010, 01, 01), new DateTime(2010, 12, 31));
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result.First().StartDate.ToString("yyMMdd"), "100101");
            Assert.AreEqual(result.First().EndDate.ToString("yyMMdd"), "101231");
        }

        [Test]
        public void SplitYearBy2()
        {
            AccountingPeriodHelper helper = new AccountingPeriodHelper(2, false);
            System.Collections.Generic.List<Model.AccountingPeriod> result = helper.GetAccountingPeriodToDate(new DateTime(2010, 01, 01), new DateTime(2010, 12, 31));
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result.First().StartDate.ToString("yyMMdd"), "100101");
            Assert.AreEqual(result.First().EndDate.ToString("yyMMdd"), "100630");
            Assert.AreEqual(result.Last().StartDate.ToString("yyMMdd"), "100701");
            Assert.AreEqual("101231", result.Last().EndDate.ToString("yyMMdd"));
            Model.AccountingPeriod instance = result.Last();
            result = result = helper.GetAccountingPeriodToDate(new DateTime(2010, 01, 01), new DateTime(2010, 12, 31));
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result.First().StartDate.ToString("yyMMdd"), "100101");
            Assert.AreEqual(result.First().EndDate.ToString("yyMMdd"), "100630");
            Assert.AreEqual(result.Last().StartDate.ToString("yyMMdd"), "100701");
            Assert.AreEqual("101231", result.Last().EndDate.ToString("yyMMdd"));
            Assert.AreSame(instance, result.Last());
        }

        [Test]
        public void SplitYearBy3()
        {
            AccountingPeriodHelper accountingPeriodHelper = new AccountingPeriodHelper(3, false);

            System.Collections.Generic.List<Model.AccountingPeriod> result = accountingPeriodHelper.GetAccountingPeriodToDate(new DateTime(2010, 01, 01), new DateTime(2010, 12, 31));
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("100101", result[0].StartDate.ToString("yyMMdd"));
            Assert.AreEqual("100430", result[0].EndDate.ToString("yyMMdd"));
            Assert.AreEqual("100501", result[1].StartDate.ToString("yyMMdd"));
            Assert.AreEqual("100831", result[1].EndDate.ToString("yyMMdd"));
            Assert.AreEqual("100901", result[2].StartDate.ToString("yyMMdd"));
            Assert.AreEqual("101231", result[2].EndDate.ToString("yyMMdd"));
        }

        [Test]
        public void QuarterlyAccountingPeriod()
        {
            System.Collections.Generic.List<Model.AccountingPeriod> result = (new AccountingPeriodHelper(4, false)).GetAccountingPeriodToDate(new DateTime(2010, 01, 01), new DateTime(2010, 12, 31));
            Assert.AreEqual(4, result.Count);

            Assert.AreEqual("100101", result[0].StartDate.ToString("yyMMdd"));
            Assert.AreEqual("100331", result[0].EndDate.ToString("yyMMdd"));

            Assert.AreEqual("100401", result[1].StartDate.ToString("yyMMdd"));
            Assert.AreEqual("100630", result[1].EndDate.ToString("yyMMdd"));

            Assert.AreEqual("100701", result[2].StartDate.ToString("yyMMdd"));
            Assert.AreEqual("100930", result[2].EndDate.ToString("yyMMdd"));

            Assert.AreEqual("101001", result[3].StartDate.ToString("yyMMdd"));
            Assert.AreEqual("101231", result[3].EndDate.ToString("yyMMdd"));
        }

        [Test]
        public void MonthelyAccountingPeriod()
        {
            System.Collections.Generic.List<Model.AccountingPeriod> result = (new AccountingPeriodHelper(12, false)).GetAccountingPeriodToDate(new DateTime(2016, 01, 01), new DateTime(2016, 12, 31));
            Assert.AreEqual(12, result.Count);

            Assert.AreEqual("0101", result[0].StartDate.ToString("MMdd"));
            Assert.AreEqual("0131", result[0].EndDate.ToString("MMdd"));

            Assert.AreEqual("0201", result[1].StartDate.ToString("MMdd"));
            Assert.AreEqual("0229", result[1].EndDate.ToString("MMdd"));

            Assert.AreEqual("0301", result[2].StartDate.ToString("MMdd"));
            Assert.AreEqual("0331", result[2].EndDate.ToString("MMdd"));

            Assert.AreEqual("0401", result[3].StartDate.ToString("MMdd"));
            Assert.AreEqual("0430", result[3].EndDate.ToString("MMdd"));

            result = (new AccountingPeriodHelper(12, false)).GetAccountingPeriodToDate(new DateTime(2010, 01, 01), new DateTime(2010, 12, 31));
            Assert.AreEqual(12, result.Count);

            Assert.AreEqual("0101", result[0].StartDate.ToString("MMdd"));
            Assert.AreEqual("0131", result[0].EndDate.ToString("MMdd"));

            Assert.AreEqual("0201", result[1].StartDate.ToString("MMdd"));
            Assert.AreEqual("0228", result[1].EndDate.ToString("MMdd"));

            Assert.AreEqual("0301", result[2].StartDate.ToString("MMdd"));
            Assert.AreEqual("0331", result[2].EndDate.ToString("MMdd"));

            Assert.AreEqual("0401", result[3].StartDate.ToString("MMdd"));
            Assert.AreEqual("0430", result[3].EndDate.ToString("MMdd"));
        }

        [Test]
        public void GetAllYearlyPeriods()
        {
            DateTime startDate = new DateTime(2011, 04, 17);
            DateTime endDate = new DateTime(2016, 12, 15);
            double count = Math.Round((endDate - startDate).TotalDays / 365.25, 0);
            System.Collections.Generic.List<Model.AccountingPeriod> resutl = (new AccountingPeriodHelper(loadHistory: false)).GetAccountingPeriodToDate(startDate, endDate);
            Assert.AreEqual(count, resutl.Count());
            Assert.IsTrue(resutl.First().StartDate <= startDate);
            Assert.IsTrue(resutl.Last().EndDate >= endDate);
        }

        [Test]
        public void GetAllMonthelyPeriods()
        {
            DateTime startDate = new DateTime(2011, 01, 17);
            DateTime endDate = new DateTime(2011, 12, 15);
            int count = 12;
            System.Collections.Generic.List<Model.AccountingPeriod> resutl = (new AccountingPeriodHelper(12, false)).GetAccountingPeriodToDate(startDate, endDate);
            Assert.AreEqual(count, resutl.Count());
            Assert.IsTrue(resutl.First().StartDate <= startDate);
            Assert.IsTrue(resutl.Last().EndDate >= endDate);
        }

        [Test]
        public void GetAccountingPeriodToDateTest()
        {
            DateTime startDate = new DateTime(2011, 01, 17);
            DateTime endDate = new DateTime(2016, 12, 15);
            System.Collections.Generic.List<Model.AccountingPeriod> resutl = (new AccountingPeriodHelper()).GetAccountingPeriodToDate(startDate, endDate);
            Assert.AreEqual(6, resutl.Count);
        }


    }
}
