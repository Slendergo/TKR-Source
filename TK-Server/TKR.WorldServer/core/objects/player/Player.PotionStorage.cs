using System;
using System.Text.RegularExpressions;
using TKR.WorldServer.core.net.stats;
using TKR.WorldServer.networking.packets.outgoing;

namespace TKR.WorldServer.core.objects
{
    partial class Player
    {
        public int SPSLifeCount { get => _SPSLifeCount.GetValue(); set => _SPSLifeCount.SetValue(value); }
        public int SPSManaCount { get => _SPSManaCount.GetValue(); set => _SPSManaCount.SetValue(value); }
        public int SPSDefenseCount { get => _SPSDefenseCount.GetValue(); set => _SPSDefenseCount.SetValue(value); }
        public int SPSAttackCount { get => _SPSAttackCount.GetValue(); set => _SPSAttackCount.SetValue(value); }
        public int SPSDexterityCount { get => _SPSDexterityCount.GetValue(); set => _SPSDexterityCount.SetValue(value); }
        public int SPSSpeedCount { get => _SPSSpeedCount.GetValue(); set => _SPSSpeedCount.SetValue(value); }
        public int SPSVitalityCount { get => _SPSVitalityCount.GetValue(); set => _SPSVitalityCount.SetValue(value); }
        public int SPSWisdomCount { get => _SPSWisdomCount.GetValue(); set => _SPSWisdomCount.SetValue(value); }
        public int SPSLifeCountMax { get => _SPSLifeCountMax.GetValue(); set => _SPSLifeCountMax.SetValue(value); }
        public int SPSManaCountMax { get => _SPSManaCountMax.GetValue(); set => _SPSManaCountMax.SetValue(value); }
        public int SPSDefenseCountMax { get => _SPSDefenseCountMax.GetValue(); set => _SPSDefenseCountMax.SetValue(value); }
        public int SPSAttackCountMax { get => _SPSAttackCountMax.GetValue(); set => _SPSAttackCountMax.SetValue(value); }
        public int SPSDexterityCountMax { get => _SPSDexterityCountMax.GetValue(); set => _SPSDexterityCountMax.SetValue(value); }
        public int SPSSpeedCountMax { get => _SPSSpeedCountMax.GetValue(); set => _SPSSpeedCountMax.SetValue(value); }
        public int SPSVitalityCountMax { get => _SPSVitalityCountMax.GetValue(); set => _SPSVitalityCountMax.SetValue(value); }
        public int SPSWisdomCountMax { get => _SPSWisdomCountMax.GetValue(); set => _SPSWisdomCountMax.SetValue(value); }

        private SV<int> _SPSLifeCount;
        private SV<int> _SPSManaCount;
        private SV<int> _SPSDefenseCount;
        private SV<int> _SPSAttackCount;
        private SV<int> _SPSDexterityCount;
        private SV<int> _SPSSpeedCount;
        private SV<int> _SPSVitalityCount;
        private SV<int> _SPSWisdomCount;

        private SV<int> _SPSLifeCountMax;
        private SV<int> _SPSManaCountMax;
        private SV<int> _SPSDefenseCountMax;
        private SV<int> _SPSAttackCountMax;
        private SV<int> _SPSDexterityCountMax;
        private SV<int> _SPSSpeedCountMax;
        private SV<int> _SPSVitalityCountMax;
        private SV<int> _SPSWisdomCountMax;
    }
}
