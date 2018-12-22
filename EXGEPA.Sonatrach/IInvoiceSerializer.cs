using EXGEPA.Model;
using EXGEPA.Sonatrach.Model;
using System.Collections.Generic;

namespace EXGEPA.Sonatrach
{
    internal interface IInvoiceSerializer : ISerializer
    {
        List<FR30_BOD> GenerateCGFRA_FR30_BOD(List<Certificate> invoices);
        List<FR32> GenerateCGFRA_FR32(List<Certificate> invoices);
    }
}
