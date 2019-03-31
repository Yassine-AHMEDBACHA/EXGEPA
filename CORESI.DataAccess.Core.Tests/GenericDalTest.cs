using EXGEPA.Model;
using NUnit.Framework;
using System;
using System.Diagnostics;
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
            TimeSpan target = new TimeSpan(0, 0, 0, 2);
            Type dal = typeof(GenericDAL<>).MakeGenericType(typeof(Item));
            dynamic ItemService = Activator.CreateInstance(dal);
            Stopwatch stopwatcher = Stopwatch.StartNew();
            ItemService.SelectAll();
            stopwatcher.Stop();
            Assert.Less(stopwatcher.Elapsed, target);
        }

        [Test]
        public void LoadAnalyticalAccount()
        {
            IDataProvider<AnalyticalAccount> dataService = ServiceLocator.Resolve<IDataProvider<AnalyticalAccount>>();
            Assert.IsNotNull(dataService);
        }

        [Test]
        public void LoadParameterHisto()
        {
            IDataProvider<Item> dataService = ServiceLocator.Resolve<IDataProvider<Item>>();
            dataService.ChangeDataSource(new DBFacade("Data Source=localhost;Initial Catalog=EXGEPA;Integrated Security=True"));
            System.Collections.Generic.IList<Item> histo = dataService.GetHistoric(1);
            Assert.IsNotEmpty(histo);
        }
    }
}
