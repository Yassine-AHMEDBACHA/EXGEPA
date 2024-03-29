﻿
using CORESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CORESI.DataAccess.Core
{
    public abstract class DbService<T> : IDataProvider<T> where T : Row
    {
        public DbService()
        {
            this.Values = new Dictionary<int, T>();
            this.DataAccessor = new GenericDAL<T>();
        }

        public IList<T> All => this.SelectAll();

        public Dictionary<int, T> Values { get; set; }

        public IGenericDAL<T> DataAccessor { get; set; }

        public virtual int Add(T instance)
        {
            return DataAccessor.Add(instance);
        }

        public virtual int Delete(T instance)
        {
            return this.DataAccessor.Delete(instance);
        }

        public virtual IList<T> SelectAll()
        {
            return this.DataAccessor.SelectAll();
        }

        public virtual IList<T> SelectAll(bool mapReferences)
        {

           var allRows = this.SelectAll();
            //if (mapReferences)
            //{
            //    var references = this.DataAccessor.Fields.Where(x => x.IsReference);

            //    var servicesResult = references.ToDictionary(x => x, x =>
            //    {
            //        var ss = typeof(IDataProvider<T>);

            //        var type = typeof(IDataProvider<>).MakeGenericType(x.Type);

            //        var sss = ServiceLocator.Resolve(ss);
            //        sss = null;
            //        dynamic service = ServiceLocator.Resolve(type);
            //        dynamic dico =  service.SelectAll(mapReferences);
            //        return dico;
            //    });


            //    Parallel.ForEach(allRows, row =>
            //    {
            //        foreach (var result in servicesResult)
            //        {
            //            var referenceID = (int)result.Key.GetValue(row);
            //            object value;
            //            if (((Dictionary<int, object>)result.Value).TryGetValue(referenceID, out value))
            //            {
            //                result.Key.PropertyInfo.SetValue(row, value, null);
            //            }
            //        }
            //    });
            //}

            return allRows;
        }

        public virtual T GetById(int id)
        {
            return this.DataAccessor.GetById<T>(id);
        }

        public virtual int Update(T instance)
        {
            return this.DataAccessor.Update(instance);
        }



        private void Fill<V>(IList<V> listOfInstance) where V : T
        {
            this.DataAccessor.Fill<V>(listOfInstance);
        }

        public T GetValueById(int id)
        {
            T result;
            if (Values.Count == 0)
            {
                Values = new Dictionary<int, T>();
                List<T> list = new List<T>();
                this.Fill<T>(list);
                list.ForEach(item =>
                    {
                        Values.Add(item.Id, item);
                    });
                result = list.FirstOrDefault(item => item.Id == id);

            }
            else if (!Values.TryGetValue(id, out result))
            {
                result = DataAccessor.GetById<T>(id);
                Values.Add(result.Id, result);
            }
            return result;
        }

        public bool HasRows()
        {
            return DataAccessor.HasRows();
        }

        public int GetRowsCount()
        {
            return DataAccessor.GetRowsCount();
        }


        public int DeleteAll()
        {
            return DataAccessor.DeleteAll();
        }

        public IDataProvider<T> ChangeDataSource(IDbFacade dbFacade)
        {
            DbService<T> newDBService = Activator.CreateInstance(this.GetType(), false) as DbService<T>;
            newDBService.DataAccessor.DbFacade = dbFacade;
            return newDBService;
        }

        public IList<T> GetAllTable()
        {
            return DataAccessor.LoadAllTable();
        }

        public IList<T> SelectAll<V>(IEnumerable<V> ElementToBind) where V : IRowId
        {
            IList<T> allElement = this.SelectAll();
            Dictionary<int, V> dico = ElementToBind.ToDictionary(x => x.Id);
            IEnumerable<Field> fieldToUpdate = this.Fields.Where(f => f.Type == typeof(V));
            foreach (Field field in fieldToUpdate)
            {
                foreach (T item in allElement)
                {
                    int referenceId = ((IRowId)field.GetValue(item))?.Id ?? -1;
                    if (dico.TryGetValue(referenceId, out V value))
                    {
                        field.SetValue(item, value);
                    }
                }
            }
            return allElement;
        }

        public IList<T> GetHistoric(int id)
        {
            return this.DataAccessor.GetHistoric(id);

        }

        public List<Field> Fields
        {
            get { return DataAccessor.Fields; }
        }


        //public List<Predicate<T>> Validators { get; private set; }
    }
}
