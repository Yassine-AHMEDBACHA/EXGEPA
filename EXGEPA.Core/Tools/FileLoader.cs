using System.Collections.Generic;
using System.IO;

namespace EXGEPA.Core.Tools
{
    public class FileLoader
    {
        public static List<string> LoadTextRows(string filePath)
        {
            List<string> result = new List<string>();
            var data = string.Empty;
            using (var streamReader = File.OpenText(filePath))
            {
                while ((data = streamReader.ReadLine()) != null)
                {
                    result.Add(data);
                }
            }

            return result;
        }
    }
}