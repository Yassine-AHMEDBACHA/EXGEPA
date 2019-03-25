﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORESI.Data;
using CORESI.DataAccess.Core;
using EXGEPA.Model;

namespace EXGEPA.Depreciations.Model
{
    public class Simulation
    {
        const string TableName = "DepreciationArchives";

        public static void SaveDepreciation(IEnumerable<Depreciation> dep)
        {
            var simulationOwner = $"{Environment.MachineName}-{Environment.UserName}";

            cleanSimulation(simulationOwner);

            var itemToSave = dep.Select(x => new DepreciationArchive
            {
                Code = x.Item.Key,
                AmortissementAnterieur = x.PreviousDepreciation,
                Annuite = x.Annuity,
                EndDate = x.EndDate,
                StartDate = x.StartDate,
                Exercice = x.AccountingPeriod.Key,
                SimulationOwner = simulationOwner,
                ValeurInitiale = x.InitialValue,
                VNC = x.AccountingNetValue
            }).ToList();
            SqlHelper.insertRawRows(itemToSave, TableName);
        }

        private static void cleanSimulation(string simulationOwner)
        {
            var query = $"Delete from {TableName} where SimulationOwner = '{simulationOwner}'";
            CORESI.IoC.ServiceLocator.Resolve<IDbFacade>().ExecuteNonQuery(query);
        }

        public static bool Any()
        {
            var simulationOwner = $"{Environment.MachineName}-{Environment.UserName}";
            var query = $"select count(1) from {TableName} where SimulationOwner = '{simulationOwner}'";
            try
            {
                return CORESI.IoC.ServiceLocator.Resolve<IDbFacade>().ExecuteScalaire<int>(query) > 0;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}