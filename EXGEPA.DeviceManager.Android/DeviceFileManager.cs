using System.Collections.Generic;
using System.Linq;
using MediaDevices;

namespace EXGEPA.DeviceManager.Android
{
    public class DeviceFileManager
    {

        private List<Device> devices;
        public DeviceFileManager()
        {
            this.Refresh();
        }

        public List<Device> Refresh()
        {
            this.devices = MediaDevice.GetDevices().Select(x => new Device(x)).ToList();
            return this.devices;
        }

        



    }
}