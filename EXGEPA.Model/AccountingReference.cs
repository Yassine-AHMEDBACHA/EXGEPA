// <copyright file="AccountingReference.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    public class AccountingReference
    {
        public int Id { get; set; }

        public virtual Provider Provider { get; set; }

    }
}
