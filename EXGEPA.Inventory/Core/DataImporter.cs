using CORESI.Data;
using CORESI.DataAccess.Core;
using CORESI.IoC;
using EXGEPA.Core.Tools;
using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EXGEPA.Inventory.Core
{
    public class DataImporter
    {
        public static void LoadFile(string filePath)
        {
            var inventoryService = ServiceLocator.Resolve<IDataProvider<InventoryRow>>();
            var itemStateService = ServiceLocator.Resolve<IDataProvider<ItemState>>();
            var parameterProvider = ServiceLocator.Resolve<IParameterProvider>();

            int codeLength = parameterProvider.GetValue($"{typeof(Item).Name}KeyLength", 6);
            
            var Allinventory = inventoryService.SelectAll().ToDictionary(x => x.Key);
            var rowsToInsert = new Dictionary<string, InventoryRow>();
            var rowsToUpdate = new Dictionary<string, InventoryRow>();
            var textRows = FileLoader.LoadTextRows(filePath);

            foreach (var textRow in textRows.Where(x => x.Length >= 13 + codeLength))
            {
                string code = textRow.Substring(13, codeLength).ToUpper();
                string localization = textRow.Substring(0, 13).ToUpper();
                InventoryRow inventory;
                if (Allinventory.TryGetValue(code, out inventory))
                {
                    inventory.Localization = localization;
                    var toUpdate = inventory;
                    if (!rowsToUpdate.TryGetValue(code, out inventory))
                    {
                        rowsToUpdate.Add(code, toUpdate);
                    }
                }
                else
                {
                    if (rowsToInsert.TryGetValue(code, out inventory))
                    {
                        inventory.Localization = localization;
                    }
                    else
                    {
                        inventory = new InventoryRow()
                        {
                            Key = code,
                            Localization = localization,
                            Data = textRow,
                            OpertationDate = DateTime.Now,
                            // ItemState = ItemStates
                        };
                        rowsToInsert.Add(code, inventory);
                    }
                }
            }
            Parallel.ForEach(rowsToUpdate.Values.ToList(), row =>
            {
                inventoryService.Update(row);
            });
            var rows = rowsToInsert.Values.ToList();
            SqlHelper.InsertBulk(rows);

            ArchiveFile(filePath);
        }

        private static void ArchiveFile(string filePath)
        {
            var parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            var path = parameterProvider.GetValue<string>("InventFileArchive", @"ArchiveInvent");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var targetfile = path + @"\Invent_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".Loaded.txt";
            File.Move(filePath, targetfile);
            File.Delete(filePath);
        }
    }
}
