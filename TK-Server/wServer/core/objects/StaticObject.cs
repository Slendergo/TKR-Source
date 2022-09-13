using common;
using System.Collections.Generic;
using System.Xml.Linq;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.core.objects
{
    public class StaticObject : Entity
    {
        private SV<int> _hp;

        public StaticObject(GameServer manager, ushort objType, int? life, bool stat, bool dying, bool hittestable) : base(manager, objType)
        {
            _hp = new SV<int>(this, StatDataType.HP, 0, dying);

            if (Vulnerable = life.HasValue)
                HP = life.Value;

            Dying = dying;
            Static = stat;
            Hittestable = hittestable;
        }

        public bool Dying { get; private set; }
        public bool Hittestable { get; private set; }
        public int HP { get => _hp.GetValue(); set => _hp.SetValue(value); }
        public bool Static { get; private set; }
        public bool Vulnerable { get; private set; }

        public override bool HitByProjectile(Projectile projectile, TickTime time)
        {
            if (Vulnerable && projectile.Host is Player)
            {
                var dmg = StatsManager.DamageWithDefense(this, projectile.Damage, projectile.ProjDesc.ArmorPiercing, ObjectDesc.Defense);

                HP -= dmg;

                World.BroadcastIfVisibleExclude(new Damage()
                {
                    TargetId = Id,
                    Effects = 0,
                    DamageAmount = dmg,
                    Kill = !CheckHP(),
                    BulletId = projectile.ProjectileId,
                    ObjectId = projectile.Host.Id
                }, this, projectile.Host as Player);
            }
            return true;
        }

        public override void Tick(ref TickTime time)
        {
            if (Vulnerable)
            {
                if (Dying)
                    HP -= time.ElapsedMsDelta;
                CheckHP();
            }

            base.Tick(ref time);
        }

        protected bool CheckHP()
        {
            if (HP <= 0)
            {
                var x = (int)(X - 0.5);
                var y = (int)(Y - 0.5);

                if (World.Map.Contains(x, y))
                    if (ObjectDesc != null && World.Map[x, y].ObjType == ObjectType)
                    {
                        var tile = World.Map[x, y];
                        tile.ObjType = 0;
                        tile.UpdateCount++;

                        foreach (var player in World.PlayersCollision.HitTest(x, y, PlayerUpdate.VISIBILITY_RADIUS))
                            if(player is Player && (player as Player).PlayerUpdate != null)
                                (player as Player).PlayerUpdate.UpdateTiles = true;
                    }
                World.LeaveWorld(this);

                return false;
            }

            return true;
        }

        protected override void ExportStats(IDictionary<StatDataType, object> stats, bool isOtherPlayer)
        {
            stats[StatDataType.HP] = (!Vulnerable) ? int.MaxValue : HP;
            base.ExportStats(stats, isOtherPlayer);
        }
    }
}
