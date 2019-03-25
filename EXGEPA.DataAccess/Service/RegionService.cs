// <copyright file="RegionService.cs" company="PlaceholderCompany">
// Copyright (c) CORESI. All rights reserved.
// </copyright>

namespace EXGEPA.DataAccess.Service
{
    using System.ComponentModel.Composition;
    using CORESI.Data;
    using CORESI.DataAccess.Core;
    using EXGEPA.Model;

    [Export(typeof(IDataProvider<Region>))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RegionService : DbService<Region>
    {
    }
}
