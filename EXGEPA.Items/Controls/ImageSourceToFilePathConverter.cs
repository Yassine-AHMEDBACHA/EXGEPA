using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace EXGEPA.Items.Controls
{
    public class ImageSourceToFilePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value != null)
            {
                var source = value.ToString();

                if (File.Exists(source))
                {
                    var uri = new Uri(source);
                    return new BitmapImage(uri);
                }
                else return null;

            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is BitmapImage bitmap)
            {
                return bitmap.UriSource.LocalPath;
            }
            else return null;
        }
    }
}
