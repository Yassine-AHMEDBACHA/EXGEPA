// <copyright file="ItemMovement.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using CORESI.Data;

    public class ItemMovement : KeyRow
    {
        public Item Item { get; set; }

        public AssignmentOffice AssignmentOffice { get; set; }

        public AssignmentPerson AssignmentPerson { get; set; }
    }
}
