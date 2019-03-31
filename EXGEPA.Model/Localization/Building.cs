// <copyright file="Building.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System.Collections.Generic;

    public class Building : ALocalization
    {
        public IList<Level> Levels { get; set; }

        public Site Site { get; set; }
    }
}
