
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

        public void LoadData()
        {
            Native.DISPLAY_DEVICE d = new Native.DISPLAY_DEVICE();
            d.cb = Marshal.SizeOf(d);
            try
            {
                for (uint id = 0; Native.Methods.EnumDisplayDevices(null, id, ref d, 0); id++)
                {
                    if (d.StateFlags.HasFlag(Native.DisplayDeviceStateFlags.AttachedToDesktop))
                    {
                        this.Id = id;
                        this.Name = d.DeviceName;
                        this.Description = d.DeviceString;
                        this.State = d.StateFlags;
                        this.DeviceId = d.DeviceID;
                        this.RegistryKey = d.DeviceKey;
                        Log(id, d);

                        d.cb = Marshal.SizeOf(d);
                        Native.Methods.EnumDisplayDevices(d.DeviceName, 0, ref d, 0);
                        this.NameExtended = d.DeviceName;
                        this.DetailedDescription = d.DeviceString;
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
            if (0 != Methods.EnumDisplaySettings(@"\\.\DISPLAY1", Methods.ENUM_CURRENT_SETTINGS, ref dm))
            {
                dm.dmPosition.x= x;
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
