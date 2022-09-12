using common;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public sealed class ShoootAckMessageHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.SHOOTACK;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();
        }
    }
}
