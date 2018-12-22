namespace CORESI.Data
{
    public interface INamedKey : IKey
    {
        string Caption { get; set; }
    }
}