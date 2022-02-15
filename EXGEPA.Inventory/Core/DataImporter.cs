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
            IDataProvider<InventoryRow> inventoryService = ServiceLocator.Resolve<IDataProvider<InventoryRow>>();
            IDataProvider<ItemState> itemStateService = ServiceLocator.Resolve<IDataProvider<ItemState>>();
            IParameterProvider parameterProvider = ServiceLocator.Resolve<IParameterProvider>();

            int codeLength = parameterProvider.GetValue($"{typeof(Item).Name}KeyLength", 6);

            Dictionary<string, InventoryRow> Allinventory = inventoryService.SelectAll().ToDictionary(x => x.Key);
            Dictionary<string, InventoryRow> rowsToInsert = new Dictionary<string, InventoryRow>();
            Dictionary<string, InventoryRow> rowsToUpdate = new Dictionary<string, InventoryRow>();
            List<string> textRows = FileLoader.LoadTextRows(filePath);

            var now = DateTime.Now;
            foreach (string textRow in textRows.Where(x => x.Length >= 13 + codeLength))
            {
                string code = textRow.Substring(13, codeLength).ToUpper();
                string localization = textRow.Substring(0, 13).ToUpper();
                if (Allinventory.TryGetValue(code, out InventoryRow inventory))
                {
                    inventory.Localization = localization;
                    InventoryRow toUpdate = inventory;
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
                            OpertationDate = now,
                            // ItemState = ItemStates
                        };
                        rowsToInsert.Add(code, inventory);
                    }
                }
            }
            Parallel.ForEach(rowsToUpdate.Values.ToList(), row =>
            {
                row.OpertationDate = now;
                inventoryService.Update(row);
            });
            List<InventoryRow> rows = rowsToInsert.Values.ToList();
            SqlHelper.InsertBulk(rows);

            ArchiveFile(filePath);
        }

        private static void ArchiveFile(string filePath)
        {
            IParameterProvider parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            string path = parameterProvider.GetValue<string>("InventFileArchive", @"ArchiveInvent");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string targetfile = path + @"\Invent_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".Loaded.txt";
            File.Move(filePath, targetfile);
            File.Delete(filePath);
        }
    }
}
