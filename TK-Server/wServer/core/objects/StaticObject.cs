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

        public StaticObject(CoreServerManager manager, ushort objType, int? life, bool stat, bool dying, bool hittestable) : base(manager, objType)
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

        public static int? GetHP(XElement elem)
        {
            var n = elem.Element("MaxHitPoints");

            if (n != null)
                return Utils.GetInt(n.Value);
            else
                return null;
        }

        public override bool HitByProjectile(Projectile projectile, TickData time)
        {
            if (Vulnerable && projectile.ProjectileOwner is Player)
            {
                var def = ObjectDesc.Defense;

                if (projectile.ProjDesc.ArmorPiercing)
                    def = 0;

                var dmg = (int)StatsManager.GetDefenseDamage(this, projectile.Damage, def);

                HP -= dmg;

                Owner.BroadcastIfVisibleExclude(new Damage()
                {
                    TargetId = Id,
                    Effects = 0,
                    DamageAmount = (ushort)dmg,
                    Kill = !CheckHP(),
                    BulletId = projectile.ProjectileId,
                    ObjectId = projectile.ProjectileOwner.Self.Id
                }, this, projectile.ProjectileOwner as Player, PacketPriority.Low);
            }

            return true;
        }

        public override void Tick(TickData time)
        {
            if (Vulnerable)
            {
                if (Dying)
                    HP -= time.ElaspedMsDelta;

                CheckHP();
            }

            base.Tick(time);
        }

        protected bool CheckHP()
        {
            if (HP <= 0)
            {
                var x = (int)(X - 0.5);
                var y = (int)(Y - 0.5);

                if (Owner?.Map?.Contains(x, y) ?? false)
                    if (ObjectDesc != null && Owner.Map[x, y].ObjType == ObjectType)
                    {
                        var tile = Owner.Map[x, y];
                        tile.ObjType = 0;
                        tile.UpdateCount++;
                    }

                Owner?.LeaveWorld(this);

                return false;
            }

            return true;
        }

        protected override void ExportStats(IDictionary<StatDataType, object> stats)
        {
            stats[StatDataType.HP] = (!Vulnerable) ? int.MaxValue : HP;

            base.ExportStats(stats);
        }
    }
}
