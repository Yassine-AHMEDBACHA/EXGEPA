namespace EXGEPA.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using CORESI.Data;
    using CORESI.Tools;
    using EXGEPA.Model;

    public static class ItemExtensions
    {
        private static readonly Dictionary<Type, PropertyInfo[]> ItemProperties = typeof(Item)
            .GetProperties()
            .GroupBy(x => x.PropertyType)
            .ToDictionary(g => g.Key, g => g.ToArray());

        public static void SetExtendedProperties(this Item item)
        {
            if (!item.Json.TryDeserialize(out ItemExtendedProperties deserializedObject))
            {
                deserializedObject = new ItemExtendedProperties
                {
                    PreviouseDepreciationDate = item.AquisitionDate
                };
            }

            item.ExtendedProperties = deserializedObject;
        }

        public static void SerializeExtendedProperties(this Item item)
        {
            item.Json = JsonHelper.Serialize(item.ExtendedProperties);
        }

        public static void Map<T>(this Item item, IDictionary<int, T> source)
            where T : INamedKeyRepository, IRowId
        {
            if (ItemProperties.TryGetValue(typeof(T), out var properties))
            {
                foreach (var field in properties)
                {
                    if (field.GetValue(item) is T prop && source.TryGetValue(prop.Id, out var value))
                    {
                        field.SetValue(item, value);
                        value.Items[item.Id] = item;
                    }
                }
            }
        }

        public static void Map<T>(this Item item, IEnumerable<T> source)
            where T : INamedKeyRepository, IRowId
        {
            if (ItemProperties.TryGetValue(typeof(T), out var properties))
            {
                foreach (var field in properties)
                {
                    var prop = (T)field.GetValue(item);
                    var value = source.FirstOrDefault(x => x.Id == prop?.Id);
                    field.SetValue(item, value);
                    value.Items[item.Id] = item;
                }
            }
        }
    }
}
