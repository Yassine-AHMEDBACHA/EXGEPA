﻿using System.Collections.ObjectModel;
using System.Linq;
using CORESI.Data;
using CORESI.IoC;
using CORESI.Security;
using CORESI.Tools;
using CORESI.Tools.Collections;
using CORESI.WPF.Controls;
using CORESI.WPF.Core;
using CORESI.WPF.Core.Interfaces;
using CORESI.WPF.Model;
using EXGEPA.Model;

namespace EXGEPA.Security.Controls
{
    public class OperatorViewModel : GenericEditableViewModel<Operator>
    {
        ILoginManager<IOperator> LoginManager { get; set; }

        public IDataProvider<Person> PersonService { get; private set; }
        
        public IDataProvider<Role> RoleService { get; private set; }

        private ObservableCollection<Role> _ListOfRoles;

        public ObservableCollection<Role> ListOfRoles
        {
            get => _ListOfRoles;
            set
            {
                _ListOfRoles = value;
                RaisePropertyChanged("ListOfRoles");
            }
        }


        public OperatorViewModel(IExportableGrid view) : base(view, false)
        {
            this.Caption = "List des utilisateurs";
            this.LoginManager = ServiceLocator.Resolve<ILoginManager<IOperator>>();
            this.PersonService = ServiceLocator.Resolve<IDataProvider<Person>>();
            this.RoleService = ServiceLocator.Resolve<IDataProvider<Role>>();
            //this.SetEditionGroup();
            //this.SetToolGroup();
            var command = this.AddNewGroup().AddCommand("Reinitialisé Mot de passe", IconProvider.BOPermission, this.ResetPassword);
            command.SetAbility("modifier");
            var ability = this.Groups.SelectMany(x=>x.Commands)
                .OfType<SimpleButton>()
                .FirstOrDefault(x => x.Caption.ContainsString("modifier"))
                ?.CommandAction
                .Ability;

            command.SetAbility(ability);
        }

        public override void InitData()
        {
            var users = this.DBservice.SelectAll().ToList();
            var persons = PersonService.SelectAll();
            this.ListOfRoles = new ObservableCollection<Role>(this.RoleService.SelectAll());
            users.ForEach(u =>
            {
                if (u.Person != null)
                {
                    u.Person = persons.FirstOrDefault(p => p.Id == u.Person.Id);
                }
                if (u.Role != null)
                {
                    u.Role = ListOfRoles.FirstOrDefault(p => p.Id == u.Role.Id);
                }
            });

            ListOfPersons = persons.ToObservable();
            ListOfRows = users.ToObservable();
        }

        public override void AddItem()
        {
            ConcernedRow = new Operator();
            this.ValidateCommand = new Command(() =>
            {
                StartBackGroundAction(() =>
                {
                    this.ConcernedRow.Password = new StringCryptor().Crypte("$");
                    DBservice.Add(ConcernedRow);
                    GetTheNewPassword(ConcernedRow.Key);
                    this.DisplayDetail = false;
                    RefreshView(ConcernedRow);
                    RaiseDataChanged(ConcernedRow);
                });
            });
            RaisePropertyChanged("ValidateCommand");
            this.DisplayDetail = true;
        }

        private ObservableCollection<Person> _ListOfPersons;

        public ObservableCollection<Person> ListOfPersons
        {
            get => _ListOfPersons;
            set
            {
                _ListOfPersons = value;
                RaisePropertyChanged("ListOfPersons");
            }
        }

        private void ResetPassword()
        {
            if (SelectedRow != null)
            {
                this.UIMessage.ConfirmeAndTryDoAction(Logger, "Est vous sur de vouloir reinitialisé le mot de passe pour l'utilisateur : "
                    + SelectedRow.Name + "-" + SelectedRow.Key + "-", () => GetTheNewPassword(SelectedRow.Key));
            }
        }

        private void GetTheNewPassword(string login)
        {
            string newPassword = LoginManager.ResetPassword(login);
            this.UIMessage.Information("Le nouveau mot de passe est : " + newPassword);
        }
    }
}