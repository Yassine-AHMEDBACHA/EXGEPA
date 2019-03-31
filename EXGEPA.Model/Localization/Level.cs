// <copyright file="Level.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System.Collections.Generic;
    using CORESI.Data;

    public class Level : ALocalization
    {
        [DataAttribute(IsList = true)]
        public virtual IList<Office> Offices { get; set; }


        public virtual Building Building { get; set; }
    }
}
