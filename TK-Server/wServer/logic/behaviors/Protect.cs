using common.resources;
using Mono.Game;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class Protect : CycleBehavior
    {
        private readonly float acquireRange;
        private readonly ushort protectee;
        private readonly float protectionRange;
        private readonly float reprotectRange;
        private readonly float speed;

        public Protect(double speed, string protectee, double acquireRange = 10, double protectionRange = 2, double reprotectRange = 1)
        {
            this.speed = (float)speed;
            this.protectee = GetObjType(protectee);
            this.acquireRange = (float)acquireRange;
            this.protectionRange = (float)protectionRange;
            this.reprotectRange = (float)reprotectRange;
        }

        private enum ProtectState
        {
            DontKnowWhere,
            Protecting,
            Protected,
        }

        protected override void TickCore(Entity host, TickData time, ref object state)
        {
            var s = state == null ? ProtectState.DontKnowWhere : (ProtectState)state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed))
                return;

            var entity = host.GetNearestEntity(acquireRange, protectee);

            Vector2 vect;

            switch (s)
            {
                case ProtectState.DontKnowWhere:
                    if (entity != null)
                    {
                        s = ProtectState.Protecting;

                        goto case ProtectState.Protecting;
                    }
                    break;

                case ProtectState.Protecting:
                    if (entity == null)
                    {
                        s = ProtectState.DontKnowWhere;

                        break;
                    }
                    vect = new Vector2(entity.X - host.X, entity.Y - host.Y);
                    if (vect.Length() > reprotectRange)
                    {
                        Status = CycleStatus.InProgress;

                        vect.Normalize();

                        var dist = host.GetSpeed(speed) * time.DeltaTime;

                        host.ValidateAndMove(host.X + vect.X * dist, host.Y + vect.Y * dist);
                    }
                    else
                    {
                        Status = CycleStatus.Completed;

                        s = ProtectState.Protected;
                    }
                    break;

                case ProtectState.Protected:
                    if (entity == null)
                    {
                        s = ProtectState.DontKnowWhere;

                        break;
                    }

                    Status = CycleStatus.Completed;

                    vect = new Vector2(entity.X - host.X, entity.Y - host.Y);

                    if (vect.Length() > protectionRange)
                    {
                        s = ProtectState.Protecting;

                        goto case ProtectState.Protecting;
                    }
                    break;
            }

            state = s;
        }
    }
}
