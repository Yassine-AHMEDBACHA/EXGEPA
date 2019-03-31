using CORESI.Data;
using System;
using System.Linq;

namespace CORESI.DataAccess.Core.SqlTools
{
    public static class TypeExtensions
    {

        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string GetDBType(this Type type, bool isLong = false)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (type.IsInheritedFrom<IRowId>())
            {
                return "[int]";
            }

            if (type.IsEnum)
            {
                return "[int]";
            }

            if (type.IsInheritedFrom<int>())
                return "[int]";

            if (type.IsInheritedFrom<long>())
                return "[bigint]";

            if (type.IsInheritedFrom<float>())
                return "[float]";


            if (type.IsInheritedFrom<decimal>())
                return "[decimal](18, 2)";

            if (type.IsInheritedFrom<DateTime>())
                return (isLong ? "[DateTime]" : "[Date]");

            if (type.IsInheritedFrom<bool>())
                return "[bit]";
            if (type.IsInheritedFrom<string>())
                return (isLong ? "[nvarchar](MAX)" : "[nvarchar](160)");
            if (type == typeof(object))
                return "[nvarchar](MAX)";
            throw new Exception("DBtype conversion failed");
        }

        public static bool IsInheritedFrom<T>(this Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        public static bool IsValueTypeNullable<T>(this Type type)
        {
            if (!type.IsValueType) return false; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }

        public static string GetTableName(this Type type)
        {
            var tableName = type.IsInterface ? type.Name.Substring(1) : type.Name;
            if (!tableName.EndsWith("s"))
            {
                tableName += "s";
            }

            return tableName;
        }

        public static bool UseTypeInHisProperties(this Type type, Type V)
        {
            logger.Warn("Comparing " + type.Name + " and " + V.Name);
            var propertyTypes = type.GetProperties().Select(p => p.PropertyType).ToList();
            if (propertyTypes.Contains(V))
                return true;
            propertyTypes = propertyTypes.Where(t => (typeof(IRowId).IsAssignableFrom(t))).ToList();
            var result = propertyTypes.Any(t => t.UseTypeInHisProperties(V));
            return result;
        }

    }
}
