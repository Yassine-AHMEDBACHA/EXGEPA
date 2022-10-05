namespace EXGEPA.Saidal.Controls
{
    using System;
    using System.Linq;
    using CORESI.IoC;
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core;
    using CORESI.WPF.Core.Interfaces;
    using CORESI.WPF.Model;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Model;
    using EXGEPA.Saidal.Core;

    public abstract class InterfaceVMBase<T> : GenericEditableViewModel<T>
        where T : Certificate
    {
        protected const string ButtonCaption = "Générer le fichier d'interface";

        public InterfaceVMBase(IExportableGrid exportableView, string entityName)
        {
            this.RemoveEditionGroup();
            this.RepositoryDataProvider = ServiceLocator.Resolve<IRepositoryDataProvider>();
            this.AutoWidth = false;
            this.SetToolGroup();
            this.SetExportGroup(exportableView);
            this.HideAddButton = true;
            this.HideEditButton = true;
            this.HideDeleteButton = true;
            this.Caption = $"Interface : {entityName}";
            this.AddRibbonButtons();
            this.StartDateEditRibbon.Date = DateTime.Today.AddMonths(-1);
            this.EndDateEditRibbon.Date = DateTime.Today;
            var sttingsGroup = this.AddNewGroup("Parametres", InsertPosition.Left);
            sttingsGroup.Commands.Add(this.StartDateEditRibbon);
            sttingsGroup.Commands.Add(this.EndDateEditRibbon);
        }

        public abstract Serializer<T> Serializer { get; }

        public DateEditRibbon StartDateEditRibbon { get; } = new DateEditRibbon("Date début");

        public DateEditRibbon EndDateEditRibbon { get; } = new DateEditRibbon("Date fin");

        protected IRepositoryDataProvider RepositoryDataProvider { get; }

        protected void AddRibbonButtons()
        {
            this.AddNewGroup().AddCommand(ButtonCaption, IconProvider.Download, () =>
            {
                var result = this.Serializer.Serialize(this.Selection);
                if (result?.Count > 0)
                {
                    result.ForEach(x => this.DBservice.Update(x));
                    this.InitData();
                    this.UIMessage.Notify("Fichier généré avec succès");
                }
            });
        }

        protected abstract bool IsToDisplay(T instance);

        private void RemoveEditionGroup()
        {
            var group = this.Groups.FirstOrDefault(x => x.Commands.Any(c => c.Caption.Equals("Edition")));
            if (group != null)
            {
                this.Groups.Remove(group);
            }
        }
    }
}
