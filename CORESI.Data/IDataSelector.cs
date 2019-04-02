using System.Collections.Generic;

namespace CORESI.Data
{

    public interface IDataSelector<T>
    {
        IList<T> All { get; }

        IList<T> SelectAll();

        IList<T> SelectAll(bool mapReferences);

        IList<T> SelectAll<V>(IEnumerable<V> ElementToBind) where V : IRowId;
    }
}
