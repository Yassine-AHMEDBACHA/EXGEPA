// <copyright file="AnalyticalAccount.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using CORESI.Data;

    public class AnalyticalAccount : NamedKeyRow
    {
        public string ThirdPartyAccount { get; set; }

        [DataAttribute(IsNullable = false)]
        public AnalyticalAccountType AnalyticalAccountType { get; set; }

    }
}
