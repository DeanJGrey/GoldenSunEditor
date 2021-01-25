using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using BlurryControls.Internals;

namespace GoldenSunEditor
{
    internal static class BlurryHelper
    {

        public static void Blur(Window win)
        {
            EnableBlur (win, true);
        }

        public static void UnBlur(Window win)
        {
            EnableBlur(win, false);
        }

        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute (IntPtr hwnd, ref WindowCompositionAttributeData data);

        /// <summary>
        /// this method uses the SetWindowCompositionAttribute to apply an AeroGlass effect to the window
        /// </summary>
        private static void EnableBlur(Window win, bool enable)
        {
            //this code is taken from a sample application provided by Rafael Rivera
            //see the full code sample here: (2016/08)
            // https://github.com/riverar/sample-win10-aeroglass

            var windowHelper = new WindowInteropHelper (win);

            var accent = new AccentPolicy
            {
                AccentState = enable ? AccentState.AccentEnableBlurbehind : AccentState.AccentDisabled
            };

            var accentStructSize = Marshal.SizeOf (accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);

            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WcaAccentPolicy,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute (windowHelper.Handle, ref data);

            Marshal.FreeHGlobal (accentPtr);
        }
    }
}