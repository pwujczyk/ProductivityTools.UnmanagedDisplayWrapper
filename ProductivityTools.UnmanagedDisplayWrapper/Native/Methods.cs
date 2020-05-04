using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.UnmanagedDisplayWrapper.Native
{
    class Methods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern int EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        //main one
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern Methods.DISP_CHANGE ChangeDisplaySettings(ref DEVMODE lpDevMode, ChangeDisplaySettingsFlags dwFlags);

        //external one
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static public  extern DISP_CHANGE ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);
        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //static extern DISP_CHANGE ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, uint dwflags, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        internal delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RectStruct lprcMonitor, IntPtr dwData);

        internal const int ENUM_CURRENT_SETTINGS = -1;
        //internal const int CDS_UPDATEREGISTRY = 0x01;
        //internal const int CDS_TEST = 0x02;
        //internal const int DISP_CHANGE_SUCCESSFUL = 0;
        //internal const int DISP_CHANGE_RESTART = 1;
        //internal const int DISP_CHANGE_FAILED = -1;

        public enum DISP_CHANGE : int
        {
            Successful = 0,
            Restart = 1,
            Failed = -1,
            BadMode = -2,
            NotUpdated = -3,
            BadFlags = -4,
            BadParam = -5,
            BadDualView = -6
        }
    }
  

    [Flags()]
    enum DM : int
    {
        Orientation = 0x1,
        PaperSize = 0x2,
        PaperLength = 0x4,
        PaperWidth = 0x8,
        Scale = 0x10,
        Position = 0x20,
        NUP = 0x40,
        DisplayOrientation = 0x80,
        Copies = 0x100,
        DefaultSource = 0x200,
        PrintQuality = 0x400,
        Color = 0x800,
        Duplex = 0x1000,
        YResolution = 0x2000,
        TTOption = 0x4000,
        Collate = 0x8000,
        FormName = 0x10000,
        LogPixels = 0x20000,
        BitsPerPixel = 0x40000,
        PelsWidth = 0x80000,
        PelsHeight = 0x100000,
        DisplayFlags = 0x200000,
        DisplayFrequency = 0x400000,
        ICMMethod = 0x800000,
        ICMIntent = 0x1000000,
        MediaType = 0x2000000,
        DitherType = 0x4000000,
        PanningWidth = 0x8000000,
        PanningHeight = 0x10000000,
        DisplayFixedOutput = 0x20000000
    }
}
