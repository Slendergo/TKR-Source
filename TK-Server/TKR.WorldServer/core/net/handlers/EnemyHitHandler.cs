﻿using TKR.Shared;
using TKR.Shared.resources;
using System;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.networking;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.utils;

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
            if (player.World == null)
                return;

            if (player.HasConditionEffect(ConditionEffectIndex.Hidden))
                return;

            var entity = player.World.GetEntity(targetId);
            if (entity == null)
                return;


            var prj = player.World.GetProjectile(player.Id, bulletId);
            if (prj == null)
            {
                Console.WriteLine($"Null Projectile: {bulletId}");
                return;
            }

            prj?.ForceHit(entity, tickTime);

            if (entity is StaticObject)
            {
                if (killed && (entity as StaticObject).HP > 0)
                    player.PlayerUpdate.ForceAdd(entity);
            }
            else if (entity is Enemy)
                 if (killed && (entity as Enemy).HP > 0)
                    player.PlayerUpdate.ForceAdd(entity);

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