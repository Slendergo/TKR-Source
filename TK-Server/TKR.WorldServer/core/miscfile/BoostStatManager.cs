﻿using TKR.Shared.resources;
using System;
using System.Linq;
using TKR.WorldServer.core.miscfile.stats;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.core.miscfile
{
    internal class BoostStatManager
    {
        public ActivateBoost[] ActivateBoost;
        private int[] _boost;
        private SV<int>[] _boostSV;
        private StatsManager _parent;
        private Player _player;

        public BoostStatManager(StatsManager parent)
        {
            _parent = parent;
            _player = parent.Owner;
            _boost = new int[StatsManager.NumStatTypes];
            _boostSV = new SV<int>[_boost.Length];

            for (var i = 0; i < _boostSV.Length; i++)
                _boostSV[i] = new SV<int>(_player, StatsManager.GetBoostStatType(i), _boost[i], i != 0 && i != 1);

            ActivateBoost = new ActivateBoost[_boost.Length];
            for (var i = 0; i < ActivateBoost.Length; i++)
                ActivateBoost[i] = new ActivateBoost();

            ReCalculateValues();
        }

        public int this[int index] => _boost[index];

        protected internal void ReCalculateValues()
        {
            for (var i = 0; i < _boost.Length; i++)
                _boost[i] = 0;

            ApplyEquipBonus();
            ApplyActivateBonus();
            IncrementStatBoost();
            ApplyTalismanBonus();

            for (var i = 0; i < _boost.Length; i++)
                _boostSV[i].SetValue(_boost[i]);
        }

        private void ApplyActivateBonus()
        {
            for (var i = 0; i < ActivateBoost.Length; i++)
            {
                // set boost
                var b = ActivateBoost[i].GetBoost();
                _boost[i] += b;

                // set condition icon

                var effect = (ConditionEffectIndex)((int)ConditionEffectIndex.HPBoost + i);
                var haveCondition = _player.HasConditionEffect(effect);
                if (b > 0)
                {
                    if (!haveCondition)
                        _player.ApplyPermanentConditionEffect(effect);
                }
                else
                {
                    if (haveCondition)
                        _player.RemoveCondition(effect);
                }
            }
        }

        private void ApplyEquipBonus()
        {
            for (var i = 0; i < 4; i++)
            {
                if (_player.Inventory[i] == null)
                    continue;

                foreach (var b in _player.Inventory[i].StatsBoost)
                    IncrementBoost((StatDataType)b.Key, b.Value);
            }
        }

        private void ApplyTalismanBonus()
        {
            foreach (var type in _player.ActiveTalismans)
            {
                var talisman = _player.GetTalisman(type);
                if (talisman == null)
                    throw new Exception($"Unknown talisman type: {type}");

                var desc = _player.GameServer.Resources.GameData.GetTalisman(type);
                if (desc == null)
                    throw new Exception($"Unknown talisman desc type: {type}");

                var tierDesc = desc.GetTierDesc(talisman.Tier);
                if (tierDesc == null)
                    throw new Exception($"Unknown talisman tier: {talisman.Tier}");

                foreach (var stat in tierDesc.StatTypes)
                {
                    // scale by level or by flat value

                    if (stat.Percentage != 0.0)
                    {
                        var scaledAmount = stat.ScalesPerLevel ? stat.Percentage * talisman.Level : stat.Percentage;

                        var statVal = _parent.Base[StatsManager.GetStatIndex((StatDataType)stat.StatType)];

                        var amountToBoostBy = (int)(statVal * scaledAmount);

                        IncrementBoost((StatDataType)stat.StatType, amountToBoostBy);
                        continue;
                    }

                    var scale = (int)(stat.ScalesPerLevel ? stat.Amount * talisman.Level : stat.Amount);
                    IncrementBoost((StatDataType)stat.StatType, scale);
                }
            }

            if (_player.TalismanPotionHealthPercent != 0.0)
            {
                var incrHealth = (int)(_parent.Base[0] * _player.TalismanPotionHealthPercent);
                IncrementBoost(StatDataType.MaximumHP, incrHealth);
            }

            if (_player.TalismanPotionManaPercent != 0.0)
            {
                var incrMana = (int)(_parent.Base[1] * _player.TalismanPotionManaPercent);
                IncrementBoost(StatDataType.MaximumMP, incrMana);
            }
        }

        private void FixedStat(StatDataType stat, int value)
        {
            var i = StatsManager.GetStatIndex(stat);
            _boost[i] = value - _parent.Base[i];
        }

        private int IncreasePercentage(int percentageToIncrease, int stat)
        {
            int percentage = percentageToIncrease;
            var result = percentage * _parent.Base[stat] / 100;
            return result;
        }

        public void IncrementBoost(StatDataType stat, int amount)
        {
            var i = StatsManager.GetStatIndex(stat);

            if (_parent.Base[i] + amount < 1)
                amount = i == 0 ? -_parent.Base[i] + 1 : -_parent.Base[i];

            _boost[i] += amount;
        }

        private void IncrementStatBoost()
        {
            if (_player == null || _player.Client == null || _player.Client.Account == null)
                return;

            for (var a = 0; a < 8; a++)
            {
                if (a >= 7)
                    a = 7;

                if (a >= 8)
                    return;
                if (_player.Client.Account.SetBaseStat > 0)
                    _boost[a] += a < 2 ? _player.Client.Account.SetBaseStat * 5 : _player.Client.Account.SetBaseStat;
            }
        }
    }
}