using System.Windows.Media;



namespace CORESI.WPF.Model
{
    public class RibbonButton : SimpleButton
    {
        public RibbonButton(string caption = null)
            : base(caption)
        { }


        public ImageSource LargeGlyph { get; set; }
        public ImageSource Glyph { get; set; }
        public bool IsSmall { get; set; }
    }
}
