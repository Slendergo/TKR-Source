using common.database.info;
using common.resources;
using System;
using System.Collections.Generic;
using wServer.networking.packets.outgoing.talisman;

namespace wServer.core.objects
{
    public partial class Player
    {
        private bool SendTalismanData = true;

        private Dictionary<int, TalismanData> TalismanDatas = new Dictionary<int, TalismanData>();
        public List<int> ActiveTalismans = new List<int>();

        public double TalismanLootBoost { get; set; }
        public double TalismanLootBoostPerPlayer { get; set; }
        public double TalismanAbilityLifeCost { get; set; }
        public bool TalismanImmuneToDamaging { get; set; } // todo talisman
        public bool TalismanImmuneToWeak { get; set; }
        public bool TalismanDamageShotsPierceArmour { get; set; }
        public bool TalismanDamageIsAverage { get; set; }
        public double TalismanExtraLifeRegen { get; set; }
        public double TalismanExtraManaRegen { get; set; }
        public double TalismanExtraAbilityDamage { get; set; }
        public double TalismanFameGainBonus { get; set; }
        public bool TalismanCantGetLoot { get; set; }
        public bool TalismanNoPotionHealing { get; set; }
        public double TalismanHealthHPRegen { get; set; }
        public double TalismanHealthRateOfFire { get; set; }
        public double TalismanPotionHealthPercent { get; set; }
        public double TalismanPotionManaPercent { get; set; }
        public bool TalismanCanOnlyGetWhiteBags { get; set; }
        public double TalismanExtraDamageOnHitHealth { get; set; }
        public double TalismanExtraDamageOnHitMana { get; set; }

        private SV<int> _noManaBar { get; set; }

        public TalismanData GetTalisman(int type) => TalismanDatas.TryGetValue(type, out var data) ? data : null;

        public void UnlockTalisman(TalismanData talismanData)
        {
            TalismanDatas.Add(talismanData.Type, talismanData);
            UpdateTalsimans();
        }

        public void ActivateTalisman(int type)
        {
            if (ActiveTalismans.Contains(type))
                return;
            ActiveTalismans.Add(type);
            UpdateTalsimans();
        }

        public void DeactivateTalisman(int type)
        {
            if (!ActiveTalismans.Contains(type))
                return;
            _ = ActiveTalismans.Remove(type);
            UpdateTalsimans();
        }

        public void GiveEssence(int amount)
        {
            var essenceCap = Client.Account.EssenceCap;
            var essence = Math.Min(essenceCap, Client.Account.Essence + amount);
            Client.Account.Essence = essence;

            if(essence >= essenceCap)
                SendInfo($"You have hit the limit of Talisman Essence");
            else
                SendInfo($"+{amount} Talisman Essence [{essence}/{essenceCap}]");
        }

        public void HandleTalismans(ref TickTime time)
        {
            try
            {
                if (SendTalismanData && (PlayerUpdate.TickId + 1) % 2 == 0)
                {
                    UpdateTalsimans();
                    SendTalismanData = false;
                }
                CheckHealthTalismans();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void SaveTalismanData()
        {
            var talismans = new List<DbTalismanEntry>(TalismanDatas.Count);
            foreach (var talisman in TalismanDatas.Values)
                talismans.Add(new DbTalismanEntry()
                {
                    Type = talisman.Type,
                    Tier = talisman.Tier,
                    Level = talisman.Level,
                    Exp = talisman.CurrentXP,
                    Goal = talisman.ExpGoal,
                });
            GameServer.Database.SaveTalismans(Client.Account.AccountId, talismans);
        }

        private void LoadTalismanData()
        {
            UpdateEssenceCap();
            var talismans = GameServer.Database.GetUnlockedTalismans(Client.Account.AccountId);
            foreach (var talisman in talismans)
                TalismanDatas.Add(talisman.Type, new TalismanData(talisman));
            ActiveTalismans = GameServer.Database.GetActiveTalismans(Client.Account.AccountId, Client.Character.CharId);
            foreach(var active in TalismanDatas)
                if(ActiveTalismans.Contains(active.Key))
                    active.Value.Active = true;
            ApplyTalismanEffects();
        }

        public void UpdateEssenceCap()
        {
            Client.Account.EssenceCap = GetTalismanEssenceCap(Stars);
        }

        protected override bool CanApplyCondition(ConditionEffectIndex effect)
        {
            if (TalismanImmuneToWeak && effect == ConditionEffectIndex.Weak)
                return false;
            if (TalismanImmuneToDamaging && effect == ConditionEffectIndex.Damaging)
                return false;
            return base.CanApplyCondition(effect);
        }

        private void ApplyTalismanEffects()
        {
            //reset
            TalismanLootBoost = 0.0;
            TalismanLootBoostPerPlayer = 0.0;
            TalismanAbilityLifeCost = 0.0;
            TalismanImmuneToDamaging = false;
            TalismanImmuneToWeak = false;
            TalismanDamageShotsPierceArmour = false;
            TalismanDamageIsAverage = false;
            _noManaBar.SetValue(0);
            TalismanExtraAbilityDamage = 0.0;
            TalismanExtraLifeRegen = 0.0;
            TalismanExtraManaRegen = 0.0;
            TalismanFameGainBonus = 0.0;
            TalismanCantGetLoot = false;
            TalismanNoPotionHealing = false;
            TalismanHealthHPRegen = 0.0;
            TalismanHealthRateOfFire = 0.0;
            TalismanCanOnlyGetWhiteBags = false;
            TalismanExtraDamageOnHitHealth = 0.0;
            TalismanExtraDamageOnHitMana = 0.0;

            RemoveCondition(ConditionEffectIndex.ArmorBreakImmune);
            RemoveCondition(ConditionEffectIndex.SlowedImmune);
            RemoveCondition(ConditionEffectIndex.DazedImmune);
            RemoveCondition(ConditionEffectIndex.StunImmune);
            RemoveCondition(ConditionEffectIndex.ParalyzeImmune);

            // readd

            foreach (var type in ActiveTalismans)
            {
                var talisman = GetTalisman(type);
                if (talisman == null)
                    continue;

                var desc = GameServer.Resources.GameData.GetTalisman(type);
                if (desc == null)
                    continue;

                var tierDesc = desc.GetTierDesc(talisman.Tier);
                if (tierDesc == null)
                    continue;

                TalismanCanOnlyGetWhiteBags = tierDesc.CanOnlyGetWhiteBags;

                foreach (var loot in tierDesc.LootBoosts)
                {
                    // scale by level or by flat value
                    var scale = loot.ScalesPerLevel ? loot.Percentage * talisman.Level : loot.Percentage;
                    TalismanLootBoost += scale;
                }

                foreach (var loot in tierDesc.LootBoostPerPlayer)
                {
                    // scale by level or by flat value
                    var scale = loot.ScalesPerLevel ? loot.Percentage * talisman.Level : loot.Percentage;
                    TalismanLootBoostPerPlayer += scale;
                }

                foreach (var cond in tierDesc.ImmuneTo)
                {
                    switch (cond)
                    {
                        case ConditionEffectIndex.Unstable:
                            ApplyPermanentConditionEffect(ConditionEffectIndex.ArmorBreakImmune);
                            break;
                        case ConditionEffectIndex.Slowed:
                            ApplyPermanentConditionEffect(ConditionEffectIndex.SlowedImmune);
                            break;
                        case ConditionEffectIndex.Dazed:
                            ApplyPermanentConditionEffect(ConditionEffectIndex.DazedImmune);
                            break;
                        case ConditionEffectIndex.Stunned:
                            ApplyPermanentConditionEffect(ConditionEffectIndex.StunImmune);
                            break;
                        case ConditionEffectIndex.Paralyzed:
                            ApplyPermanentConditionEffect(ConditionEffectIndex.ParalyzeImmune);
                            break;
                        case ConditionEffectIndex.Weak:
                            TalismanImmuneToWeak = true;
                            break;
                        case ConditionEffectIndex.Damaging:
                            TalismanImmuneToDamaging = true;
                            break;
                    }
                }

                TalismanDamageIsAverage = tierDesc.DamageIsAverage;
                TalismanDamageShotsPierceArmour = tierDesc.ShotsPierceArmour;

                if (tierDesc.RemoveManaBar)
                    _noManaBar.SetValue(1);

                if (tierDesc.AbilityLifeCost > 0.0f)
                    TalismanAbilityLifeCost = tierDesc.AbilityLifeCost;

                foreach (var extra in tierDesc.Extra)
                {
                    var scale = extra.ScalesPerLevel ? extra.Percentage * talisman.Level : extra.Percentage;
                    switch (extra.Type)
                    {
                        case TalismanExtra.ABILITY_DAMAGE:
                            TalismanExtraAbilityDamage += scale;
                            break;
                        case TalismanExtra.LIFE_REGEN:
                            TalismanExtraLifeRegen += scale;
                            break;
                        case TalismanExtra.MANA_REGEN:
                            TalismanExtraManaRegen += scale;
                            break;
                    }
                }

                foreach (var extra in tierDesc.ExtraDamageOn)
                {
                    var scale = extra.ScalesPerLevel ? talisman.Level * extra.Percentage : extra.Percentage;
                    if (extra.StatType == TalismanExtraDamageOn.HEALTH)
                        TalismanExtraDamageOnHitHealth += scale;

                    if (extra.StatType == TalismanExtraDamageOn.MANA)
                        TalismanExtraDamageOnHitMana += scale;
                }

                foreach (var percentage in tierDesc.FameGainBonus)
                    TalismanFameGainBonus += percentage;
                TalismanCantGetLoot = tierDesc.CantGetLoot;
                TalismanNoPotionHealing = tierDesc.NoPotionHealing;
            }


            var doubleprevent = RecalculateStackedPotions();
            if(!doubleprevent)
                Stats.ReCalculateValues();
        }

        public bool RecalculateStackedPotions()
        {
            TalismanPotionHealthPercent = 0.0f;
            TalismanPotionManaPercent = 0.0f;

            foreach (var type in ActiveTalismans)
            {
                var talisman = GetTalisman(type);
                if (talisman == null)
                    continue;

                var desc = GameServer.Resources.GameData.GetTalisman(type);
                if (desc == null)
                    continue;

                var tierDesc = desc.GetTierDesc(talisman.Tier);
                if (tierDesc == null)
                    continue;

                foreach(var potion in tierDesc.PotionStack)
                {
                    var scale = potion.ScalesPerLevel ? (potion.Percentage * talisman.Level) : potion.Percentage;
                    switch (potion.Type)
                    {
                        case TalismanPotionStack.HEALTH:
                            TalismanPotionHealthPercent += scale * Stacks[0].Count;
                            break;
                        case TalismanPotionStack.MANA:
                            TalismanPotionManaPercent += scale * Stacks[1].Count;
                            break;
                    }
                }
            }

            Stats.ReCalculateValues();

            return TalismanPotionHealthPercent != 0.0 && TalismanPotionManaPercent != 0.0;
        }

        public void CheckHealthTalismans()
        {
            TalismanHealthRateOfFire = 0.0;
            TalismanHealthHPRegen = 0.0;

            foreach (var type in ActiveTalismans)
            {
                var talisman = GetTalisman(type);
                if (talisman == null)
                    continue;

                var desc = GameServer.Resources.GameData.GetTalisman(type);
                if (desc == null)
                    continue;

                var tierDesc = desc.GetTierDesc(talisman.Tier);
                if (tierDesc == null)
                    continue;

                foreach (var health in tierDesc.Health)
                {
                    var perc = (Stats[0] * health.HealthPercent);

                    var scale = health.ScalesPerLevel ? (health.AddPercent * talisman.Level) : health.AddPercent;
                    if (!health.Above && HP <= perc)
                    {
                        switch (health.Type)
                        {
                            case TalismanHealth.RATE_OF_FIRE:
                                TalismanHealthRateOfFire += scale;
                                break;
                            case TalismanHealth.HEALTH_REGEN:
                                TalismanHealthHPRegen += scale;
                                break;
                        }
                        continue;
                    }

                    if (HP >= perc)
                    {
                        switch(health.Type)
                        {
                            case TalismanHealth.RATE_OF_FIRE:
                                TalismanHealthRateOfFire += scale;
                                break;
                            case TalismanHealth.HEALTH_REGEN:
                                TalismanHealthHPRegen += scale;
                                break;
                        }
                    }
                }
            }
        }

        public void UpdateTalsimans()
        {
            ApplyTalismanEffects();
            var data = new TalismanEssenceData()
            {
                Essence = Client.Account.Essence,
                EssenceCap = Client.Account.EssenceCap
            };
            foreach (var talisman in TalismanDatas.Values)  
                data.Talismans.Add(talisman);
            Client.SendPacket(data);
        }
    }
}
