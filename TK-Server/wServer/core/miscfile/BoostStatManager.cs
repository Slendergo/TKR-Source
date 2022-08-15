using common.resources;
using System;
using System.Linq;
using wServer.core.objects;
using wServer.utils;

namespace wServer.core
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

        public void CheckItems()
        {
            if (_player == null || _player.Client == null || _player.Client.Account == null)
                return;

            for (var i = 0; i < 20; i++)
            {
                if (_player.Inventory[i] == null)
                    continue;
                if (_player.Inventory[i].ObjectId == "Cerberus's Left Claw" && _player.Inventory[i] != null)
                {
                    try
                    {
                        var surrounding = _player.World.EnemiesCollision.HitTest(_player.X, _player.Y, 10).Count();
                        var maxBoost = 20;
                        var countBoost = 0;
                        for (int j = 0; j < surrounding; j++)
                            if (countBoost <= maxBoost)
                            {
                                IncrementBoost(StatDataType.Attack, 1);
                                countBoost++;
                            }
                    }
                    catch (NullReferenceException e)
                    {
                        SLogger.Instance.Info(e);
                        continue;
                    }
                }
                if (_player.Inventory[i].ObjectId == "Cerberus's Right Claw" && _player.Inventory[i] != null)
                {
                    var acc = _player.GameServer.Database.GetAccount(_player.AccountId);
                    var enemiesKilled = acc.EnemiesKilled;
                    var countHPBoost = 0;
                    for (int k = 0; k < enemiesKilled; k++)
                    {
                        if (k % 1000 == 0 && countHPBoost<=10) // every 1500 of the enemies
                        {
                            IncrementBoost(StatDataType.MaximumHP, 50);
                            countHPBoost++;
                        }
                    }
                    
                }
                foreach (var b in _player.Inventory[i].StatsBoostOnHandle)
                    IncrementBoost((StatDataType)b.Key, b.Value);
                
            }
        }

        public void CheckItemsNoStack() //TODO
        {
            if (_player == null || _player.Client == null || _player.Client.Account == null)
                return;

            for (var i = 0; i < 20; i++)
            {
                if (_player.Inventory[i] == null)
                    continue;

                if (_player.Inventory[i].SetStatsNoStack)
                {
                    IncrementBoost((StatDataType)_player.Inventory[i].StatSetNoStack, _player.Inventory[i].AmountSetNoStack);
                    break;
                }
            }
        }

        protected internal void ReCalculateValues()
        {
            for (var i = 0; i < _boost.Length; i++)
                _boost[i] = 0;

            ApplyEquipBonus();
            ApplyActivateBonus();
            //CheckItems();
            CheckItemsNoStack();
            IncrementStatBoost();
            IncrementSkillBoosts();
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
            foreach(var type in _player.ActiveTalismans)
            {
                var talisman = _player.GetTalisman(type);
                if (talisman == null)
                    throw new Exception($"Unknown talisman type: {type}");

                var desc = _player.GameServer.Resources.GameData.GetTalisman(type);
                if(desc == null)
                    throw new Exception($"Unknown talisman desc type: {type}");

                var tierDesc = desc.GetTierDesc(talisman.Tier);
                if(tierDesc == null)
                    throw new Exception($"Unknown talisman tier: {talisman.Tier}");

                foreach (var stat in tierDesc.StatTypes)
                {
                    // scale by level or by flat value
                    var scale = stat.ScalesPerLevel ? stat.Amount * talisman.Level : stat.Amount;
                    IncrementBoost((StatDataType)stat.StatType, scale);
                }

                foreach (var loot in tierDesc.LootBoosts)
                {
                    // scale by level or by flat value
                    var scale = loot.ScalesPerLevel ? loot.Amount * talisman.Level : loot.Amount;
                    _player.TalismanLootBoost += scale;
                }

                foreach (var cond in tierDesc.ImmuneTo)
                {
                    switch (cond)
                    {
                        case ConditionEffectIndex.Unstable:
                            _player.ApplyConditionEffect(ConditionEffectIndex.ArmorBreakImmune);
                            break;
                        case ConditionEffectIndex.Slowed:
                            _player.ApplyConditionEffect(ConditionEffectIndex.SlowedImmune);
                            break;
                        case ConditionEffectIndex.Dazed:
                            _player.ApplyConditionEffect(ConditionEffectIndex.DazedImmune);
                            break;
                        case ConditionEffectIndex.Stunned:
                            _player.ApplyConditionEffect(ConditionEffectIndex.StunImmune);
                            break;
                    }
                }
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
                amount = (i == 0) ? -_parent.Base[i] + 1 : -_parent.Base[i];

            _boost[i] += amount;
        }

        private void IncrementSkillBoosts() //Skill tree
        {
            if (_player == null || _player.Client == null || _player.Client.Account == null)
                return;

            var life = 0;
            var mana = 1;
            var att = 2;
            var def = 3;
            var spd = 4;
            var dex = 5;
            var vit = 6;
            var wis = 7;

            //Positive Stats %
            switch (_player.Node1Med)
            {
                case 1: _boost[att] += IncreasePercentage(5, att); break;
                case 2: _boost[att] += IncreasePercentage(5, att); _boost[dex] += IncreasePercentage(5, dex); break;
                case 3: _boost[att] += IncreasePercentage(10, att); _boost[dex] += IncreasePercentage(5, dex); break;
                default: break;
            }

            switch (_player.Node2Med)
            {
                case 1: _boost[wis] += IncreasePercentage(5, wis); break;
                case 2: _boost[wis] += IncreasePercentage(5, wis); _boost[mana] += IncreasePercentage(30, mana); break;
                case 3: _boost[wis] += IncreasePercentage(10, wis); _boost[mana] += IncreasePercentage(30, mana); break;
                default: break;
            }

            switch (_player.Node3Med)
            {
                case 1: _boost[def] += IncreasePercentage(5, def); break;
                case 2: _boost[def] += IncreasePercentage(5, def); _boost[life] += IncreasePercentage(30, life); break;
                case 3: _boost[def] += IncreasePercentage(10, def); _boost[life] += IncreasePercentage(30, life); break;
                default: break;
            }

            switch (_player.Node4Med)
            {
                case 1: _boost[spd] += IncreasePercentage(5, spd); break;
                case 2: _boost[spd] += IncreasePercentage(5, spd); _boost[dex] += IncreasePercentage(5, dex); break;
                case 3: _boost[spd] += IncreasePercentage(10, spd); _boost[dex] += IncreasePercentage(5, dex); break;
                default: break;
            }

            //Negative Stats %
            _boost[life] -= _player.Node1Big > 0 ? IncreasePercentage(15, life) : 0;

            _boost[att] -= _player.Node2Big > 0 ? IncreasePercentage(15, att) : 0;

            _boost[mana] -= _player.Node3Big > 0 ? IncreasePercentage(15, spd) : 0;

            _boost[def] -= _player.Node4Big > 0 ? IncreasePercentage(15, def) : 0;

            //Flat Stats Increases
            _boost[mana] += _player.Node2TickMin * 10;
            _boost[att] += _player.Node1TickMaj;
            _boost[def] += _player.Node3TickMaj * 2;
            _boost[spd] += _player.Node4TickMaj;
            _boost[dex] += _player.Node1TickMin + _player.Node4TickMin;
            _boost[wis] += _player.Node2TickMaj;
            _boost[vit] += _player.Node3TickMin * 4;
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
