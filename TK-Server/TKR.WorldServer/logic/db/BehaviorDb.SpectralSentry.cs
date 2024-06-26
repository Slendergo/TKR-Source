﻿using TKR.Shared.resources;
using TKR.WorldServer.logic.loot;
using TKR.WorldServer.logic.behaviors;
using TKR.WorldServer.logic.transitions;

namespace TKR.WorldServer.logic
{
    partial class BehaviorDb
    {
        private _ SpectralSentry = () => Behav()
        .Init("Spectral Sentry",
            new State(             
                new DropPortalOnDeath("Lost Halls Portal", 1, 120),
                new ScaleHP2(20),
                new StayCloseToSpawn(1, 9),
                new ConditionEffectBehavior(ConditionEffectIndex.DazedImmune),
                new ConditionEffectBehavior(ConditionEffectIndex.StasisImmune),
                new ConditionEffectBehavior(ConditionEffectIndex.ParalyzeImmune),
                new Prioritize(
                    new Wander(0.5)
                    ),
                new State("Attack",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Wander(0.5),
                    new Shoot(20, count: 5, shootAngle: 45, projectileIndex: 0, predictive: 1, coolDown: 1000),
                    new Shoot(20, count: 5, shootAngle: 20, projectileIndex: 1, predictive: 0.4, coolDown: 800),
                    new Shoot(20, count: 4, shootAngle: 20, projectileIndex: 2, predictive: 0.2, coolDown: 800),
                    new TimedTransition(10000, "Attack 2")
                    ),
                new State("Attack 2",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                    new Flash(0xFF0000, 0.5, 5),
                    new Shoot(radius: 20, count: 15, shootAngle: 30, projectileIndex: 1, coolDownOffset: 600, coolDown: 1400),
                    new Shoot(radius: 20, count: 15, shootAngle: 30, projectileIndex: 1, coolDownOffset: 800, coolDown: 1400),
                    new Shoot(radius: 20, count: 15, shootAngle: 30, projectileIndex: 1, coolDownOffset: 1000, coolDown: 1400),
                    new TimedTransition(1100, "Attack 3")
                    ),
                new State("Attack 3",                   
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(radius: 15, count: 5, shootAngle: 35, projectileIndex: 2, predictive: 1, coolDownOffset: 300, coolDown: 500),
                    new Shoot(radius: 15, count: 5, shootAngle: 35, projectileIndex: 2, predictive: 1, coolDownOffset: 400, coolDown: 500),
                    new Shoot(radius: 15, count: 5, shootAngle: 35, projectileIndex: 2, predictive: 1, coolDownOffset: 500, coolDown: 500),
                    new TimedTransition(3000, "Reset")
                    ),
                new State("Reset",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable, true),
                    new Flash(0xFF0000, 0.1, 5),
                    new TimedTransition(4000, "Attack")
                    )
                ),
            new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.001,
                new ItemLoot("Potion of Attack", 0.5, 0, 0.0012),
                new ItemLoot("Potion of Speed", 0.5, 0, 0.0012),
                new ItemLoot("Glowing Talisman", 0.005),
                new ItemLoot("Potion of Life", 1, 0, 0.0012),
                new ItemLoot("Potion of Mana", 1, 0, 0.0012),
                new TierLoot(13, ItemType.Weapon, 0.08, 0, 0.002),
                new TierLoot(13, ItemType.Armor, 0.08, 0, 0.002),

                new ItemLoot("Magic Dust", 0.5)
                ),
            new Threshold(0.01,
                new ItemLoot("Necklace of Stolen Life", 0.00125, 0, 0.03)
                ),
            new Threshold(0.01,
                new ItemLoot("Spectral Robe", 0.00125, 0, 0.05),
                new ItemLoot("Talisman Fragment", 0.009),
                new ItemLoot("Scythe of the Reaper", 0.001, 0, 0.05),
                new ItemLoot("Haunting Incantation", 0.00125, 0, 0.05)
                )
            );
    }
}
