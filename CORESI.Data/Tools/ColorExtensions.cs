using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  System.Windows.Media;


namespace CORESI.Data.Tools
{
   public static class ColorExtensions
    {
        public static Color ToMediaColor(this Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
