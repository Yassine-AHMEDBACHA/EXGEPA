using CORESI.Data;
using CORESI.IoC;
using System;

namespace Hyproc.Core
{
    public class TableAnalyzer<TKey, TData> : IAnlayzer where TData : KeyRow
    {
        Func<TData, TKey> Selector { get; set; }

        public TableAnalyzer(Func<TData, TKey> selector)
        {
            this.Selector = selector;
        }

        public void UpdateDatabase(IDbFacade targetDbFacade)
        {
            var sourceProvider = ServiceLocator.Resolve<IDataProvider<TData>>();
            var targetProvider = sourceProvider.ChangeDataSource(targetDbFacade);
            var Source = sourceProvider.GetAllTable();
            var target = targetProvider.GetAllTable();
            var result = Comparator<TKey, TData>.Compare(Source, target, this.Selector);
            foreach (var item in result)
            {
                switch (item.DbOperation)
                {
                    case DbOperation.Update:
                        targetProvider.Update(item.Data);
                        break;
                    case DbOperation.Delete:
                        targetProvider.Delete(item.Data);
                        break;
                    case DbOperation.Insert:
                        targetProvider.Add(item.Data);
                        break;
                    default:
                        throw new Exception("Db operation not defind");
                }
            }
        }
    }
}
