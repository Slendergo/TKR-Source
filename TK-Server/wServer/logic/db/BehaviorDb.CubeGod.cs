using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ CubeGod = () => Behav()
        .Init("Cube God",
            new State(
                new ScaleHP2(20),
                new TransformOnDeath("Medium Cubes", min: 4),
                new State("Start",
                    new Wander(1),
                    new Shoot(30, 9, 10, 0, predictive: .5, coolDown: 750),
                    new Shoot(30, 4, 10, 1, predictive: .5, coolDown: 1500),
                    new HpLessTransition(0.06, "SpawnMed")
                    ),
                new State("SpawnMed",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xFF0000, 1, 4),
                    new TimedTransition(4000, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
        )
        .Init("Medium Cubes",
            new State(
                new ScaleHP2(20),
                new Wander(1),
                new TransformOnDeath("Small Cube", min: 1),
                new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Shoot(radius: 30, count: 9, shootAngle: 10, projectileIndex: 0, predictive: .5, coolDown: 750),
                    new Shoot(30, 4, 10, 1, predictive: .5, coolDown: 1500),
                    new HpLessTransition(0.1, "SpawnSmall")
                    ),
                new State("SpawnSmall",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFF0000, 1, 4),
                        new TimedTransition(4000, "Suicide")
                    ),
               new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Small Cube",
            new State(
                new ScaleHP2(20),
                new Wander(1),
                new Shoot(radius: 30, count: 9, shootAngle: 10, projectileIndex: 0, predictive: .5, coolDown: 750),
                new Shoot(radius: 20, count: 20, shootAngle: 20, projectileIndex: 0, coolDown: 2000)
                ),
           new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Dirk of Cronus", 0.0006)
                ),
            new Threshold(0.0015,
                new TierLoot(10, ItemType.Armor, 0.12),
                new TierLoot(11, ItemType.Armor, 0.09),
                new TierLoot(10, ItemType.Weapon, 0.12),
                new TierLoot(11, ItemType.Weapon, 0.09),
                new TierLoot(4, ItemType.Ring, 0.07),
                new TierLoot(5, ItemType.Ring, 0.03),
                new TierLoot(4, ItemType.Ability, 0.07),
                new TierLoot(5, ItemType.Ability, 0.03),
                new ItemLoot("Potion of Speed", 1),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Defense", 1),
                new ItemLoot("Potion of Speed", 1),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Defense", 1),

                new ItemLoot("Magic Dust", 0.5)

                )
            );
    }
}
