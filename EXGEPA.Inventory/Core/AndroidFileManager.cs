using System.IO;
using System.Linq;
using MediaDevices;

namespace EXGEPA.Inventory.Core
{
    public class AndroidFileManager : ADeviceFileManager
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AndroidFileManager() : base()
        {
        }

        public override bool DownloadFile(string destinationPath = tempFile)
        {
            this.logger.Info("Retrieving devices...");
            System.Collections.Generic.IEnumerable<MediaDevice> devices = MediaDevice.GetDevices();
            this.logger.Info($"{devices.Count()} device was found.");
            foreach (MediaDevice item in devices)
            {
                item.Connect();
                System.Collections.Generic.IEnumerable<string> folders = item.EnumerateDirectories(@"/");
                foreach (string folder in folders)
                {
                    foreach (string file in item.EnumerateFiles(folder))
                    {
                        if (file.ToLowerInvariant().Equals(TargetPath.ToLowerInvariant()))
                        {
                            FileStream stream = File.Open(TargetPath, FileMode.OpenOrCreate);
                            string path = Path.Combine(folder, file);
                            item.DownloadFile(path, stream);
                            stream.Close();
                            item.DeleteFile(path);
                            return true;
                        }
                    }
                }

                item.Disconnect();
            }
            return false;
        }
    }
}
