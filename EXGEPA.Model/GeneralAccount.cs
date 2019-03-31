// <copyright file="GeneralAccount.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using CORESI.Data;

    public class GeneralAccount : NamedKeyRow
    {
        public decimal Rate { get; set; }

        public GeneralAccountType GeneralAccountType { get; set; }

        public GeneralAccount Children { get; set; }
    }
}
