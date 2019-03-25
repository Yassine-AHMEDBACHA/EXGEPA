using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
