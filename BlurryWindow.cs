using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using BlurryControls.Internals;

namespace GoldenSunEditor
{
    public class BlurryWindow : Window
    {
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //apply blurry filter to the window
            BlurryHelper.Blur (this);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
    }
}