using CORESI.Data;
using CORESI.IoC;
using CORESI.Tools;
using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EXGEPA.Sonatrach.Model;
using System.Linq;

namespace EXGEPA.Sonatrach.Core
{
    public class InvoiceSerializer : IInvoiceSerializer
    {
        private readonly string OutPutDirectory;

        private IParameterProvider ParameterProvider;
        public InvoiceSerializer()
        {
            ParameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            this.OutPutDirectory = this.ParameterProvider.GetValue<string>("outPutDirectory", "DATA");
        }

        public void Serialize()
        {

        }

        public void SerializeInvoice(Invoice invoice)
        {
            string header = this.GetCGFRA_FR30_BOD(invoice);
            List<string> itemRows = this.GetCGFRA_FR32(invoice.Items.Values);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(header);
            foreach (string item in itemRows)
            {
                stringBuilder.AppendLine(item);
            }
            string fileName = this.GetFileName(invoice);
            TextAppender.Append(fileName, stringBuilder.ToString(), true);
        }

        private string GetFileName(Invoice invoice)
        {
            string fileName = "Invoice.csv";
            return Path.Combine(this.OutPutDirectory, fileName);
        }

        private string GetCGFRA_FR30_BOD(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        private List<string> GetCGFRA_FR32(IEnumerable<Item> items)
        {
            throw new NotImplementedException();
        }

        public List<FR32> GenerateCGFRA_FR32(List<Certificate> invoices)
        {
            throw new NotImplementedException();
        }

        private FR30_BOD GenerateCGFRA_FR30_BOD(Certificate certificate)
        {
            if (certificate is Invoice invoice)
                return invoice.ToFR32_BOD();
            else
                throw new NotImplementedException();
        }

        public List<FR30_BOD> GenerateCGFRA_FR30_BOD(List<Certificate> invoices)
        {
            List<FR30_BOD> result = invoices.Select(invoice => GenerateCGFRA_FR30_BOD(invoice)).ToList();
            return result;
        }
    }
}
