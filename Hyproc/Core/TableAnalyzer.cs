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
            IDataProvider<TData> sourceProvider = ServiceLocator.Resolve<IDataProvider<TData>>();
            IDataProvider<TData> targetProvider = sourceProvider.ChangeDataSource(targetDbFacade);
            System.Collections.Generic.IList<TData> Source = sourceProvider.GetAllTable();
            System.Collections.Generic.IList<TData> target = targetProvider.GetAllTable();
            System.Collections.Generic.List<Wrapper<TData>> result = Comparator<TKey, TData>.Compare(Source, target, this.Selector);
            foreach (Wrapper<TData> item in result)
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
