// <copyright file="AssignmentOffice .cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System;
    using CORESI.Data;

    public class AssignmentOffice : KeyRow
    {
        public Office Office { get; set; }

        public DateTime OfficeAssignmentStartDate { get; set; }

        public DateTime OfficeAssignmentEndDate { get; set; }
    }
}
