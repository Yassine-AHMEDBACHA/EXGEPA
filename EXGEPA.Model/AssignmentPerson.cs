// <copyright file="AssignmentPerson.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System;
    using CORESI.Data;

    public class AssignmentPerson : KeyRow
    {
        public Person Person { get; set; }

        public DateTime UserAssignmentStartDate { get; set; }

        public DateTime UserAssignmentEndDate { get; set; }
    }
}
