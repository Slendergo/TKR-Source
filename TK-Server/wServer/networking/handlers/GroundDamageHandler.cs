using wServer.core;
using wServer.core.objects;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class GroundDamageHandler : PacketHandlerBase<GroundDamage>
    {
        public override PacketId ID => PacketId.GROUNDDAMAGE;

        protected override void HandlePacket(Client client, GroundDamage packet) => Handle(client.Player, new TickData(), packet.Position, packet.Time);

        private void Handle(Player player, TickData time, Position pos, int timeHit)
        {
            if (player?.Owner == null)
                return;

            player.ForceGroundHit(time, pos, timeHit);
        }
    }
}
