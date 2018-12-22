using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.WPF.Model
{
    public class CheckedRibbonButton : RibbonButton
    {
        public CheckedRibbonButton(string caption)
            : base(caption)
        { }

        public CheckedRibbonButton(string caption = "", bool isChecked = false)
            : base(caption)
        {
            this.IsChecked = isChecked;
        }
        private bool _IsChecked;

        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                _IsChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }

    }
}
