using System;
using wServer.core;
using wServer.core.objects;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class MoveHandler : PacketHandlerBase<Move>
    {
        public override PacketId ID => PacketId.MOVE;

        protected override void HandlePacket(Client client, Move packet)
        {
            var player = client.Player;

            if (player == null || player.Owner == null)
                return;

            player.AddPendingAction(t => Handle(client.Player, t, packet));
        }

        private void Handle(Player player, TickData time, Move packet)
        {
            if (player?.Owner == null)
                return;

            player.MoveReceived(time, packet);

            var newX = packet.NewPosition.X;
            var newY = packet.NewPosition.Y;

            if (newX != -1 && newX != player.X || newY != -1 && newY != player.Y)
            {
                if (!player.Owner.Map.Contains(newX, newY))
                {
                    player.Client.Disconnect("Out of map bounds");
                    return;
                }

                //var distance = player.Dist(packet.NewPosition);
                //if (distance > 15 && player._tps <= 0)
                //{
                //    //player.CoreServerManager.Database.Ban(player.AccountId, $"TQ: Distance traveled: {distance} | Last pos: {player.X}, {player.Y} | New pos: {newX}, {newY} | Speed stat: {player.Stats[4]}");
                //    player.Client.Disconnect("TP Hack");
                //    return;
                //}
                //if (player._tps > 0)
                //    player._tps -= 1;
                //if (player._tps < 0)
                //    player._tps = 0;

                player.Move(newX, newY);
                player.PlayerUpdate.UpdateTiles = true;
            }
        }
    }
}
