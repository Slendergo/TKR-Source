using common;
using wServer.core;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class OtherHitHandler : IMessageHandler
    {
        public override PacketId MessageId => PacketId.OTHERHIT;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
           var Time = rdr.ReadInt32();
           var BulletId = rdr.ReadByte();
           var ObjectId = rdr.ReadInt32();
           var TargetId = rdr.ReadInt32();


        }
    }
}
