using common;
using wServer.core;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class MoveHandler : IMessageHandler
    {
        public override PacketId MessageId => PacketId.MOVE;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var TickId = rdr.ReadInt32();
            var Time = rdr.ReadInt32();
            var NewPosition = Position.Read(rdr);
            var Records = new TimedPosition[rdr.ReadInt16()];
            for (var i = 0; i < Records.Length; i++)
                Records[i] = TimedPosition.Read(rdr);

            var player = client.Player;

            if (player == null || player.World == null)
                return;

            player.MoveReceived(tickTime, Time, TickId);

            var newX = NewPosition.X;
            var newY = NewPosition.Y;

            if (newX != -1 && newX != player.X || newY != -1 && newY != player.Y)
            {
                if (!player.World.Map.Contains(newX, newY))
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
