using System.Collections.Generic;

namespace EXGEPA.Core.Interfaces
{
    public interface IDataFilter<T> where T : class
    {
        List<T> Filter(IList<T> ItemsTofilter);
    }
}
