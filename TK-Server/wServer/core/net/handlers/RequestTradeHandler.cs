using common;
using wServer.core;
using wServer.core.worlds.logic;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class RequestTradeHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.REQUESTTRADE;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var name = rdr.ReadUTF();

            if (client.Player == null || client?.Player?.World is TestWorld) 
                return;

            client.Player.RequestTrade(name);
        }
    }
}
