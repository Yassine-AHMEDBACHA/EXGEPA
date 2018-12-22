using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CORESI.Data
{
    public interface IDataProvider<T> : IDataSelector<T>
    {
        int Add(T instance);
        int Delete(T instance);
        int DeleteAll();
        IList<T> GetAllTable();
        T GetById(int id);
        int Update(T instance);
        bool HasRows();
        int GetRowsCount();
        List<Field> Fields { get; }
        IDataProvider<T> ChangeDataSource(IDbFacade dbFacade);
        //List<Predicate<T>> Validators { get;  }
        //Dictionary<int, IList<T>> GetHistoric();
        IList<T> GetHistoric(int id);
    }
}
