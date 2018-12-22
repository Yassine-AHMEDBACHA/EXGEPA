using CORESI.Data;
using CORESI.IoC;
using CORESI.Security;
using CORESI.WPF.Controls;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace EXGEPA.Settings.Controls
{
    public class OperatorViewModel : GenericEditableViewModel<Operator>
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ILoginManager<IOperator> LoginManager { get; set; }
        IDataProvider<Person> PersonService { get; set; }
        public OperatorViewModel() :  base()

        {
            this.Caption = "List des utilisateurs";
            this.LoginManager = ServiceLocator.Resolve<ILoginManager<IOperator>>();
            this.PersonService = ServiceLocator.Resolve<IDataProvider<Person>>();
            this.SetEditionGroup();
            //var resetPassword = new RibbonButton("Reinitialisé Mot de passe")
            //{
            //    LargeGlyph = IconProvider.BOPermission,
            //    Action = this.ResetPassword
            //};
            this.AddNewGroup().AddCommand("Reinitialisé Mot de passe", IconProvider.BOPermission, this.ResetPassword);
        }

        public override void InitData()
        {
            var users = this.DBservice.SelectAll().ToList();
            var persons = PersonService.SelectAll();
            users.ForEach(u =>
            {
                if (u.Person != null)
                {
                    u.Person = persons.FirstOrDefault(p => p.Id == u.Person.Id);
                }
            });
            ListOfPersons = new ObservableCollection<Person>(persons);
            ListOfRows = new ObservableCollection<Operator>(users);
        }

        public override void AddItem()
        {
            ConcernedRow = new Operator();
            this.ValidateCommand = new Command(() =>
            {
                StartBackGroundAction(() =>
                {
                    DBservice.Add(ConcernedRow);
                    GetTheNewPassword(ConcernedRow.Login);
                    this.DisplayDetail = false;
                    RefreshView(ConcernedRow);
                    RaiseDataChanged(ConcernedRow);
                });
            });
            RaisePropertiesChanged("ValidateCommand");
            this.DisplayDetail = true;

        }

        private ObservableCollection<Person> _ListOfPersons;

        public ObservableCollection<Person> ListOfPersons
        {
            get { return _ListOfPersons; }
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
                UIMessage.ConfirmeAndTryDoAction(logger, "Est vous sur de vouloir reinitialisé le mot de passe pour l'utilisateur : "
                    + SelectedRow.Name + "-" + SelectedRow.Login + "-", () => GetTheNewPassword(SelectedRow.Login));
            }
        }

        private void GetTheNewPassword(string login)
        {
            var newPassword = LoginManager.ResetPassword(login);
            UIMessage.Information("Le nouveau mot de passe est : " + newPassword);
        }



    }
}