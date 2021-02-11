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
    public class Tabs
    {
        public List <TabItem> tabsList = new List <TabItem> ();

        public Tabs (TabControl tabControl)                             // CLASS CONSTRUCTOR, Runs whenever this class is instanciated
        {
            tabsList.Add (TabText (tabControl));
        }

        public TabItem TabText (TabControl tabControl)
        {
            byte [] rOMData = Compression.DecompressText (Globals.rOM);

            // ACTUAL TABITEM TAB
            TabItem tabItem = new TabItem ()
            {
                Header = "Classes"
            };

            // CONTENT
            DockPanel dockPanelMomma = new DockPanel ()
            {
                Margin = new Thickness (10)
            };

            tabItem.Content = dockPanelMomma;

                // DOCKPANEL FOR EDITOR TEXTBOX AND CONFIRM BUTTON
                DockPanel dockPanelEditorAndButton = new DockPanel ()
                {
                    Margin = new Thickness (0,0,0,10)
                };

                DockPanel.SetDock (dockPanelEditorAndButton, Dock.Top);

                dockPanelMomma.Children.Add (dockPanelEditorAndButton);

                    // TEXTBOX
                    TextBox textBox = new TextBox ()
                    {
                        Width = 500,
                        Height = 100,
                        Margin = new Thickness (0,0,0,0),
                        TextWrapping = TextWrapping.Wrap
                    };

                    DockPanel.SetDock (textBox, Dock.Left);

                    dockPanelEditorAndButton.Children.Add (textBox);

                    // BUTTON
                    Button button = new Button ()
                    {
                        Content = "BUTTON",
                        Margin = new Thickness (100,40,100,40)
                    };

                    DockPanel.SetDock (button, Dock.Right);

                    dockPanelEditorAndButton.Children.Add (button);

                // LISTVIEW
                SearchList searchList = new SearchList (dockPanelMomma,
                                                        rOMData,
                                                        Bits.NumList (12461));

            return tabItem;
        }
    }
}

























// ALTERNATE WAY OFF DOING THINGS
// WORSE THOUGH, I THINK

/*
namespace GoldenSunEditor
{
    public class TabClasses : TabItem
    {
        public TabClasses ()
        {
            Header = "Classes";

            Canvas canvas = new Canvas ();

            Content = canvas;

            CheckBox checkBox = new CheckBox ();

            canvas.Children.Add (checkBox);

            Style = (Style) Application.Current.TryFindResource (typeof (TabItem)); // Little trick to have the default style I 
                                                                                    // set for 'TabItem' apply to this custom 
                                                                                    // TabItem I have made since it unfortunately
                                                                                    // doesn't do it by itself.
        }
    }
}
*/