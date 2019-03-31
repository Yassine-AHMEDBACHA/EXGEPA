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
            var devices = MediaDevice.GetDevices();
            this.logger.Info($"{devices.Count()} device was found.");
            foreach (var item in devices)
            {
                item.Connect();
                var folders = item.EnumerateDirectories(@"/");
                foreach (var folder in folders)
                {
                    foreach (var file in item.EnumerateFiles(folder))
                    {
                        if (file.ToLowerInvariant().Equals(targetPath.ToLowerInvariant()))
                        {
                            var stream = File.Open(targetPath, FileMode.OpenOrCreate);
                            var path = Path.Combine(folder, file);
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
