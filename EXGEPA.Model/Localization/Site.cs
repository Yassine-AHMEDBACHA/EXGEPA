// <copyright file="Site.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System.Collections.Generic;

    public class Site : ALocalization
    {
        public IList<Building> Buildings { get; set; }

        public Region Region { get; set; }
    }
}
