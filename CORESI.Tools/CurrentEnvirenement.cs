using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.Tools
{
    public class CurrentEnvirenement
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void LogEnvirenementInfo()
        {
            logger.Info("***************************************************Environment Informations******************************************************************");
            logger.Info("Current culture : " + System.Threading.Thread.CurrentThread.CurrentCulture);
            logger.Info("Windows Login = " + Environment.UserName);
            logger.Info("Machine Name = " + Environment.MachineName);
            logger.Info("User Domain Name = " + Environment.UserDomainName);
            logger.Info("MOSVersion = " + Environment.OSVersion);
            logger.Info("64Bit OS = " + Environment.Is64BitOperatingSystem);
            logger.Info("64Bit Process = " + Environment.Is64BitProcess);
            logger.Info("Working Set = " + Environment.WorkingSet);
            logger.Info("Processor Count = " + Environment.ProcessorCount);
            logger.Info("Current Directory = " + Environment.CurrentDirectory);
            logger.Info("*********************************************************************************************************************************************");
        }


    }
}
