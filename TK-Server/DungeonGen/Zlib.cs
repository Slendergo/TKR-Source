using System;
using System.IO;
using System.IO.Compression;

namespace dungeonGen
{
    public static class Zlib
    {
        public static byte[] Compress(byte[] buffer)
        {
            byte[] comp;

            using (var output = new MemoryStream())
            {
                using (var deflate = new DeflateStream(output, CompressionMode.Compress))
                    deflate.Write(buffer, 0, buffer.Length);

                comp = output.ToArray();
            }

            const byte CM = 8;
            const byte CINFO = 7;
            const byte CMF = CM | (CINFO << 4);
            const byte FLG = 0xDA;

            var result = new byte[comp.Length + 6];
            result[0] = CMF;
            result[1] = FLG;

            Buffer.BlockCopy(comp, 0, result, 2, comp.Length);

            var cksum = ADLER32(buffer);
            var index = result.Length - 4;

            result[index++] = (byte)(cksum >> 24);
            result[index++] = (byte)(cksum >> 16);
            result[index++] = (byte)(cksum >> 8);
            result[index++] = (byte)(cksum >> 0);

            return result;
        }

        private static uint ADLER32(byte[] data)
        {
            const uint MODULO = 0xfff1;

            uint A = 1, B = 0;

            for (var i = 0; i < data.Length; i++)
            {
                A = (A + data[i]) % MODULO;
                B = (B + A) % MODULO;
            }

            return (B << 16) | A;
        }
    }
}
