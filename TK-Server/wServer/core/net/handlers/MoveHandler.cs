using common;
using System;
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

            var moveRecords = new MoveRecord[rdr.ReadInt16() + 1];
            for (var i = 0; i < moveRecords.Length - 1; i++)
                moveRecords[i] = new MoveRecord(rdr.ReadInt32(), rdr.ReadSingle(), rdr.ReadSingle());
            moveRecords[moveRecords.Length - 1] = new MoveRecord(time, newX, newY);

            // inv swap
            var invSwapLen = rdr.ReadInt16();
            for (var i = 0; i < invSwapLen; i++)
                client.Player.ClientState.OnInvSwap(rdr);

            // player shoot
            var playerShootLen = rdr.ReadInt16();
            for (var i = 0; i < playerShootLen; i++)
                client.Player.ClientState.OnPlayerShoot(rdr, moveRecords);

            // aoe acks
            var aoeLen = rdr.ReadInt16();
            for (var i = 0; i < aoeLen; i++)
                client.Player.ClientState.OnAoeAck(tickId, newX, newY);
            
            // player shoots
            client.Player.ClientState.OnMove(tickId, time, newX, newY, moveRecords);
        }
    }
}
