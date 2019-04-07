// <copyright file="TextAppender.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class TextAppender
    {
        public static void Append(string fileName, string text, bool deleteFileIfExist = false)
        {
            if (deleteFileIfExist && File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            string directory = Path.GetDirectoryName(fileName);
            Directory.CreateDirectory(directory);
            using (StreamWriter sr = File.AppendText(fileName))
            {
                sr.WriteLine(text);
            }
        }

        public static void Append(string fileName, IList<string> rows, bool deleteFileIfExist = false)
        {
            if (deleteFileIfExist && File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            string directory = Path.GetDirectoryName(fileName);
            Directory.CreateDirectory(directory);
            using (StreamWriter sr = File.AppendText(fileName))
            {
                for (int i = 0; i < rows.Count - 2; i++)
                {
                    sr.WriteLine(rows[i]);
                }

                sr.Write(rows.Last());
            }
        }

    }
}