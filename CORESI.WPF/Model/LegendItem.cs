using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CORESI.WPF.Model
{
    public class LegendItem : SimpleItem
    {
        public LegendItem(string caption = null, Color? color = null) : base(caption)
        {
            this.Color = color ?? Color.FromRgb(255, 255, 255);
        }

        public Color Color { get; set; }
    }
}
