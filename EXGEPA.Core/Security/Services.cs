﻿using System.Collections.Generic;
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
            Role role = base.GetById(id);
            FillMissingData(new List<Role> { role });
            return role;
        }

        public override IList<Role> SelectAll()
        {

            IList<Role> allRows = base.SelectAll();
            FillMissingData(allRows);

            return allRows;
        }

        private void FillMissingData(IList<Role> allRows)
        {
            Dictionary<int, List<Ability>> abilities = AbilityService
                                        .SelectAll()
                                        .GroupBy(x => x.Role.Id)
                                        .ToDictionary(g => g.Key, x => x.ToList());
            Parallel.ForEach(allRows, role =>
            {
                if (abilities.TryGetValue(role.Id, out List<Ability> roleAbility))
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
            IList<Ability> allRows = base.SelectAll();
            FillReferenceDate(allRows);
            return allRows;
        }

        public override Ability GetById(int id)
        {
            Ability ability = base.GetById(id);
            FillReferenceDate(new List<Ability> { ability });
            return ability;
        }

        private void FillReferenceDate(IList<Ability> allRows)
        {
            Dictionary<int, Operation> operations = OperationService.SelectAll().ToDictionary(x => x.Id);
            Dictionary<int, Resource> resources = ResourceService.SelectAll().ToDictionary(x => x.Id);
            Parallel.ForEach(allRows, row =>
            {
                if (operations.TryGetValue(row.Operation.Id, out Operation operation))
                {
                    row.Operation = operation;
                }
                if (resources.TryGetValue(row.Resource.Id, out Resource resource))
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

