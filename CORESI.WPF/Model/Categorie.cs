using System.Collections.ObjectModel;
using System.Windows.Media;

namespace CORESI.WPF.Model
{
    public class Categorie : UiNotifier
    {
        public Categorie()
        {
            Pages = new ObservableCollection<Page>();
        }
        public Categorie(string caption, Color? color = null) : this()
        {
            this.Caption = caption;
            this.Color = color ?? Color.FromRgb(120, 120, 200);
        }
        public string Caption { get; set; }
        public Color Color { get; set; }
        public ObservableCollection<Page> Pages { get; set; }
        public bool IsDefault { get; set; }
        public string Name { get; set; }
    }
}
