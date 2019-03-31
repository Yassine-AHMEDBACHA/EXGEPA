using CORESI.Data;

namespace CORESI.DataAccess.Core
{
    public class Parameter : KeyRow
    {
        [DataAttribute(Ordinal = 10)]
        public string Value { get; set; }
    }
}
