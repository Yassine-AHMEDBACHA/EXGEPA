// <copyright file="DepreciationArchive.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using System;
    using CORESI.Data;

    public class DepreciationArchive : RowId
    {

        public string Code { get; set; }

        public string Exercice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal ValeurInitiale { get; set; }

        public decimal AmortissementAnterieur { get; set; }

        public decimal Annuite { get; set; }

        public decimal VNC { get; set; }

        public string SimulationOwner { get; set; }
    }
}
