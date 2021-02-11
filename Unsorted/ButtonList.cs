using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; //Control
using System.Drawing; //Point, Size

namespace GoldenSunEditor
{
	class ButtonList : Button
	{
		/*
		Control pnl;
		public byte[] buf;
		public int addr;
		public int bits;
		public byte[] txt;

		public List <int> items;

		protected override bool IsInputKey(Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Up:
				case Keys.Down:
					return true;
			}

			return base.IsInputKey(keyData);
		}

		public Control DoButtonList (Control panel, int x, int y, byte[] textBank, List<int> items, byte[] buffer, int address, int numOfBits)
		{
			pnl = panel;

			AutoEllipsis = true;

			Location = new Point(x, y);

			Size = new Size(180, 20);

			FlatStyle = FlatStyle.Flat;

			FlatAppearance.BorderSize = 0;

			pnl.Controls.Add (this);

			buf = buffer;
			addr = address;
			bits = numOfBits;

			TextAlign = ContentAlignment.MiddleLeft;

			txt = textBank;

			this.items = items;

			PreviewKeyDown += new PreviewKeyDownEventHandler (ComboKeyDown);

			Click += OpenButtonList;

			return this;
		}

		ButtonList curList;

		Panel listPanel = new Panel();

		public SearchList searchList2 = new SearchList ();

		private void OpenButtonList (object sender, EventArgs e)
		{
			if ((Control.ModifierKeys & Keys.Control) == Keys.Control)				// Small code hack to support the sorting w/o opening pop-up list...
				return;

			listPanel = new Panel ();

			curList = (ButtonList) sender;

			listPanel.Show();

			searchList2.DoTableListbox (listPanel, txt, items, 200);

			//
			//PANEL
			//
			listPanel.Left = 300;
			listPanel.Width = 200;
			listPanel.Top = 20;
			listPanel.Height = pnl.Height - 40;

			searchList2.listView.Height -= 20;

			//
			// BUTTON "OK"
			//
			Button ok = new Button ();
			ok.Left = 0;
			ok.Top = listPanel.Height - 20;
			ok.Width = 120;
			ok.Height = 20;
			ok.Text = "Ok";
			listPanel.Controls.Add (ok);
			ok.Click += OkClick;

			//
			//BUTTON "CANCEL"
			//
			Button cancel = new Button ();
			cancel.Left = 120;
			cancel.Top = listPanel.Height - 20;
			cancel.Width = 120;
			cancel.Height = 20;
			cancel.Text = "Cancel";
			listPanel.Controls.Add(cancel);
			cancel.Click += CancelClick;

			pnl.Controls.Add (listPanel);

			listPanel.BringToFront ();
		}

		private void OkClick (object sender, EventArgs e)
		{
			listPanel.Hide();
			curList.Focus();

			if (searchList2.listView.SelectedIndices.Count != 1) 
				return;

			int value = searchList2.sItems [searchList2.listView.SelectedIndices [0]];

			for (int j = 0; j < bits; j += 8)
			{
				buf[addr + (j >> 3)] = (byte)value;

				value >>= 8;
			}

			value = Bits.getBits(buf, addr, bits);

			Text = Bits.GetTextShort(txt, items[value]);
		}

		private void CancelClick (object sender, EventArgs e)
		{
			listPanel.Hide();

			curList.Focus();
		}

		private void ComboKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			int value = Bits.getBits(buf, addr, bits);

			switch (e.KeyCode)
			{
				case Keys.Up:
					if (value <= 0)
						break;

					Bits.setBits(buf, addr, bits, --value);

					Text = Bits.GetTextShort(txt, searchList2.mItems[searchList2.sItems[value]]);

					break;

				case Keys.Down:
					if (value >= items.Count() - 1)
						break;

					Bits.setBits(buf, addr, bits, ++value);

					Text = Bits.GetTextShort(txt, searchList2.mItems[searchList2.sItems[value]]);

					break;
			}
		}
		*/
	}
}