global using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Stream2.Test2
{
    public class Global
    {
        public static string HexDump(byte[] bytes, int bytesPerLine = 16)
        {
            if (bytes == null) return "<null>";
            int bytesLength = bytes.Length;

            char[] HexChars = "0123456789ABCDEF".ToCharArray();

            int firstHexColumn =
                  8                   // 8 characters for the address
                + 3;                  // 3 spaces

            int firstCharColumn = firstHexColumn
                + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                + 2;                  // 2 spaces 

            int lineLength = firstCharColumn
                + bytesPerLine           // - characters to show the ascii value
                + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

            char[] line = (new String(' ', lineLength - Environment.NewLine.Length) + Environment.NewLine).ToCharArray();
            int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
            StringBuilder result = new StringBuilder(expectedLines * lineLength);

            for (int i = 0; i < bytesLength; i += bytesPerLine)
            {
                line[0] = HexChars[(i >> 28) & 0xF];
                line[1] = HexChars[(i >> 24) & 0xF];
                line[2] = HexChars[(i >> 20) & 0xF];
                line[3] = HexChars[(i >> 16) & 0xF];
                line[4] = HexChars[(i >> 12) & 0xF];
                line[5] = HexChars[(i >> 8) & 0xF];
                line[6] = HexChars[(i >> 4) & 0xF];
                line[7] = HexChars[(i >> 0) & 0xF];

                int hexColumn = firstHexColumn;
                int charColumn = firstCharColumn;

                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (j > 0 && (j & 7) == 0) hexColumn++;
                    if (i + j >= bytesLength)
                    {
                        line[hexColumn] = ' ';
                        line[hexColumn + 1] = ' ';
                        line[charColumn] = ' ';
                    }
                    else
                    {
                        byte b = bytes[i + j];
                        line[hexColumn] = HexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = HexChars[b & 0xF];
                        line[charColumn] = (b < 32 ? '·' : (char)b);
                    }
                    hexColumn += 3;
                    charColumn++;
                }
                result.Append(line);
            }
            return result.ToString();
        }

        public static byte[] Test(WipeID wi, string message, string key, bool store = false)
        {
            return _test(wi, Encoding.UTF8.GetBytes(message), Encoding.UTF8.GetBytes(key), store);
            Console.WriteLine();
        }

        public static byte[] Test(WipeID wi, string message, byte[] key, bool store = false)
        {
            return _test(wi, Encoding.UTF8.GetBytes(message), key, store);
        }

        public static byte[] _test(WipeID wi, byte[] message, byte[] key, bool store = false)
        {
            using (MemoryStream input = new MemoryStream(message))
            {
                if (!store)
                    Console.WriteLine($"[before] 明文是: {Encoding.UTF8.GetString(message)}");
                else
                    Console.WriteLine($"message len: {message.Length}");
                using (MemoryStream output = new MemoryStream())
                {
                    var data = wi.Encrypt(input, output);

                    File.WriteAllBytes($"{Guid.NewGuid().ToString().Replace("-", "")}.log", data);
                    Console.WriteLine("encrypt successfully!");
                    //Console.WriteLine($"密文是: {Encoding.UTF8.GetString(data)}");

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (MemoryStream ms2 = new MemoryStream())
                        {
                            ms.Write(data, 0, data.Length);
                            var plaindata = wi.Decrypt(ms, ms2);
                            if (!store)
                                Console.WriteLine($"[after] 明文是:  {Encoding.UTF8.GetString(plaindata)}");
                            else
                                Console.WriteLine($"plaindata len: {plaindata.Length}");
                            if (store)
                            {
                                File.WriteAllBytes("./plain.exe", plaindata);
                                Console.WriteLine("write file");
                            }
                            return plaindata;
                        }
                    }
                }
            }
        }
    }
}