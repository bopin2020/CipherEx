using System.Text;

using static Stream2.Test2.Global;

namespace Stream2.Test2
{
    [TestClass]
    public class UnitTest1
    {

        const string key = "bopinkey";

        private WipeID wipeID;

        public UnitTest1()
        {
            wipeID = new WipeID();
        }

        [TestMethod]
        public void TestMethod4()
        {
            // collectionassert
            CollectionAssert.AreEqual(new byte[] { 0x00, 0x02, 0x03 }, new byte[] { 0x00, 0x02, 0x03 });
        }

        [TestMethod]
        public void TestMethod3()
        {
            var result = wipeID.SetCipherStream(Encoding.ASCII.GetBytes(key));

            Action<string> dele = (a =>
            {
                CollectionAssert.AreEqual(Encoding.UTF8.GetBytes(a), Test(wipeID, a, key));
            });

            Action<byte[]> dele2 = (a =>
            {
                CollectionAssert.AreEqual(a, _test(wipeID, a, Encoding.UTF8.GetBytes(key)));
            });

            Action<string> delefile = (a =>
            {
                CollectionAssert.AreEqual(File.ReadAllBytes(a), _test(wipeID, File.ReadAllBytes(a), Encoding.ASCII.GetBytes(key), true));
            });
            int length = 100;
            Random r = new Random();
            for (int i = length - 1; i >= 0; i--)
            {
                byte[] data = new byte[20];
                r.NextBytes(data);
                Console.WriteLine("[1] origin bytes =>");
                Console.WriteLine(Convert.ToBase64String(data));
                Console.WriteLine(Global.HexDump(data));
                dele2(data);
                Console.WriteLine("[2] end          =>\n");
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            byte[] calc = File.ReadAllBytes(@"c:\windows\system32\calc.exe");

            var result = wipeID.SetCipherStream(Encoding.ASCII.GetBytes(key));

            Console.WriteLine(Convert.ToBase64String(result));
            Action<string> test_str = (a =>
            {
                CollectionAssert.AreEqual(Encoding.UTF8.GetBytes(a), Test(wipeID, a, key));
            });

            Action<byte[]> dele2 = (a =>
            {
                CollectionAssert.AreEqual(a, _test(wipeID, a, Encoding.UTF8.GetBytes(key)));
            });

            Action<string> test_file = (a =>
            {
                if (File.Exists(a))
                    CollectionAssert.AreEqual(File.ReadAllBytes(a), _test(wipeID, File.ReadAllBytes(a), Encoding.ASCII.GetBytes(key), true));
            });

            test_file(@"D:\Code\CSharp\StrikeServer_v0.0.2_bugVersion\Ghoul Zombie\Ghoul Zombie\Misc\Core\mimikatz.exe");


            test_str("\0\0\0\0\0\0\0\0");
            test_str("\0\0\0\0\0\0\0\0\0");
            test_str("aaa\0sas\0\0\0\0\0\0sasa\0\0");
            test_str("aaa\0sas\0\0sasa\0\0\0\0sasa\0\0");
            test_str("111111111111111");
            test_str("000000000000000000000");
            test_str("0s");
            test_str("s21u92hhhhhhh421u021f");
            test_str("s21u91421u021");
            test_str("las21u90421u0212");
            test_str("aslas21u90421u021");
            test_str("naslas21u90421u021");
            test_str("fasnfnaslas21u90421u021");
            test_str("1222fnsjksaknfasnfnaslas21u90421u021");
            test_str("1222fnsjksaknfasnfnaslas21u90421u021ur1209eu1211212nslanas/><<>?_**()$%###$&0*()*JNNKHGVMLNm");
            test_str("hello2");
            test_str("thiss a test,bopin");
            test_str("hello");
            test_str("测试，中文明文字符串.../?");
            test_str("421u021");
            test_file(@"c:\windows\system32\calc.exe");
            test_file(@"c:\windows\system32\ntdll.dll");
            test_file(@"c:\windows\system32\kernel32.dll");
        }

        [TestMethod]
        public void TestMethod1()
        {
            byte[] hexData = {
            0x01,0x01,0x01,0x01,0x01,0x01,0x01,0x00,0x00,0x00,0x00,0x00,0x01,0x01,0x01,0x01,0x01,
            0x4D, 0x5A, 0x90, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x00, 0x00,
            0xB8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF8, 0x00, 0x00, 0x00,
            0x0E, 0x1F, 0xBA, 0x0E, 0x00, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x01, 0x4C, 0xCD, 0x21, 0x54, 0x68,
            0x69, 0x73, 0x20, 0x70, 0x72, 0x6F, 0x67, 0x72, 0x61, 0x6D, 0x20, 0x63, 0x61, 0x6E, 0x6E, 0x6F,
            0x74, 0x20, 0x62, 0x65, 0x20, 0x72, 0x75, 0x6E, 0x20, 0x69, 0x6E, 0x20, 0x44, 0x4F, 0x53, 0x20,
            0x6D, 0x6F, 0x64, 0x65, 0x2E, 0x0D, 0x0D, 0x0A, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x76, 0xAD, 0xF9, 0x7C, 0x17, 0xC3, 0xAA, 0x7C, 0x17, 0xC3, 0xAA, 0x7C, 0x17, 0xC3, 0xAA,
            0x75, 0x6F, 0x50, 0xAA, 0x7A, 0x17, 0xC3, 0xAA, 0x7C, 0x17, 0xC2, 0xAA, 0x54, 0x17, 0xC3, 0xAA,
            0x37, 0x6F, 0xC2, 0xAB, 0x75, 0x17, 0xC3, 0xAA, 0x37, 0x6F, 0xC7, 0xAB, 0x6E, 0x17, 0x7E, 0x81,0x00,0x00,
            };

            string key = "bopin668";
            var result = wipeID.SetCipherStream(Encoding.UTF8.GetBytes(key));
            CollectionAssert.AreEqual(hexData, _test(wipeID, hexData, result));
        }

        [TestMethod]
        public void Example_HowToUse()
        {
            using (WipeID wi = new WipeID())
            {
                wi.SetCipherStream("this is key");
                byte[] input = new byte[]
                {
                    0x90,0x90,0x90,0x90,
                    0x90,0x90,0x90,0x90,
                };
                using (MemoryStream outs = new MemoryStream())
                {
                    byte[] output = wi.Encrypt(input, outs);

                    using (MemoryStream outs2 = new MemoryStream())
                    {
                        CollectionAssert.AreEquivalent(input, wi.Decrypt(output, outs2));
                    }
                }
            }
        }
    }
}