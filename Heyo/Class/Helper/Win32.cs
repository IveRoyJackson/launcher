using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;

namespace Heyo.Class.Helper
{
    public static class Win32
    {
        [DllImport("shell32.DLL", EntryPoint = "ExtractAssociatedIcon")]
        private static extern int ExtractAssociatedIconA(int hInst, string lpIconPath, ref int lpiIcon);
        
        public static System.Drawing.Icon GetIcon(string path)
        {
            IntPtr thisHandle;
            int RefInt = 0;
            thisHandle = new IntPtr(ExtractAssociatedIconA(0, path, ref RefInt));
            return System.Drawing.Icon.FromHandle(thisHandle);
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
            public POINT(Point pt)
            {
                X = Convert.ToInt32(pt.X);
                Y = Convert.ToInt32(pt.Y);
            }
        };

        [DllImport("user32.dll")]
        internal static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll")]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
    }
}
