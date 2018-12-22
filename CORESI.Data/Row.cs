
using System;

namespace CORESI.Data
{
    public abstract class Row : ArchivableRow, ICloneable
    {
        [DataAttribute( Ordinal = 100)]
        public object Tag { get; set; }

        [DataAttribute(Ordinal = 99)]
        public string Json { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public interface IRow { }
}