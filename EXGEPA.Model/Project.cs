// <copyright file="Project.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System;
    using CORESI.Data;

    public class Project : NamedKeyRow
    {
        public AnalyticalAccount AnalyticalAccount { get; set; }

        public GeneralAccount GeneralAccount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
