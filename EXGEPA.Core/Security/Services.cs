using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CORESI.Data;
using CORESI.DataAccess.Core;
using CORESI.IoC;
using CORESI.Security;

namespace EXGEPA.Core.Security.DataAccess
{
    [Export(typeof(IDataProvider<Role>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class RoleService : DbService<Role>
    {
        private IDataProvider<Ability> AbilityService { get; set; }

        public RoleService()
        {
            this.AbilityService = ServiceLocator.Resolve<IDataProvider<Ability>>();
        }

        public override Role GetById(int id)
        {
            var role = base.GetById(id);
            FillMissingData(new List<Role> { role });
            return role;
        }

        public override IList<Role> SelectAll()
        {

            var allRows = base.SelectAll();
            FillMissingData(allRows);

            return allRows;
        }

        private void FillMissingData(IList<Role> allRows)
        {
            var abilities = AbilityService
                                        .SelectAll()
                                        .GroupBy(x => x.Role.Id)
                                        .ToDictionary(g => g.Key, x => x.ToList());
            Parallel.ForEach(allRows, role =>
            {
                List<Ability> roleAbility;
                if (abilities.TryGetValue(role.Id, out roleAbility))
                {
                    role.Abilities = roleAbility;
                    Parallel.ForEach(role.Abilities, ability => ability.Role = role);
                }
                else
                {
                    role.Abilities = new List<Ability>();
                }
            });
        }
    }

    [Export(typeof(IDataProvider<Ability>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class AbilityService : DbService<Ability>
    {
        private IDataProvider<Operation> OperationService { get; set; }
        private IDataProvider<Resource> ResourceService { get; set; }

        public AbilityService()
        {
            this.OperationService = ServiceLocator.Resolve<IDataProvider<Operation>>();
            this.ResourceService = ServiceLocator.Resolve<IDataProvider<Resource>>();
        }

        public override IList<Ability> SelectAll()
        {
            var allRows = base.SelectAll();
            FillReferenceDate(allRows);
            return allRows;
        }

        public override Ability GetById(int id)
        {
            var ability = base.GetById(id);
            FillReferenceDate(new List<Ability> { ability });
            return ability;
        }

        private void FillReferenceDate(IList<Ability> allRows)
        {
            var operations = OperationService.SelectAll().ToDictionary(x => x.Id);
            var resources = ResourceService.SelectAll().ToDictionary(x => x.Id);
            Parallel.ForEach(allRows, row =>
            {
                Operation operation;
                if (operations.TryGetValue(row.Operation.Id, out operation))
                {
                    row.Operation = operation;
                }
                Resource resource;
                if (resources.TryGetValue(row.Resource.Id, out resource))
                {
                    row.Resource = resource;
                }

            });
        }
    }

    [Export(typeof(IDataProvider<Operation>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class OperationService : DbService<Operation>
    { }

    [Export(typeof(IDataProvider<Resource>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class ResourceService : DbService<Resource>
    { }
}

