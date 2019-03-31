using System.Reflection;

namespace CORESI.Tools
{
    public class RunTimeHelper
    {
        public static string GetApplicationName()
        {
            return Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}
