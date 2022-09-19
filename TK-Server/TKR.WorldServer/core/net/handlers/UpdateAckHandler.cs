using TKR.Shared;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.networking;

namespace TKR.WorldServer.core.net.handlers
{
    public class UpdateAckHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.UPDATEACK;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
        }
    }
}
