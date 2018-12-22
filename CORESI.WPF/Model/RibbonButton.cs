using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using CORESI.WPF.Model;



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
