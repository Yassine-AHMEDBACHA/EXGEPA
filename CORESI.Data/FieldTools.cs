namespace CORESI.Data
{
    public static class FieldTools
    {
        public static object GetValue(this Field field,object instance)
        {
            var value = field.PropertyInfo.GetValue(instance, null);
            return value;
        }

        public static void SetValue(this Field field, object instance, object value)
        {
            field.PropertyInfo.SetValue(instance, value, null);
        }
    }
}
