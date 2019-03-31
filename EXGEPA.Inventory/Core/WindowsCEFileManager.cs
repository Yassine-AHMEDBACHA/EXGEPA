using System;
using System.IO;
using System.Linq;
using System.Windows;
using OpenNETCF.Desktop.Communication;

namespace EXGEPA.Inventory.Core
{
    public class WindowsCEFileManager : ADeviceFileManager
    {

        public RAPI RemoteAPI { get; set; }
        public WindowsCEFileManager() : base()
        {
            this.RemoteAPI = new RAPI();
        }
        bool CheckConnexionWithDevice()
        {
            var s = AppDomain.CurrentDomain.GetAssemblies();
            var t = s.ToArray();

            bool connexionStatus = false;
            while (!RemoteAPI.DevicePresent)
            {
                var userChoice = base.UIMessage.Warning("Aucun lecteur n'est detecté\nAssurez vous que le PDA est allumer et connecter à l'ordinateur puis cliquez sur OK\npour quitter l'opération appuyez sur annuler", MessageBoxButton.OKCancel);
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
