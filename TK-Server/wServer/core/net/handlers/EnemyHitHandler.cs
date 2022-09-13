using common;
using common.resources;
using System;
using wServer.core.objects;
using wServer.networking;
using wServer.utils;

namespace wServer.core.net.handlers
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
            
            if (player.HasConditionEffect(ConditionEffectIndex.Hidden))
                return;
            
            var entity = player?.World?.GetEntity(targetId);
            if(entity == null)
                return;

            var projectile = player.GetProjectile(player.Id, bulletId);
            if (projectile == null)
            {
                //Console.WriteLine($"NULL PROJ | {player.Id} {bulletId}");
                return;
            }

            var delta = time - projectile.StartTime;

            var hitSpot = projectile.GetPosition(delta);

            var distanceFromHit = entity.DistTo(hitSpot.X, hitSpot.Y);
            if (distanceFromHit > 7.5)
            {
                foreach (var other in player.World.Players.Values)
                    if (other.IsAdmin || other.IsCommunityManager)
                        other.SendInfo($"Warning: [{player.Name}] {player.AccountId}-{player.Client.Character.CharId} potentially AOE hacks! ({distanceFromHit} | tolerance: 7.5)");
                StaticLogger.Instance.Warn($"[{player.Name}] {player.AccountId}-{player.Client.Character.CharId} potentially AOE hacks! ({distanceFromHit} | tolerance: 7.5)");
                return;
            }

            projectile.HitEntity(player, entity, tickTime);

            var totalLife = 0.0;
            var totalMana = 0.0;
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
                    if (player.World.Random.NextDouble() <= leech.Probability)
                    {
                        var scale = leech.ScalesPerLevel ? talisman.Level * leech.Percentage : leech.Percentage;
                        switch (leech.Type)
                        {
                            case 0:
                                totalLife += player.Stats[0] * scale;
                                break;
                            case 1:
                                totalMana += player.Stats[1] * scale;
                                break;
                        }
                    }
                }

                if (totalLife > 0)
                    Player.HealDiscrete(player, (int)totalLife, false);
                if (totalMana > 0)
                    Player.HealDiscrete(player, (int)totalMana, true);
            }
        }
    }
}
