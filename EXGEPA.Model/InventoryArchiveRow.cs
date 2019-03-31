using System;
using CORESI.Data;

namespace EXGEPA.Model
{
    public class InventoryArchiveRow : RowId
    {
        public string Code { get; set; }
        public string Ancien_Code { get; set; }
        public string Type_ecart { get; set; }
        public string Nom_inventaire { get; set; }
        public DateTime Date_archive { get; set; } 
        public string Reference { get; set; }
        public string Designation_article { get; set; }
        public DateTime date_import { get; set; }
        public string Localisation_theorique { get; set; }
        public string Libelle_Local_theorique { get; set; }
        public string Localisation_physique { get; set; }
        public string Libelle_Local_physique { get; set; }
        public string Etat_theorique { get; set; }
        public DateTime Date_aquisition { get; set; } 
        public string Compte_investissement { get; set; }
        public string Libelle_compte_investissement { get; set; }
        public decimal Montant { get; set; }
        public string Compte_analytique { get; set; }
        public string Libelle_compte_analytique { get; set; }
        public string Observation { get; set; } 
    }
}
