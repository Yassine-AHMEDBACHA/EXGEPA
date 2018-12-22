using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.WPF.Tools
{
    public class SaveFile
    {

        public static string ShowDialog(string fileName, string Filter)
        {
            string path = null;
            var dlg = new SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

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

