using common;
using wServer.core;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class ShootAckHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.SHOOTACK;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();

        }
    }
}
