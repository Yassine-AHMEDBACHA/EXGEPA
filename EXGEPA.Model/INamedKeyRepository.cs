// <copyright file="INamedKeyRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using CORESI.Data;

    public interface INamedKeyRepository : INamedKey
    {
        IDictionary<int, Item> Items { get; }
    }
}
