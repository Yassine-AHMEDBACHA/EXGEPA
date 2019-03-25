// <copyright file="RoleService.cs" company="PlaceholderCompany">
// Copyright (c) CORESI. All rights reserved.
// </copyright>

namespace EXGEPA.Core.Security.DataAccess
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;
    using CORESI.Data;
    using CORESI.DataAccess.Core;
    using CORESI.IoC;
    using CORESI.Security;

    [Export(typeof(IDataProvider<Role>))]
    [PartCreationPolicy(CreationPolicy.Shared)]
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
            this.FillMissingData(new List<Role> { role });
            return role;
        }

        public override IList<Role> SelectAll()
        {
            var allRows = base.SelectAll();
            this.FillMissingData(allRows);

            return allRows;
        }

        private void FillMissingData(IList<Role> allRows)
        {
            var abilities = this.AbilityService
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
}

