using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.WPF.Model
{
    public abstract class EditSettingRibbon : RibbonButton
    {
        public EditSettingRibbon(string caption = "")
            : base(caption)
        {
            this.Width = 110;
        }

        public int _Width;
        public int Width
        {
            get { return _Width; }
            set
            {
                _Width = value;
                RaisePropertyChanged("Width");
            }
        }
    }
}
