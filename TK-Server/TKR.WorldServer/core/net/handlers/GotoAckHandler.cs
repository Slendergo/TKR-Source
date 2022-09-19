using TKR.Shared;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.networking;

namespace TKR.WorldServer.core.net.handlers
{
    public class GotoAckHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.GOTOACK;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();
            client.Player.GotoAckReceived();
        }
    }
}
