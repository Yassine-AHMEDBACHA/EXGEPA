using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CORESI.Data;
using CORESI.DataAccess.Core;
using CORESI.IoC;
using EXGEPA.Model;

namespace EXGEPA.DataAccess.Service
{
    [Export(typeof(IDataProvider<OrderDocument>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class OrderDocumentService : DbService<OrderDocument>
    {
        private IDataProvider<OrderDocumentType> _OrderDocumentTypeService;
        public OrderDocumentService()
        {
            ServiceLocator.Resolve(out this._OrderDocumentTypeService);
        }

        public override IList<OrderDocument> SelectAll()
        {
            IList<OrderDocument> allRows = base.SelectAll();
            IList<OrderDocumentType> types = this._OrderDocumentTypeService.SelectAll();

            foreach (OrderDocument item in allRows)
            {
                item.OrderDocumentType = types.Single(x => x.Id == item.OrderDocumentType?.Id);
            }

            return allRows;
        }
    }
}
