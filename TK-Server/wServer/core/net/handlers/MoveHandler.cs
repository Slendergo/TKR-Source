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

                player.Move(newX, newY);
                player.PlayerUpdate.UpdateTiles = true;
            }
        }
    }
}
