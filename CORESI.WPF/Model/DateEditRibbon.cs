using System;

namespace CORESI.WPF.Model
{
    public class DateEditRibbon : EditSettingRibbon
    {

       

        public DateEditRibbon(string caption = "")
            : base(caption)
        {
          
        }

        private DateTime _Date;

        public DateTime Date
        {
            get { return _Date; }
            set
            {
                _Date = value;
                RaisePropertyChanged("Date");
            }
        }
    }
}
