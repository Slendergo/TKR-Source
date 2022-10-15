using TKR.Shared;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.worlds.logic;
using TKR.WorldServer.networking;

namespace TKR.WorldServer.core.net.handlers
{
    public class RequestTradeHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.REQUESTTRADE;

        public override void Handle(Client client, NetworkReader rdr, ref TickTime tickTime)
        {
            var name = rdr.ReadUTF16();

            if (client.Player == null || client?.Player?.World is TestWorld)
                return;

            client.Player.RequestTrade(name);
        }
    }
}
