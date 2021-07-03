namespace CORESI.Data
{
    public abstract class RowId : IRowId
    {
        [DataAttribute(IsIdentity = true,
               IsNullable = false,
               IsUnique = true,
            IsPrimaryKey = true,
            Ordinal = -10,
               SqldefaultColumValue = "IDENTITY(1, 1)",

               IsList = false)]
        public int Id { get; set; }

        public bool EqualsTo(IRowId instance)
        {
            return this.Id == instance?.Id;
        }
    }
}