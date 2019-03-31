namespace CORESI.Data
{
    public abstract class ArchivableRow : RowId
    {
        [DataAttribute(Ordinal = 102)]
        public Session Session { get; set; }

        [DataAttribute(SqldefaultColumValue = "0", Ordinal = 101)]
        public bool IsDeleted { get; set; }
    }
}
