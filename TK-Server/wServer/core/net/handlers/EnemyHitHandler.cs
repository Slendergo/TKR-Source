using common;
using common.resources;
using System;
using wServer.core.objects;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class EnemyHitHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.ENEMYHIT;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();
            var bulletId = rdr.ReadByte();
            var targetId = rdr.ReadInt32();
            var killed = rdr.ReadBoolean();
            var itemType = rdr.ReadUInt16();


            var player = client.Player;
            var entity = player?.World?.GetEntity(targetId);

            if (entity?.World == null || entity.HasConditionEffect(ConditionEffects.Invulnerable))
                return;

            if (player.HasConditionEffect(ConditionEffects.Hidden))
                return;

            var prj = player.World.GetProjectile(player.Id, bulletId);
            if (prj == null)
                return;

            prj?.ForceHit(entity, tickTime);

            using (var t = new TimedProfiler("CheckTalismans"))
            {
                var totalLife = 0;
                var totalMana = 0;
                foreach (var type in player.ActiveTalismans)
                {
                    var talisman = player.GetTalisman(type);
                    if (talisman == null)
                        continue;

                    var desc = player.GameServer.Resources.GameData.GetTalisman(type);
                    if (desc == null)
                        continue;

                    var tierDesc = desc.GetTierDesc(talisman.Tier);
                    if (tierDesc == null)
                        continue;

                    foreach (var leech in tierDesc.Leech)
                    {
                        if (player.World.Random.NextDouble() < leech.Probability)
                        {
                            var scale = leech.ScalesPerLevel ? talisman.Level * leech.Percentage : leech.Percentage;
                            switch (leech.Type)
                            {
                                case 0:
                                    totalLife += (int)(player.Stats[0] * scale);
                                    break;
                                case 1:
                                    totalMana += (int)(player.Stats[1] * scale);
                                    break;
                            }
                        }
                    }
                }

                if (totalLife > 0)
                    Player.HealDiscrete(player, totalLife, false);
                if (totalMana > 0)
                    Player.HealDiscrete(player, totalMana, true);
            }
        }
    }
}
