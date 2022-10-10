using TKR.Shared.resources;
using TKR.WorldServer.logic.loot;
using TKR.WorldServer.logic.behaviors;
using TKR.WorldServer.logic.transitions;

namespace TKR.WorldServer.logic
{
    partial class BehaviorDb
    {
        private _ CubeGod = () => Behav()
        .Init("Cube God",
            new State(
                new ScaleHP2(20),
                new State("Start",
                    new Wander(0.3),
                    new TossObject2("Medium Cubes", 3, coolDown: 99999, randomToss: true),
                    new TossObject2("Medium Cubes", 4, coolDown: 99999, randomToss: true),
                    new TossObject2("Medium Cubes", 1, coolDown: 99999, randomToss: true),
                    new TossObject2("Medium Cubes", 5, coolDown: 99999, randomToss: true),
                    new TossObject2("Small Cube", 3, coolDown: 99999, randomToss: true),
                    new TossObject2("Small Cube", 4, coolDown: 99999, randomToss: true),
                    new TossObject2("Small Cube", 1, coolDown: 99999, randomToss: true),
                    new TossObject2("Small Cube", 5, coolDown: 99999, randomToss: true),
                    new Shoot(30, 9, 10, 0, predictive: 1.5, coolDown: 750),
                    new Shoot(30, 4, 10, 1, predictive: 1.5, coolDown: 1500),
                    new HpLessTransition(0.06, "SpawnMed")
                    ),
                new State("SpawnMed",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                    new Flash(0xFF0000, 1, 4),
                    new TimedTransition(4000, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                   )
                ),
           new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Talisman Fragment", 0.009),
                new ItemLoot("Dirk of Cronus", 0.001)
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
            )
        .Init("Medium Cubes",
            new State(
                new ScaleHP2(20),
                new Wander(0.3),
                new State("Start",
                    new ConditionEffectBehavior(ConditionEffectIndex.Armored),
                    new Shoot(radius: 30, count: 5, shootAngle: 10, projectileIndex: 0, predictive: .5, coolDown: 750),
                    new Shoot(30, 4, 10, 1, predictive: .5, coolDown: 1500)
                    )
                )
            )
        .Init("Small Cube",
            new State(
                new ScaleHP2(20),
                new Wander(0.3),
                new Shoot(radius: 30, count: 3, shootAngle: 10, projectileIndex: 0, predictive: .5, coolDown: 750),
                new Shoot(radius: 20, count: 20, shootAngle: 20, projectileIndex: 0, coolDown: 2000)
                )
            );
    }
}
