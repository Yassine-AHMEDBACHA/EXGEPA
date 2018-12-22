using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.Data
{

    public interface IDataSelector<T>
    {
        IList<T> SelectAll();

        IList<T> SelectAll(bool mapReferences);

        IList<T> SelectAll<V>(IEnumerable<V> ElementToBind) where V : IRowId;
    }
}
