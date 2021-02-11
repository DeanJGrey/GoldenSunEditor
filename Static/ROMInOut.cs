using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace GoldenSunEditor
{
    public static class ROMInOut
    {
		//
		// OPEN
		//
		public static bool OpenFile ()
		{
			string fileName;
			int version = -1;
			int fileTable = -1;
			char language;
			string palettePath = "";

			// OPEN FILE CHOOSER
			OpenFileDialog ofd = new OpenFileDialog ();

			ofd.Title = "Open a GBA/NDS File";

			ofd.Filter = "GBA/NDS/3DS file(*.gba;*.nds;*.3ds)|*.gba;*.nds;*.3ds";

			if (ofd.ShowDialog () == DialogResult.OK)
			{
				fileName = ofd.FileName;												// fileName includes path as well

				 Console.WriteLine (fileName);
			}
			else
				return false;

			// CHECK ROM LOADED
			if (fileName.EndsWith (".gba", true, null))
			{
				try
				{
					Globals.rOM = System.IO.File.ReadAllBytes (fileName);       //Load entire ROM to buffer; GBA ROMs are maxed at 32 MB.
				}
				catch
				{
					return false;
				}

				if (Globals.rOM == null)                                        // If rom was not writen to
					return false;                                                     // Stop
				else
					Console.WriteLine ("ROM was able to be read and writen.");
			}
			else
				return false;

			// CHECK ROM LANGUAGE
			switch (Bits.GetString (Globals.rOM, 0x000000A0, 15))
			{
				// Golden Sun 1
				case "Golden_Sun_AAGS": //United States
				case "OugonTaiyo_AAGS": //Japan
				case "GOLDEN_SUN_AAGS": //Italy
					version = 0;
					fileTable = 0x08320000;
					break;

				// Golden Sun 2
				case "GOLDEN_SUN_BAGF": // United States
				case "OUGONTAIYO_BAGF": // Japan
					version = 1;
					fileTable = 0x08680000;
					break;

				// Mario Golf
				case "MARIOGOLFGBABMG":
					version = 10;
					fileTable = 0x08800000;
					break;

				// Mario Tennis
				case "MARIOTENNISABTM":
					version = 11;
					fileTable = 0x08C28000;
					break;
			}

			language = (char) Globals.rOM [0x000000AF];

			if (language == 'E' && version == 1)
			{ //Chinese check
				if (Bits.getUInt32 (Globals.rOM, 0x08090000 & 0x01FFFFFF) == 0xEA00002E &&
					Bits.getUInt32 (Globals.rOM, 0x08090004 & 0x01FFFFFF) == 0x51AEFF24 &&
					Bits.getUInt32 (Globals.rOM, 0x08090008 & 0x01FFFFFF) == 0x21A29A69 &&
					Bits.getUInt32 (Globals.rOM, 0x0809000C & 0x01FFFFFF) == 0x0A82843D)
					language = 'C';
			}
			else if (version == 0)
			{
				switch (language)
				{
					case 'E':
					case 'I':
						fileTable = 0x08320000;
						break;
					case 'J':
						fileTable = 0x08317000;
						break;
					case 'D':
						fileTable = 0x0831FE00;
						break;
					case 'F':
					case 'S':
						fileTable = 0x08321800;
						break;
				}
			}

			Console.WriteLine ("Language check complete.");

			Console.WriteLine ("ROM load complete.");

			return true;
		}

		public static byte [] OpenFilePart (string fileName, int address, int size)
		{
			byte [] data = new byte [size];

			using (FileStream a = new FileStream (fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				a.Seek (address, SeekOrigin.Begin);

				a.Read (data, 0, size);
			}

			return data;
		}










		//
		// SAVE
		//
		public static void SaveFile (string filename, byte [] buffer)
		{
			System.IO.File.WriteAllBytes (filename, buffer);
		}

		public static byte [] SaveFilePart (string filename, int addr, int size, byte [] data)
		{
			using (FileStream a = new FileStream (filename, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
			{
				a.Seek (addr, SeekOrigin.Begin);

				a.Write (data, 0, size);
			}

			return data;
		}
	}
}
