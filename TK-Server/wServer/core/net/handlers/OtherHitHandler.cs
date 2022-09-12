using common;
using wServer.core;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class OtherHitHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.OTHERHIT;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
           var Time = rdr.ReadInt32();
           var BulletId = rdr.ReadInt32();
           var ObjectId = rdr.ReadInt32();
           var TargetId = rdr.ReadInt32();


        }
    }
}
