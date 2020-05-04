using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProductivityTools.UnmanagedDisplayWrapper
{
    //public class XXX
    //{
    //    [DllImport("user32.dll")]
    //    static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

    //    [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    //    internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);


    //    delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RectStruct lprcMonitor, IntPtr dwData);

    //    //[StructLayout(LayoutKind.Sequential)]
    //    //public struct Rect
    //    //{
    //    //    public int left;
    //    //    public int top;
    //    //    public int right;
    //    //    public int bottom;
    //    //}

    //    /// <summary>
    //    /// The struct that contains the display information
    //    /// </summary>
    //    public class DisplayInfo
    //    {
    //        public string Availability { get; set; }
    //        public string ScreenHeight { get; set; }
    //        public string ScreenWidth { get; set; }
    //        public RectStruct MonitorArea { get; set; }
    //        public RectStruct WorkArea { get; set; }
    //    }

    //    /// <summary>
    //    /// Collection of display information
    //    /// </summary>
    //    public class DisplayInfoCollection : List<DisplayInfo>
    //    {
    //    }

    //    /// <summary>
    //    /// Returns the number of Displays using the Win32 functions
    //    /// </summary>
    //    /// <returns>collection of Display Info</returns>
    //    public DisplayInfoCollection GetDisplays()
    //    {
    //        DisplayInfoCollection col = new DisplayInfoCollection();

    //        EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
    //            delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref RectStruct lprcMonitor, IntPtr dwData)
    //            {
    //                MonitorInfoEx mi = new MonitorInfoEx();
    //                 mi.Size = (uint)Marshal.SizeOf(mi);
    //                bool success = GetMonitorInfo(hMonitor, ref mi);
    //                if (success)
    //                {
    //                    DisplayInfo di = new DisplayInfo();
    //                    di.ScreenWidth = (mi.Monitor.Right - mi.Monitor.Left).ToString();
    //                    di.ScreenHeight = (mi.Monitor.Bottom - mi.Monitor.Top).ToString();
    //                    di.MonitorArea = mi.Monitor;
    //                    di.WorkArea = mi.WorkArea;
    //                    di.Availability = mi.Flags.ToString();
    //                    col.Add(di);
    //                }
    //                return true;
    //            }, IntPtr.Zero);
    //        return col;
    //    }
    //}
}
