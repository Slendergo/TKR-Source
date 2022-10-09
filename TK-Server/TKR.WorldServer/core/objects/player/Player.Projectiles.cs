using NLog.LayoutRenderers.Wrappers;
using Pipelines.Sockets.Unofficial.Arenas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Xml;
using TKR.Shared.resources;
using TKR.WorldServer.core._core.entities;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.stats;
using TKR.WorldServer.core.miscfile.structures;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.logic;
using TKR.WorldServer.networking.packets.outgoing;

namespace TKR.WorldServer.core.objects
{
    public partial class Player
    {
        private struct ShootAcknowledgement
        {
            public readonly EnemyShoot EnemyShoot;
            public readonly ServerPlayerShoot ServerPlayerShoot;

            public ShootAcknowledgement(EnemyShoot enemyShoot)
            {
                EnemyShoot = enemyShoot;
                ServerPlayerShoot = null;
            }

            public ShootAcknowledgement(ServerPlayerShoot serverPlayerShoot)
            {
                EnemyShoot = null;
                ServerPlayerShoot = serverPlayerShoot;
            }
        }

        struct ValidatedProjectile
        {
            public int BulletType;
            public int StartTime;
            public float StartX;
            public float StartY;
            public float Angle;
            public int ObjectType;
            public int Damage;
            public bool Spawned;

            public ValidatedProjectile(int time, bool spawned, int projectileType, float x, float y, float angle, int objectType, int damage)
            {
                Spawned = spawned;
                BulletType = projectileType;
                StartTime = time;
                StartX = x;
                StartY = y;
                Angle = angle;
                ObjectType = objectType;
                Damage = damage;
            }

            public Position GetPosition(int elapsed, int bulletId, ProjectileDesc desc)
            {
                double periodFactor;
                double amplitudeFactor;
                double theta;

                var pX = (double)StartX;
                var pY = (double)StartY;
                var dist = elapsed * desc.Speed / 10000.0;
                var phase = bulletId % 2 == 0 ? 0 : Math.PI;

                if (desc.Wavy)
                {
                    periodFactor = 6 * Math.PI;
                    amplitudeFactor = Math.PI / 64;
                    theta = Angle + amplitudeFactor * Math.Sin(phase + periodFactor * elapsed / 1000);
                    pX += dist * Math.Cos(theta);
                    pY += dist * Math.Sin(theta);
                }
                else if (desc.Parametric)
                {
                    var t = elapsed / desc.LifetimeMS * 2 * Math.PI;
                    var x = Math.Sin(t) * (bulletId % 2 == 0 ? 1 : -1);
                    var y = Math.Sin(2 * t) * (bulletId % 4 < 2 ? 1 : -1);
                    var sin = Math.Sin(Angle);
                    var cos = Math.Cos(Angle);
                    pX += (x * cos - y * sin) * desc.Magnitude;
                    pY += (x * sin + y * cos) * desc.Magnitude;
                }
                else
                {
                    if (desc.Boomerang)
                    {
                        var halfway = desc.LifetimeMS * (desc.Speed / 10000.0) / 2.0;
                        if (dist > halfway)
                            dist = halfway - (dist - halfway);
                    }
                    pX += dist * Math.Cos(Angle);
                    pY += dist * Math.Sin(Angle);

                    if (desc.Amplitude != 0.0)
                    {
                        var deflection = desc.Amplitude * Math.Sin(phase + elapsed / desc.LifetimeMS * desc.Frequency * 2.0 * Math.PI);
                        pX += deflection * Math.Cos(Angle + Math.PI / 2.0);
                        pY += deflection * Math.Sin(Angle + Math.PI / 2.0);
                    }
                }
                return new Position((float)pX, (float)pY);
            }

        }

        private Queue<ShootAcknowledgement> PendingShootAcknowlegements = new Queue<ShootAcknowledgement>();
        private Dictionary<int, ValidatedProjectile> VisiblePlayerShoot = new Dictionary<int, ValidatedProjectile>(0xFF);
        private Dictionary<int, Dictionary<int, ValidatedProjectile>> VisibleEnemyShoot = new Dictionary<int, Dictionary<int, ValidatedProjectile>>();

        public void EnemyShoot(EnemyShoot enemyShoot) => PendingShootAcknowlegements.Enqueue(new ShootAcknowledgement(enemyShoot));
        public void ServerPlayerShoot(ServerPlayerShoot serverPlayerShoot) => PendingShootAcknowlegements.Enqueue(new ShootAcknowledgement(serverPlayerShoot));

        public void PlayerShoot(int time, int newBulletId, Position startingPosition, float angle, int slot)
        {
            var item = Inventory[slot];
            var projectileDesc = item.Projectiles[0];

            var damage = Stats.GetAttackDamage(projectileDesc.MinDamage, projectileDesc.MaxDamage, false);

            VisiblePlayerShoot[newBulletId] = new ValidatedProjectile(time, false, 0, startingPosition.X, startingPosition.Y, angle, item.ObjectType, damage);

            var allyShoot = new AllyShoot(newBulletId, Id, item.ObjectType, angle);
            World.BroadcastIfVisibleExclude(allyShoot, this, this);
            FameCounter.Shoot();
        }

        public void ShootAck(int time)
        {
            var topLevelShootAck = PendingShootAcknowlegements.Dequeue();
            if(time == -1)
            {
                // check entity doesnt exist in our visible list
                // if it doesnt its valid
                // if it does its not valid
                return;
            }

            // validate projectiles here
            var enemyShoot = topLevelShootAck.EnemyShoot;

            if (enemyShoot != null)
            {
                if (!VisibleEnemyShoot.ContainsKey(enemyShoot.OwnerId))
                    VisibleEnemyShoot.Add(enemyShoot.OwnerId, new Dictionary<int, ValidatedProjectile>(0xFF));

                for (var i = 0; i < enemyShoot.NumShots; i++)
                {
                    var angle = enemyShoot.Angle + enemyShoot.AngleInc * i;
                    var bulletId = enemyShoot.BulletId + i;
                    VisibleEnemyShoot[enemyShoot.OwnerId][bulletId] = new ValidatedProjectile(time, enemyShoot.Spawned, enemyShoot.BulletType, enemyShoot.StartingPos.X, enemyShoot.StartingPos.Y, angle, enemyShoot.ObjectType, enemyShoot.Damage);
                }
                return;
            }

            var serverPlayerShoot = topLevelShootAck.ServerPlayerShoot;
            VisiblePlayerShoot[serverPlayerShoot.BulletId] = new ValidatedProjectile(time, false, serverPlayerShoot.BulletType, serverPlayerShoot.StartingPos.X, serverPlayerShoot.StartingPos.Y, serverPlayerShoot.Angle, serverPlayerShoot.ObjectType, serverPlayerShoot.Damage);
        }

        public void PlayerHit(int bulletId, int objectId)
        {
            var found = VisibleEnemyShoot.TryGetValue(objectId, out var dict);
            if (!found)
            {
                Console.WriteLine($"[PlayerHit -> VisibleEnemyShoot.TryGetValue] {Id} -> {Name} | {objectId} | {bulletId} | Unable to find valid entry");
                return;
            }

            found = dict.TryGetValue(bulletId, out var projectile);
            if (!found)
            {
                Console.WriteLine($"[PlayerHit -> dict.TryGetValue] {Id} -> {Name} | {objectId} | {bulletId} | Unable to find valid projectile from VisibleEnemyShoot");
                return;
            }

            var objectDesc = GameServer.Resources.GameData.ObjectDescs[(ushort)projectile.ObjectType];
            var projectileDesc = objectDesc.Projectiles[projectile.BulletType];

            // todo validate hit position

            for (var i = 0; i < 4; i++)
            {
                var item = Inventory[i];
                if (item == null || !item.Legendary && !item.Mythical)
                    continue;

                if (item.MonkeyKingsWrath)
                    MonkeyKingsWrath(i);
                if (item.GodTouch)
                    GodTouch(i);
                if (item.GodBless)
                    GodBless(i);
                if (item.Clarification)
                    Clarification(i);
            }

            var dmg = StatsManager.DamageWithDefense(this, projectile.Damage, projectileDesc.ArmorPiercing, Stats[3]);
            HP -= dmg;

            ApplyConditionEffect(projectileDesc.Effects);
            World.BroadcastIfVisibleExclude(new Damage()
            {
                TargetId = Id,
                Effects = 0,
                DamageAmount = dmg,
                Kill = HP <= 0,
                BulletId = bulletId,
                ObjectId = Id
            }, this, this);

            if (HP <= 0)
                Death(objectDesc.DisplayId ?? objectDesc.ObjectId, projectile.Spawned);

            // remove 
            if (!projectileDesc.MultiHit)
                _ = dict.Remove(bulletId);
        }

        public void EnemyHit(ref TickTime tickTime, int time, int bulletId, int targetId, bool killed)
        {
            var found = VisiblePlayerShoot.TryGetValue(bulletId, out var projectile);
            if (!found)
            {
                Console.WriteLine($"[EnemyHit -> VisibleEnemyShoot.TryGetValue] {Id} -> {Name} | {bulletId} | Unable to find projectile");
                Client.Disconnect($"[EnemyHit -> VisibleEnemyShoot.TryGetValue] {Id} -> {Name} | {bulletId} | Unable to find projectile");
                return;
            }

            var objectDesc = GameServer.Resources.GameData.Items[(ushort)projectile.ObjectType];
            var projectileDesc = objectDesc.Projectiles[projectile.BulletType];

            // remove projectile if not multihit
            if (!projectileDesc.MultiHit)
                _ = VisiblePlayerShoot.Remove(bulletId);

            var elapsedSinceStart = time - projectile.StartTime;
            if(elapsedSinceStart > projectileDesc.LifetimeMS)
            {
                Console.WriteLine("[EnemyHit] -> A expired shot tried to hit entity");
                Client.Disconnect($"[EnemyHit] -> A expired shot tried to hit entity");
                return;
            }

            var e = World.GetEntity(targetId);
            if(e == null)
            {
                //Console.WriteLine("[EnemyHit] -> NULL ENTITY ALREADY DEAD?");
                return;
            }

            if (e is Enemy)
            {
                var entity = e as Enemy;
                if (!entity.Static)
                {
                    var player = this;

                    var dmg = StatsManager.DamageWithDefense(entity, projectile.Damage, projectileDesc.ArmorPiercing, entity.Defense);
                    entity.HP -= dmg;

                    for (var i = 0; i < 4; i++)
                    {
                        var item = player.Inventory[i];
                        if (item == null || !item.Legendary && !item.Mythical)
                            continue;

                        if (item.Demonized)
                            entity.Demonized(player, i);

                        if (item.Vampiric)
                            entity.VampireBlast(player, i, ref tickTime, entity, projectileDesc.MultiHit);

                        if (item.Electrify)
                            entity.Electrify(player, i, ref tickTime, entity);
                    }

                    entity.ApplyConditionEffect(projectileDesc.Effects);

                    World.BroadcastIfVisibleExclude(new Damage()
                    {
                        TargetId = entity.Id,
                        Effects = 0,
                        DamageAmount = dmg,
                        Kill = entity.HP < 0,
                        BulletId = bulletId,
                        ObjectId = Id
                    }, entity, this);

                    entity.DamageCounter.HitBy(this, dmg);

                    if (entity.HP < 0)
                        entity.Death(ref tickTime);
                }
            }

            if(e is StaticObject)
            {
                var s = e as StaticObject;
                if (s.Vulnerable)
                {
                    var dmg = StatsManager.DamageWithDefense(s, projectile.Damage, projectileDesc.ArmorPiercing, s.ObjectDesc.Defense);
                    s.HP -= dmg;

                    World.BroadcastIfVisibleExclude(new Damage()
                    {
                        TargetId = Id,
                        Effects = 0,
                        DamageAmount = dmg,
                        Kill = !s.CheckHP(),
                        BulletId = bulletId,
                        ObjectId = Id
                    }, s, this);
                }
            }
        }

        public void SquareHit(ref TickTime tickTime, int time, int bulletId, int objectId)
        {
            var found = VisibleEnemyShoot.TryGetValue(objectId, out var dict);
            if (!found)
            {
                Console.WriteLine($"[SquareHit -> VisibleEnemyShoot.TryGetValue] {Id} -> {Name} | {objectId} | {bulletId} | Unable to find valid entry");
                Client.Disconnect($"[SquareHit -> VisibleEnemyShoot.TryGetValue] {Id} -> {Name} | {objectId} | {bulletId} | Unable to find valid entry");
                return;
            }

            found = dict.TryGetValue(bulletId, out var projectile);
            if (!found)
            {
                Console.WriteLine($"[SquareHit -> dict.TryGetValue] {Id} -> {Name} | {objectId} | {bulletId} | Unable to find valid projectile from VisibleEnemyShoot");
                Client.Disconnect($"[SquareHit -> dict.TryGetValue] {Id} -> {Name} | {objectId} | {bulletId} | Unable to find valid projectile from VisibleEnemyShoot");
                return;
            }

            var objectDesc = GameServer.Resources.GameData.ObjectDescs[(ushort)projectile.ObjectType];
            var projectileDesc = objectDesc.Projectiles[projectile.BulletType];

            var elapsedSinceStart = time - projectile.StartTime;
            if (elapsedSinceStart > projectileDesc.LifetimeMS)
            {
                Console.WriteLine("[SquareHit] -> A expired shot tried to hit entity");
                Client.Disconnect("[SquareHit] -> A expired shot tried to hit entity");
                return;
            }

            if(!projectileDesc.MultiHit)
                _ = dict.Remove(bulletId);
        }

        public void OtherHit(ref TickTime tickTime, int time, int bulletId, int objectId, int targetId)
        {
            var found = VisibleEnemyShoot.TryGetValue(objectId, out var dict);
            if (!found)
            {
                Console.WriteLine($"[OtherHit -> VisibleEnemyShoot.TryGetValue] {Id} -> {Name} | {objectId} | {bulletId} | Unable to find valid entry");
                Client.Disconnect($"[OtherHit -> VisibleEnemyShoot.TryGetValue] {Id} -> {Name} | {objectId} | {bulletId} | Unable to find valid entry");
                return;
            }

            found = dict.TryGetValue(bulletId, out var projectile);
            if (!found)
            {
                Console.WriteLine($"[OtherHit -> dict.TryGetValue] {Id} -> {Name} | {objectId} | {bulletId} | Unable to find valid projectile from VisibleEnemyShoot");
                Client.Disconnect($"[OtherHit -> dict.TryGetValue] {Id} -> {Name} | {objectId} | {bulletId} | Unable to find valid projectile from VisibleEnemyShoot");
                return;
            }

            var objectDesc = GameServer.Resources.GameData.ObjectDescs[(ushort)projectile.ObjectType];
            var projectileDesc = objectDesc.Projectiles[projectile.BulletType];

            var elapsedSinceStart = time - projectile.StartTime;
            if (elapsedSinceStart > projectileDesc.LifetimeMS)
            {
                Console.WriteLine("[OtherHit] -> A expired shot tried to hit entity");
                Client.Disconnect("[OtherHit] -> A expired shot tried to hit entity");
                return;
            }

            // validate shot position is valid?
            // cross entity validation for wether this was a valid hit?

            if(!projectileDesc.MultiHit)
                _ = dict.Remove(bulletId);
        }

        public void HandleProjectileDetection(int time, float x, float y, ref TimedPosition[] moveRecords)
        {
            var visiblePlayerShootToRemove = new List<int>();
            foreach (var kvp in VisiblePlayerShoot)
            {
                var objectDesc = GameServer.Resources.GameData.Items[(ushort)kvp.Value.ObjectType];
                var projectileDesc = objectDesc.Projectiles[0];

                var elapsed = time - kvp.Value.StartTime;
                if (elapsed > projectileDesc.LifetimeMS)
                    visiblePlayerShootToRemove.Add(kvp.Key);
            }
            foreach (var bulletId in visiblePlayerShootToRemove)
                _ = VisiblePlayerShoot.Remove(bulletId);

            var visibleEnemyShootToRemove = new List<ValueTuple<int, int>>();
            foreach (var dict in VisibleEnemyShoot)
                foreach (var kvp in dict.Value)
                {
                    var objectDesc = GameServer.Resources.GameData.ObjectDescs[(ushort)kvp.Value.ObjectType];
                    try
                    {
                        var projectileDesc = objectDesc.Projectiles[kvp.Value.BulletType];

                        var elapsed = time - kvp.Value.StartTime;
                        if (elapsed > projectileDesc.LifetimeMS)
                            visibleEnemyShootToRemove.Add(ValueTuple.Create(dict.Key, kvp.Key));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"VisibleEnemyShoot: {objectDesc.DisplayId ?? objectDesc.ObjectId} -> {kvp.Value.BulletType}");
                    }
                }
            foreach (var kvp in visibleEnemyShootToRemove)
                _ = VisibleEnemyShoot[kvp.Item1].Remove(kvp.Item2);
        }
    }
}
