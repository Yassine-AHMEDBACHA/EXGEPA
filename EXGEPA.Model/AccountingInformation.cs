// <copyright file="AccountingInformation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model.Model
{
    using System;
    using System.Collections.Generic;
    using CORESI.Data;

    public class AccountingInformations
    {
        public int Id { get; set; }

        public virtual Item Item { get; set; }

        public DateTime AcquisitionDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        [DataAttribute(IsList = true)]
        public virtual List<Depreciation> Depreciations { get; set; }

    }
}
