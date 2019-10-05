using CORESI.WPF.Model;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace CORESI.WPF.Core.Shell
{
    [Export(typeof(ShellViewModel)), PartCreationPolicy(CreationPolicy.Shared)]
    public class ShellViewModel : UiNotifier
    {


        private bool _IsWaintingCursor;

        internal const string Copyright = "Copyright © SARL CORESI 2020";

        public bool IsWaintingCursor
        {
            get => _IsWaintingCursor;
            set
            {
                _IsWaintingCursor = value;
                RaisePropertyChanged("IsWaintingCursor");
            }
        }

        private string _CopyRight;
        public string CopyRight
        {
            get => _CopyRight;
            set
            {
                _CopyRight = value;
                RaisePropertyChanged("CopyRight");
            }
        }

        private ClientInformation _ClientInformation;

        public ClientInformation ClientInformation
        {
            get => _ClientInformation;
            set
            {
                _ClientInformation = value;
                RaisePropertyChanged("ClientInformation");
            }
        }

        public string CurrentOperator => $"{this.ClientInformation.Name} : {this.ClientInformation.Role.Key}";

        private string _WindowTitle;

        public string WindowTitle
        {
            get => _WindowTitle;
            set { _WindowTitle = value; RaisePropertyChanged("WindowTitle"); }
        }

        public ICommand GotFocus { get; set; }
        public ICommand UpdateView { get; set; }
        private Page _CurrentPage;

        public Page CurrentPage
        {
            get => _CurrentPage;
            set
            {

                _CurrentPage = value;

                RaisePropertyChanged("CurrentPage");
            }
        }


        //public Page CurrentPage { get; set; }
        private System.Windows.Controls.UserControl _UserControl;

        public System.Windows.Controls.UserControl UserControl
        {
            get => _UserControl;
            set
            {
                _UserControl = value;
                RaisePropertyChanged("UserControl");
            }
        }

        public Categorie DefaultCategory { get; set; }
        public ObservableCollection<Categorie> Categories { get; set; }
        public Page HomePage { get; set; }
        public ShellViewModel()
        {
            HomePage = new Page("Acceuil");
            Group exitGroup = HomePage.AddNewGroup();
            exitGroup.AddCommand("Quitter", IconProvider.Close, () => Application.Current.Shutdown());
            Categories = new ObservableCollection<Categorie>();
            DefaultCategory = new Categorie();
            DefaultCategory.Pages.Add(HomePage);
            Categories.Add(DefaultCategory);
            UpdateView = new DelegateCommand<Page>(SwitchView);
            this.CopyRight = Copyright;
        }

        void SwitchView(Page page)
        {
            if (page != null)
            {
                if (page != HomePage)
                {
                    this.UserControl = page.UserControl;
                }

                this.CurrentPage = page;
            }
        }
    }
}