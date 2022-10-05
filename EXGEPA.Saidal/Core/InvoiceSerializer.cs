namespace EXGEPA.Saidal.Core
{
    using EXGEPA.Model;

    internal class InvoiceSerializer : Serializer<Invoice>
    {
        public InvoiceSerializer()
            : base()
        {
        }

        public override string OperationHeader => "21";

        public override string GetLastPart(Invoice invoice)
        {
            return $"FACT {invoice.Key} {invoice.Date:dd/MM/yyyy} {invoice.Provider.Caption};{invoice.Key}";
        }

        protected override string GetFileNamePattern()
        {
            return this.parameterProvider.TryGet("InterfaceFileNamePattern", "Dump");
        }
    }
}
