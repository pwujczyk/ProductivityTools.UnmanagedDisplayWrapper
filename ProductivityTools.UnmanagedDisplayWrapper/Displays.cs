using ProductivityTools.UnmanagedDisplayWrapper.Native;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProductivityTools.UnmanagedDisplayWrapper
{
    public class Displays : List<Display>
    {
        //List<Display> ConnectedDisplays { get; set; }

        public void LoadData()
        {
            LoadBasicInfo();
            LoadResolution();
        }

        public void MoveMainToLeft()
        {
            LoadData();
            ChangePosition(this[0].Name, -1 * this[0].MonitorArea.Right, 0);
        }

        //public void MoveExternalToLeft()
        //{
        //    LoadData();
        //    ChangePosition(this[1].Name, -1600, 0);
        //}

        public void MoveMainDisplayToRight()
        {
            LoadData();
            ChangePosition(this[0].Name, this[0].MonitorArea.Right, 0);
        }

        //public void MoveExternalDisplayToRight()
        //{
        //    LoadData();
        //    ChangePosition(this[1].Name, this[1].MonitorArea.Right, 0);
        //}

        private void LoadBasicInfo()
        {
            //thisConnectedDisplays = new List<Display>();
            Native.DISPLAY_DEVICE d = new Native.DISPLAY_DEVICE();
            d.cb = Marshal.SizeOf(d);
            try
            {
                for (uint id = 0; Native.Methods.EnumDisplayDevices(null, id, ref d, 0); id++)
                {
                    if (d.StateFlags.HasFlag(Native.DisplayDeviceStateFlags.AttachedToDesktop))
                    {
                        Display display = new Display();
                        this.Add(display);
                        display.Id = id;
                        display.Name = d.DeviceName;
                        display.Description = d.DeviceString;
                        display.State = d.StateFlags;
                        display.DeviceId = d.DeviceID;
                        display.RegistryKey = d.DeviceKey;
                        Log(id, d);

                        d.cb = Marshal.SizeOf(d);
                        Native.Methods.EnumDisplayDevices(d.DeviceName, 0, ref d, 0);
                        display.NameExtended = d.DeviceName;
                        display.DetailedDescription = d.DeviceString;
                        Log(id, d);
                        Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
                    }
                    d.cb = Marshal.SizeOf(d);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("{0}", ex.ToString()));
                throw ex;
            }
        }

        private Display GetDisplay(string name)
        {
            foreach (var item in this)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            throw new Exception();
        }

        private void LoadResolution()
        {
            Native.Methods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
                delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref RectStruct lprcMonitor, IntPtr dwData)
                {
                    MonitorInfoEx mi = new MonitorInfoEx();
                    mi.Size = (uint)Marshal.SizeOf(mi);
                    bool success = Native.Methods.GetMonitorInfo(hMonitor, ref mi);
                    if (success)
                    {
                        var di = GetDisplay(mi.DeviceName);
                        di.ScreenWidth = (mi.Monitor.Right - mi.Monitor.Left).ToString();
                        di.ScreenHeight = (mi.Monitor.Bottom - mi.Monitor.Top).ToString();
                        di.MonitorArea = mi.Monitor;
                        di.WorkArea = mi.WorkArea;
                        di.Availability = mi.Flags.ToString();
                    }
                    return true;
                }, IntPtr.Zero);
        }

        private DEVMODE GetDEVMODE()
        {
            DEVMODE dm = new DEVMODE();
            dm.dmDeviceName = new String(new char[32]);
            dm.dmFormName = new String(new char[32]);
            dm.dmSize = (short)Marshal.SizeOf(dm);
            return dm;
        }

        public void External(uint deviceId)
        {
            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            DEVMODE dm = new DEVMODE();
            d.cb = Marshal.SizeOf(d);
          //  int deviceID = 1; // This is only for my device setting. Go through the whole EnumDisplayDevices to find your device  
            Native.Methods.EnumDisplayDevices(null, deviceId, ref d, 0); //
            Native.Methods.EnumDisplaySettings(d.DeviceName, 0, ref dm);
            // dm.dmPelsWidth = 1024;
            // dm.dmPelsHeight = 768;
            dm.dmPositionX = -1600;
            dm.dmPositionY = 0;
            //dm.dmPositionX = Screen.PrimaryScreen.Bounds.Right;
            dm.dmFields = (int)DM.Position;
            Native.Methods.ChangeDisplaySettingsEx(d.DeviceName, ref dm, IntPtr.Zero, ChangeDisplaySettingsFlags.CDS_UPDATEREGISTRY, IntPtr.Zero);
        }

        public string ChangePosition(string name, int x, int y)
        {
            DEVMODE dm = GetDEVMODE();
            //if (0 != Native.Methods.EnumDisplaySettings(@"\\.\DISPLAY1", Native.Methods.ENUM_CURRENT_SETTINGS, ref dm))
            if (0 != Native.Methods.EnumDisplaySettings(name, Native.Methods.ENUM_CURRENT_SETTINGS, ref dm))
            {
                dm.dmPositionX = x;
                dm.dmPositionY = y;

                int iRet = Native.Methods.ChangeDisplaySettings(ref dm, Native.Methods.CDS_TEST);
                if (iRet == Native.Methods.DISP_CHANGE_FAILED)
                {
                    return "Unable To Process Your Request. Sorry For This Inconvenience.";
                }
                else
                {
                    iRet = Native.Methods.ChangeDisplaySettings(ref dm, 0);
                    switch (iRet)
                    {
                        case Native.Methods.DISP_CHANGE_SUCCESSFUL:
                            {
                                return "Success";
                            }
                        case Native.Methods.DISP_CHANGE_RESTART:
                            {
                                return "You Need To Reboot For The Change To Happen.\n If You Feel Any Problem After Rebooting Your Machine\nThen Try To Change Resolution In Safe Mode.";
                            }
                        default:
                            {
                                return "Failed To Change The Position";
                            }
                    }
                }
            }
            else
            {
                return "Failed To Change The Position.";
            }
        }

        //public string ChangePosition(string name, int x, int y)
        //{
        //    DEVMODE dm = GetDEVMODE();
        //    x = 0;
        //    y = 0;
        // //   var xx = Methods.EnumDisplaySettings(@"\\.\DISPLAY1", Methods.ENUM_CURRENT_SETTINGS, ref dm);
        //    //if (0 != Methods.EnumDisplaySettings(name, Methods.ENUM_CURRENT_SETTINGS, ref dm))
        //        if (0 != Methods.EnumDisplaySettings(@"\\.\DISPLAY1", Methods.ENUM_CURRENT_SETTINGS, ref dm))
        //        {
        //        dm.dmPosition.x = x;
        //        dm.dmPosition.y = y;

        //        int iRet = Methods.ChangeDisplaySettings(ref dm, Methods.CDS_TEST);
        //        if (iRet == Methods.DISP_CHANGE_FAILED)
        //        {
        //            return "Unable To Process Your Request. Sorry For This Inconvenience.";
        //        }
        //        else
        //        {
        //            iRet = Methods.ChangeDisplaySettings(ref dm, 0);
        //            switch (iRet)
        //            {
        //                case Methods.DISP_CHANGE_SUCCESSFUL:
        //                    {
        //                        return "Success";
        //                    }
        //                case Methods.DISP_CHANGE_RESTART:
        //                    {
        //                        return "You Need To Reboot For The Change To Happen.\n If You Feel Any Problem After Rebooting Your Machine\nThen Try To Change Resolution In Safe Mode.";
        //                    }
        //                default:
        //                    {
        //                        return "Failed To Change The Position";
        //                    }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return "Failed To Change The Position.";
        //    }
        //}

        private void Log(uint id, Native.DISPLAY_DEVICE d)
        {
            Console.WriteLine($"id:{id}");
            Console.WriteLine($"DeviceName:{d.DeviceName}");
            Console.WriteLine($"DeviceString:{d.DeviceString}");
            Console.WriteLine($"StateFlags:{d.StateFlags}");
            Console.WriteLine($"DeviceId:{d.DeviceID}");
            Console.WriteLine($"DeviceKey:{d.DeviceKey}");
        }
    }
}
