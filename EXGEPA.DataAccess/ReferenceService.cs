using CORESI.Data;
using CORESI.DataAccess.Core;
using CORESI.IoC;
using EXGEPA.Model;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace EXGEPA.DataAccess
{
    [Export(typeof(IDataProvider<Reference>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class ReferenceService : DbService<Reference>
    {

        private IDataProvider<GeneralAccount> GeneralAccountService { get; set; }

        public ReferenceService()
        {
            GeneralAccountService = ServiceLocator.Resolve<IDataProvider<GeneralAccount>>();
        }

        public override IList<Reference> SelectAll()
        {
            var list = new List<Reference>();
            this.DataAccessor.Fill<Reference>(list);
            UpdateListOfReference<Reference>(list);
            return list;
        }

        private void UpdateListOfReference<V>(IList<V> list) where V : Reference
        {
            var ListOfGeneralAccount = this.GeneralAccountService.SelectAll();
            foreach (var item in list)
            {
                if (item.InvestmentAccount != null)
                    item.InvestmentAccount = ListOfGeneralAccount.FirstOrDefault(x => x.Id == item.InvestmentAccount.Id);
                if (item.ChargeAccount != null)
                    item.ChargeAccount = ListOfGeneralAccount.FirstOrDefault(x => x.Id == item.ChargeAccount.Id);
            }
        }




    }
}
