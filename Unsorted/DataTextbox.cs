using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; //Control
using System.Drawing; //Point, Size

namespace GoldenSunEditor
{
	class DataTextbox
	{
		/*
		//		Control pnl;

		public byte[] txt;

		public int baseIndex;

		public int theIndex;

		public TextBox textBox = new TextBox ();

		public Button button = new Button ();

		public RichTextBox richTextBox = new RichTextBox();

		//
		// Makes a text box and an accompanying button
		//
		public void DoTextBox (Control panel, int x, int y, byte [] textBuf, int index)
		{
			txt = textBuf;
			baseIndex = index;
			theIndex = index;

			//
			//TextBox
			//
			textBox.ForeColor = Globals.color0;
			textBox.BackColor = Globals.color3;
			textBox.BorderStyle = BorderStyle.None;
			textBox.AutoSize = false;
			textBox.Font = Globals.font;
			textBox.Location = new Point (x, y);
			textBox.Width = 120;

			//Hack For Making textbox height correct, since setting BorderStyle = None fucks up the height // DOESNT WORK
			//textBox.Multiline = true;
			//Size s = TextRenderer.MeasureText (textBox.Text, textBox.Font, Size.Empty, TextFormatFlags.TextBoxControl);
			//textBox.MinimumSize = new Size(0, s.Height + 1);
			//textBox.Multiline = false;

			panel.Controls.Add (textBox);

			//ctrls.Add(tbx);

			//
			//Button
			//
			button.FlatStyle = FlatStyle.Flat;
			button.FlatAppearance.BorderSize = 0;
			button.BackColor = Globals.color1;
			button.ForeColor = Globals.color3;
			button.Location = new Point (x + textBox.Width, y);
			button.Width = 50;
			button.Height = 20;
			button.Font = Globals.font;
			button.Text = "Edit";
			button.Click += button_Click;
			panel.Controls.Add (button);
		}

		private void button_Click (object sender, EventArgs e)
		{ //TODO Move decomptext to Form1, compress text on save.
		  // * Function may need refactoring, but it works.

			String str = textBox.Text; //textBox1.Text;

			byte [] bytes = new byte [0x200];

			int a = 0, b = 0;

			while (a < str.Length)
			{
				if (str [a] == '[')
				{
					int num = 0;

					while (str [++a] != ']')
					{
						num = (num * 10) + (byte) (str [a]) - 0x30;
					}

					a++;

					bytes [b++] = (byte) num;
				}
				else if (((byte) str [a] == 13) && ((byte) str [a + 1] == 10))
				{
					a += 2;
				}
				else
				{
					bytes [b++] = (byte) str [a++];
				}
			}

			b++; //B/c 00 character at end.
				 //byte[] bytes = toRawStrData(textBox1.Text);
				 //int b = bytes.Length + 1; //=0x200 + 1 (NEEDS FIXING)
			//if (listView1.SelectedIndices.Count != 1) { return; }

			int srcEntry = theIndex * 4;

			//int srcEntry = listBox2.SelectedIndex * 4;

			int neaddr = 0xC300 + Bits.getInt32(txt, srcEntry + 4);

			int lendif = Bits.getInt32(txt, srcEntry) - Bits.getInt32(txt, srcEntry + 4) + b;

			int c = srcEntry + 4;

			while ((Bits.getInt32 (txt, c) != 0))
			{
				Bits.setInt32 (txt, c, Bits.getInt32(txt, c) + lendif);
				c += 4;
			}

			c = 0xC300 + Bits.getInt32 (txt, c - 4) - lendif;

			while (txt[c++] != 0) 
			{ 
			
			}

			if (Bits.getInt32 (txt, srcEntry + 4) != 0) 
			{
				Array.Copy (txt, 
							neaddr, 
							txt, 
							0xC300 + Bits.getInt32 (txt, 
													srcEntry + 4), 
							c - neaddr); 
			}
			
			int d = 0xC300 + Bits.getInt32 (txt, srcEntry);
			
			while (b-- > 0)
			{
				txt [d] = bytes [d++ - (0xC300 + Bits.getInt32 (txt, srcEntry))];
			}

			Compression.CompressText (	txt, 
										Globals.mainForm.rom);

			//b=length needed. ; small - big + length

			//listView1.Invalidate();
		}
		*/
	}
}