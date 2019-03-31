// <copyright file="ImageSourceToFilePathConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Repository.Controls
{
    using System;
    using System.IO;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;

    public class ImageSourceToFilePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string source = value.ToString();

                if (File.Exists(source))
                {
                    Uri uri = new Uri(source);
                    return new BitmapImage(uri);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is BitmapImage bitmap)
            {
                return bitmap.UriSource.LocalPath;
            }
            else
            {
                return null;
            }
        }
    }
}
