using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
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
using System.Windows.Shell;
using System.Security;
using System.Drawing.Drawing2D;

namespace GoldenSunEditor
{
	class SearchList
	{
		// GLOBALS
		public byte [] rOM;
		public Panel panel;
		public List <int> itemsIndexList;
		public TextBox searchBar;
		public ListView listViewTable;

		// CONSTRUCTOR
		public SearchList (Panel panel, byte [] rOM, List <int> itemsIndexList)
		{
			// DEFINITIONS
			this.panel =			panel;
			this.rOM =				rOM;
			this.itemsIndexList =	itemsIndexList;
			searchBar =				SearchBar ();
			listViewTable =			ListViewTable ();

			ListViewItemsRefresh ();

			// LAYOUT
			LayoutCreate ();
		}

		// LAYOUT
		void LayoutCreate ()
		{
			// IF DOCKPANEL
			if (panel.GetType () == typeof (DockPanel))
			{
				DockPanel.SetDock (searchBar, Dock.Top);
				DockPanel.SetDock (listViewTable, Dock.Bottom);
			}

			// ADD TO PANEL
			panel.Children.Add (searchBar);
			panel.Children.Add (listViewTable);
		}

		// SEARCHBAR-LISTVIEW MATCHES
		List <int> SearchBarMatches ()
		{
			List <int> searchBarMatches = Bits.GetTextMatchIndexes (rOM,
																	searchBar.Text,
																	itemsIndexList);

			return searchBarMatches;
		}

		// LISTVIEW ITEMS
		private void ListViewItemsRefresh ()
		{
			// CLEAR OLD ITEMS
			listViewTable.Items.Clear ();																	// Get ready so we can add new items

			List <ListViewItem> matchList = new List <ListViewItem> ();

			List <int> itemsToCheckAgainst;

			// CHECK SEARCHBAR
			if (SearchBarMatches () == null)                                                                // If no matches from search bar
				itemsToCheckAgainst = itemsIndexList;                                                       // List will be empty
			else
				itemsToCheckAgainst = SearchBarMatches ();

			// SEARCH RESULTS
			for (int matchItemsIndex = 0; matchItemsIndex < SearchBarMatches ().Count; matchItemsIndex++)   // For each result SearchBar Match
			{
				// STRING
				String  matchString = itemsToCheckAgainst [matchItemsIndex].ToString ();                    // Index of searchBar match
				matchString += " - ";                                                                       // Spacer
				matchString += Bits.GetTextShort (	rOM,                                                    // Actual Text (What we're here for)
													itemsToCheckAgainst [matchItemsIndex]);
				
				// ITEM
				ListViewItem match = new ListViewItem ()													// match to add to list of matches
				{
					Content = matchString																	// Set the item text to the string
				};

				matchList.Add (match);																		// Add match to list
			}

			// ADD TO LISTVIEW
			for (int listViewItemIndex = 0; listViewItemIndex < matchList.Count; listViewItemIndex++)		// Time to add items
				listViewTable.Items.Add (matchList [listViewItemIndex]);									// to ListView for viewing
		}

		// SEARCHBAR TEXT CHANGE
		private void SearchBarTextChanged (object sender, EventArgs e)
		{
			ListViewItemsRefresh ();
		}

		// SEARCH BAR
		public TextBox SearchBar ()
		{
			TextBox textBox = new TextBox ()
			{
				Margin = new Thickness (0,0,0,10)
			};

			textBox.TextChanged += SearchBarTextChanged;

			return textBox;
		}

		// LISTVIEW
		public ListView ListViewTable ()
		{
			ListView listView = new ListView ();

			return listView;
		}
	}
}