using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Editors;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace CORESI.WPF.Controls
{
    public class ImageEdit : DevExpress.Xpf.Editors.ImageEdit
    {

        public static readonly DependencyProperty SourceDirectoryProperty = DependencyProperty
            .Register("SourceDirectory", typeof(string), typeof(ImageEdit), new PropertyMetadata(null));


        public static readonly DependencyProperty ImagePathProperty = DependencyProperty
            .Register("ImagePath", typeof(string), typeof(ImageEdit), new PropertyMetadata(ImagePathPropertyChnaged));

        private static void ImagePathPropertyChnaged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (ImageEdit)d;
            sender.SetImage((string)e.NewValue);
        }

        public string SourceDirectory
        {
            get
            {
                return GetValue(SourceDirectoryProperty).ToString();
            }
            set
            {
                SetValue(SourceDirectoryProperty, value);
            }
        }

        public string ImagePath
        {
            get
            {
                var result = GetValue(ImagePathProperty).ToString();

                return result;
            }
            set
            {
                SetValue(ImagePathProperty, value);
            }
        }

        private void SetImage(string fileName)
        {
            var image = GetImage(fileName);
            if (image != null)
                EditStrategy.SetImage(image);
        }

        protected override void LoadCore()
        {
            if (Image == null)
            {
                return;
            }

            var dlg = new OpenFileDialog
            {
                Filter = EditorLocalizer.GetString(EditorStringId.ImageEdit_OpenFileFilter)
            };
            if (dlg.ShowDialog() == true)
            {
                string path = dlg.FileName;
                this.ImagePath = path;
                SetImage(path);
            }
        }

        public override void Clear()
        {
            ImagePath = null;
            base.Clear();
        }

        private ImageSource GetImage(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return null;
            }

            using (Stream stream = File.Open(path, FileMode.Open))
            {
                MemoryStream ms = new MemoryStream(stream.GetDataFromStream());
                return ImageHelper.CreateImageFromStream(ms);
            }
        }
    }
}
