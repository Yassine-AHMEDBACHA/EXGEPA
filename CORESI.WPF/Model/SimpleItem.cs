namespace CORESI.WPF.Model
{
    public class SimpleItem : UiNotifier
    {
        public SimpleItem(string caption = "")
        {
            this.Caption = caption;
            IsVisible = true;
        }
        public string Caption { get; set; }

        private bool _IsVisible;
        public bool IsVisible
        {
            get => _IsVisible;
            set
            {
                _IsVisible = value;
                RaisePropertyChanged("IsVisible");
            }
        }
    }
}
