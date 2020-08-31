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
            Group InventoryGroup = new Group()
            {
                Caption = "Inventaire"
            };

            InventoryGroup.Commands.Add(new RibbonButton()
            {
                LargeGlyph = IconProvider.Inventory,
                Caption = "Consultation",
                Action = () =>
                {
                    InventoryView view = new InventoryView();
                    InventoryViewModel inventoryViewModel = new InventoryViewModel(view);
                    Page page = new Page(inventoryViewModel, view, true);
                    UIService.AddPage(page, true);
                    inventoryViewModel.InitData();
                }
            });

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
                        var group = Label.Core.ReportProvider.GetOfficeLabelDialog();
                        officeLabel.ShowOfficeAttribution(labelPropertyInfo, true, false, group);
                    }
            });

            UIService.AddGroupToHomePage(InventoryGroup);
        }




    }
}
