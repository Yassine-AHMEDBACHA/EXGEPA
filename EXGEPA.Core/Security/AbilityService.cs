// <copyright file="AbilityService.cs" company="PlaceholderCompany">
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

    [Export(typeof(IDataProvider<Ability>))]
    [PartCreationPolicy(CreationPolicy.Shared)]
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
            this.FillReferenceDate(allRows);
            return allRows;
        }

        public override Ability GetById(int id)
        {
            var ability = base.GetById(id);
            this.FillReferenceDate(new List<Ability> { ability });
            return ability;
        }

        private void FillReferenceDate(IList<Ability> allRows)
        {
            var operations = this.OperationService.SelectAll().ToDictionary(x => x.Id);
            var resources = this.ResourceService.SelectAll().ToDictionary(x => x.Id);
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
}

