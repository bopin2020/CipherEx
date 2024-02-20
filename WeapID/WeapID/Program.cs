using System.ComponentModel;
using System.Text;

namespace Stream2
{
    [Description("WipeID cipher algorithm")]
    public class WipeID : IDisposable
    {
        private byte[] _inner_s_box;
        private byte[] _inner_k_box;
        public byte[] SetCipherStream(string key, int cslen = 256)
        {
            return SetCipherStream(Encoding.ASCII.GetBytes(key), cslen);
        }

        public byte[] SetCipherStream(byte[] key, int cslen = 256)
        {
            if (key.Length % 8 != 0 && cslen % 256 != 0)
            {
                throw new ArgumentException($"{nameof(key)} or {nameof(cslen)} invalid");
            }

            byte[] s_box = new byte[cslen];
            byte[] k_box = new byte[256];
            int nonce = 0;
            byte swap = 0;
            // step 1. random cipher stream init state
            for (int i = 0; i < cslen; i++)
            {
                if (i < 256)
                {
                    s_box[i] = (byte)(i);
                }
                else
                {
                    s_box[i] = (byte)((byte)(i % 256) ^ key[i]);
                }
                k_box[i] = key[i % key.Length];
            }

            for (int i = 0; i < cslen; i++)
            {
                // step 2. iterator nonce 
                nonce = (nonce + s_box[i] + k_box[i % 256]) % cslen;
                swap = s_box[i];
                s_box[i] = s_box[nonce];
                s_box[nonce] = swap;
            }
            _inner_s_box = s_box;
            _inner_k_box = k_box;
            return s_box;
        }

        public byte[] Decrypt(MemoryStream input, MemoryStream output)
        {
            byte[] data = input.ToArray();
            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= _inner_s_box[i % _inner_s_box.Length];
            }
            int kindex = 0;
            for (int i = 0; i < data.Length; i += 3)
            {
                kindex += 2;
                try
                {
                    if (i + 1 == data.Length)
                    {
                        output.WriteByte(data[i]);
                        break;
                    }

                    if ((data[i] != 0 && data[i + 1] != 0))
                    {
                        if (data[i + 2] == 0)
                        {
                            output.WriteByte(data[i]);
                            output.WriteByte(data[i + 1]);
                            continue;
                        }

                        byte[] resulthex = new byte[2];
                        resulthex[0] = data[i];
                        resulthex[1] = data[i + 1];

                        ushort result = BitConverter.ToUInt16(resulthex);
                        // reverse multiplication
                        output.WriteByte((byte)(result / data[i + 2]));
                        output.WriteByte(data[i + 2]);
                    }
                    else
                    {
                        output.WriteByte(data[i]);
                        output.WriteByte(data[i + 1]);
                    }

                }
                catch (Exception)
                {
                    output.WriteByte(data[i]);
                }
            }

            byte[] data2 = output.ToArray();
            return data2;
        }

        public byte[] Decrypt(byte[] input, MemoryStream output)
        {
            using (MemoryStream ms = new MemoryStream(input))
            {
                return Decrypt(ms, output);
            }
        }

        public byte[] Encrypt(MemoryStream input, MemoryStream output)
        {
            byte[] data = input.ToArray();
            for (int i = 0; i < data.Length; i += 2)
            {
                try
                {
                    if (i + 1 == data.Length)
                    {
                        output.WriteByte(data[i]);
                        break;
                    }
                    if (data[i] != 0 && data[i + 1] != 0)
                    {
                        // multiplication
                        ushort result = (ushort)(data[i] * data[i + 1]);
                        byte[] resulthex = new byte[2];
                        resulthex = BitConverter.GetBytes(result);
                        if (resulthex[0] == 0 || resulthex[1] == 0)
                        {
                            output.WriteByte(data[i]);
                            output.WriteByte(data[i + 1]);
                            output.WriteByte(0x00);
                        }
                        else
                        {
                            output.Write(resulthex, 0, 2);
                            output.WriteByte(data[i + 1]);
                        }
                    }
                    else
                    {
                        output.WriteByte(data[i]);
                        output.WriteByte(data[i + 1]);
                        output.WriteByte(0x00);
                    }
                }
                catch (Exception e)
                {
                    output.WriteByte(data[i]);
                    break;
                }

            }

            byte[] data2 = output.ToArray();
            for (int i = 0; i < data2.Length; i++)
            {
                data2[i] ^= _inner_s_box[i % _inner_s_box.Length];
            }
            return data2;
        }

        public byte[] Encrypt(byte[] input, MemoryStream output)
        {
            using (MemoryStream ms = new MemoryStream(input))
            {
                return Encrypt(ms, output);
            }
        }

        public void Dispose()
        {
            // todo
        }
    }
}