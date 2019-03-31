﻿// <copyright file="Reference.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System.Collections.Generic;
    using CORESI.Data;

    public class Reference : NamedKeyRow
    {
        public ReferenceType ReferenceType { get; set; }

        public virtual GeneralAccount InvestmentAccount { get; set; }

        public virtual GeneralAccount ChargeAccount { get; set; }

        public string ImagePath { get; set; }

        [DataAttribute(IsList = true)]
        public virtual List<Item> Items { get; set; }
    }
}
