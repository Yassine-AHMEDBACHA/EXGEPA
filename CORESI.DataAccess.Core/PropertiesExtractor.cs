using CORESI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CORESI.Tools;
using CORESI.Tools.Collections;

namespace CORESI.DataAccess.Core
{

    public static class PropertiesExtractor
    {
        public static Field PropertyToField(this PropertyInfo property)
        {
            if (typeof(string) != property.PropertyType && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                return null;
            var attributes = property.GetCustomAttributes(typeof(DataAttribute), false);
            var dataAttribute = attributes.FirstOrDefault() as DataAttribute;
            if (dataAttribute?.Ignore == true)
                return null;
            var field = new Field()
            {
                PropertyInfo = property,
                Type = property.PropertyType,
                IsReference = property.PropertyType.IsReference(),
                Name = property.Name
            };
            if (field.IsReference)
                field.Name += "_Id";
            if (dataAttribute != null)
            {
                field.IsIdentity = dataAttribute.IsIdentity;
                field.IsNullable = dataAttribute.IsNullable;
                field.IsUnique = dataAttribute.IsUnique;
                field.IsLong = dataAttribute.IsLong;
                field.Ordinal = dataAttribute.Ordinal;
                field.IsAutomatique = dataAttribute.IsAuto;
                field.SqldefaultColumValue = dataAttribute.SqldefaultColumValue;
                field.IsPrimeryKey = dataAttribute.IsPrimaryKey;
            }
            return field;
        }

        public static bool IsReference(this Type type)
        {
            return typeof(IRowId).IsAssignableFrom(type);
        }

        public static List<Field> GetAllFields(this Type type)
        {
            var result = type.GetProperties()
                .Select(p => p.PropertyToField())
                .Where(f => f != null)
                .OrderBy(x => x.Ordinal)
                .ThenBy(x => x.Name)
                .ToList();
            return result;
        }


        public static List<Field> ExtractFields(Type type)
        {
            var result = type.GetAllFields().Where(f => !f.IsPrimeryKey && !f.IsAutomatique).ToList(); ;
            return result;
        }

        public static IEnumerable<Field> ExtractAllFields(Type type)
        {
            int i = 1;
            return type.GetAllFields().Where(f => !f.IsPrimeryKey).ApplyOnAll(x => x.Ordinal = i++);
        }

    }
}
