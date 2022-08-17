using common;
using wServer.core;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class UpdateAckHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.UPDATEACK;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
        }
    }
}
