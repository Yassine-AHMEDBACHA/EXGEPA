using CORESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyproc.Core
{
    public class Comparator<TKey, TData> where TData : KeyRow
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<Wrapper<TData>> Compare(IList<TData> master, IList<TData> slave, Func<TData, TKey> keySelector)
        {
            logger.Debug("Comparing table : " + typeof(TData).Name);
            var errorlist = slave.GroupBy(x => keySelector(x)).Where(group => group.Count() > 1).Select(group => group.Key).ToList();
            errorlist.ForEach(x => logger.Error("Duplicate Key found = " + x));
            var slaveDictionary = slave.ToDictionary(x => keySelector(x), x => x);
            logger.Debug("generating data for table : " + typeof(TData).Name);
            return master.Select(x => GetData(x, slaveDictionary, keySelector)).Where(x => x != null).ToList();
        }

        private static Wrapper<TData> GetData(TData x, Dictionary<TKey, TData> slaveDictionary, Func<TData, TKey> keySelector)
        {
            Wrapper<TData> data = null;
            var key = keySelector(x);
            TData slaveItem;
            if (slaveDictionary.TryGetValue(key, out slaveItem))
            {
                x.Id = slaveItem.Id;
                if ((int)x.Tag > (int)slaveItem.Tag)
                {
                    //if (x.IsDeleted)
                    //{
                    //    data = new Wrapper<TData>(x, DbOperation.Delete);
                    //}
                    //else
                    //{
                    //    data = new Wrapper<TData>(x, DbOperation.Update);
                    //}
                }
            }
            else
            {
                data = new Wrapper<TData>(x, DbOperation.Insert);
            }
            return data;
        }
    }
}
