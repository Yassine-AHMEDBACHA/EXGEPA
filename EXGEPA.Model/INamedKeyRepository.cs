// <copyright file="INamedKeyRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System.Collections.Generic;
    using CORESI.Data;

    public interface INamedKeyRepository : INamedKey
    {
        List<Item> Items { get; }
    }
}
