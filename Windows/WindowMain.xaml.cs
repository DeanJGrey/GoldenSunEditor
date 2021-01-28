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
        public WindowMain ()
        {
            InitializeComponent ();

            this.Loaded += WindowLoaded;
            this.MouseDown += WindowDrag;
        }

        private void WindowLoaded (object sender, RoutedEventArgs e)
        {
            // BLUR
            WindowInteropHelper windowHelper = new WindowInteropHelper (this);

            BlurryHelper.EnableBlur (windowHelper);

            // CONTENT
            Canvas canvas = new Canvas ()
            {
                Width = this.Width,
                Height = this.Height,
                Margin = new Thickness (0)
            };

            this.AddChild (canvas);

                // 
                StackPanel stackPanel = new StackPanel ()
                {
                    Orientation = Orientation.Horizontal,
                    Width = this.Width,
                    Height = this.Height,
                    Margin = new Thickness (0)
                };

                canvas.Children.Add (stackPanel);

                    // LISTVIEW & BUTTONS
                    StackPanel stackPanelListViewButtons = new StackPanel ()
                    {
                        Orientation = Orientation.Vertical,
                        Width = 200,
                        Height = this.Height,
                        Margin = new Thickness (0),
                        HorizontalAlignment = HorizontalAlignment.Left
                    };

                    stackPanel.Children.Add (stackPanelListViewButtons);

                        // LISTVIEW
                        ListView listView = new ListView ()
                        {
                            Width = stackPanelListViewButtons.Width - 20,
                            Height = this.Height - 50,
                            Margin = new Thickness (10),
                            HorizontalAlignment = HorizontalAlignment.Left
                        };

                        for (int i = 0; i < 90; i++)
                        {
                            listView.Items.Add (new ListViewItem ().Content = "Item" + " " + (i + 1));
                        }

                        stackPanelListViewButtons.Children.Add (listView);

                        // BUTTONS
                        StackPanel stackPanelButton = new StackPanel ()
                        {
                            Orientation = Orientation.Horizontal,
                            Height = 30,
                            Margin = new Thickness (10, 0, 10, 0)
                        };

                        stackPanelListViewButtons.Children.Add (stackPanelButton);

                            Button button1 = new Button ()
                            {
                                Width = 85,
                                Margin = new Thickness (0, 0, 10, 10),
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Content = "Button 1"
                            };

                            stackPanelButton.Children.Add (button1);

                            Button button2 = new Button ()
                            {
                                Width = 85,
                                Margin = new Thickness (0, 0, 0, 10),
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Content = "Button 2"
                            };

                            stackPanelButton.Children.Add (button2);

            // TABCONTROL
            TabControl tabControl = new TabControl ()
            {
                Margin = new Thickness (0, 10, 10, 10),
                Width = this.Width - 210,
                Height = this.Height - 20
            };

            stackPanel.Children.Add (tabControl);

                for (int i = 0; i < 20; i++)
                {
                    TabItem tabItem = new TabItem ()
                    {
                        Header = "tabItem" + " " + i,
                        Height = 20,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center
                    };

                tabControl.Items.Add (tabItem);
            }
        }

        private void WindowDrag (object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton != MouseButtonState.Pressed)
                this.DragMove ();
        }

        private void buttonRedClick(object sender, RoutedEventArgs e)
        {
            this.Close ();
        }

        private void buttonGreenClick (object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}