using TKR.Shared;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.networking;

namespace TKR.WorldServer.core.net.handlers
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
