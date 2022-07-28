using System.Collections.Generic;
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

        protected override void OnStateEntry(Entity host, TickData time, ref object state) => state = new ScaleHPState { pNamesCounted = new List<string>(), initialScaleAmount = _scaleAfter, cooldown = 0 };

        protected override void TickCore(Entity host, TickData time, ref object state)
        {
            var scstate = (ScaleHPState)state;

            if (scstate == null)
                return;

            if (scstate.cooldown <= 0)
            {
                scstate.cooldown = 1000;

                if (!(host is Enemy))
                    return;

                var enemy = host as Enemy;
                var plrCount = 0;

                foreach (var i in host.Owner.Players)
                {
                    if (scstate.pNamesCounted.Contains(i.Value.Name))
                        continue;

                    if (_range > 0)
                    {
                        if (host.Dist(i.Value) < _range)
                            scstate.pNamesCounted.Add(i.Value.Name);
                    }
                    else
                        scstate.pNamesCounted.Add(i.Value.Name);
                }

                plrCount = scstate.pNamesCounted.Count;

                if (plrCount > scstate.initialScaleAmount)
                {
                    var amountPerPlayer = _percentage * enemy.ObjectDesc.MaxHP / 100;
                    var amountInc = (plrCount - scstate.initialScaleAmount) * amountPerPlayer;

                    scstate.initialScaleAmount += plrCount - scstate.initialScaleAmount;

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
