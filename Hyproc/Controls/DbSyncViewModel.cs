using CORESI.Data;
using CORESI.Data.Tools;
using CORESI.IoC;
using CORESI.WPF.Core;
using CORESI.WPF.Core.Framework;
using CORESI.WPF.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hyproc.Controls
{
    public class DbSyncViewModel : PageViewModel
    {
        public DbSyncViewModel()
        {
            this.Caption = "Hyproc";
            this.DataSource = "Localhost";
            this.UseWindowsAutentification = true;
            this.TestConnexionString = new Command(testConnextion);
            LaunchSync = new Command(sync);
            Group saveDbGroup = new Group("Sauvgarde");
            Group restorDbGroup = new Group("Restoration");
            this.Groups.Add(saveDbGroup);
            this.Groups.Add(restorDbGroup);
            saveDbGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Base local",
                Action = () =>
                {
                    Core.DataBaseContent.BackupDataBase();
                }
            });

            saveDbGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Base distante",
                Action = () =>
                {

                    IDbFacade dbfacade = ServiceLocator.Resolve<IDbFacade>();
                    string connextionString = this.GetConnextionString(this.DataBase);
                    if (dbfacade.TestConnection(connextionString))
                    {
                        IDbFacade newDbFacade = dbfacade.ChangeDB(connextionString);
                        Core.DataBaseContent.BackupDataBase(newDbFacade);
                    }
                }
            });

            this.AddNewGroup().AddCommand("Options", IconProvider.Settings, this.ShowOptionWindow);


        }

        private void ShowOptionWindow()
        {
            ClientInformation result = this.UIService.ShowLoginWindow();
            if (result != null)
                if (result.Login.ToLower() == "admin")
                {
                    HyprocSettingViewModel viewModel = new HyprocSettingViewModel();
                    HyprocSettings view = new HyprocSettings()
                    {
                        DataContext = viewModel
                    };
                    viewModel.CloseWindow = view.Close;
                    view.ShowDialog();
                }
                else
                    this.UIMessage.Information("Vous n'avez pas les droits pour continuer !");
        }

        private void sync()
        {
            IDbFacade dbfacade = ServiceLocator.Resolve<IDbFacade>();
            string connextionString = this.GetConnextionString(this.DataBase);
            AskForSync(dbfacade, connextionString);
        }

        //UseWindowsAutentification,
        public Command TestConnexionString { get; set; }
        public Command LaunchSync { get; set; }

        private void testConnextion()
        {

            this.UIMessage.TryDoAction(Logger, () =>
            {
                IDbFacade dbFacade = ServiceLocator.Resolve<IDbFacade>();
                string connextionString = this.GetConnextionString(this.DataBase);
                if (dbFacade.TestConnection(connextionString))
                {
                    IDbFacade remotedbFacade = dbFacade.ChangeDB(connextionString);
                    this.UIMessage.Information("Test reussit");
                    this.AvailableDataBase = remotedbFacade.ExecuteReader<string>("SELECT name  FROM sys.databases  where database_id>4 ", (dr) => dr["name"].ToString());
                }
                else
                    this.UIMessage.Error("Test echoué");
            });
        }

        private void AskForSync(IDbFacade dbFacade, string connextionString)
        {
            this.UIMessage.ConfirmeAndTryDoAction(Logger, "Est vous sur de vouloir lancer la synchronisation avec la base distante ?",
                                () =>
                                {
                                    this.ProgressBarVisible = true;
                                    IParameterProvider parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
                                    IDbFacade newDbFacade = dbFacade.ChangeDB(connextionString);
                                    System.Collections.IDictionary allParameters = parameterProvider.GetAllParameters();
                                    this.ProgressValue = 5;
#if !DEBUG
                                    Core.DataBaseContent.BackupDataBase();
                                    this.ProgressValue = 15;
                                    Core.DataBaseContent.BackupDataBase(newDbFacade);
                                    this.ProgressValue = 30;
#endif

                                    this.Synchronize(newDbFacade);

#if !DEBUG
                                    this.ProgressValue = 92;
                                    string path = Core.DataBaseContent.BackupDataBase(newDbFacade);
                                    this.ProgressValue = 98;
                                    Core.DataBaseContent.RestorDataBase(path);
                                    File.Delete(path);
#endif

                                    foreach (object key in allParameters.Keys)
                                    {
                                        parameterProvider.TrySetOrAdd(key.ToString(), allParameters[key]);
                                    }
                                    this.ProgressValue = 0;
                                    this.ProgressBarVisible = false;
                                }, true);
        }

        private string GetConnextionString(string dataBase = null)
        {

            DataSource.CheckData("Data source");
            string connexionString = "Data Source=" + this.DataSource + ";";
            if (dataBase != null)
                connexionString += "Initial Catalog=" + dataBase + ";";
            if (this.UseWindowsAutentification)
            {
                connexionString += "Integrated Security=True;";
            }
            else
            {
                UserName.CheckData("UserName");
                Password.CheckData("Password");
                connexionString += "User ID=" + UserName + "; Password=" + Password + ";";
            }
            return connexionString;

        }


        private void Synchronize(IDbFacade dbFacade)
        {
            List<Core.IAnlayzer> tables = Core.DataBaseContent.GetListOfAnalyzer();
            int step = 80 / tables.Count();
            foreach (Core.IAnlayzer item in tables)
            {
                item.UpdateDatabase(dbFacade);
                ProgressValue += step;
            }
        }

        private int _ProgressValue;

        public int ProgressValue
        {
            get => _ProgressValue;
            set
            {
                _ProgressValue = value;
                RaisePropertyChanged("ProgressValue");
            }
        }


        private bool _ProgressBarVisible;

        public bool ProgressBarVisible
        {
            get => _ProgressBarVisible;
            set
            {
                _ProgressBarVisible = value;
                RaisePropertyChanged("ProgressBarVisible");
            }
        }

        private string _DataSource;

        public string DataSource
        {
            get => _DataSource;
            set
            {
                _DataSource = value;
                RaisePropertyChanged("DataSource");
            }
        }

        private string _DataBase;
        public string DataBase
        {
            get => _DataBase;
            set
            {
                _DataBase = value;
                RaisePropertyChanged("DataBase");
            }
        }

        private string _UserName;

        public string UserName
        {
            get => _UserName;
            set
            {
                _UserName = value;
                RaisePropertyChanged("UserName");
            }

        }

        private string _Password;

        public string Password
        {
            get => _Password;
            set
            {
                _Password = value;
                RaisePropertyChanged("Password");
            }
        }

        private List<string> _AvailableDataBase;

        public List<string> AvailableDataBase
        {
            get => _AvailableDataBase;
            set
            {
                _AvailableDataBase = value;
                RaisePropertyChanged("AvailableDataBase");
            }
        }

        private bool _UseWindowsAutentification;

        public bool UseWindowsAutentification
        {
            get => _UseWindowsAutentification;
            set
            {
                _UseWindowsAutentification = value;
                RaisePropertyChanged("UseWindowsAutentification");
            }
        }
    }
}