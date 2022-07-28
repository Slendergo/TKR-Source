using System.Collections.Concurrent;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers
{
    internal class BountyMemberListRequestHandler : PacketHandlerBase<BountyMemberListRequest>
    {
        public override PacketId ID => PacketId.BOUNTYMEMBERLISTREQUEST;

        protected override void HandlePacket(Client client, BountyMemberListRequest packet)
        {
            if (client == null || client.Player == null || client.Player.Owner == null)
                return;

            Handle(client, packet);
        }

        private void Handle(Client client, BountyMemberListRequest packet)
        {
            var player = client.Player;
            if (player.GuildRank < 30)
            {
                player.SendError("You aren't a Leader/Founder!");
                return;
            }

            var playersIds = new ConcurrentBag<int>();
            client.Player.Owner.PlayersBroadcastAsParallel(_ =>
            {
                if (_.Guild != player.Guild)
                    return;

                playersIds.Add(_.Id);
            });
            client.SendPacket(
                new BountyMemberListSend() { AccountIds = playersIds.ToArray() },
                PacketPriority.Normal
            );
        }
    }
}
