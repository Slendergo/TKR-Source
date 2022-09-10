using wServer.core;
using wServer.core.objects;
using wServer.networking.packets.outgoing;

namespace wServer.logic.behaviors
{
    public class EnemyAOE : Behavior
    {
        private readonly ARGB color;
        private readonly int maxDamage;
        private readonly int minDamage;
        private readonly bool noDef;
        private readonly bool players;
        private readonly float radius;

        public EnemyAOE(double radius, bool players, int minDamage, int maxDamage, bool noDef, uint color)
        {
            this.radius = (float)radius;
            this.players = players;
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.noDef = noDef;
            this.color = new ARGB(color);
        }

        protected override void OnStateEntry(Entity host, TickTime time, ref object state)
        {
            var pos = new Position { X = host.X, Y = host.Y };
            var damage = Random.Next(minDamage, maxDamage);

            host.World.AOE(pos, radius, players, enemy =>
            {
                if (!players)
                    host.World.BroadcastAoeIfVisible(new AoeData() { Pos = pos, Radius = radius, Damage = (ushort)damage, Duration = 0, Effect = 0, OrigType = host.ObjectType }, host);
            });
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        { }
    }
}
