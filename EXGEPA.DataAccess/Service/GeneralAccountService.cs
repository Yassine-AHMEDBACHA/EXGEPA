using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CORESI.Data;
using CORESI.DataAccess.Core;
using CORESI.IoC;
using EXGEPA.Model;

namespace EXGEPA.DataAccess.Service
{
    [Export(typeof(IDataProvider<GeneralAccount>)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class GeneralAccountService : DbService<GeneralAccount>
    {
        private IDataProvider<GeneralAccountType> generalAccountTypeServices;

        public GeneralAccountService()
        {
            ServiceLocator.Resolve(out this.generalAccountTypeServices);
        }

        public override IList<GeneralAccount> SelectAll()
        {
            var allAccounts = base.SelectAll();
            var types = generalAccountTypeServices.SelectAll();

            foreach (var item in allAccounts)
            {
                item.GeneralAccountType = types.Single(x => x.Id == item.GeneralAccountType?.Id);
                if (item.Children != null)
                {
                    item.Children = allAccounts.Single(x => x.Id == item.Children.Id);
                }
            }

            return allAccounts;
        }
    }
}
