﻿using CORESI.DataAccess.Core.Database;
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
            using (DbInitializer dbInitializer = new DbInitializer())
            {
                dbInitializer.AddSettings();
                dbInitializer.SetInitialRights();
            }
        }
    }
}
