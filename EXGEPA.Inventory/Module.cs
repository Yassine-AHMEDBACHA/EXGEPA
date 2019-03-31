using CORESI.IoC;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Core.Interfaces;
using EXGEPA.Inventory.Controls;
using EXGEPA.Model;
using System.Linq;

namespace EXGEPA.Inventory
{

    public class Module : AModule
    {
        IUIItemService UIItemService { get; set; }


        public Module() : base()
        {
            UIItemService = ServiceLocator.Resolve<IUIItemService>();
        }

        public override int Priority
        {
            get { return 4; }
        }

        public override void AddGroups()
        {
            var InventoryGroup = new Group()
            {
                Caption = "Inventaire"
            };

            InventoryGroup.Commands.Add(new RibbonButton()
            {
                LargeGlyph = IconProvider.Inventory,
                Caption = "Consultation",
                Action = () =>
                {
                    var view = new InventoryView();
                    var inventoryViewModel = new InventoryViewModel(view);
                    var page = new Page(inventoryViewModel, view, true);
                    uIService.AddPage(page, true);
                    inventoryViewModel.InitData();
                }
            });

            //InventoryGroup.Commands.Add(new RibbonButton()
            //{
            //    LargeGlyph = IconProvider.Inventory,
            //    Caption = "Statistique",
            //    Action = () =>
            //    {
            //        var view = new InventoryStatisticsView();
            //        var inventoryViewModel = new InventoryStatisticsViewModel();
            //        var page = new Page(inventoryViewModel, view, true);
            //        uIService.AddPage(page, true);
            //    }
            //});

            InventoryGroup.Commands.Add(new RibbonButton()
            {
                IsSmall = true,
                Caption = "Articles",
                Glyph = IconProvider.BarCodeSmall,
                Action = () =>
                {
                    UIItemService.ShowPrintLabelPanel();
                }
            });
            IOfficeLabel officeLabel = ServiceLocator.Resolve<IOfficeLabel>();
            InventoryGroup.Commands.Add(new RibbonButton()
            {
                IsSmall = true,
                Caption = "Locaux",
                Glyph = IconProvider.BarCodeSmall,
                Action = () =>
                    {
                        var fields = CORESI.DataAccess.Core.PropertiesExtractor.ExtractFields(typeof(Office));
                        var labelPropertyInfo = fields.First(x => x.Name == "PrintLabel").PropertyInfo;
                        Group group = Label.Core.ReportProvider.GetOfficeLabelDialog();
                        officeLabel.ShowOfficeAttribution(labelPropertyInfo, true, false, group);
                    }
            });

            uIService.AddGroupToHomePage(InventoryGroup);
        }




    }
}
