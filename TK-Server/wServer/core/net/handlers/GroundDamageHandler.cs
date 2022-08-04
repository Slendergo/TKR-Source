using common;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class GroundDamageHandler : IMessageHandler
    {
        public override PacketId MessageId => PacketId.GROUNDDAMAGE;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
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
