using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; //Control
using System.Drawing; //Point, Size
namespace GoldenSunEditor
{
	class Table_Manager
	{
		/*
		List <DataNumericUpDown> nuds = new List <DataNumericUpDown>();
		List <ButtonList> buttonList = new List <ButtonList> ();
		List <DataTextbox> dtbs = new List <DataTextbox>();

		byte[] buf;
		Control pnl;
		int baseAddress = 0;
		int entryLength = 0;
		int entryAddress = 0;
		Label countAddress;

		public void SetPanel (Control panel)										// Maybe just init stuff?
		{
			pnl = panel;

			if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
				combo2numeric = 1;
		}

		public void SetTable (byte[] buffer, int baseA, int eLen)
		{
			buf = buffer;

			baseAddress = baseA;

			entryLength = eLen;

			entryAddress = baseAddress;
		}

		public SearchList sl = new SearchList ();

		int combo2numeric = 0;

		public void DoTableListbox (byte[] txt, List <int> items, int width)
		{
			sl.DoTableListbox (	pnl,
								txt,
								items,
								width);

			sl.listView.SelectedIndexChanged += new EventHandler (LoadEntry);		// When clicking on new item in list, run LoadEntry

			if (entryLength != 0)													// Entry length is "0" for text editor at present.
				countAddress = DoLabel (0,
										0,
										"        ");
		}

		public void DoTableListbox2 (byte[] rom, List <int> items)
		{
			sl.DoTableListbox2 (pnl, 
								rom, 
								items);

			sl.listView.SelectedIndexChanged += new EventHandler (LoadEntry);

			if (entryLength != 0)													// Entry length is "0" for text editor at present.
				countAddress = DoLabel (	240, 
									0, 
									"        ");
		}

		public Label DoLabel (int x, int y, String text)
		{
			Label lbl = new Label ();

			lbl.Location = new Point(x, y);
			lbl.Size = new Size(text.Length * 8 + 8, 20);
			lbl.Text = text;
			pnl.Controls.Add(lbl);

			return lbl;
		}

		//
		// CHANGES HEX ADDRESS LABEL
		//
		public void LoadEntry (object sender, EventArgs e)
		{
			int sourceEntry = 0;

			int entryAddr2 = entryAddress;

			if (sender != null)
			{
				ListView listView = ((ListView) sender);

				if (listView.SelectedIndices.Count != 1) 
					return; 

				sourceEntry = sl.sItems [listView.SelectedIndices [0]];

				entryAddr2 = baseAddress + sourceEntry * entryLength;

				if (entryAddress != 0)																	//Ensuring that there is an item to copy. Also prevents CtrL+Shift form load bug because an item is selected on form load.
					if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
						if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
							for (int i = 0; i < entryLength; i++)
								buf [entryAddr2 + i] = buf [entryAddress + i];

				if (entryLength != 0)																	// Entry length is "0" for text editor at present.
					countAddress.Text = (0x8000000 | entryAddr2).ToString("X8");
			}
			else 
				entryAddress = baseAddress; 
			
			for (int i = 0; i < nuds.Count; i++)
			{
				nuds[i].addr = nuds[i].addr - entryAddress + entryAddr2;
				nuds[i].Value = Bits.getBits(nuds[i].buf, nuds[i].addr, nuds[i].bits);
			}

			for (int i = 0; i < buttonList.Count; i++)
			{
				buttonList[i].addr = buttonList[i].addr - entryAddress + entryAddr2;

				buttonList[i].Text = Bits.GetTextShort(buttonList[i].txt, buttonList[i].items[Bits.getBits(buttonList[i].buf, buttonList[i].addr, buttonList[i].bits)]);
			}

			for (int i = 0; i < dtbs.Count; i++)
			{
				dtbs[i].theIndex = dtbs[i].baseIndex + sourceEntry;

				dtbs[i].textBox.Text = Bits.GetTextLong (dtbs[i].txt, dtbs[i].theIndex);
			}

			entryAddress = entryAddr2;
		}

		public NumericUpDown doNumericUpDown (int x, int y, int offset, int numOfBits)
		{
			var dataNumericUpDown = new DataNumericUpDown ();

			NumericUpDown nud = dataNumericUpDown.doNud (	pnl, 
															x, 
															y, 
															buf, 
															entryAddress + offset, 
															numOfBits);

			nud.MouseClick += new MouseEventHandler (nudrc2);

			nuds.Add (dataNumericUpDown);

			return nud;
		}

		private void nudrc2(object sender, MouseEventArgs e)
		{
			var dnud = (DataNumericUpDown)sender;
			nudrcMain(dnud.addr - entryAddress, dnud.bits);
		}

		private void nudrcMain(int offset, int bits)						//Sorts main list based on data.
		{
			if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
				return;
			if (sl == null)													//Some error-proofing if you don't have a main list.
				return;

			for (int i = 0; i < sl.sItems.Count; i++)
			{
				int x = sl.sItems[i];
				int value = Bits.getBits(buf, baseAddress + x * entryLength + offset, bits);
				int j = i;
				while ((j > 0) && (Bits.getBits(buf, (baseAddress + sl.sItems[j - 1] * entryLength) + offset, bits) > value))
				{
					sl.sItems[j] = sl.sItems[j - 1];
					j = j - 1;
				}
				sl.sItems[j] = x;
			}
			sl.listView.Invalidate();
		}

		public Control DoButtonList (int x, int y, byte[] txt, List<int> items, int offset, int numOfBits)
		{
			if (combo2numeric == 1)
			{
				return doNumericUpDown (x, 
										y, 
										offset, 
										numOfBits);
			}

			ButtonList bn = new ButtonList ();

			bn.DoButtonList (	pnl, 
								x, 
								y, 
								txt, 
								items, 
								buf, 
								entryAddress + offset, 
								numOfBits);

			bn.MouseClick += new MouseEventHandler (NumericUpDownRC3);

			buttonList.Add (bn);

			return bn;
		}

		private void NumericUpDownRC3 (object sender, MouseEventArgs e)
		{
			var dnud = (ButtonList) sender;
			nudrcMain (dnud.addr - entryAddress, dnud.bits);
		}

		public DataTextbox DoNamebox (int x, int y, byte[] textBuf, int index)
		{
			DataTextbox tbx = new DataTextbox ();

			tbx.DoTextBox (	pnl, 
							x, 
							y, 
							textBuf, 
							index);

			dtbs.Add (tbx);

			tbx.button.Click += button_Click;

			return tbx;
		}

		private void button_Click(object sender, EventArgs e)
		{
			sl.listView.Invalidate();
		}
		*/
	}
}