using System;

namespace CORESI.Data
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class DataAttribute : Attribute
    {
        public DataAttribute()
        {
            IsNullable = true;
        }
        public bool IsIdentity { get; set; }
        public string FieldName { get; set; }
        public bool IsList { get; set; }
        public bool IsLong { get; set; }
        public bool IsNullable { get; set; }
        public bool IsUnique { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsAuto { get; set; }

        public int Ordinal { get; set; }
        public string SqldefaultColumValue { get; set; }

        public bool Ignore { get; set; }
    }
}
