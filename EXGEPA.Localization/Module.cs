using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows.Media;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Core.Interfaces;

namespace EXGEPA.Localization
{

    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class Module : AModule, IOfficeLabel
    {
        public override int Priority
        {
            get { return 6; }
        }

        private static Categorie printeLabelCategorie = new Categorie()
        {
            Caption = "Création des etiquettes",
            Color = Color.FromRgb(15, 15, 15)
        };

        public override void AddGroups()
        {
            Group localisation = new Group();
            localisation.Commands.Add(GetHomePageRibbonButton());
            UIService.AddGroupToHomePage(localisation);
        }

        private RibbonButton GetHomePageRibbonButton()
        {
            return new RibbonButton()
            {
                Caption = "Localisations",
                LargeGlyph = IconProvider.Localization,
                Action = () =>
                {
                    this.UIService.AddPage(GetModulePage(), true);
                }
            };
        }

        private Page GetModulePage()
        {
            Controls.LocalizationViewModel localizationViewModel = new EXGEPA.Localization.Controls.LocalizationViewModel();
            Controls.LocalizationControl localizationView = new EXGEPA.Localization.Controls.LocalizationControl();

            Page page = new Page(localizationViewModel, localizationView, true)
            {
                Caption = "Localisations",
            };

            Group siteGroup = new Group() { Caption = "Sites" };

            siteGroup.Commands.Add(localizationViewModel.SiteAddNewRibbonButton);
            siteGroup.Commands.Add(localizationViewModel.SiteEditRibbonButton);
            siteGroup.Commands.Add(localizationViewModel.SiteDeleteRibbonButton);

            page.Groups.Add(siteGroup);

            Group buildingGroup = new Group() { Caption = "Batiments" };

            buildingGroup.Commands.Add(localizationViewModel.BuildingAddNewRibbonButton);
            buildingGroup.Commands.Add(localizationViewModel.BuildingEditRibbonButton);
            buildingGroup.Commands.Add(localizationViewModel.BuildingDeleteRibbonButton);

            page.Groups.Add(buildingGroup);

            Group NiveauGroup = new Group() { Caption = "Niveaux" };
            NiveauGroup.Commands.Add(localizationViewModel.LevelAddNewRibbonButton);
            NiveauGroup.Commands.Add(localizationViewModel.LevelEditRibbonButton);
            NiveauGroup.Commands.Add(localizationViewModel.LevelDeleteRibbonButton);
            page.Groups.Add(NiveauGroup);

            Group OfficeGroup = new Group() { Caption = "Locaux" };
            OfficeGroup.Commands.Add(localizationViewModel.OfficeAddNewRibbonButton);
            OfficeGroup.Commands.Add(localizationViewModel.OfficeEditRibbonButton);
            OfficeGroup.Commands.Add(localizationViewModel.OfficeDeleteRibbonButton);
            page.Groups.Add(OfficeGroup);

            return page;
        }

        public void ShowOfficeAttribution(PropertyInfo property, object value, object defaultValue, Group group = null)
        {
            Controls.OfficeAttribution view = new Controls.OfficeAttribution();
            Controls.OfficeAttributionViewModel viewModel = new Controls.OfficeAttributionViewModel(property, true, false)
            {
                Caption = "Locaux"
            };
            Page page = new Page(viewModel, view)
            {
                Categorie = printeLabelCategorie
            };
            if (group != null)
                page.Groups.Add(group);
            UIService.AddPage(page);
            viewModel.InitData();
        }

        public override void InitializeModule()
        {
            DXControlInitializer.LoadComponent<Controls.OfficeAttribution>();
            DXControlInitializer.LoadComponent<Controls.LocalizationControl>();
        }
    }
}
