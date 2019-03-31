using EXGEPA.Depreciations.Core;
using NUnit.Framework;
using System;

namespace EXGEPA.Depriciations.Tests
{
    [TestFixture]
    public class DepriciationHelperTest
    {
        [Test]
        public void GetLimiteDateBefor15()
        {
            DateTime x = Depreciations.Core.DepriciationHelper.GetLimiteDate(20, new DateTime(2010, 01, 01));
            Assert.AreEqual(2014, x.Year);
            Assert.AreEqual(12, x.Month);
            Assert.AreEqual(31, x.Day);
        }

        [Test]
        public void GetLimiteDateAfter15()
        {
            DateTime x = Depreciations.Core.DepriciationHelper.GetLimiteDate(20, new DateTime(2010, 01, 31));
            Assert.AreEqual(2015, x.Year);
            Assert.AreEqual(01, x.Month);
            Assert.AreEqual(30, x.Day);
        }

        [Test]
        public void TestCaseMouh()
        {
            int duration = GetMonthCount(@"16/11/2016", "15/11/2036");
            Assert.AreEqual(240, duration);
        }

        private int GetMonthCount(string strStartDate, string strEndDate)
        {
            DateTime startDate = DateTime.Parse(strStartDate);
            DateTime endDate = DateTime.Parse(strEndDate);
            MonthelyCalculator calculator = new MonthelyCalculator(new AccountingPeriodHelper(loadHistory: false));
            int duration = calculator.GetMonthCount(startDate, endDate);
            return duration;
        }

        [Test]
        public void GetLimiteDateAfter15_2()
        {

            DateTime x = Depreciations.Core.DepriciationHelper.GetLimiteDate(20, new DateTime(2010, 04, 23));
            Assert.AreEqual(2015, x.Year);
            Assert.AreEqual(04, x.Month);
            Assert.AreEqual(22, x.Day);
        }

        [Test]
        public void GetDurationAfter15()
        {
            MonthelyCalculator calculator = new MonthelyCalculator(new AccountingPeriodHelper(loadHistory: false));
            int duree = calculator.GetMonthCount(new DateTime(2010, 12, 23), new DateTime(2010, 12, 31));
            Assert.AreEqual(0, duree);
        }
        [Test]
        public void GetDurationBefor15()
        {
            MonthelyCalculator calculator = new MonthelyCalculator(new AccountingPeriodHelper(loadHistory: false));
            int duree = calculator.GetMonthCount(new DateTime(2010, 12, 10), new DateTime(2010, 12, 31));
            Assert.AreEqual(1, duree);
        }

        [Test]
        public void GetDurationWhen15()
        {
            MonthelyCalculator calculator = new MonthelyCalculator(new AccountingPeriodHelper(loadHistory: false));
            int duree = calculator.GetMonthCount(new DateTime(2010, 12, 15), new DateTime(2010, 12, 31));
            Assert.AreEqual(1, duree);
        }


        [Test]
        public void GetLimiteDateWith15PerCent()
        {
            DateTime x = DepriciationHelper.GetLimiteDate(15, new DateTime(2010, 01, 01));
            Assert.AreEqual(2016, x.Year);
            Assert.AreEqual(08, x.Month);
            Assert.AreEqual(31, x.Day);
        }



    }
}
