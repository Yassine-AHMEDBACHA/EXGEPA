using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace CORESI.WPF.Model
{
    public class Page : UiNotifier
    {
        public Page(string caption) : this()
        {
            this.Caption = caption;
        }

        public Page()
        {
            Groups = new ObservableCollection<Group>();
        }

        public Page(IPageSetter pageSetter, UserControl userControl = null, bool pageSetterIsViewModel = true)
        {
            if (pageSetter == null)
                throw new Exception("Page setter is null");
            Caption = pageSetter.Caption;
            this.IsSelected = pageSetter.IsSelected;
            this.Categorie = pageSetter.Categorie;
            Groups = pageSetter.Groups;
            if (userControl != null)
            {
                this.UserControl = userControl;
                if (pageSetterIsViewModel)
                {
                    userControl.DataContext = pageSetter;
                }
            }

            pageSetter.ClosePage = () => this.Close();
        }
        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public Action Close { get; set; }

        public Color Color { get; set; }

        public Categorie Categorie { get; set; }

        public string Caption { get; set; }
        public ObservableCollection<Group> Groups { get; set; }

        private System.Windows.Controls.UserControl _UserControl;  

        public System.Windows.Controls.UserControl UserControl
        {
            get { return _UserControl; }
            private set
            {
                _UserControl = value;
                RaisePropertyChanged("UserControl");
            }
        }

    }
}
