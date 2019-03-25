// <copyright file="ResourceService.cs" company="PlaceholderCompany">
// Copyright (c) CORESI. All rights reserved.
// </copyright>

namespace EXGEPA.Core.Security.DataAccess
{
    using System.ComponentModel.Composition;
    using CORESI.Data;
    using CORESI.DataAccess.Core;
    using CORESI.Security;

    [Export(typeof(IDataProvider<Resource>))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ResourceService : DbService<Resource>
    {
    }
}

