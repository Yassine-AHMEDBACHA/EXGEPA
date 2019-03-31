// <copyright file="Depreciation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System;
    using CORESI.Data;

    public class Depreciation : KeyRow
    {
        public AccountingPeriod AccountingPeriod { get; set; }

        public Item Item { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Rate { get; set; }

        public int Period { get; set; }

        public decimal InitialValue { get; set; }

        public decimal PreviousDepreciation { get; set; }

        public decimal Annuity { get; set; }

        public decimal AccountingNetValue { get; set; }

        public DepreciationType DepreciationType { get; set; }
    }
}
