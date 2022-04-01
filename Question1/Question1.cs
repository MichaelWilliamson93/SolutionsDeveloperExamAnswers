/*
 * This program has been designed to take a string value (temp_string), before encoding (Cipher.Encode) and decoding (Cipher.Decode) the string value. The encoding and decoding functions are then tested by
 * comparing the initial string value with itself once it has been both encoded and decoded.
*/

// If you would like to include the debugging comments that print the encoding and decoding results to the console, un-comment the #define debug variable below.
//#define debug

using System;

namespace Question1
{
    class Question1
    {
        private static readonly string temp_string = "Hi, my name is Michael.";

        static void Main(string[] args)
        {
            Cipher.Prep();
            Test();
        }

        private static void Test()
        {
            if (String.Equals(temp_string, Cipher.Decode(Cipher.Encode(temp_string)), StringComparison.InvariantCulture))
            {
                Console.WriteLine("Test succeeded");
            }
            else
            {
                Console.WriteLine("Test failed");
            }
        }
    }

    class Cipher
    {
        private static readonly char[] transcode = new char[64];

        public static char[] Prep()
        {
            for (int i = 0; i < (transcode.Length - 3); i++)
            {
                transcode[i] = (char)((int)'A' + i);
                if (i > 25) transcode[i] = (char)((int)transcode[i] + 6);
                if (i > 51) transcode[i] = (char)((int)transcode[i] - 0x4b);
            }
            transcode[(transcode.Length - 3)] = '+';
            transcode[(transcode.Length - 2)] = '/';
            transcode[(transcode.Length - 1)] = '=';

            return transcode;
        }

        public static string Encode(string input)
        {
            int l = input.Length;
            int cb = (l / 3 + (Convert.ToBoolean(l % 3) ? 1 : 0)) * 4;

            char[] output = new char[cb];
            for (int i = 0; i < cb; i++)
            {
                output[i] = '=';
            }

            int c = 0;
            int reflex = 0;
            const int s = 0x3f;

            for (int j = 0; j < l; j++)
            {
                reflex <<= 8;
                reflex &= 0x00ffff00;
                reflex += input[j];

                int x = ((j % 3) + 1) * 2;
                int mask = s << x;
                while (mask >= s)
                {
                    int pivot = (reflex & mask) >> x;
                    output[c++] = transcode[pivot];
                    int invert = ~mask;
                    reflex &= invert;
                    mask >>= 6;
                    x -= 6;
                }
            }

            switch (l % 3)
            {
                case 1:
                    reflex <<= 4;
                    output[c++] = transcode[reflex];
                    break;

                case 2:
                    reflex <<= 2;
                    output[c++] = transcode[reflex];
                    break;

            }
#if debug
            Console.WriteLine("[DEBUG] The test string (" + input + ") has been encoded: " + new string(output) + "\n");
#endif
            return new string(output);
        }

        public static string Decode(string input)
        {
            int l = input.Length;
            int cb = (l / 4 + ((Convert.ToBoolean(l % 4)) ? 1 : 0)) * 3 + 1;
            char[] output = new char[cb];
            int c = 0;
            int bits = 0;
            int reflex = 0;
            for (int j = 0; j < l; j++)
            {
                reflex <<= 6;
                bits += 6;
                bool fTerminate = ('=' == input[j]);
                if (!fTerminate)
                    reflex += IndexOf(input[j]);

                while (bits >= 8)
                {
                    int mask = 0x000000ff << (bits % 8);
                    output[c++] = (char)((reflex & mask) >> (bits % 8));
                    int invert = ~mask;
                    reflex &= invert;
                    bits -= 8;
                }

                if (fTerminate)
                    break;
            }
#if debug
            Console.WriteLine("[DEBUG] The dencoded test string (" + input + ") has been decoded: " + new string(output) + "\n");
#endif
            return new string(output);
        }

        private static int IndexOf(char ch)
        {
            int index;
            for (index = 0; index < transcode.Length; index++)
                if (ch == transcode[index])
                    break;
            return index;
        }
    }

}


