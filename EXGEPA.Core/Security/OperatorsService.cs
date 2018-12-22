using CORESI.Data;
using CORESI.DataAccess.Core;
using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using CORESI.Security;
using CORESI.IoC;

namespace EXGEPA.Core.Security
{
    [Export(typeof(IDataProvider<Operator>))]
    public class OperatorsService : DbService<Operator>
    {
        IDataProvider<Role> RoleService { get; set; }

        public OperatorsService()
        {
            this.RoleService = ServiceLocator.Resolve<IDataProvider<Role>>();
        }
        public override IList<Operator> SelectAll()
        {
            var allOperators = base.SelectAll();
            var roles = RoleService.SelectAll();
            foreach (var item in allOperators)
            {
                item.Role = roles.FirstOrDefault(x => item.Role.Id == x.Id);
            }
            return allOperators;
        }
    }
}
