using CORESI.Data;
using System.Collections.Generic;
namespace CORESI.DataAccess.Core
{
    public interface IGenericDAL<T>
    {
        int Add(T instance);
        int Delete(T instance);
        int DeleteAll();
        IList<T> SelectAll();
        IList<T> LoadAllTable();
        void Fill<V>(System.Collections.Generic.IList<V> ListOfInstances) where V : T;
        void FillInstance<V>(V instance) where V : T;
        V GetById<V>(int id) where V : T;
        int Update(T instance);
        bool HasRows();
        int GetRowsCount();
        List<Field> Fields { get; set; }
        IDbFacade DbFacade { get; set; }
        //Dictionary<int, IList<T>> GetHistoric();
        IList<T> GetHistoric(int id);
    }
}
