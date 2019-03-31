using Microsoft.Win32;

namespace CORESI.WPF.Tools
{
    public class SaveFile
    {

        public static string ShowDialog(string fileName, string Filter)
        {
            string path = null;
            var dlg = new SaveFileDialog
            {
                FileName = "Document", // Default file name
                DefaultExt = ".text", // Default file extension
                Filter = "Text documents (.txt)|*.txt" // Filter files by extension
            };

            // Show save file dialog box
            var result = dlg.ShowDialog();
            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                path = dlg.FileName;
            }
            return path;
        }
    }
}

