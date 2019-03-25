using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CORESI.IoC;
using CORESI.Tools;
using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;
using EXGEPA.Saidal.Core;

namespace EXGEPA.Saidal.Controls
{
    public class InterfaceViewModel : GenericEditableViewModel<Item>
    {
        readonly IRepositoryDataProvider repositoryDataProvider;

        public InterfaceViewModel(IExportable exportableView) : base(exportableView, false)
        {
            this.AutoWidth = false;
            this.Caption = "Module d'interface";
            LoadButtons(); ServiceLocator.Resolve(out this.repositoryDataProvider);
            this.InitData();
        }

        private void LoadButtons()
        {
            var aquisitionSeriliazer = new AquisitionSerializer();

            this.AddNewGroup().AddCommand("Aquisitions", () => aquisitionSeriliazer.Serialize(this.Selection));
        }

        public override void InitData()
        {
            StartBackGroundAction(() =>
            {
                using (var scoopLogger = new ScoopLogger("Loading Data", logger, true))
                {
                    var list = DBservice.SelectAll();
                    scoopLogger.Snap("Loading Data ");
                    Parallel.ForEach(list, repositoryDataProvider.BindItemFields);
                    ListOfRows = new ObservableCollection<Item>(list);
                }
            });
        }

    }
}