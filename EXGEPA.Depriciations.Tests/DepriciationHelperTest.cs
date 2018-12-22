using EXGEPA.Depreciations.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXGEPA.Depriciations.Tests
{
    [TestFixture]
    public class DepriciationHelperTest
    {
        [Test]
        public void GetLimiteDateBefor15()
        {
            var x = Depreciations.Core.DepriciationHelper.GetLimiteDate(20, new DateTime(2010, 01, 01));
            Assert.AreEqual(2014, x.Year);
            Assert.AreEqual(12, x.Month);
            Assert.AreEqual(31, x.Day);
        }

        [Test]
        public void GetLimiteDateAfter15()
        {
            var x = Depreciations.Core.DepriciationHelper.GetLimiteDate(20, new DateTime(2010, 01, 31));
            Assert.AreEqual(2015, x.Year);
            Assert.AreEqual(01, x.Month);
            Assert.AreEqual(30, x.Day);
        }

        [Test]
        public void TestCaseMouh()
        {
            var duration = GetMonthCount(@"16/11/2016", "15/11/2036");
            Assert.AreEqual(240, duration);
        }

        private int GetMonthCount(string strStartDate, string strEndDate)
        {
            var startDate = DateTime.Parse(strStartDate);
            var endDate = DateTime.Parse(strEndDate);
            var calculator = new MonthelyCalculator(new AccountingPeriodHelper(loadHistory: false));
            var duration = calculator.GetMonthCount(startDate, endDate);
            return duration;
        }

        [Test]
        public void GetLimiteDateAfter15_2()
        {

            var x = Depreciations.Core.DepriciationHelper.GetLimiteDate(20, new DateTime(2010, 04, 23));
            Assert.AreEqual(2015, x.Year);
            Assert.AreEqual(04, x.Month);
            Assert.AreEqual(22, x.Day);
        }

        [Test]
        public void GetDurationAfter15()
        {
            var calculator = new MonthelyCalculator(new AccountingPeriodHelper(loadHistory: false));
            var duree = calculator.GetMonthCount(new DateTime(2010, 12, 23), new DateTime(2010, 12, 31));
            Assert.AreEqual(0, duree);
        }
        [Test]
        public void GetDurationBefor15()
        {
            var calculator = new MonthelyCalculator(new AccountingPeriodHelper(loadHistory: false));
            var duree = calculator.GetMonthCount(new DateTime(2010, 12, 10), new DateTime(2010, 12, 31));
            Assert.AreEqual(1, duree);
        }

        [Test]
        public void GetDurationWhen15()
        {
            var calculator = new MonthelyCalculator(new AccountingPeriodHelper(loadHistory: false));
            var duree = calculator.GetMonthCount(new DateTime(2010, 12, 15), new DateTime(2010, 12, 31));
            Assert.AreEqual(1, duree);
        }


        [Test]
        public void GetLimiteDateWith15PerCent()
        {
            var x = DepriciationHelper.GetLimiteDate(15, new DateTime(2010, 01, 01));
            Assert.AreEqual(2016, x.Year);
            Assert.AreEqual(08, x.Month);
            Assert.AreEqual(31, x.Day);
        }



    }
}
