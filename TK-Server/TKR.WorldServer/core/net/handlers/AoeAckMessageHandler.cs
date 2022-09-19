using TKR.Shared;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.networking;

namespace TKR.WorldServer.core.net.handlers
{
    public sealed class AoeAckMessageHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.AOEACK;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();
            var x = rdr.ReadSingle();
            var y = rdr.ReadSingle();

        }
    }
}
