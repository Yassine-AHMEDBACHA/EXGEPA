namespace CORESI.DataAccess.Core.Tools
{
    using System.Collections.Generic;
    using System.Linq;
    using CORESI.Data;

    public static class RowExtensions
    {
        public static void SetProperties<T, V>(this T instance, IEnumerable<V> source)
            where T : Row
            where V : Row
        {
            var referenceField = typeof(T).GetProperties().Where(f => f.PropertyType.Equals(typeof(V)));
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
            var referenceField = typeof(T).GetProperties().Where(f => f.PropertyType.Equals(typeof(V)));
            foreach (var field in referenceField)
            {
                if (field.GetValue(instance) is V prop && source.TryGetValue(prop.Id, out var value))
                {
                    field.SetValue(instance, value);
                }
            }
        }
    }
}
