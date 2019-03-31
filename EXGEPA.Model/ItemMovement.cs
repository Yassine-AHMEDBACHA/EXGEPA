using CORESI.Data;

namespace EXGEPA.Model
{
    public class ItemMovement : KeyRow
    {
        public Item Item { get; set; }
        public AssignmentOffice AssignmentOffice { get; set; }

        public AssignmentPerson AssignmentPerson { get; set; }
    }
}
