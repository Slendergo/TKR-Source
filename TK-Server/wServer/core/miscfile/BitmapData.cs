using common;

namespace wServer
{
    public struct BitmapData
    {
        public byte[] Bytes { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public static BitmapData Read(NReader rdr)
        {
            var ret = new BitmapData
            {
                Width = rdr.ReadInt32(),
                Height = rdr.ReadInt32()
            };
            ret.Bytes = new byte[ret.Width * ret.Height * 4];
            ret.Bytes = rdr.ReadBytes(ret.Bytes.Length);
            return ret;
        }

        public void Write(NWriter wtr)
        {
            wtr.Write(Width);
            wtr.Write(Height);
            wtr.Write(Bytes);
        }
    }
}
