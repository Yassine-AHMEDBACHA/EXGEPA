namespace CORESI.DataAccess.Core.Tools
{
    using CORESI.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class RowExtensions
    {
        private static Dictionary<Type, Dictionary<Type, List<PropertyInfo>>> propertyByType = new Dictionary<Type, Dictionary<Type, List<PropertyInfo>>>();
        private static object locker = new object();

        public static void SetProperties<T, V>(this T instance, IEnumerable<V> source)
            where T : Row
            where V : Row
        {
            var referenceField = GetProperties(typeof(T), typeof(V));
            foreach (var field in referenceField)
            {
                var prop = (V)field.GetValue(instance);
                var value = source.FirstOrDefault(x => x.Id == prop?.Id);
                field.SetValue(instance, value);
            }
        }

        public static void SetProperties<T, V>(this T instance, IDictionary<int, V> source)
            where T : Row
            where V : Row
        {
            var referenceField = GetProperties(typeof(T), typeof(V));
            foreach (var field in referenceField)
            {
                if (field.GetValue(instance) is V prop && source.TryGetValue(prop.Id, out var value))
                {
                    field.SetValue(instance, value);
                }
            }
        }

        private static List<PropertyInfo> GetProperties(Type ownerType, Type propertyType)
        {
            lock (locker)
            {
                if (!propertyByType.TryGetValue(ownerType, out var dico))
                {
                    propertyByType[ownerType] = dico = new Dictionary<Type, List<PropertyInfo>>();
                }

                if (!dico.TryGetValue(propertyType, out var properties))
                {
                    dico[propertyType] = properties = ownerType.GetProperties()
                        .Where(f => f.PropertyType.Equals(propertyType))
                        .ToList();
                }

                return properties;
            }
        }
    }
}
