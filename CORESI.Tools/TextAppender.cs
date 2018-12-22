using System.IO;

namespace CORESI.Tools
{
    public class TextAppender
    {
        public static void Append(string fileName, string text, bool deleteFileIfExist = false)
        {
            if (deleteFileIfExist)
            {
                File.Delete(fileName);
            }

            using (var sr = File.AppendText(fileName))
            {
                sr.WriteLine(text);
            }
        }
    }
}
