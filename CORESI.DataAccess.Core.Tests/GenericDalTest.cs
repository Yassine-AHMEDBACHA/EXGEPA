using EXGEPA.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CORESI.IoC;
using CORESI.Data;

namespace CORESI.DataAccess.Core.Tests
{
    [TestFixture]
    public class GenericDalTest
    {
        [Test]
        public void LoadItemsPerfs()
        {
            var target = new TimeSpan(0, 0, 0, 2);
            var dal = typeof(GenericDAL<>).MakeGenericType(typeof(Item));
            dynamic ItemService = Activator.CreateInstance(dal);
            var stopwatcher = Stopwatch.StartNew();
            ItemService.SelectAll();
            stopwatcher.Stop();
            Assert.Less(stopwatcher.Elapsed, target);
        }

        [Test]
        public void LoadAnalyticalAccount()
        {
            var dataService = ServiceLocator.Resolve<IDataProvider<AnalyticalAccount>>();
            Assert.IsNotNull(dataService);
        }

        [Test]
        public void LoadParameterHisto()
        {
            var dataService = ServiceLocator.Resolve<IDataProvider<Item>>();
            dataService.ChangeDataSource(new DBFacade("Data Source=localhost;Initial Catalog=EXGEPA;Integrated Security=True"));
            var histo = dataService.GetHistoric(1);
            Assert.IsNotEmpty(histo);
        }
    }
}
