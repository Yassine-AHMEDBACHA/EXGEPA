﻿// <copyright file="Provider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System.Collections.Generic;
    using CORESI.Data;

    public class Provider : NamedKeyRow
    {
        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PhoneNumber { get; set; }

        public string FaxNumber { get; set; }

        public string TaxId { get; set; }

        public string TradeRegisterId { get; set; }

        [DataAttribute(IsList = true)]
        public virtual List<Item> Items { get; set; }

        public string ThirdPartyAccount { get; set; }
    }
}
