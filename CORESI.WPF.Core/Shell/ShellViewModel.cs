using CORESI.WPF.Model;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;

namespace CORESI.WPF.Core.Shell
{
    [Export(typeof(ShellViewModel)), PartCreationPolicy(CreationPolicy.Shared)]
    public class ShellViewModel : UiNotifier
    {

        private bool _IsWaintingCursor;

        public bool IsWaintingCursor
        {
            get { return _IsWaintingCursor; }
            set
            {
                _IsWaintingCursor = value;
                RaisePropertyChanged("IsWaintingCursor");
            }
        }

        private string _CopyRight;
        public string CopyRight
        {
            get
            {
                return _CopyRight;
            }
            set
            {
                _CopyRight = value;
                RaisePropertyChanged("CopyRight");
            }
        }

        private ClientInformation _ClientInformation;

        public ClientInformation ClientInformation
        {
            get { return _ClientInformation; }
            set
            {
                _ClientInformation = value;
                RaisePropertyChanged("ClientInformation");
            }
        }
        private string _WindowTitle;

        public string WindowTitle
        {
            get { return _WindowTitle; }
            set { _WindowTitle = value; RaisePropertyChanged("WindowTitle"); }
        }

        public ICommand GotFocus { get; set; }
        public ICommand UpdateView { get; set; }
        private Page _CurrentPage;

        public Page CurrentPage
        {
            get { return _CurrentPage; }
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
            get { return _UserControl; }
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
            var exitGroup = HomePage.AddNewGroup();
            exitGroup.AddCommand("Quitter", IconProvider.Close, () => Application.Current.Shutdown());
            Categories = new ObservableCollection<Categorie>();
            DefaultCategory = new Categorie();
            DefaultCategory.Pages.Add(HomePage);
            Categories.Add(DefaultCategory);
            UpdateView = new DelegateCommand<Page>(SwitchView);
            this.CopyRight = "CORESI � 2019";
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