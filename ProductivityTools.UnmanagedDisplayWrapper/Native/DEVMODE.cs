using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProductivityTools.UnmanagedDisplayWrapper.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DEVMODE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;

        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;
        public int dmPositionX;
        public int dmPositionY;
        public int dmDisplayOrientation;
        public int dmDisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;

        public short dmLogPixels;
        public short dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;
        public int dmPanningWidth;
        public int dmPanningHeight;
    }

    struct POINTL
    {
        public Int32 x;
        public Int32 y;
    }

    internal enum DM : short
    {
        /// <summary>
        /// Unknown setting.
        /// </summary>
        DMDUP_UNKNOWN = 0,

        /// <summary>
        /// Normal (nonduplex) printing.
        /// </summary>
        DMDUP_SIMPLEX = 1,

        /// <summary>
        /// Long-edge binding, that is, the long edge of the page is vertical.
        /// </summary>
        DMDUP_VERTICAL = 2,

        /// <summary>
        /// Short-edge binding, that is, the long edge of the page is horizontal.
        /// </summary>
        DMDUP_HORIZONTAL = 3,
    }
}
