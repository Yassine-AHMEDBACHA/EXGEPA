using EXGEPA.Model;
using EXGEPA.Sonatrach.Model;

namespace EXGEPA.Sonatrach.Core
{
    internal static class InvoiceExtentions
    {
        internal static FR30_BOD ToFR32_BOD(this Invoice invoice)
        {
            FR30_BOD fr30_BOD = new FR30_BOD()
            {
                E_JRNL = "28",
                E_CART = "30",
                ID_FACT = invoice.Key
            };
            return fr30_BOD;
        }
    }
}
