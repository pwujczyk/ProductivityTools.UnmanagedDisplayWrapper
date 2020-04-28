using ProductivityTools.UnmanagedDisplayWrapper.Native;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProductivityTools.UnmanagedDisplayWrapper
{
    public class Displays :List<Display>
    {
        //List<Display> ConnectedDisplays { get; set; }

        public void LoadData()
        {
            LoadBasicInfo();
            LoadResolution();
        }

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
            foreach(var item in this)
            {
                if (item.Name==name)
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

        public string ChangePosition(int x, int y)
        {
            DEVMODE dm = GetDEVMODE();
            var xx = Methods.EnumDisplaySettings(@"\\.\DISPLAY1", Methods.ENUM_CURRENT_SETTINGS, ref dm);
            if (0 != Methods.EnumDisplaySettings(@"\\.\DISPLAY1", Methods.ENUM_CURRENT_SETTINGS, ref dm))
            {
                dm.dmPosition.x = x;
                dm.dmPosition.y = y;

                int iRet = Methods.ChangeDisplaySettings(ref dm, Methods.CDS_TEST);
                if (iRet == Methods.DISP_CHANGE_FAILED)
                {
                    return "Unable To Process Your Request. Sorry For This Inconvenience.";
                }
                else
                {
                    iRet = Methods.ChangeDisplaySettings(ref dm, 0);
                    switch (iRet)
                    {
                        case Methods.DISP_CHANGE_SUCCESSFUL:
                            {
                                return "Success";
                            }
                        case Methods.DISP_CHANGE_RESTART:
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
