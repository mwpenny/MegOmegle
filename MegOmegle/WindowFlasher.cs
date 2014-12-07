/* WindowFlasher.cs
 * 
 * A helper class for flashing a window in the taskbar.
 * 
 */

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MegOmegle
{
    public class WindowFlasher
    {
        [DllImport("user32.dll")]
        static extern Int32 FlashWindowEx(ref FLASHWINFO pwfi);

        [StructLayout(LayoutKind.Sequential)]
        struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public Int32 dwFlags;
            public UInt32 uCount;
            public Int32 dwTimeout;
        }

        const int FLASHW_STOP = 0;
        const int FLASHW_CAPTION = 1;
        const int FLASHW_TRAY = 2;
        const int FLASHW_ALL = 3;
        const int FLASHW_TIMER = 4;
        const int FLASHW_TIMERNOFG = 12;

        /// <summary>
        /// Flashes a window in the taskbar until it is brought into the foreground.
        /// </summary>
        /// <param name="hwnd">The window's handle.</param>
        public static void flash(Form window)
        {
            //Only works on Windows 2000 or above
            if (Environment.OSVersion.Version.Major < 5)
                return;

            FLASHWINFO fInfo = new FLASHWINFO();
            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = window.Handle;
            fInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
            fInfo.dwTimeout = 0;
            fInfo.uCount = UInt32.MaxValue;

            FlashWindowEx(ref fInfo);
        }
    }
}
