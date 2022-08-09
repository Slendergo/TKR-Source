using System.Linq;
using wServer.core;
using wServer.core.objects;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.logic.behaviors
{
    internal class KillPlayer : Behavior
    {
        private readonly bool _killAll;
        private readonly string _killMessage;
        private Cooldown _coolDown;

        public KillPlayer(string killMessage, Cooldown coolDown = new Cooldown(), bool killAll = false)
        {
            _coolDown = coolDown.Normalize();
            _killMessage = killMessage;
            _killAll = killAll;
        }

        protected override void OnStateEntry(Entity host, TickTime time, ref object state) => state = _coolDown.Next(Random);

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
            if (host == null || host.AttackTarget == null || host.AttackTarget.World == null)
                return;

            var cool = (int)state;

            if (cool <= 0)
            {
                // death strike
                if (_killAll)
                    host.World.ForeachPlayer(_ => Kill(host, _));
                else
                    Kill(host, host.AttackTarget);

                Enemy enemy = null;

                if (host is Enemy)
                    enemy = host as Enemy;

                var displayenemy =
                      enemy.Legendary ? $"Legendary {host.ObjectDesc.DisplayId ?? host.ObjectDesc.ObjectId}" :
                      enemy.Epic ? $"Epic {host.ObjectDesc.DisplayId ?? host.ObjectDesc.ObjectId}" :
                      enemy.Rare ? $"Rare {host.ObjectDesc.DisplayId ?? host.ObjectDesc.ObjectId}" :
                      host.ObjectDesc.DisplayId ?? host.ObjectDesc.ObjectId;

                // send kill message
                if (_killMessage != null)
                {
                    var packet = new Text() { Name = "#" + displayenemy, ObjectId = host.Id, NumStars = -1, BubbleTime = 3, Recipient = "", Txt = _killMessage };

                    foreach (var i in host.World.PlayersCollision.HitTest(host.X, host.Y, 15).Where(e => e is Player))
                        if (i is Player && host.Dist(i) < 15)
                            (i as Player).Client.SendPacket(packet);
                }

                cool = _coolDown.Next(Random);
            }
            else
                cool -= time.ElaspedMsDelta;

            state = cool;
        }

        private void Kill(Entity host, Player player)
        {
            host.World.BroadcastIfVisible(new ShowEffect() { EffectType = EffectType.Trail, TargetObjectId = host.Id, Pos1 = new Position { X = player.X, Y = player.Y }, Color = new ARGB(0xffffffff) }, host);

            Enemy enemy = null;

            if (host is Enemy)
                enemy = host as Enemy;

            var displayenemy =
                  enemy.Legendary ? $"Legendary {host.ObjectDesc.DisplayId ?? host.ObjectDesc.ObjectId}" :
                  enemy.Epic ? $"Epic {host.ObjectDesc.DisplayId ?? host.ObjectDesc.ObjectId}" :
                  enemy.Rare ? $"Rare {host.ObjectDesc.DisplayId ?? host.ObjectDesc.ObjectId}" :
                  host.ObjectDesc.DisplayId ?? host.ObjectDesc.ObjectId;

            // kill player
            player.Death(displayenemy);
        }
    }
}
