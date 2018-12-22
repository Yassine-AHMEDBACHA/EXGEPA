using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXGEPA.Transfert.Console
{
    public class LocalisationLoader
    {
        Dictionary<string, Office> Offices = new Dictionary<string, Office>();
        Dictionary<string, Level> levels = new Dictionary<string, Level>();
        Dictionary<string, Building> buildings = new Dictionary<string, Building>();
        Dictionary<string, Site> sites = new Dictionary<string, Site>();

        
        public LocalisationLoader()
        {

        }

        public Office GetOffices()
        {

            //var query = "select distinct COD_UNI from equip";
            //DataBaseTransfertEngin.LoadData<Office>(query,(r)=>
            //{
            //    return new Office()
            //    {
            //        Approved = true,
            //        Code = "Exercice " + year,
            //        StartDate = startdate,
            //        EndDate = endtdate
            //    };
            //})
            return null;
        }

    }
}
