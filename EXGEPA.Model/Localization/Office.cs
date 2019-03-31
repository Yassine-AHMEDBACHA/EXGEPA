// <copyright file="Office.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System.Collections.Generic;
    using CORESI.Data;

    public class Office : ALocalization
    {
        public Level Level { get; set; }

        public AnalyticalAccount AnalyticalAccount { get; set; }

        [DataAttribute(IsList = true)]
        public List<Item> Items { get; set; }

        public bool PrintLabel { get; set; }
    }
}
