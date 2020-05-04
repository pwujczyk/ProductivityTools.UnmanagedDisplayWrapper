
using ProductivityTools.UnmanagedDisplayWrapper.Native;
using System;
using System.Runtime.InteropServices;

namespace ProductivityTools.UnmanagedDisplayWrapper
{
    public class Display
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DisplayDeviceStateFlags State { get; set; }
        public string DeviceId { get; set; }
        public string RegistryKey { get; set; }
        public string NameExtended { get; set; }
        public string DetailedDescription { get; set; }

        public string Availability { get; set; }
        public int ScreenHeight { get; set; }
        public int ScreenWidth { get; set; }
        public RectStruct MonitorArea { get; set; }
        public RectStruct WorkArea { get; set; }
    }
}
