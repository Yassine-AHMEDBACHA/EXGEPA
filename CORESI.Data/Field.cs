using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CORESI.Data
{
    public class Field
    {
        public Field()
        {
            IsNullable = true;
        }
        public string Name { get; set; }
        public Type Type { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public bool IsNullable { get; set; }
        public bool IsLong { get; set; }
        public bool IsUnique { get; set; }
        public bool IsReference { get; set; }
        public bool IsAutomatique { get; set; }
        public bool IsPrimeryKey { get; set; }
        public bool IsIdentity { get; set; }
        public string SqldefaultColumValue { get; set; }

        public int Ordinal { get; set; }

        public override string ToString()
        {
            return $"Type of Field : {this.Type?.Name}";
        }
    }
}
