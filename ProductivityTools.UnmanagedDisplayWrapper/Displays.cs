using ProductivityTools.UnmanagedDisplayWrapper.Native;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProductivityTools.UnmanagedDisplayWrapper
{
    public interface IDisplays
    {
        void LoadData();
        void MoveExternalDisplayToRight();
        void MoveExternalDisplayToLeft();
        void MoveMainDisplayToLeft();
        void MoveMainDisplayToRight();
    }

    public class Displays : List<Display>, IDisplays
    {
        public void LoadData()
        {
            LoadBasicInfo();
            LoadResolution();
        }

        public void MoveMainDisplayToLeft()
        {
            LoadData();
            OnlyOneDisplay();
            Display secondDisplay = this[0];
            string name = this[0].Name;
            int xmove = -1 * this[0].MonitorArea.Right;
            ChangeMainDisplayPosition(name, xmove, 0);
        }

        public void MoveMainDisplayToRight()
        {
            LoadData();
            OnlyOneDisplay();
            Display secondDisplay = this[0];
            string name = this[0].Name;
            int xmove = this[0].MonitorArea.Right;
            ChangeMainDisplayPosition(name, xmove, 0);
        }

        public void MoveExternalDisplayToLeft()
        {
            LoadData();
            OnlyOneDisplay();
            Display secondDisplay = this[1];
            uint id = secondDisplay.Id;
            int xmove = -1 * secondDisplay.ScreenWidth;
            ChangeExternalDisplayPosition(id, xmove, 0);
        }

        public void MoveExternalDisplayToRight()
        {
            LoadData();
            OnlyOneDisplay();
            Display secondDisplay = this[1];
            uint id = secondDisplay.Id;
            int xmove = secondDisplay.ScreenWidth;
            ChangeExternalDisplayPosition(id, xmove, 0);
        }

        private void LoadBasicInfo()
        {
            this.Clear();
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

                        d.cb = Marshal.SizeOf(d);
                        Native.Methods.EnumDisplayDevices(d.DeviceName, 0, ref d, 0);
                        display.NameExtended = d.DeviceName;
                        display.DetailedDescription = d.DeviceString;
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
                        di.ScreenWidth = (mi.Monitor.Right - mi.Monitor.Left);
                        di.ScreenHeight = (mi.Monitor.Bottom - mi.Monitor.Top);
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

        private void ChangeExternalDisplayPosition(uint deviceId, int x, int y)
        {
            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            DEVMODE dm = new DEVMODE();
            d.cb = Marshal.SizeOf(d);
            Native.Methods.EnumDisplayDevices(null, deviceId, ref d, 0); //
            Native.Methods.EnumDisplaySettings(d.DeviceName, 0, ref dm);
            // dm.dmPelsWidth = 1024;
            // dm.dmPelsHeight = 768;
            dm.dmPositionX = x;
            dm.dmPositionY = y;
            dm.dmFields = (int)DMFlags.Position;
            Native.Methods.ChangeDisplaySettingsEx(d.DeviceName, ref dm, IntPtr.Zero, ChangeDisplaySettingsFlags.CDS_UPDATEREGISTRY, IntPtr.Zero);
        }

        private string ChangeMainDisplayPosition(string name, int x, int y)
        {
            DEVMODE dm = GetDEVMODE();
            //if (0 != Native.Methods.EnumDisplaySettings(@"\\.\DISPLAY1", Native.Methods.ENUM_CURRENT_SETTINGS, ref dm))
            if (0 != Native.Methods.EnumDisplaySettings(name, Native.Methods.ENUM_CURRENT_SETTINGS, ref dm))
            {
                dm.dmPositionX = x;
                dm.dmPositionY = y;

                DisplayChangeResult iRet = Native.Methods.ChangeDisplaySettings(ref dm, ChangeDisplaySettingsFlags.CDS_TEST);
                if (iRet == DisplayChangeResult.Failed)
                {
                    return "Unable To Process Your Request. Sorry For This Inconvenience.";
                }
                else
                {
                    iRet = Native.Methods.ChangeDisplaySettings(ref dm, 0);
                    switch (iRet)
                    {
                        case DisplayChangeResult.Successful:
                            {
                                return "Success";
                            }
                        case DisplayChangeResult.Restart:
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

        private void OnlyOneDisplay()
        {
            if (this.Count == 1)
            {
                throw new Exception("Only one display detected, what would you like to move?");
            }
        }
    }
}
