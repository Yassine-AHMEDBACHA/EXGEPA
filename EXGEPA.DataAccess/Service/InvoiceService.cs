using CORESI.Data;
using CORESI.DataAccess.Core;
using CORESI.IoC;
using EXGEPA.Model;
using System.ComponentModel.Composition;

namespace EXGEPA.DataAccess.Service
{
    [Export(typeof(IDataProvider<Invoice>))]
    public class InvoiceService : DbService<Invoice>
    {
        IDbFacade DbFacade { get; set; }
        public InvoiceService() : base()
        {
            DbFacade = ServiceLocator.Resolve<IDbFacade>();
            //this.Validators.Add((invoice => CanBeValidated(invoice)));
        }
        //public bool CanBeValidated(Invoice invoice)
        //{
        //  var result  = GetTotalItemAmountByInvoice(invoice);
        //  return (result == invoice.Amount);
        //}

        //public decimal GetTotalItemAmountByInvoice(Invoice invoice)
        //{
        //    var query = "select isnull(sum(Items.Amount),0) from Items where invoice_id = " + invoice.Id.ToString();
        //    decimal result = DbFacade.ExecuteScalaire<decimal>(query);
        //    return result;
        //}
    }
}
