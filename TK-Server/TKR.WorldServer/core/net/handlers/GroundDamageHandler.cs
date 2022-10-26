using TKR.Shared;
using TKR.WorldServer.core.structures;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.networking;

namespace TKR.WorldServer.core.net.handlers
{
    public class GroundDamageHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.GROUNDDAMAGE;

        public override void Handle(Client client, NetworkReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();
            var position = Position.Read(rdr);

            var player = client.Player;
            if (player?.World == null)
                return;

            player.ForceGroundHit(tickTime, position, time);
        }
    }
}
