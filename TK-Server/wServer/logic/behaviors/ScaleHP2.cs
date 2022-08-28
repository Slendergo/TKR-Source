using System.Collections.Generic;
using System.Linq;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class ScaleHP2 : Behavior
    {
        private readonly int _percentage;
        private readonly double _range;
        private readonly int _scaleAfter;

        public ScaleHP2(int amount, int scaleStart = 0, double range = 25.0)
        {
            _percentage = amount;
            _range = range;
            _scaleAfter = scaleStart;
        }

        protected override void OnStateEntry(Entity host, TickTime time, ref object state) => state = new ScaleHPState { pNamesCounted = new List<string>(), initialScaleAmount = _scaleAfter, cooldown = 0 };

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
            var scstate = (ScaleHPState)state;

            if (scstate == null)
                return;

            if (scstate.cooldown <= 0)
            {
                scstate.cooldown = 10000;

                if (!(host is Enemy))
                    return;

                var enemy = host as Enemy;
                var plrCount = 0;
                var itemCount = 0;

                foreach (var player in host.GetNearestEntities(_range, null, true).OfType<Player>())
                {
                    if (scstate.pNamesCounted.Contains(player.Name))
                        continue;

                    if (_range > 0)
                    {
                        if (host.Dist(player.Pos) < _range)
                        {
                            for (var i = 0; i < 4; i++)
                            {
                                var item = player.Inventory[i];

                                if (item == null || !item.Legendary && !item.Revenge && !item.Eternal && !item.Mythical)
                                    continue;

                                itemCount++;
                            }
                            scstate.pNamesCounted.Add(player.Name);
                        }
                    }
                    else
                        scstate.pNamesCounted.Add(player.Name);
                }

                plrCount = scstate.pNamesCounted.Count;

                if (itemCount * 2 > scstate.initialScaleAmount)
                {
                    var amountPerPlayer = (plrCount * 2) * enemy.ObjectDesc.MaxHP / 100;
                    var amountInc = ((itemCount * 2) - scstate.initialScaleAmount) * amountPerPlayer;

                    scstate.initialScaleAmount += itemCount - scstate.initialScaleAmount;

                    var newHpMaximum = enemy.MaximumHP + amountInc;

                    enemy.HP += amountInc;
                    enemy.MaximumHP = newHpMaximum;
                }
            }
            else
                scstate.cooldown -= time.ElaspedMsDelta;

            state = scstate;
        }

        private class ScaleHPState
        {
            public int cooldown;
            public int initialScaleAmount = 0;
            public IList<string> pNamesCounted;
        }
    }
}
