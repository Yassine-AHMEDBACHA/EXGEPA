// <copyright file="Assignment.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System;
    using CORESI.Data;

    public class Assignment : KeyRow
    {
        public Item Item { get; set; }

        public Person Person { get; set; }

        public Office Office { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
