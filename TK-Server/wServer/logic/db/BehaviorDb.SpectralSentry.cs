using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ SpectralSentry = () => Behav()
        .Init("Spectral Sentry",
            new State(             
                new DropPortalOnDeath("Lost Halls Portal", 1, 120),
                new ScaleHP2(20),
                new ConditionalEffect(ConditionEffectIndex.DazedImmune),
                new ConditionalEffect(ConditionEffectIndex.StasisImmune),
                new ConditionalEffect(ConditionEffectIndex.ParalyzeImmune),
                new Prioritize(
                    new Wander(0.5)
                    ),
                new State("Attack",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Wander(0.5),
                    new Shoot(20, count: 5, shootAngle: 45, projectileIndex: 0, predictive: 1, coolDown: 1300),
                    new Shoot(20, count: 5, shootAngle: 20, projectileIndex: 1, predictive: 0.4, coolDown: 1300),
                    new Shoot(20, count: 4, shootAngle: 20, projectileIndex: 2, predictive: 0.2, coolDown: 1300),
                    new TimedTransition(10000, "Attack 2")
                    ),
                new State("Attack 2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xFF0000, 0.5, 5),
                    new Shoot(radius: 20, count: 15, shootAngle: 30, projectileIndex: 1, coolDownOffset: 600, coolDown: 2000),
                    new Shoot(radius: 20, count: 15, shootAngle: 30, projectileIndex: 1, coolDownOffset: 800, coolDown: 2000),
                    new Shoot(radius: 20, count: 15, shootAngle: 30, projectileIndex: 1, coolDownOffset: 1000, coolDown: 2000),
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
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
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
                new ItemLoot("Potion of Life", 1, 0, 0.0012),
                new ItemLoot("Potion of Mana", 1, 0, 0.0012),
                new TierLoot(13, ItemType.Weapon, 0.08, 0, 0.002),
                new TierLoot(13, ItemType.Armor, 0.08, 0, 0.002),

                new ItemLoot("Magic Dust", 0.5)
                ),
            new Threshold(0.03,
                new ItemLoot("Necklace of Stolen Life", 0.003, 0, 0.03)
                ),
            new Threshold(0.05,
                new ItemLoot("Spectral Robe", 0.0014, 0, 0.05),
                new ItemLoot("Scythe of the Reaper", 0.0014, 0, 0.05),
                new ItemLoot("Haunting Incantation", 0.0014, 0, 0.05)
                )
            );
    }
}
