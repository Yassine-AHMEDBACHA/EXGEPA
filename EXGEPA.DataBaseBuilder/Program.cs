using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CORESI.DataAccess.Core.Database;
using EXGEPA.Core.Database;
using log4net.Config;

namespace EXGEPA.DataBaseBuilder
{
    class Program
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            DbBuilder.BuildNewDatabase();
            using (var dbInitializer = new DbInitializer())
            {
                dbInitializer.AddSettings();
                dbInitializer.SetInitialRights();
            }
        }
    }
}
