// <copyright file="Region.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System.Collections.Generic;
    using CORESI.Data;

    public class Region : NamedKeyRow
    {

        [DataAttribute(IsList = true)]
        public virtual IList<Site> Sites { get; set; }

    }
}
