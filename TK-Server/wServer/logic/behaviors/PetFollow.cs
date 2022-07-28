using Mono.Game;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class PetFollow : CycleBehavior
    {
        private enum F
        {
            DontKnowWhere,
            Acquired,
        }

        protected override void TickCore(Entity host, TickData time, ref object state)
        {
            if ((host as Pet)?.PlayerOwner == null)
                return;

            var pet = (Pet)host;
            var s = state == null ? new FollowState { State = F.DontKnowWhere, RemainingTime = 1000 } : (FollowState)state;

            Status = CycleStatus.NotStarted;

            if (!(host.Owner.GetEntity(pet.PlayerOwner.Id) is Player player))
                return;

            Status = CycleStatus.InProgress;

            switch (s.State)
            {
                case F.DontKnowWhere:
                    if (s.RemainingTime > 0)
                        s.RemainingTime -= time.ElaspedMsDelta;
                    else
                        s.State = F.Acquired;
                    break;

                case F.Acquired:
                    if (player == null)
                    {
                        s.State = F.DontKnowWhere;
                        s.RemainingTime = 1000;
                        break;
                    }

                    if (s.RemainingTime > 0)
                        s.RemainingTime -= time.ElaspedMsDelta;

                    var vect = new Vector2(player.X - host.X, player.Y - host.Y);

                    if (vect.Length() > 20)
                        host.Move(player.X, player.Y);
                    else if (vect.Length() > 1)
                    {
                        var dist = host.GetSpeed(0.5f) * time.DeltaTime;

                        if (vect.Length() > 2)
                            dist = host.GetSpeed(0.7f + ((float)player.Stats[4] / 100)) * time.DeltaTime;

                        vect.Normalize();

                        host.ValidateAndMove(host.X + vect.X * dist, host.Y + vect.Y * dist);
                    }

                    break;
            }

            state = s;
        }

        private class FollowState
        {
            public int RemainingTime;
            public F State;
        }
    }
}
