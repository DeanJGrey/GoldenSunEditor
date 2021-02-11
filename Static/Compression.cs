using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenSunEditor
{
    //static class Comp
    static class Compression

    {
        //
        // DECOMPRESS
        //

        static public byte[] DecompressTextOld (byte[] src)
        {
            DateTime c = DateTime.Now;

            int asmpchar = Bits.getInt32 (src, 0x38578) - 0x8000000;
            int asmptext = Bits.getInt32 (src, 0x385DC) - 0x8000000;
            int chardata = Bits.getInt32 (src, asmpchar) - 0x08000000;
            int charpntrs = Bits.getInt32 (src, asmpchar + 4) - 0x08000000;

            byte[] des = new byte[0x800000]; int desEntry = 0, desPos = 0xC300;

            for (int srcI = 0; srcI < 12461; srcI++)
            {
                Bits.setInt32(des, desEntry, desPos - 0xC300); desEntry += 4;

                int srcInd = srcI;

                int textTree = Bits.getInt32(src, asmptext + ((srcInd >> 8) << 3)) - 0x08000000;

                int textLenAddr = Bits.getInt32(src, asmptext + ((srcInd >> 8) << 3) + 4) - 0x08000000;

                srcInd &= 0xFF;
                while (srcInd-- != 0)
                {
                    int cLen;
                    do
                    {
                        cLen = src[textLenAddr++];

                        textTree += cLen;
                    } 
                    while (cLen == 0xFF);
                }

                int initChar = 0;

                textTree <<= 3;

                do
                {
                    int charTree = (chardata + Bits.getInt16 (src, charpntrs + (initChar << 1))) << 3;

                    int charSlot = charTree - 12;

                    while (((src[charTree >> 3] >> (charTree++ & 7)) & 1) == 0)
                    {
                        if (((src[textTree >> 3] >> (textTree++ & 7)) & 1) == 1)
                        {
                            int depth = 0;

                            while (depth >= 0)
                            {
                                while (((src[charTree >> 3] >> (charTree++ & 7)) & 1) == 0)
                                {
                                    depth++;
                                }

                                charSlot -= 12;

                                depth--;
                            }
                        }
                    }

                    initChar = (Bits.getInt16(src, charSlot >> 3) >> (charSlot & 7)) & 0xFFF;

                    des[desPos++] = (byte)initChar;

                }
                while (initChar != 0);
            }

            Console.WriteLine(DateTime.Now - c + " (Old Text Decompression)");

            return des;
        }






















        static byte[] DecompressBattleBackground (byte[] src, int srcPos)
        {
            byte[] des = new byte[0x8000];
            int desPos = 0;
            int bits = 0,
                i = 0x60,
                bitnum = 0;

            bits += ((int)src[srcPos++] << bitnum) + ((int)src[srcPos++] << (8 + bitnum));

            for (int j = 0; j < 0x7800; j++)
            {
                switch (bits & 7)
                {
                    case 0:
                    case 4:
                        bits >>= 2; bitnum -= 2;
                        break;
                    case 1:
                        bits >>= 3; bitnum -= 3;
                        i += 1 + (bits & 1); bits >>= 1; bitnum -= 1;
                        break;
                    case 2:
                        bits >>= 3; bitnum -= 3;
                        if ((bits & 1) == 0)
                        {
                            bits >>= 1; bitnum -= 1;
                            i += 11 + (bits & 0xF); bits >>= 4; bitnum -= 4;
                        }
                        else
                        {
                            bits >>= 1; bitnum -= 1;
                            i -= 11 + (bits & 0xF); bits >>= 4; bitnum -= 4;
                        }
                        break;
                    case 3:
                        bits >>= 3; bitnum -= 3;
                        i += 3 + (bits & 7); bits >>= 3; bitnum -= 3;
                        break;
                    case 5:
                        bits >>= 3; bitnum -= 3;
                        i -= 1 + (bits & 1); bits >>= 1; bitnum -= 1;
                        break;
                    case 6:
                        bits >>= 3; bitnum -= 3;
                        i = 0x60 + (bits & 0x7F); bits >>= 7; bitnum -= 7;
                        break;
                    case 7:
                        bits >>= 3; bitnum -= 3;
                        i -= 3 + (bits & 7); bits >>= 3; bitnum -= 3;
                        break;
                }

                des[desPos++] = (byte)i;

                if (bitnum < 0)
                {
                    bitnum += 16;

                    bits += ((int)src[srcPos++] << bitnum) + ((int)src[srcPos++] << (8 + bitnum));
                }
            }

            return des;
        }






















        static public byte [] DecompressText (byte [] source)
        {
            DateTime dateTime = DateTime.Now;

            int[] bitCodeArray = new int[0x00010000];
            byte[] bitLength = new byte[0x00010000];
            short[] bitChar = new short[0x00010000];

            int asmpChar = Bits.getInt32 (source, 0x00038578) - 0x8000000;
            int asmpText = Bits.getInt32 (source, 0x000385DC) - 0x8000000;
            int charData = Bits.getInt32 (source, asmpChar) - 0x08000000;
            int charPointers = Bits.getInt32 (source, asmpChar + 4) - 0x08000000;

            int iii = 0;
            for (int char1 = 0; char1 < 0x00000100; char1++)
            {
                if (charPointers == asmpChar)
                    break;

                if (Bits.getInt16 (source, charPointers) == 0x00008000)
                {
                    charPointers += 2;

                    continue;
                }

                int charTree;
                charTree = charData;
                charTree += Bits.getInt16 (source, charPointers);
                charTree <<= 3;

                charPointers += 2;

                int charSlot;
                charSlot = charTree;
                charSlot -= 12;

                byte bits = 0;
                int bitCode = 0;

                int entry;
                entry = char1;
                entry <<= 8;

                int jjj = 0;
                do
                {
                    while (((source [charTree >> 3] >> (charTree++ & 7)) & 1) == 0)
                        bits++;
                    
                    bitChar [entry] = (short) ((Bits.getInt16 (source, charSlot >> 3) >> (charSlot & 7)) & 0xFFF);

                    charSlot -= 12;

                    bitLength [entry] = bits;

                    if (bits >= 24)
                        return DecompressTextOld (source);

                    bitCodeArray [entry] = bitCode;

                    int kkk = 0;
                    while (((bitCode >> (bits - 1)) & 1) == 1)
                    {
                        bits -= 1;

                        bitCode ^= 1 << bits;

                        Console.WriteLine ("iii: " + iii++ + ", jjj: " + jjj++ + ", kkk: " + kkk++);
                    }

                    bitCode |= 1 << (bits - 1);

                    entry += 1;
                }
                while (bits > 0);
            }

            int textTree = 0;

            int textLengthAddress = 0;

            byte[] destination = new byte [0x00800000];

            int destinationEntry = 0;

            int destinationPosition = 0x0000C300;

            for (int ii = 0; ii < 12461; ii++)
            {
                Bits.setInt32 (destination, destinationEntry, destinationPosition - 0x0000C300);

                destinationEntry += 4;

                int sourceIndex = ii;

                if ((sourceIndex & 0xFF) == 0)
                {
                    textTree = Bits.getInt32 (source, asmpText + ((sourceIndex >> 8) << 3));
                    textTree -= 0x08000000;

                    textLengthAddress = Bits.getInt32 (source, asmpText + ((sourceIndex >> 8) << 3) + 4);
                    textLengthAddress -= 0x08000000;
                }
                else
                {
                    int cLength;

                    do
                    {
                        cLength = source [textLengthAddress++];

                        textTree += cLength;
                    }
                    while (cLength == 0xFF);
                }

                int initChar = 0;

                int bitNumber = 0;

                int val = 0;

                int textTree2 = textTree;

                do
                {
                    while (bitNumber < 24)
                    {
                        val |= (int) (source [textTree2++] << bitNumber);

                        bitNumber += 8;
                    }

                    int entry = initChar << 8;

                    while ((val & ((1 << bitLength [entry]) - 1)) != bitCodeArray [entry])
                        entry++;

                    initChar = bitChar [entry];

                    val >>= bitLength [entry];

                    bitNumber -= bitLength [entry];

                    destination [destinationPosition ++] = (byte) initChar;
                }
                while (initChar != 0);
            }

            Console.WriteLine (DateTime.Now - dateTime + " (Text Decompression)");

            return destination;
        }























        static public int[] DecompressF (byte[] src, int srcPos, byte[] des, int desPos, int format) 
        {
            int desStart = desPos;
            
            int bits, 
                readcount, 
                i, 
                _byte;
            
            uint n; 

            ulong z = 0xFEDCBA9876543210;
            
            if (format == 0 || format == 2) 
            {
                bits = 0;

                int bitnum = 0;

                if ((srcPos & 1) == 1) 
                {
                    bits = src [srcPos++]; 

                    bitnum = 8; 
                }

                bits += ((int)src[srcPos++] << bitnum) + ((int)src[srcPos++] << (8 + bitnum));

                while (true) 
                {
                    readcount = 0;

                    if ((bits & 1) == 1) 
                    {
                        bits >>= 1; 
                        
                        bitnum -= 1;

                        if (format == 0) 
                        { //Format 0: Constant
                            des[desPos++] = (byte)bits; 
                            
                            bits >>= 8; bitnum -= 8;
                        } 
                        else 
                        { //Format 2: Recent Priority
                            if ((bits & 1) == 1) 
                            {
                                i = 2; bits >>= 1; 
                                
                                bitnum -= 1;
                            } 
                            else if ((bits & 2) == 2) 
                            {
                                i = 3; bits >>= 2; 
                                
                                bitnum -= 2;
                            } 
                            else 
                            {
                                i = 4; bits >>= 2; 
                                
                                bitnum -= 2;
                            }

                            for (int offset = 0; offset < 8; offset += 4) 
                            {
                                _byte = (bits & ((1 << i) - 1)); bits >>= i; 
                                
                                bitnum -= i; //Read from compressed data.
                                
                                n = (uint) (0xF & (z >> (int) (_byte << 2))); //Get selected value.
                                
                                if (_byte != 0xF)
                                { // Check 0xF because <<0x40 does nothing. (ex: no-op?)
                                    z = (z & (unchecked((ulong)-1) << ((_byte + 1) << 2))) |    //Keep tail end.
                                        ((z & ((((ulong)1) << (_byte << 2)) - 1)) << 4) |       //Close gap.
                                        n;                                                      //Put selected value in front.
                                } 
                                else 
                                {
                                    z = (z << 4) | n; 
                                }

                                des [desPos] |= (byte) (n << offset); //Write decompressed 4-bit.
                            }

                            desPos++;
                        }
                        if (bitnum < 0) 
                        {
                            bitnum += 16;

                            bits += ((int)src[srcPos++] << bitnum) + ((int) src [srcPos ++] << (8 + bitnum));
                        }
                    } 
                    else if ((bits & 3) == 0) { readcount = 2; bits >>= 2; bitnum -= 2;} //Format 0 & 2: Distance Length: Length
                    else if ((bits & 7) == 2) {readcount = 3; bits >>= 3; bitnum -= 3;} 
                    else if ((bits & 0xF) == 6) {readcount = 4; bits >>= 4; bitnum -= 4;} 
                    else if ((bits & 0x1F) == 0xE) {readcount = 5; bits >>= 5; bitnum -= 5;} 
                    else if ((bits & 0x7F) == 0x1E) {readcount = 6; bits >>= 7; bitnum -= 7;} 
                    else if ((bits & 0x7F) == 0x5E) {readcount = 7; bits >>= 7; bitnum -= 7;} 
                    else if ((bits & 0x3F) == 0x3E) 
                    {
                        readcount = 7 + ((bits >> 6) & 3); bits >>= 8; bitnum -= 8;

                        if (readcount == 7) 
                        {
                            readcount = 10 + (bits & 127); bits >>= 7; bitnum -= 7;

                            if (readcount == 10) 
                            { 
                                return new int [] {srcPos, desPos}; 
                            } // des; }
                        }
                    }

                    if (bitnum < 0) 
                    {
                        bitnum += 16;

                        bits += ((int)src[srcPos++] << bitnum) + ((int)src[srcPos++] << (8 + bitnum));
                    }

                    if (readcount != 0) 
                    { //Format 0 & 2: Distance Length: Distance
                        uint offset = 0;
                        
                        if ((bits & 1) == 0) 
                        { //Long Distance - 0~12-bit distance
                            bits >>= 1; bitnum -= 1;

                            offset = (uint)desPos - (uint)desStart - 33;

                            _byte = 12;

                            while (offset < (1 << (_byte - 1))) 
                            {
                                _byte -= 1;
                            }

                            offset = (uint)(32 + (bits & ((1 << _byte) - 1))); bits >>= _byte; bitnum -= _byte;

                        } 
                        else 
                        { //Short Distance - 5-bit distance
                            bits >>= 1; 
                            
                            bitnum -= 1;
                            
                            offset = (uint)(bits & 31); 
                            
                            bits >>= 5; bitnum -= 5;
                        }

                        if (bitnum < 0) 
                        {
                            bitnum += 16;
                            
                            bits += ((int)src[srcPos++] << bitnum) + ((int)src[srcPos++] << (8 + bitnum));
                        }

                        //if (desPos < offset) { return null; }

                        while (readcount-- > 0) 
                        {
                            des[desPos] = des[desPos - offset - 1]; 
                            
                            desPos++;
                        }
                    }
                }
            } 
            else 
            { //Format 1
                while (true) 
                {
                    bits = src[srcPos++];

                    for (int j = 0x80; j > 0; j >>= 1) 
                    {
                        if ((bits & j) == 0) 
                        {
                            des[desPos++] = src[srcPos++];
                        } 
                        else 
                        {
                            readcount = src[srcPos++];
                            int offset = src[srcPos++] | ((readcount & 0xF0) << 4);
                            readcount = readcount & 15;
                            if (readcount == 0) 
                            {
                                if (offset == 0) 
                                { 
                                    return new int[] { srcPos, desPos };
                                } // des; }

                                readcount = src[srcPos++] + 16;
                            }

                            //if (desPos < offset) { return null; }

                            while (readcount-- >= 0) 
                            {
                                des[desPos] = des[desPos - offset]; 
                                desPos++;
                            }
                        }
                    }
                }
            }

            //return des;
        }

























        static public int [] Decompress (byte [] src, int srcPos, byte[] des, int desPos)
        {
            return DecompressF (src, 
                                srcPos + 1, 
                                des, 
                                desPos, 
                                src [srcPos]);
        }





























        static public byte[] Decompress16All (byte[] src, int srcPos)
        {
            int entries = 0;

            int srcPos2 = srcPos;

            while (Bits.getInt32(src, srcPos2) != -1)
            {
                entries++;

                srcPos2 += 4;
            }

            //Console.WriteLine(srcPos2.ToString("x8") + "  " + entries);//Bits.getInt32(src,0x4f124));

            byte[] des = new byte[entries * 0x100];

            for (int desPos = 0; desPos < des.Length; desPos += 0x100)
            {
                Decompress16(src,
                                Bits.getInt32(src,
                                                srcPos) &
                                0x1ffffff,
                                des,
                                desPos);

                srcPos += 4;
            }

            return des;
        }




























        static public void Decompress16(byte[] src, int srcPos, byte[] des, int desPos)
        {
            //byte[] des = new byte[0x4000];

            int bits = 0,
                i = 0,
                bitnum = 0;

            uint n;
            ulong z = 0xFEDCBA9876543210;

            bits += ((int)src[srcPos++] << bitnum) + ((int)src[srcPos++] << (8 + bitnum));

            while (true)
            {
                if ((bits & 0x1) == 0x0) { i = 0; bits >>= 1; bitnum -= 1; }
                else if ((bits & 0x7) == 0x1) { i = 1; bits >>= 3; bitnum -= 3; }
                else if ((bits & 0xF) == 0x5) { i = 2; bits >>= 4; bitnum -= 4; }
                else if ((bits & 0xF) == 0xD) { i = 3; bits >>= 4; bitnum -= 4; }
                else if ((bits & 0xF) == 0x3) { i = 4; bits >>= 4; bitnum -= 4; }
                else if ((bits & 0xF) == 0xB) { i = 5; bits >>= 4; bitnum -= 4; }
                else if ((bits & 0xF) == 0x7) { i = 6; bits >>= 4; bitnum -= 4; }
                else if ((bits & 0x3F) == 0xF) { i = 7; bits >>= 6; bitnum -= 6; }
                else if ((bits & 0x3F) == 0x2F) { i = 8; bits >>= 6; bitnum -= 6; }
                else if ((bits & 0x3F) == 0x1F) { i = 9; bits >>= 6; bitnum -= 6; }
                else if ((bits & 0xFF) == 0x3F) { i = 10; bits >>= 8; bitnum -= 8; }
                else if ((bits & 0xFF) == 0xBF) { i = 11; bits >>= 8; bitnum -= 8; }
                else if ((bits & 0xFF) == 0x7F) { i = 12; bits >>= 8; bitnum -= 8; }
                else if ((bits & 0x3FF) == 0xFF) { i = 13; bits >>= 10; bitnum -= 10; }
                else if ((bits & 0x3FF) == 0x2FF) { i = 14; bits >>= 10; bitnum -= 10; }
                else if ((bits & 0x3FF) == 0x1FF) { i = 15; bits >>= 10; bitnum -= 10; }
                else if ((bits & 0x3FF) == 0x3FF) { return; }// des;}

                n = (uint)(0xF & (z >> (int)(i << 2))); //Get selected value.

                if (i != 0xF)
                { // Check 0xF because <<0x40 does nothing. (ex: no-op?)
                    z = (z & (unchecked((ulong)-1) << ((i + 1) << 2))) //Keep tail end.
                        | ((z & ((((ulong)1) << (i << 2)) - 1)) << 4) //Close gap.
                        | n; //Put selected value in front.
                }
                else
                {
                    z = (z << 4) | n;
                }

                des[desPos++] = (byte)n; //Write decompressed 4-bit.

                if (bitnum < 0)
                {
                    bitnum += 16;
                    bits += ((int)src[srcPos++] << bitnum) + ((int)src[srcPos++] << (8 + bitnum));
                }
            }
        }



























        //
        // COMPRESS
        //

        static public int[] CompressFormat1 (byte[] src, int srcPos, byte[] des, int desPos) 
        {
			int dst = 0, len = 0;
			while (true)
			{
				int fPos = desPos++;
				for (int a = 0x80; a != 0; a >>= 1)
				{
					if (srcPos >= src.Length)
					{
						des[fPos] |= (byte)a;
						des[desPos++] = 0;
						des[desPos++] = 0;
						return new int[] { srcPos, desPos };
						//return desPos;
					}
					//Find best Distance Length. (If current=Constant)
					int sp2 = srcPos + 1, dst2 = 0, len2 = 0, maxLen2 = Math.Min(0x110, src.Length - sp2);
					int dictEnd2 = Math.Max(0, sp2 - 0xFFF);
					for (int i = sp2 - 1; i >= dictEnd2; i--)
					{
						int j = 0;
						while (j < maxLen2)
						{
							if (src[sp2 + j] != src[i + j])
								break;
							j++;
						}
						if (j > len2)
						{
							dst2 = i; len2 = j;
							if (j >= 0x10F)
								break;
						}
					}
					//Find best Distance Length. (If current=Best Distance Length)
					int sp3 = srcPos + len, dst3 = 0, len3 = 0, maxLen3 = Math.Min(0x110, src.Length - sp3);
					int dictEnd3 = Math.Max(0, sp3 - 0xFFF);
					for (int i = sp3 - 1; i >= dictEnd3; i--)
					{
						int j = 0;
						while (j < maxLen3)
						{
							if (src[sp3 + j] != src[i + j])
								break;
							j++;
						}
						if (j > len3)
						{
							dst3 = i; len3 = j;
							if (j >= 0x10F)
								break;
						}
					}

					if (len2 == 0)
						len2 = 1;
					if (len3 == 0)
						len3 = 1;
					if ((len == len2) && (len3 == 1))
					{
					}
					else
					if ((1 + len2) >= (len + len3)) //Constant is better in this condition.
						len = 1;

					if (len < 2)
					{ //Insert Constant into compression.
						des[desPos++] = src[srcPos++];
						dst = dst2; len = len2;
					}
					else
					{ //Insert Distance Length into compression.
						des [fPos] |= (byte) a;

						if (len <= 16)
						{
							des[desPos++] = (byte)((((srcPos - dst) >> 8) << 4) | (len - 1));

							des[desPos++] = (byte)((srcPos - dst) & 0xFF);
						}
						else
						{
							des[desPos++] = (byte)(((srcPos - dst) >> 8) << 4);

							des[desPos++] = (byte)((srcPos - dst) & 0xFF);

							des[desPos++] = (byte)(len - 16 - 1);
						}

						srcPos += len;

						dst = dst3; len = len3;
					}
				}
			}
		}






















        static int[] bitTable = {0x1, 0x0, 0x3, 0x1, 0x4, 0x5, 0x4, 0xD,
                                 0x4, 0x3, 0x4, 0xB, 0x4, 0x7, 0x6, 0xF,
                                 0x6, 0x2F, 0x6, 0x1F, 0x8, 0x3F, 0x8, 0xBF,
                                 0x8, 0x7F, 0xA, 0xFF, 0xA, 0x2FF, 0xA, 0x1FF,
                                 0xA, 0x3FF};





























        static void Compress16 (byte[] src, int srcPos, byte[] des, int desPos) 
        { //Not tested.
            int bitnum = 0, 
                bits = 0, 
                i = 0;
            
            uint n = 0; 

            ulong z = 0xFEDCBA9876543210;
            
            while (srcPos < src.Length) 
            { //Length may be changed?
                while ((((z >> i) & 0xF) != src[srcPos])) 
                {
                    i += 4;
                }

                bits = bits | (bitTable[(i >> 1) + 1] << bitnum); bitnum += bitTable[i >> 1];

                while (bitnum >= 8) 
                {
                    des[desPos] = (byte)bits; bits >>= 8; bitnum -= 8;
                }

                n = (uint)(0xF & (z >> (int)i)); //Get selected value.

                if (i != 0x3C) 
                { // Check 0xF because <<0x40 does nothing. (ex: no-op?)
                    z = (z & (unchecked((ulong)-1) << (i + 4))) //Keep tail end.
                        | ((z & ((((ulong)1) << i) - 1)) << 4) //Close gap.
                        | n; //Put selected value in front.
                } 
                else 
                { 
                    z = (z << 4) | n; 
                }
                
                srcPos++;
            }

            bits = bits | (bitTable[0x21] << bitnum); bitnum += bitTable[0x20];

            while (bitnum > 0) 
            {
                des[desPos] = (byte)bits; bits >>= 8; bitnum -= 8;
            }
        }

        













        static public void CompressText (byte[] src, byte[] dest) 
        {
            DateTime c = DateTime.Now;//.Ticks;
            //Welcome to my Huffman Hamburger! (Scan text, do complex char tables, compress text.)
            //Scan text to generate frequency table.
            ushort char1 = 0, char2 = 0;
            ushort[] freq = new ushort[0x10000]; //We need to know how often each character combination occurs to determine best bit amounts.
            ushort[] clen = new ushort[0x100]; //How many chars each char has associated with it.
            ushort[] clst = new ushort[0x10000]; //Char list in the order they appear in the text.
            int srcEntry = 0;
            
            while ((Bits.getInt32(src, srcEntry) != 0) || (srcEntry == 0)) 
            { //Set up frequency table and char list (in order displayed in text.)
                int srcPos = 0xC300 + Bits.getInt32(src, srcEntry);
                
                do 
                {
                    char2 = src[srcPos++];

                    if (freq[char1 * 0x100 + char2]++ == 0) 
                    {
                        clst[char1 * 0x100 + clen[char1]++] = char2; //clen[char1]++;// += 1;
                    }

                    char1 = char2;
                } 
                while (char1 != 0); //Change to while < textArray?-- No, go through offset list instead.

                srcEntry += 4;
            }

            byte[] bitLen = new byte[0x10000];

            int[] bitCode = new int[0x10000];

            int addr2 = 0, chrptlen = 0;

            byte[] chrTbl = new byte[0x8000];

            byte[] chrPtrs = new byte[0x200];

            for (int c1 = 0; c1 < 0x100; c1++) 
            {
                if (clen[c1] == 0) 
                { 
                    chrPtrs[(c1 << 1) + 1] = 0x80;  
                    
                    continue; 
                }

                chrptlen = (c1 + 1) << 1;
                
                //if (c1 > 5) { continue; } //For testing.
                //Sort chars by symbol frequency (simple)
                //See https://en.wikipedia.org/wiki/Sorting_algorithm - Use a Stable one so same-freq chars stay in order.
                //I pick simple Insertion Sort for now, since we are dealing with small sets. (https://en.wikipedia.org/wiki/Insertion_sort)
                
                for (int i = 1; i < clen [c1]; i ++) 
                {
                    ushort x = clst[(c1 << 8) + i];

                    int j = i;

                    while ((j > 0) && (freq[(c1 << 8) + clst[(c1 << 8) + j - 1]] > freq[(c1 << 8) + x])) 
                    {
                        clst [(c1 << 8) + j] = clst[(c1 << 8) + j - 1];

                        j = j - 1;
                    }

                    clst[(c1 << 8) + j] = x;
                }
                
                //Sort chars by node frequency (More advanced)
                int[] symbSort = new int[0x100]; //Basically points to chars in order to be displayed in data.
                int[] symbBits = new int[0x100];
                int[] nodeHead = new int[0x100];
                int[] nodeTail = new int[0x100];
                int[] nodeFreq = new int[0x100]; nodeFreq[0] = 0x7FFFFFFF; nodeFreq[1] = 0x7FFFFFFF; //Ensure unused/node2 when there is none.
                int nodeA = 0, nodeI = 0, symbI = 0;
                
                if (clen[c1] > 1) 
                {
                    while ((symbI < clen[c1]) || (nodeA < nodeI - 1)) 
                    {
                        int symfreq1 = freq[(c1 << 8) + clst[(c1 << 8) + symbI]];

                        int symfreq2 = freq[(c1 << 8) + clst[(c1 << 8) + symbI + 1]];

                        if ((symbI + 1 < clen[c1]) && (symfreq2 <= nodeFreq[nodeA])) 
                        { //Symbol - Symbol
                            symbSort[symbI] = symbI + 1;
                            nodeHead[nodeI] = symbI; 
                            nodeTail[nodeI] = symbI + 1;
                            nodeFreq[nodeI] = symfreq1 + symfreq2;
                            symbI += 2;
                        } 
                        else if ((symbI < clen[c1]) && (symfreq1 <= nodeFreq[nodeA])) 
                        { // Symbol - Node
                            symbSort[symbI] = nodeHead[nodeA];
                            nodeHead[nodeI] = symbI; nodeTail[nodeI] = nodeTail[nodeA];
                            nodeFreq[nodeI] = symfreq1 + nodeFreq[nodeA];
                            symbI++; 
                            nodeA++;
                        } 
                        else if ((nodeA < nodeI - 1) && ((nodeFreq[nodeA + 1] < symfreq1) || ((symbI >= clen[c1])))) 
                        { // Node - Node
                            symbSort[nodeTail[nodeA]] = nodeHead[nodeA + 1];
                            nodeHead[nodeI] = nodeHead[nodeA]; 
                            nodeTail[nodeI] = nodeTail[nodeA + 1];
                            nodeFreq[nodeI] = nodeFreq[nodeA] + nodeFreq[nodeA + 1];
                            nodeA += 2;
                        } 
                        else if (nodeFreq[nodeA] < symfreq1) 
                        { // Node - Symbol
                            symbSort[nodeTail[nodeA]] = symbI;
                            nodeHead[nodeI] = nodeHead[nodeA]; 
                            nodeTail[nodeI] = symbI;
                            nodeFreq[nodeI] = nodeFreq[nodeA] + symfreq1;
                            symbI++; 
                            nodeA++;
                        }

                        symbBits[clst[(c1 << 8) + nodeHead[nodeI++]]] += 1;
                    }
                }

                addr2 += (((clen[c1] * 12) + 4) & -8);
                chrPtrs[(c1 << 1)] = (byte)(addr2 >> 3);
                chrPtrs[(c1 << 1) + 1] = (byte)(addr2 >> 11);
                int addr1 = addr2 - 12;
                byte bitsL = 0;
                int bitC = 0;
                
                for (int n = clen[c1]; n > 0; n--) 
                {
                    //List chars
                    chrTbl[(addr1 >> 3)] |= (byte)(clst[(c1 << 8) + nodeHead[nodeA]] << (addr1 & 7));
                    chrTbl[(addr1 >> 3) + 1] |= (byte)(clst[(c1 << 8) + nodeHead[nodeA]] >> (8 - (addr1 & 7)));
                    addr1 -= 12; 

                    //List the char's tree/flags
                    addr2 += symbBits[clst[(c1 << 8) + nodeHead[nodeA]]];
                    chrTbl[addr2 >> 3] |= (byte)(1 << (addr2++ & 7));
                    
                    //Calculate bit lengths for bit code.
                    bitsL += (byte)symbBits[clst[(c1 << 8) + nodeHead[nodeA]]];

                    bitLen[(c1 << 8) + clst[(c1 << 8) + nodeHead[nodeA]]] = bitsL;

                    //Generate bitCode table.
                    bitCode[(c1 << 8) + clst[(c1 << 8) + nodeHead[nodeA]]] = bitC;
                    while (((bitC >> (bitsL - 1)) & 1) == 1) { bitsL -= 1; bitC ^= 1 << bitsL; }
                    bitC |= 1 << (bitsL - 1);
                    
                    nodeHead[nodeA] = symbSort[nodeHead[nodeA]];
                }

                addr2 = (addr2 + 8) & -8;
            }

            //Finally compress the text.

            int val = 0, 
                bitnum = 0, 
                ctAddr = 0, 
                cstrstart = 0;

            byte[] cText = new byte[src.Length];

            byte[] txtref1 = new byte[0x200]; 

            int tr1Addr = 0;

            byte[] txtref2 = new byte[0x8000]; 

            int tr2Addr = 0;

            srcEntry = 0; char1 = 0;

            while ((Bits.getInt32 (src, srcEntry) != 0) || (srcEntry == 0)) 
            {
                if ((srcEntry & 0x3FC) == 0) 
                {
                    Bits.setInt32(txtref1, tr1Addr, ctAddr); 

                    tr1Addr += 4;

                    Bits.setInt32(txtref1, tr1Addr, tr2Addr); 

                    tr1Addr += 4;
                }
                cstrstart = ctAddr;

                int srcPos = 0xC300 + Bits.getInt32 (src, srcEntry); 
                
                val = 0;

                do 
                {
                    char2 = src[srcPos++];

                    val |= bitCode[(char1 << 8) + char2] << bitnum;

                    bitnum += bitLen[(char1 << 8) + char2];

                    while (bitnum >= 8) 
                    {
                        cText [ctAddr ++] = (byte) val; 

                        val >>= 8; 
                        
                        bitnum -= 8;
                    }

                    char1 = char2;
                } 
                
                while (char1 != 0); //Change to while < textArray?-- No, go through offset list instead.
                    srcEntry += 4; 
                
                if (bitnum != 0) 
                { 
                    cText [ctAddr++] = (byte) val; 

                    bitnum = 0; 
                }
                
                while ((ctAddr - cstrstart) > 0xFE) 
                { 
                    txtref2 [tr2Addr ++] = 0xFF; 
                    cstrstart += 0xFF; 
                }

                txtref2[tr2Addr++] = (byte)(ctAddr - cstrstart); //cstrstart = ctAddr;
            }

            //Now insert everything into the ROM.
            int insAddr = 0xFA0000;

            int loc1 = insAddr;

            Array.Copy (chrTbl, 
                        0, dest, 
                        insAddr, 
                        addr2 >> 3); 
            
            insAddr += addr2 >> 3;

            insAddr = (insAddr + 1) & -2;

            int loc2 = insAddr;

            Array.Copy (chrPtrs, 
                        0, 
                        dest, 
                        insAddr, 
                        chrptlen); 
            
            insAddr += chrptlen;

            Bits.setInt32 ( dest, 
                            0x38578, 
                            0x08000000 + insAddr);

            Bits.setInt32 ( dest, 
                            insAddr, 
                            0x08000000 + loc1); 
            
            insAddr += 4;

            Bits.setInt32 ( dest, 
                            insAddr, 
                            0x08000000 + loc2); 
            
            insAddr += 4;

            loc1 = insAddr;

            Array.Copy (cText, 
                        0, 
                        dest, 
                        insAddr, 
                        ctAddr); 
            
            insAddr += ctAddr;

            loc2 = insAddr;

            Array.Copy (txtref2, 
                        0, 
                        dest, 
                        insAddr, 
                        tr2Addr); 
            
            insAddr += tr2Addr;

            insAddr = (insAddr + 3) & -4;

            Bits.setInt32(dest, 0x385DC, 0x08000000 + insAddr);

            for (int a = 0; a < tr1Addr; a += 8) 
            {
                Bits.setInt32 ( dest, 
                                insAddr + a, 
                                0x08000000 + Bits.getInt32 (txtref1, 
                                                            a) + 
                                                            loc1);

                Bits.setInt32 (dest, insAddr + a + 4, 0x08000000 + Bits.getInt32 (txtref1, a + 4) + loc2);
            }

            Console.WriteLine ((DateTime.Now - c).ToString ());
        }
    }
}