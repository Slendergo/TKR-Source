using common;
using wServer.core;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class PongHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.PONG;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var serial = rdr.ReadInt32();
            var time = rdr.ReadInt32();

            client?.Player?.Pong(tickTime, time, serial);
        }
    }
}
