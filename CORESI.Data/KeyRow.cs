namespace CORESI.Data
{
    public abstract class KeyRow : Row, IKey
    {
        [DataAttribute(IsUnique = true, Ordinal = 1)]
        public string Key { get; set; }



    }
}
