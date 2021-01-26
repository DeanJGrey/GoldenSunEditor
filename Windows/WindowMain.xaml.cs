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
using System.Security;
using System.Drawing.Drawing2D;

namespace GoldenSunEditor
{
    public partial class WindowMain : Window
    {
        [DllImport("user32.dll")]
        static extern Int32 SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);

        /*
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

        */














        /*
        public WindowMain ()
        {
            InitializeComponent();

            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;

            SourceInitialized += OnSourceInitialized;
        }

        private void OnSourceInitialized(object sender, EventArgs eventArgs)
        {
            if (!NativeMethods.DwmIsCompositionEnabled())
                return;

            var hwnd = new WindowInteropHelper(this).Handle;

            var hwndSource = HwndSource.FromHwnd(hwnd);
            var sizeFactor = hwndSource.CompositionTarget.TransformToDevice.Transform(new Vector(1.0, 1.0));

            Background = System.Windows.Media.Brushes.Transparent;
            hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;

            using (var path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, (int)(ActualWidth * sizeFactor.X), (int)(ActualHeight * sizeFactor.Y));

                using (var region = new Region(path))
                using (var graphics = Graphics.FromHwnd(hwnd))
                {
                    var hRgn = region.GetHrgn(graphics);

                    var blur = new NativeMethods.DWM_BLURBEHIND ()
                    {
                        dwFlags = NativeMethods.DWM_BB.DWM_BB_ENABLE | NativeMethods.DWM_BB.DWM_BB_BLURREGION | NativeMethods.DWM_BB.DWM_BB_TRANSITIONONMAXIMIZED,
                        fEnable = true,
                        hRgnBlur = hRgn,
                        fTransitionOnMaximized = true
                    };

                    NativeMethods.DwmEnableBlurBehindWindow (hwnd, ref blur);

                    region.ReleaseHrgn(hRgn);
                }
            }
        }

        [SuppressUnmanagedCodeSecurity]
        private static class NativeMethods
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct DWM_BLURBEHIND
            {
                public DWM_BB dwFlags;
                public bool fEnable;
                public IntPtr hRgnBlur;
                public bool fTransitionOnMaximized;
            }

            [Flags]
            public enum DWM_BB
            {
                DWM_BB_ENABLE = 1,
                DWM_BB_BLURREGION = 2,
                DWM_BB_TRANSITIONONMAXIMIZED = 4
            }

            [DllImport("dwmapi.dll", PreserveSig = false)]
            public static extern bool DwmIsCompositionEnabled();

            [DllImport("dwmapi.dll", PreserveSig = false)]
            public static extern void DwmEnableBlurBehindWindow (IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);
        }
        */









        [DllImport("dwmapi.dll")]
        static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);

        public WindowMain ()
        {
            InitializeComponent();

            this.SourceInitialized += MainWindow_SourceInitialized;
            this.KeyDown += MainWindow_KeyDown;
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) this.Close();
        }

        void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            var helper = new WindowInteropHelper(this);
            var hwnd = helper.Handle;
            var src = HwndSource.FromHwnd(hwnd);

            src.CompositionTarget.BackgroundColor = Colors.Transparent;

            WindowChrome.SetWindowChrome(this, new WindowChrome
            {
                CaptionHeight = 500,
                CornerRadius = new CornerRadius(0),
                GlassFrameThickness = new Thickness(0),
                NonClientFrameEdges = NonClientFrameEdges.None,
                ResizeBorderThickness = new Thickness(0),
                UseAeroCaptionButtons = false
            });

            GraphicsPath path = new GraphicsPath(FillMode.Alternate);
            path.StartFigure();
            path.AddArc(new RectangleF(0, 0, 500, 500), 0, 360);
            path.CloseFigure();

            var dbb = new DWM_BLURBEHIND(true);//new DwmBlurBehind(true);
            dbb.SetRegion(Graphics.FromHwnd(hwnd), new Region(path));
            DwmEnableBlurBehindWindow(hwnd, ref dbb);//DwmApi.DwmEnableBlurBehindWindow(hwnd, ref dbb);
        }

        [Flags]
        enum DWM_BB
        {
            Enable = 1,
            BlurRegion = 2,
            TransitionMaximized = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DWM_BLURBEHIND
        {
            public DWM_BB dwFlags;
            public bool fEnable;
            public IntPtr hRgnBlur;
            public bool fTransitionOnMaximized;

            public DWM_BLURBEHIND(bool enabled)
            {
                fEnable = enabled ? true : false;
                hRgnBlur = IntPtr.Zero;
                fTransitionOnMaximized = false;
                dwFlags = DWM_BB.Enable;
            }

            public Region Region
            {
                get { return Region.FromHrgn(hRgnBlur); }
            }

            public bool TransitionOnMaximized
            {
                get { return fTransitionOnMaximized; }
                set
                {
                    fTransitionOnMaximized = value ? true : false;
                    dwFlags |= DWM_BB.TransitionMaximized;
                }
            }

            public void SetRegion(Graphics graphics, Region region)
            {
                hRgnBlur = region.GetHrgn(graphics);
                dwFlags |= DWM_BB.BlurRegion;
            }
        }
    }
}