using CORESI.Data;
using EXGEPA.Model;
using System.ComponentModel.Composition;

namespace EXGEPA.DataAccess.Service
{
    [Export(typeof(IDataProvider<InventoryRow>))]
    public class InventoryRowService : AFilterdService<InventoryRow>
    { }
}