using common.resources;
using System;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class TransformOnDeath : Behavior
    {
        private int max;
        private int min;
        private readonly float probability;
        private readonly ushort target;

        public TransformOnDeath(string target, int min = 1, int max = 1, double probability = 1)
        {
            this.target = GetObjType(target);
            this.min = min;
            this.max = max;
            this.probability = (float)probability;
        }

        protected internal override void Resolve(State parent) => parent.Death += (sender, e) =>
        {
            if (e.Host.CurrentState.Is(parent) && Random.NextDouble() < probability)
            {
                if (Entity.Resolve(e.Host.CoreServerManager, target) is Portal && e.Host.Owner.Name.Contains("Arena"))
                    return;

                if (min > max)
                    max = min;//exception error

                var count = Random.Next(min, max + 1);

                for (var i = 0; i < count; i++)
                {
                    var entity = Entity.Resolve(e.Host.CoreServerManager, target);
                    entity.Move(e.Host.X, e.Host.Y);

                    if (e.Host is Enemy && entity is Enemy && (e.Host as Enemy).Spawned)
                    {
                        (entity as Enemy).Spawned = true;
                        (entity as Enemy).ApplyConditionEffect(new ConditionEffect() { Effect = ConditionEffectIndex.Invisible, DurationMS = -1 });
                    }

                    e.Host.Owner.EnterWorld(entity);
                }
            }
        };

        protected override void TickCore(Entity host, TickData time, ref object state)
        { }
    }
}
