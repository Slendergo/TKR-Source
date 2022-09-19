using TKR.Shared;
using System;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class Reconnect : OutgoingMessage
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int GameId { get; set; }
        public int KeyTime { get; set; }
        public byte[] Key { get; private set; }

        public override MessageId MessageId => MessageId.RECONNECT;

        public Reconnect()
        {
            Key = Guid.NewGuid().ToByteArray();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.WriteUTF(Host);
            wtr.Write(Port);
            wtr.Write(GameId);
            wtr.Write(KeyTime);
            wtr.Write((short)Key.Length);
            wtr.Write(Key);
        }
    }
}
