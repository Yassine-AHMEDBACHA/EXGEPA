using CORESI.DataAccess.Core.Database;
using EXGEPA.Core.Database;

namespace EXGEPA.Transfert.Core
{
    public static class TransfertManager
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void StartDbLoading()
        {
            Loader loader = new Loader();
            DbBuilder.BuildNewDatabase();
            using (DbInitializer dbInitializer = new DbInitializer())
            {
                logger.Info("Loading rights");
                dbInitializer.SetInitialRights();
                logger.Info("Loading settings");
                dbInitializer.AddSettings();
                loader.Load();
            }
        }
    }
}
