using System;
using System.Text.RegularExpressions;
using wServer.networking.packets.outgoing;

namespace wServer.core.objects
{
    partial class Player
    {
        public bool MaxedAtt { get => _maxedAtt.GetValue(); set => _maxedAtt.SetValue(value); }
        public bool MaxedDef { get => _maxedDef.GetValue(); set => _maxedDef.SetValue(value); }
        public bool MaxedDex { get => _maxedDex.GetValue(); set => _maxedDex.SetValue(value); }
        public bool MaxedLife { get => _maxedLife.GetValue(); set => _maxedLife.SetValue(value); }
        public bool MaxedMana { get => _maxedMana.GetValue(); set => _maxedMana.SetValue(value); }
        public bool MaxedSpd { get => _maxedSpd.GetValue(); set => _maxedSpd.SetValue(value); }
        public bool MaxedVit { get => _maxedVit.GetValue(); set => _maxedVit.SetValue(value); }
        public bool MaxedWis { get => _maxedWis.GetValue(); set => _maxedWis.SetValue(value); }

        private SV<bool> _maxedAtt;
        private SV<bool> _maxedDef;
        private SV<bool> _maxedDex;
        private SV<bool> _maxedLife;
        private SV<bool> _maxedMana;
        private SV<bool> _maxedSpd;
        private SV<bool> _maxedVit;
        private SV<bool> _maxedWis;

        public StatsManager Stats;
    }
}
