using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF;
using CORESI.WPF.Model;
using EXGEPA.Core.Interfaces;
using EXGEPA.Items.Controls;
using EXGEPA.Label.Core;
using EXGEPA.Model;
using System;
using System.ComponentModel.Composition;
using System.Windows.Media;
using CORESI.Tools;
using System.Collections.Generic;

namespace EXGEPA.Items
{
    [Export(typeof(IUIItemService)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class UIItemService : IUIItemService
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IUIService UIService { get; set; }
        IDataProvider<Item> ItemService { get; set; }
        public UIItemService()
        {
            logger.Debug("Starting composing UIItemService...");
            this.UIMessage = ServiceLocator.GetPriorizedInstance<IUIMessage>();
            this.UIService = ServiceLocator.Resolve<IUIService>();
            this.ItemService = ServiceLocator.Resolve<IDataProvider<Item>>();
            logger.Info("UIItemService is ready for use");
        }

        public Categorie ItemAttributionCategorie { get; private set; }
        public IUIMessage UIMessage { get; private set; }

        private Categorie printeLabelCategorie = new Categorie()
        {
            Caption = "Création des etiquettes",
            Color = Color.FromRgb(15, 15, 15)
        };

        public void AddNewItem()
        {
            UIMessage.TryDoUIActionAsync(logger, () =>
            {
                using (var scoopeLogger = new ScoopLogger("adding new Item", this.logger))
                {
                    var view = new Items.Controls.ItemView();
                    scoopeLogger.Snap("creating view");
                    var vm = new NewItemViewModel();
                    scoopeLogger.Snap("creating viewModel");
                    var page = new Page(vm, view);
                    scoopeLogger.Snap("creating page");
                    this.UIService.AddPage(page);
                    scoopeLogger.Snap("adding page");
                }
            });
        }

        public void EditItem(Item item)
        {
            var itemService = ServiceLocator.Resolve<IDataProvider<Item>>();
            logger.Info("loading Item data access");

            UIMessage.TryDoAction(logger, () =>
            {
                var view = new ItemView();

                var vm = new EditItemViewModel(item);

                var page = new Page(vm, view, true);

                this.UIService.AddPage(page);
            });
        }

        public void DisplayAllItems()
        {
            var view = new ItemGridView();
            var itemGridViewModel = new ItemGridViewModel(view);
            var page = new Page(itemGridViewModel, view, true);
            UIService.AddPage(page, true);
            itemGridViewModel.InitData();
        }

        public void ShowPrintLabelPanel()
        {
            var itemAttributionOptions = new ItemAttributionOptions()
            {
                Categorie = printeLabelCategorie,
                Resetter = (item) => item.PrintLabel = false,
                Setter = (item) => item.PrintLabel = true,
                Tester = (item) => item.PrintLabel == true,
                RightPanelCaption = "Etiquettes à imprimer",
                PageCaption = "Etiquettes d'articles"
            };
            itemAttributionOptions.Groups.Add(ReportProvider.GetItemLabelDialog());
            ShowItemAttribution(itemAttributionOptions);
        }

        public void ShowItemAttribution(ItemAttributionOptions itemAttributionOptions)
        {
            var view = new ItemAttributionView();
            var viewModel = new ItemAttributionVM(itemAttributionOptions);
            var page = new Page(viewModel, view, true);
            UIService.AddPage(page);
            viewModel.InitData();
        }

        public void DisplayItems(Predicate<Item> filter, string pageCaption, Action<IEnumerable<Item>> report)
        {
            var view = new ItemGridView();
            var viewModel = new ItemGridBaseViewModel(filter, view, report);
            viewModel.Caption = pageCaption;
            var page = new Page(viewModel, view, true);
            UIService.AddPage(page, true);
            
            viewModel.InitData();
        }
    }
}
