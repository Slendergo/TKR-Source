using common;
using System.Collections.Generic;
using wServer.core.objects.player.state;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class MoveHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.MOVE;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var tickId = rdr.ReadInt32();
            var time = rdr.ReadInt32();
            var newX = rdr.ReadSingle();
            var newY = rdr.ReadSingle();
            var moveRecords = new MoveRecord[rdr.ReadInt16()];
            for (var i = 0; i < moveRecords.Length; i++)
                moveRecords[i] = new MoveRecord(rdr.ReadInt32(), rdr.ReadSingle(), rdr.ReadSingle());
            client.Player.ClientState.OnMove(tickId, time, newX, newY, moveRecords);
        }
    }
}
