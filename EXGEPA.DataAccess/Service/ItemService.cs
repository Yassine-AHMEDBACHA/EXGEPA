using CORESI.Data;
using EXGEPA.Model;
using System.ComponentModel.Composition;

namespace EXGEPA.DataAccess.Service
{
    [Export(typeof(IDataProvider<Item>))]
    public class ItemService : AFilterdService<Item>
    { }
}