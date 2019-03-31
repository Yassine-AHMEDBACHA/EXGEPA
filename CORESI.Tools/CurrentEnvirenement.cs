// <copyright file="CurrentEnvirenement.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools
{
    using System;

    public class CurrentEnvirenement
    {
        public static readonly string ApplicationName = RunTimeHelper.GetApplicationName();

        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogEnvirenementInfo()
        {
            Logger.Info("***************************************************Environment Informations******************************************************************");
            Logger.Info("Current culture : " + System.Threading.Thread.CurrentThread.CurrentCulture);
            Logger.Info("Windows Login = " + Environment.UserName);
            Logger.Info("Machine Name = " + Environment.MachineName);
            Logger.Info("User Domain Name = " + Environment.UserDomainName);
            Logger.Info("MOSVersion = " + Environment.OSVersion);
            Logger.Info("64Bit OS = " + Environment.Is64BitOperatingSystem);
            Logger.Info("64Bit Process = " + Environment.Is64BitProcess);
            Logger.Info("Working Set = " + Environment.WorkingSet);
            Logger.Info("Processor Count = " + Environment.ProcessorCount);
            Logger.Info("Current Directory = " + Environment.CurrentDirectory);
            Logger.Info("*********************************************************************************************************************************************");
        }
    }
}
