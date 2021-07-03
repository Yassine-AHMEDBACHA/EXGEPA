namespace CORESI.Data
{
    public interface IRowId
    {
        int Id { get; set; }

        bool EqualsTo(IRowId instance);
    }
}