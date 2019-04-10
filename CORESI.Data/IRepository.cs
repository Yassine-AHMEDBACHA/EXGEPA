using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORESI.Data
{
    public interface IRepository<T> 
        where T : IRowId
    {
        IList<T> Elements { get; }
    }
}
