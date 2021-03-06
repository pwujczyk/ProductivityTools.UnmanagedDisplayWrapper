﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.UnmanagedDisplayWrapper.Native
{
    class Methods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern int EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        //main display 
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern DisplayChangeResult ChangeDisplaySettings(ref DEVMODE lpDevMode, ChangeDisplaySettingsFlags dwFlags);

        //external display
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static public  extern DisplayChangeResult ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        internal delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RectStruct lprcMonitor, IntPtr dwData);

        internal const int ENUM_CURRENT_SETTINGS = -1;
    }
}
