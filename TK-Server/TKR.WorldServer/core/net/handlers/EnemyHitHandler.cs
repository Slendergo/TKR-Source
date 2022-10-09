using TKR.Shared;
using TKR.Shared.resources;
using System;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.networking;

namespace TKR.WorldServer.core.net.handlers
{
    public class EnemyHitHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.ENEMYHIT;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();
            var bulletId = rdr.ReadInt32();
            var targetId = rdr.ReadInt32();
            var killed = rdr.ReadBoolean();

            var player = client.Player;

            player.EnemyHit(ref tickTime, time, bulletId, targetId, killed);
        }
    }
}
