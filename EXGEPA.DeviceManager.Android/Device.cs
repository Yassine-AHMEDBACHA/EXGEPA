using System.IO;
using MediaDevices;

namespace EXGEPA.DeviceManager.Android
{
    public class Device
    {
        private MediaDevice mediaDevice;
        public Device(MediaDevice mediaDevice)
        {
            this.mediaDevice = mediaDevice;
        }

        public void DownloadFile(string sourcepath, string targetPath, bool deleteSource = false)
        {
            FileStream stream = File.Open(targetPath, FileMode.OpenOrCreate);
            mediaDevice.DownloadFile(sourcepath, stream);
            stream.Close();
        }

        public void DeleteFile(string path)
        {
            mediaDevice.DeleteFile(path);
        }
    }
}