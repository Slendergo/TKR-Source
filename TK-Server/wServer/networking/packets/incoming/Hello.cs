using common;
using System.Text;

namespace wServer.networking.packets.incoming
{
    public class Hello : IncomingMessage
    {
        public string BuildVersion { get; set; }
        public int GameId { get; set; }
        public string GUID { get; set; }
        //public string Hash { get; set; }
        public override PacketId ID => PacketId.HELLO;
        public byte[] Key { get; set; }
        public int KeyTime { get; set; }
        public string MapJSON { get; set; }
        public string Password { get; set; }

        public override Packet CreateInstance() => new Hello();

        protected override void Read(NReader rdr)
        {
            BuildVersion = rdr.ReadUTF();
            GameId = rdr.ReadInt32();
            GUID = RSA.Instance.Decrypt(rdr.ReadUTF());
            Password = RSA.Instance.Decrypt(rdr.ReadUTF());
            KeyTime = rdr.ReadInt32();
            Key = rdr.ReadBytes(rdr.ReadInt16());
            MapJSON = rdr.Read32UTF();

            // this is a hackish solution for some scriptkids
            // that think they are "hackers"
            // understanding this on client is a bit confuse
            // but this is just a distraction

            /*try
            {
                var length = rdr.ReadInt32();
                var buffer = rdr.ReadBytes(length);
                var sb = new StringBuilder(buffer.Length * 2);

                foreach (var data in buffer)
                    sb.Append($"{data:X2}");

                Hash = sb.ToString();
            }
            catch { Hash = string.Empty; }*/
        }

        protected override void Write(NWriter wtr) { }
    }
}
