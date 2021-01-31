using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Drawing;
using System.Windows.Shell;

namespace GoldenSunEditor
{
    public partial class WindowStart : Window
    {
        public WindowStart()
        {
            InitializeComponent();

            this.Loaded += ThisLoaded;
            this.MouseDown += ThisMouseDrag;
        }

        private void ThisMouseDrag (object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton != MouseButtonState.Pressed)
                this.DragMove ();
        }

        private void ThisLoaded (object sender, RoutedEventArgs e)
        {
            BlurryHelper.EnableBlur (new WindowInteropHelper (this));                           // Window Blur Effect

            ROMInOut.OpenFile ();
        }

        private void LoadWindowMain (object sender, RoutedEventArgs e)
        {
            Window windowMain = new WindowMain();

            windowMain.Show();

            this.Close();
        }
	}
}