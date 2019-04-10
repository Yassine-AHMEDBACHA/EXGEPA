// <copyright file="Certificate.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using CORESI.Data;

    public abstract class Certificate : NamedKeyRow, IDatable, INamedKeyRepository
    {
        public Certificate()
        {
            this.Items = new ConcurrentDictionary<int, Item>();
        }

        public DateTime Date { get; set; }

        public IDictionary<int, Item> Items { get; set; }
    }
}
