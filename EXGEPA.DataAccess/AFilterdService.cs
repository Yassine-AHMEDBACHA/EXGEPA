using CORESI.Data;
using CORESI.DataAccess.Core;
using CORESI.IoC;
using System.Collections.Generic;

namespace EXGEPA.DataAccess
{
    public abstract class AFilterdService<T> : DbService<T> where T:KeyRow
    {
        public AFilterdService() : base()
        {
            this.DataFilter = ServiceLocator.GetPriorizedInstance<IDataFilter<T>>();
        }

        public IDataFilter<T> DataFilter { get; set; }

        public override IList<T> SelectAll()
        {
            return DataFilter.Filter(base.SelectAll());
        }
    }
}