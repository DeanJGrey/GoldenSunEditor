using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace GoldenSunEditor
{
    internal enum AccentState
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
        ACCENT_INVALID_STATE = 5
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public uint AccentFlags;
        public uint GradientColor;
        public uint AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    internal enum WindowCompositionAttribute
    {
        // ...
        WCA_ACCENT_POLICY = 19
        // ...
    }

    internal static class BlurryHelper
    {

        [DllImport("user32.dll")]
        static extern Int32 SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);

        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);






        /*
        public static uint _blurOpacity;
        
        public static double BlurOpacity
        {
            get 
            { 
                return _blurOpacity;
            }
            set 
            { 
                _blurOpacity = (uint) value; 
                EnableBlur (window); 
            }
        }

        private static uint _blurBackgroundColor = 0x990000;
        */
        
        





        internal static void EnableBlur (WindowInteropHelper windowHelper)
        {
            var accent = new AccentPolicy ();

            accent.AccentState = AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND;

            accent.AccentFlags = 2;

            accent.GradientColor = 0x02000000; // (_blurOpacity << 24) | (_blurBackgroundColor & 0xFFFFFF);

            var accentStructSize = Marshal.SizeOf (accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);

            Marshal.StructureToPtr (accent, accentPtr, false);

            var data = new WindowCompositionAttributeData ();

            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;

            data.SizeOfData = accentStructSize;

            data.Data = accentPtr;

            SetWindowCompositionAttribute (windowHelper.Handle, ref data);

            Marshal.FreeHGlobal (accentPtr);
        }
    }
}