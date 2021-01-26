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
    public partial class WindowMain : Window
    {
        [DllImport("user32.dll")]
        static extern Int32 SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);


        public WindowMain ()
        {
            InitializeComponent ();
        }

        private void WindowLoaded (object sender, RoutedEventArgs e)
        {
            Panel panel = UIHelper.GetVisualChild <Panel>(this, "canvas1");

            ListView listView = new ListView ()
            {
                Width = 200,
                Margin = new Thickness(10)
            };

            Button button = new Button()
            {
                Height = 40,
                Width = 100,
                Margin = new Thickness (220,10,10,10)
            };

            listView.Items.Add (new ListViewItem ().Content = "Item1");
            
            panel.Children.Add (listView);

            panel.Children.Add (button);




            WindowInteropHelper windowHelper = new WindowInteropHelper(this);

            BlurryHelper.EnableBlur(windowHelper);

            IntPtr hwnd = windowHelper.Handle; //IntPtr hwnd = new WindowInteropHelper(this).Handle;

            SetWindowRgn(hwnd, CreateRoundRectRgn(0, 0, 300, 300, 75, 75), true);
        }

        private void WindowDrag (object sender, MouseButtonEventArgs e)
        {
            this.DragMove ();
        }
    }
}