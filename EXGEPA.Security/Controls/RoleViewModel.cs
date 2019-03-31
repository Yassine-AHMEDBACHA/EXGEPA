using System.Collections.Generic;
using System.Linq;
using CORESI.Data;
using CORESI.Data.Tools;
using CORESI.IoC;
using CORESI.Security;
using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using CORESI.WPF.Model;

namespace EXGEPA.Security.Controls
{
    public class RoleViewModel : GenericEditableViewModel<Role>
    {
        public IDataProvider<Ability> AbilityService { get; private set; }
        public IDataProvider<Operation> OperationService { get; private set; }

        public IDataProvider<Resource> ResourceService { get; private set; }

        public RoleViewModel(IExportableGrid exportableView) : base(exportableView)
        {
            this.Caption = "List de roles";
            this.AbilityService = ServiceLocator.Resolve<IDataProvider<Ability>>();
            this.OperationService = ServiceLocator.Resolve<IDataProvider<Operation>>();
            this.ResourceService = ServiceLocator.Resolve<IDataProvider<Resource>>();
        }

        private string _PanelCaption;
        public string PanelCaption
        {
            get { return _PanelCaption; }
            set
            {
                _PanelCaption = value;
                RaisePropertyChanged("PanelCaption");
            }
        }

        public override void InitData()
        {
            base.InitData();
        }

        public override void EditItem()
        {
            if (this.SelectedRow != null)
            {
                this.ConcernedRow = (Role)SelectedRow.Clone();
                this.DisplayDetail = true;

                this.ValidateCommand = new Command(UpdateRole);
                RaisePropertyChangedForEditionPanel();
                RaisePropertyChanged("Abilities");
            }
            this.PanelCaption = $"Modification du role : {ConcernedRow?.Key}";
        }

        private void UpdateRole()
        {
            var oldRole = DBservice.GetById(ConcernedRow.Id);
            var allAbilities = oldRole.Abilities.Union(this.ConcernedRow.Abilities);
            var abilityToUpdate = allAbilities.GroupBy(x => x.Id).Where(g => g.Count() > 1 && g.First().HasAccess != g.Last().HasAccess).Select(g => this.ConcernedRow.Abilities.FirstOrDefault(x => x.Id == g.Key));

            foreach (var item in abilityToUpdate)
            {
                this.AbilityService.Update(item);
            }

            DBservice.Update(ConcernedRow);
            this.DisplayDetail = false;
            RefreshView(ConcernedRow);
            RaiseDataChanged(ConcernedRow);
        }

        private void AddRole()
        {
            if (!this.ConcernedRow.Key.IsValidData())
            {
                this.UIMessage.Error("Nom du role est invalide !");
                return;
            }
            this.DisplayDetail = false;
            this.StartBackGroundAction(() =>
            {
                this.DBservice.Add(this.ConcernedRow);
                foreach (var item in this.ConcernedRow.Abilities)
                {
                    this.AbilityService.Add(item);
                }

            }, () => NotifyUpdate(this, this.ConcernedRow), true);
        }



        public List<Ability> Abilities
        {
            get { return this.ConcernedRow?.Abilities; }
            set
            {
                this.ConcernedRow.Abilities = value;
                RaisePropertyChanged("Abilities");
            }
        }

        public override void AddItem()
        {
            this.ConcernedRow = new Role();
            this.Abilities = this.OperationService
                .SelectAll()
                .SelectMany(o => this.ResourceService
                                        .SelectAll()
                                        .Select(r => new Ability
                                        {
                                            HasAccess = true,
                                            Role = this.ConcernedRow,
                                            Operation = o,
                                            Resource = r
                                        }))
                            .ToList();
            this.PanelCaption = $"Nouveau role :";
            this.DisplayDetail = true;
            this.ValidateCommand = new Command(AddRole);
            RaisePropertyChangedForEditionPanel();
        }
    }
}