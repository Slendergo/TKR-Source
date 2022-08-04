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
        public bool SuperMaxedAtt { get => _superMaxedAtt.GetValue(); set => _superMaxedAtt.SetValue(value); }
        public bool SuperMaxedDef { get => _superMaxedDef.GetValue(); set => _superMaxedDef.SetValue(value); }
        public bool SuperMaxedDex { get => _superMaxedDex.GetValue(); set => _superMaxedDex.SetValue(value); }
        public bool SuperMaxedLife { get => _superMaxedLife.GetValue(); set => _superMaxedLife.SetValue(value); }
        public bool SuperMaxedMana { get => _superMaxedMana.GetValue(); set => _superMaxedMana.SetValue(value); }
        public bool SuperMaxedSpd { get => _superMaxedSpd.GetValue(); set => _superMaxedSpd.SetValue(value); }
        public bool SuperMaxedVit { get => _superMaxedVit.GetValue(); set => _superMaxedVit.SetValue(value); }
        public bool SuperMaxedWis { get => _superMaxedWis.GetValue(); set => _superMaxedWis.SetValue(value); }

        private SV<bool> _maxedAtt;
        private SV<bool> _maxedDef;
        private SV<bool> _maxedDex;
        private SV<bool> _maxedLife;
        private SV<bool> _maxedMana;
        private SV<bool> _maxedSpd;
        private SV<bool> _maxedVit;
        private SV<bool> _maxedWis;
        private SV<bool> _superMaxedAtt;
        private SV<bool> _superMaxedDef;
        private SV<bool> _superMaxedDex;
        private SV<bool> _superMaxedLife;
        private SV<bool> _superMaxedMana;
        private SV<bool> _superMaxedSpd;
        private SV<bool> _superMaxedVit;
        private SV<bool> _superMaxedWis;

        public StatsManager Stats;
    }
}
