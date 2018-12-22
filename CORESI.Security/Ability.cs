using CORESI.Data;

namespace CORESI.Security
{
    public class Ability : Row
    {
        [DataAttribute(IsNullable = false)]
        public Role Role { get; set; }

        [DataAttribute(IsNullable = false)]
        public Resource Resource { get; set; }
        [DataAttribute(IsNullable = false)]
        public Operation Operation { get; set; }

        [DataAttribute(IsNullable = false)]
        public bool HasAccess { get; set; }
    }
}
