using System;
using CORESI.Data;

namespace EXGEPA.Model
{
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
