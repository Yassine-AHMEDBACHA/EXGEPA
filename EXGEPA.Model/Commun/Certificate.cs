// <copyright file="Certificate.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System;
    using System.Collections.Generic;
    using CORESI.Data;

    public abstract class Certificate : NamedKeyRow, IDatable, INamedKeyRepository
    {
        public Certificate()
        {
            this.Items = new Dictionary<int, Item>();
        }

        public DateTime Date { get; set; }

        public Dictionary<int, Item> Items { get; set; }
    }
}
