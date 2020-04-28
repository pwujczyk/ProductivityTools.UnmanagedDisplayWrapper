using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.UnmanagedDisplayWrapper.Native
{
    class Methods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE lpDevMode, int dwFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        public const int ENUM_CURRENT_SETTINGS = -1;
        public const int CDS_UPDATEREGISTRY = 0x01;
        public const int CDS_TEST = 0x02;
        public const int DISP_CHANGE_SUCCESSFUL = 0;
        public const int DISP_CHANGE_RESTART = 1;
        public const int DISP_CHANGE_FAILED = -1;

    }
}
