using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GoldenSunEditor
{
    static class Bits 
	{
		//
		// INT
		//
        static public int getInt16 (byte[] buffer, int pos) 
		{
            return buffer[pos++] | (buffer[pos] << 8);
        }

        static public int getInt16 (byte[] buffer, uint pos) 
		{
            return buffer[pos++] | (buffer[pos] << 8);
        }

        static public void setInt16 (byte[] buffer, int pos, int value) 
		{
            buffer[pos++] = (byte)value;
            buffer[pos++] = (byte)(value >> 8);
        }

        static public int getInt32 (byte[] buffer, int pos) 
		{
            return buffer[pos++] | (buffer[pos++] << 8) | buffer[pos++] << 16 | (buffer[pos] << 24);
        }

        static public void setInt32 (byte[] buffer, int pos, int value) 
		{
            buffer[pos++] = (byte)value;
            buffer[pos++] = (byte)(value >> 8);
            buffer[pos++] = (byte)(value >> 16);
            buffer[pos++] = (byte)(value >> 24);
        }

        static public uint getUInt32 (byte[] buffer, int pos) 
		{
            return unchecked((uint)(buffer[pos++] | (buffer[pos++] << 8) | buffer[pos++] << 16 | (buffer[pos] << 24)));
        }








		//
		// BITS
		//
		static public int getBits (byte [] buf, int addr, int numOfBits)
		{
			int value = 0; // What we will be writing bits to

			for (int i = 0; i < numOfBits; i += 8) // for each "numOfBits", increase iteration by 8 (1 byte)
			{
				value |= buf [addr ++] << i;
			}

			Console.WriteLine(value);

			return value;
		}

		static public void setBits (byte[] buf, int addr, int numOfBits, int value)
		{
			for (int j = 0; j < numOfBits; j += 8)
			{
				buf [addr + (j >> 3)] = (byte) value;

				value >>= 8;
			}
		}









		//
		// STRING
		//
		public static string GetString (byte[] buffer, int pos, int length) 
		{
            StringBuilder strbuild = new System.Text.StringBuilder (16);

            while (length-- > 0) 
			{
                strbuild.Append ((char) buffer [pos++]);
            }

            return strbuild.ToString ();
        }

		public static string GetTextShort (byte[] txt, int index)
		{
			byte	p = 0,
					n = 0;


			StringBuilder stringBuilder = new StringBuilder(0x200);

			if (index < 0)
				return "";

			int sourcePosition = Bits.getInt32(txt, index << 2);

			if (Bits.getInt32(txt, 0) == 0)
				sourcePosition += 0x0000C300;

			do
			{
				n = txt [sourcePosition ++];

				if (n != 0)
				{
					if (n < 32 || n > 0x0000007E)
					{
						stringBuilder.Append('[' + (n.ToString()) + ']');

						if ((n == 1 || n == 3) && (p < 17 || p > 20) && p != 26 && p != 29)
						{
							n = 0;
						}
					}
					else
					{
						stringBuilder.Append((char)n);
					}
				}

				p = n;
			}

			while (n != 0);
				return stringBuilder.ToString();
		}

		public static string GetTextLong (byte[] txt, int index)
		{
			int srcEntry = index; byte p = 0, n = 0;

			StringBuilder str = new StringBuilder(0x200);

			int srcPos = 0xC300 + Bits.getInt32(txt, srcEntry * 4);

			do
			{
				n = txt[srcPos++];

				if (n != 0)
				{
					if (n < 32 || n > 0x7E)
					{
						str.Append('[' + (n.ToString()) + ']');

						if ((n == 1 || n == 3) && (p < 17 || p > 20) && p != 26 && p != 29)
						{
							str.Append("\r\n");
						}
					}
					else 
					{ 
						str.Append ((char) n); 
					}
				}
				
				p = n;
			} 
			
			while (n != 0);
				return str.ToString();
		}

		public static string GetText2 (byte [] buffer, int index)
		{
			System.Text.StringBuilder strbuild = new StringBuilder (0x200);

			int addr2 = Bits.getInt32 (buffer, 0x220B80 + index * 4) + 0x220B80;

			int length = 2000; 
			
			int pos = addr2 & 0xFFFFFF;

			while (length-- > 0)
			{
				int c = Bits.getInt16 (buffer, pos); 
				
				pos += 2;

				if (c == 0)
					break;

				if ((c < 0x100) && (c > 0xF))
					strbuild.Append ((char) c);
				else
					strbuild.Append ("[" + c.ToString ("X4") + "]");
			}

			return strbuild.ToString();
		}









		//
		// CONVERSION
		//
		public static byte[] TextToBytes(string[] items)
		{
			byte[] bytes = new byte[0x80000];
			int a = 0;
			int b = 0;

			for (int i = 0; i < items.Count(); i++)
			{
				bytes[a++] = (byte)b;
				bytes[a++] = (byte)(b >> 8);
				bytes[a++] = (byte)(b >> 16);
				bytes[a++] = (byte)(b >> 24);

				for (int j = 0; j < items[i].Count(); j++)
					bytes[0xC300 + b++] = (byte)items[i][j];

				bytes[b++] = 0;
			}

			return bytes;
		}











		//
		// LIST
		//
		public static List <int> GetTextMatchIndexes (byte[] txt, String textToFindMatchesOf, List <int> itemsIndex)
		{
			byte [] bytes = new byte [0x00000200];												// Array of bytes of particular size ???

			// for each character of string textToFindMatchesOf
            for (int ii = 0, jj = 0; ii < textToFindMatchesOf.Length; ii += 0)
            {
				// ???
				if (textToFindMatchesOf [ii] == '[')                                            // If the character is a [
				{
					int num = 0;																// ???

					// Until we hit ] 
					while (textToFindMatchesOf [++ii] != ']')                                   // ???
					{
						num *= 10;                                                              // ???
						num += (byte) textToFindMatchesOf [ii];									// ???
						num -= 0x00000030;                                                      // ???
					}

					ii ++;                                                                      // ??? 

					bytes [jj ++] = (byte) num;
				}
				else if ((byte) textToFindMatchesOf [ii] == 0x0000000D && //13
						(byte) textToFindMatchesOf [ii + 1] == 0x0000000A) //10
					ii += 2;
				else
					bytes [jj ++] = (byte) textToFindMatchesOf [ii ++];
			}

			int i = 0;

			int sourceEntry = itemsIndex [i] * 4;
			
			List <int> matchList = new List <int> ();

			while (true)
			{
				if ((sourceEntry < 0) || (sourceEntry >= 12460))
				{
					if (bytes [0] == 0)
						matchList.Add (i);

					i++;

					if (i >= itemsIndex.Count ())
						break;

					sourceEntry = itemsIndex [i] * 4;
					
					continue;
				}

				int srcPos = 0x0000C300 + Bits.getInt32 (	txt,
															sourceEntry);

				while (true)
				{
					int n = 0;

					while ((txt[srcPos + n] != 0) && (txt[srcPos + n] != bytes[0]))
						srcPos++;

					while ((bytes[n] != 0) && (txt[srcPos + n] == bytes[n]))
						n++;

					if (bytes[n] == 0) 
					{ 
						matchList.Add (i); 
						
						break; 
					}

					if (txt[srcPos + n] == 0) 
						break; 

					srcPos++;
				}

				i++;

				if (i >= itemsIndex.Count ())
					break;

				sourceEntry = itemsIndex [i] * 4;
			}

			return matchList;
		}

		public static List <int> GetTextMatches2 (byte[] txt, String str, List <int> items) //, int[] matchList)
		{
			byte[] bytes = new byte [0x00000200];

			int a = 0, b = 0;

			while (a < str.Length)
			{
				if (str[a] == '[')
				{
					int num = 0;

					while (str[++a] != ']')
					{
						int n = str[a];

						if ((n >= 0x00000041) && (n <= 0x00000046))
							num = (num * 16) + 10 + (n - 0x00000041);
						else if ((n >= 0x00000061) && (n <= 0x00000066))
							num = (num * 16) + 10 + (n - 0x00000061);
						else
							num = (num * 16) + (byte)(str[a]) - 0x00000030;
					}

					a++;

					bytes[b++] = (byte)num;
				}
				else if (((byte)str[a] == 13) && ((byte)str[a + 1] == 10))
					a += 2;
				else
					bytes[b++] = (byte)str[a++];
			}

			int i = 0;

			int srcEntry = items[i] * 4;
			
			int sortInd = 0; 

			List <int> matchList = new List <int> ();

			while (true)
			{
				if ((srcEntry < 0) || (srcEntry >= 12460))
				{
					if (bytes[0] == 0)
						matchList.Add(i);

					i++;

					if (i >= items.Count())
						break;
					
					srcEntry = items[i] * 4;
					
					continue;
				}

				int srcPos =  Bits.getInt32(txt, 0x220B80 + srcEntry) + 0x220B80;

				while (true)
				{
					int n = 0;

					while ((txt[srcPos + n] != 0) && (txt[srcPos + n] != bytes[0]))
						srcPos++;

					while ((bytes[n] != 0) && (txt[srcPos + n] == bytes[n]))
						n++;

					if (bytes[n] == 0) 
					{
						matchList.Add(i); 

						break; 
					}

					if (txt[srcPos + n] == 0)
						break;

					srcPos++;
				}

				i++;

				if (i >= items.Count ())
					break;

				srcEntry = items[i] * 4;
			}

			return matchList;
		}

		public static List <int> NumList (int range)
		{
			List <int> entries = new List <int> ();

			for (int i = 0; i < range; i++)
				entries.Add (i);

			return entries;
		}

		public static List <int> NumList (int start, int range)
		{
			List <int> entries = new List <int> ();

			for (int i = 0; i < range; i++)
				entries.Add (start + i);

			return entries;
		}
	}
}