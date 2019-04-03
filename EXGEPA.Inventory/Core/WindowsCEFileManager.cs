namespace EXGEPA.Inventory.Core
{
    using System;
    using System.IO;
    using System.Windows;
    using OpenNETCF.Desktop.Communication;

    public class WindowsCEFileManager : ADeviceFileManager
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowsCEFileManager()
            : base()
        {
            try
            {
                this.RemoteAPI = default;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public RAPI RemoteAPI { get; set; }

        bool CheckConnexionWithDevice()
        {
            System.Reflection.Assembly[] s = AppDomain.CurrentDomain.GetAssemblies();

            bool connexionStatus = false;
            while (!RemoteAPI.DevicePresent)
            {
                MessageBoxResult userChoice = base.UIMessage.Warning("Aucun lecteur n'est detecté\nAssurez vous que le PDA est allumer et connecter à l'ordinateur puis cliquez sur OK\npour quitter l'opération appuyez sur annuler", MessageBoxButton.OKCancel);
                if (userChoice == System.Windows.MessageBoxResult.Cancel)
                {
                    break;
                }
            }
            connexionStatus = RemoteAPI.DevicePresent;
            return connexionStatus;
        }

        public override bool DownloadFile(string destinationPath = tempFile)
        {

            bool result = false;
            if (CheckConnexionWithDevice())
            {
                RemoteAPI.Connect();
                if (RemoteAPI.Connected)
                    if (RemoteAPI.DeviceFileExists(remoteFileName))
                    {

                        File.Delete(destinationPath);
                        RemoteAPI.CopyFileFromDevice(destinationPath, remoteFileName);
                        RemoteAPI.DeleteDeviceFile(remoteFileName);
                        result = true;
                    }
                    else
                    {
                        UIMessage.Information("Aucun Fichier inventaire n'est présent sur le PDA");
                    }

            }
            return result;
        }
    }
}
