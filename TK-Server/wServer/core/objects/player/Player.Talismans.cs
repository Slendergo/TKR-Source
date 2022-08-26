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

        public float TalismanLootBoost { get; set; }
        public float TalismanLootBoostPerPlayer { get; set; }
        public float TalismanAbilityLifeCost { get; set; }
        public bool TalismanImmuneToDamaging { get; set; } // todo talisman
        public bool TalismanImmuneToWeak { get; set; }
        public bool TalismanDamageShotsPierceArmour { get; set; }
        public bool TalismanDamageIsAverage { get; set; }
        public float TalismanExtraLifeRegen { get; set; }
        public float TalismanExtraManaRegen { get; set; }
        public float TalismanExtraAbilityDamage { get; set; }
        public float TalismanFameGainBonus { get; set; }
        public bool TalismanCantGetLoot { get; set; }
        public bool TalismanNoPotionHealing { get; set; }
        public float TalismanHealthHPRegen { get; set; }
        public float TalismanHealthRateOfFire { get; set; }

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
            var essenceCap = Client.Character.EssenceCap;
            var essence = Math.Min(essenceCap, Client.Character.Essence + amount);
            Client.Character.Essence = essence;

            if(essence == essenceCap)
                SendInfo($"You have hit the limit of Talisman Essence");
            else
                SendInfo($"You have gained: {amount} Talisman Essence");
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
                    Active = talisman.Active
                });
            GameServer.Database.SaveTalismansToCharacter(Client.Account.AccountId, Client.Character.CharId, talismans);
        }

        private void LoadTalismanData()
        {
            UpdateEssenceCap();
            var talismans = GameServer.Database.GetTalismansFromCharacter(Client.Account.AccountId, Client.Character.CharId);
            foreach (var talisman in talismans)
            {
                TalismanDatas.Add(talisman.Type, new TalismanData(talisman));
                if (talisman.Active)
                    ActiveTalismans.Add(talisman.Type);
            }
            ApplyTalismanEffects();
        }

        public void UpdateEssenceCap()
        {
            Client.Character.EssenceCap = GetTalismanEssenceCap(Fame);
        }

        private void ApplyTalismanEffects()
        {
            //reset
            TalismanLootBoost = 0.0f;
            TalismanLootBoostPerPlayer = 0.0f;
            TalismanAbilityLifeCost = 0.0f;
            TalismanImmuneToDamaging = false;
            TalismanImmuneToWeak = false;
            TalismanDamageShotsPierceArmour = false;
            TalismanDamageIsAverage = false;
            _noManaBar.SetValue(0);
            TalismanExtraAbilityDamage = 0.0f;
            TalismanExtraLifeRegen = 0.0f;
            TalismanExtraManaRegen = 0.0f;
            TalismanFameGainBonus = 0.0f;
            TalismanCantGetLoot = false;
            TalismanNoPotionHealing = false;
            TalismanHealthHPRegen = 0.0f;
            TalismanHealthRateOfFire = 0.0f;

            ApplyConditionEffect(ConditionEffectIndex.ArmorBreakImmune, 0);
            ApplyConditionEffect(ConditionEffectIndex.SlowedImmune, 0);
            ApplyConditionEffect(ConditionEffectIndex.DazedImmune, 0);
            ApplyConditionEffect(ConditionEffectIndex.StunImmune, 0);
            ApplyConditionEffect(ConditionEffectIndex.Paralyzed, 0);

            // readd

            Stats.ReCalculateValues();
            
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

                // todo rest

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
                            ApplyConditionEffect(ConditionEffectIndex.ArmorBreakImmune);
                            break;
                        case ConditionEffectIndex.Slowed:
                            ApplyConditionEffect(ConditionEffectIndex.SlowedImmune);
                            break;
                        case ConditionEffectIndex.Dazed:
                            ApplyConditionEffect(ConditionEffectIndex.DazedImmune);
                            break;
                        case ConditionEffectIndex.Stunned:
                            ApplyConditionEffect(ConditionEffectIndex.StunImmune);
                            break;
                        case ConditionEffectIndex.Paralyzed:
                            ApplyConditionEffect(ConditionEffectIndex.Paralyzed);
                            break;
                        case ConditionEffectIndex.Weak:
                            TalismanImmuneToWeak = true;
                            break;
                        case ConditionEffectIndex.Damaging:
                            TalismanImmuneToDamaging = true;
                            break;
                    }
                }

                if (tierDesc.RemoveManaBar)
                    _noManaBar.SetValue(1);

                if (tierDesc.AbilityLifeCost > 0.0f)
                    TalismanAbilityLifeCost = tierDesc.AbilityLifeCost;

                foreach (var extra in tierDesc.Extra)
                {
                    switch (extra.Type)
                    {
                        case TalismanExtra.ABILITY_DAMAGE:
                            TalismanExtraAbilityDamage += extra.Percentage;
                            break;
                        case TalismanExtra.LIFE_REGEN:
                            TalismanExtraLifeRegen += extra.Percentage;
                            break;
                        case TalismanExtra.MANA_REGEN:
                            TalismanExtraManaRegen += extra.Percentage;
                            break;
                    }
                }

                foreach (var percentage in tierDesc.FameGainBonus)
                    TalismanFameGainBonus += percentage;
                TalismanCantGetLoot = tierDesc.CantGetLoot;
                TalismanNoPotionHealing = tierDesc.NoPotionHealing;
            }
        }

        public void CheckHealthTalismans()
        {
            TalismanHealthRateOfFire = 0.0f;
            TalismanHealthHPRegen = 0.0f;

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

                    if (!health.Above && HP <= perc)
                    {
                        switch (health.Type)
                        {
                            case TalismanHealth.RATE_OF_FIRE:
                                TalismanHealthRateOfFire += health.ScalesPerLevel ? (health.AddPercent * talisman.Level) : health.AddPercent;
                                break;
                            case TalismanHealth.HEALTH_REGEN:
                                TalismanHealthHPRegen += health.ScalesPerLevel ? (health.AddPercent * talisman.Level) : health.AddPercent;
                                break;
                        }
                        continue;
                    }

                    if (HP >= perc)
                    {
                        switch(health.Type)
                        {
                            case TalismanHealth.RATE_OF_FIRE:
                                TalismanHealthRateOfFire += health.ScalesPerLevel ? (health.AddPercent * talisman.Level) : health.AddPercent;
                                break;
                            case TalismanHealth.HEALTH_REGEN:
                                TalismanHealthHPRegen += health.ScalesPerLevel ? (health.AddPercent * talisman.Level) : health.AddPercent;
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
                Essence = Client.Character.Essence,
                EssenceCap = Client.Character.EssenceCap
            };
            foreach (var talisman in TalismanDatas.Values)  
                data.Talismans.Add(talisman);
            Client.SendPacket(data);
        }
    }
}
