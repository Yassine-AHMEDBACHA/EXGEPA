namespace EXGEPA.Model
{
    public class Invoice : Certificate
    {
        public Invoice()
        {
            this.Forex = 1;
        }
        public string Type { get; set; }
        public Provider Provider { get; set; }
        public InvoiceType invoiceType { get; set; }
        public OrderDocument OrderDocument { get; set; }
        public InputSheet InputSheet { get; set; }
        public decimal Amount { get; set; }
        public decimal TvaAmount { get; set; }
        public decimal Forex { get; set; }
        public decimal Holdback { get; set; }
        public GeneralAccount HoldbackGeneralAccount { get; set; }
        public GeneralAccount TvaGeneralAccount { get; set; }
        public Currency Currency { get; set; }
        public bool IsPayed { get; set; }
        public string Way { get; set; }
        public string Comment { get; set; }
        public Project Project { get; set; }
        public bool IsValidated { get; set; }
    }
}