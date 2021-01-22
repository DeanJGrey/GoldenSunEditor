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

namespace GoldenSunEditor
{
    public partial class WindowStart : Window
    {
        public WindowStart()
        {
            InitializeComponent();
        }

        private void WindowDrag (object sender, MouseButtonEventArgs e)
        {
            this.DragMove ();
        }

        private void WindowLoad (object sender, RoutedEventArgs e)
        {
            Canvas canvas = UIHelper.GetVisualChild <Canvas> (this, "canvas1");

            StackPanel stackPanel = new StackPanel();

            canvas.Children.Add(stackPanel);

            Button[] buttons = new Button [3];

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons [i] = new Button ()
                {
                    Content = "BUTTON",
                    //Margin = new Thickness (10, 100, 10, 10),
                    //Width = 100,
                    //Height = 25,
                };

                buttons [i].Click += new RoutedEventHandler (LoadWindowMain);

                stackPanel.Children.Add (buttons [i]);
            }
        }

        private void LoadWindowMain (object sender, RoutedEventArgs e)
        {
            Window windowMain = new WindowMain();

            windowMain.Show();

            this.Close();
        }
    }
}