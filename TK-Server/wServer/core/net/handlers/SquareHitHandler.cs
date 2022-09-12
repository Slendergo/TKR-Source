using common;
using wServer.core;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class SquareHitHandler : IMessageHandler
    {
        public int Time { get; set; }
        public int BulletId { get; set; }
        public int ObjectId { get; set; }

        public override MessageId MessageId => MessageId.SQUAREHIT;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            Time = rdr.ReadInt32();
            BulletId = rdr.ReadInt32();
            ObjectId = rdr.ReadInt32();
        }
    }
}
