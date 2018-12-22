using CORESI.IoC;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace CORESI.Data
{
    public interface IDataFilter<T> : IPriority where T : class
    {
        IList<T> Filter(IList<T> ItemsTofilter);
    }
}
