using CORESI.Data;

namespace EXGEPA.Model
{
    public class GeneralAccount : NamedKeyRow
    {
        public decimal Rate { get; set; }
        public GeneralAccountType GeneralAccountType { get; set; }

        public GeneralAccount Children { get; set; }
    }
}
