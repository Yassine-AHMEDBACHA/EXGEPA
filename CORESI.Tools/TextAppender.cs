// <copyright file="TextAppender.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools
{
    using System.IO;

    public class TextAppender
    {
        public static void Append(string fileName, string text, bool deleteFileIfExist = false)
        {
            if (deleteFileIfExist && File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var directory = Path.GetDirectoryName(fileName);
            Directory.CreateDirectory(directory);
            using (var sr = File.AppendText(fileName))
            {
                sr.WriteLine(text);
            }
        }
    }
}