// <copyright file="InventoryRow.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System;
    using CORESI.Data;

    public class InventoryRow : KeyRow
    {
        public string Localization { get; set; }

        public string Data { get; set; }

        public ItemState ItemState { get; set; }

        public DateTime OpertationDate { get; set; }
    }
}
