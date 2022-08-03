using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Mushroom = () => Behav()
        .Init("Mushroom",
            new State(
                new ScaleHP2(20),
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Spawn("Mushroom Anchor", 1, 1),
                new State("Check Player",
                    new PlayerWithinTransition(15, "Start")
                    ),
                new State("Start",
                    new Taunt("HAHAHA!"),
                    new TimedTransition(3000, "Start To Shoot")
                    ),
                new State("Start To Shoot",
                    new Taunt("Let the bacteria CONSUME YOU!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new TossObject2("Mushroom Bomb", 6, 0, coolDownOffset: 0, coolDown: 5000),
                    new TossObject2("Mushroom Bomb", 6, 72, coolDownOffset: 1000, coolDown: 5000),
                    new TossObject2("Mushroom Bomb", 6, 144, coolDownOffset: 2000, coolDown: 5000),
                    new TossObject2("Mushroom Bomb", 6, 216, coolDownOffset: 3000, coolDown: 5000),
                    new TossObject2("Mushroom Bomb", 6, 288, coolDownOffset: 4000, coolDown: 5000),
                    new Shoot(radius: 15, count: 12, projectileIndex: 0, coolDown: 1500, predictive: 1),

                    new Shoot(radius: 15, count: 1, projectileIndex: 1, fixedAngle: 0, coolDownOffset: 0, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 2, fixedAngle: 0, shootAngle: 10, coolDownOffset: 0, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 3, fixedAngle: 0, shootAngle: 30, coolDownOffset: 0, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 1, projectileIndex: 1, fixedAngle: 90, coolDownOffset: 0, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 2, fixedAngle: 90, shootAngle: 10, coolDownOffset: 0, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 3, fixedAngle: 90, shootAngle: 30, coolDownOffset: 0, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 1, projectileIndex: 1, fixedAngle: 180, coolDownOffset: 0, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 2, fixedAngle: 180, shootAngle: 10, coolDownOffset: 0, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 3, fixedAngle: 180, shootAngle: 30, coolDownOffset: 0, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 1, projectileIndex: 1, fixedAngle: 270, coolDownOffset: 0, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 2, fixedAngle: 270, shootAngle: 10, coolDownOffset: 0, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 3, fixedAngle: 270, shootAngle: 30, coolDownOffset: 0, coolDown: 2000, predictive: 1),

                    new Shoot(radius: 15, count: 1, projectileIndex: 1, fixedAngle: 45, coolDownOffset: 1000, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 2, fixedAngle: 45, shootAngle: 10, coolDownOffset: 1000, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 3, fixedAngle: 45, shootAngle: 30, coolDownOffset: 1000, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 1, projectileIndex: 1, fixedAngle: 135, coolDownOffset: 1000, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 2, fixedAngle: 135, shootAngle: 10, coolDownOffset: 1000, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 3, fixedAngle: 135, shootAngle: 30, coolDownOffset: 1000, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 1, projectileIndex: 1, fixedAngle: 225, coolDownOffset: 1000, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 2, fixedAngle: 225, shootAngle: 10, coolDownOffset: 1000, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 3, fixedAngle: 225, shootAngle: 30, coolDownOffset: 1000, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 1, projectileIndex: 1, fixedAngle: 315, coolDownOffset: 1000, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 2, fixedAngle: 315, shootAngle: 10, coolDownOffset: 1000, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 3, fixedAngle: 315, shootAngle: 30, coolDownOffset: 1000, coolDown: 2000, predictive: 1),
                    new HpLessTransition(0.66, "Orbit")
                    ),
                new State("Orbit",
                    new SetAltTexture(1, 1, 0, false),
                    new Prioritize(
                        new Orbit(5, 5, 10, "Mushroom Anchor", speedVariance: 0.5, radiusVariance: 0.5)),
                    new Shoot(15, 12, projectileIndex: 4, coolDown: 500),
                    new Shoot(12, 5, shootAngle: 20, projectileIndex: 1, coolDown: 800, predictive: 1.2),
                    new Spawn("Mini Mushroom", 5, 0.01, 4000, true),
                    new Spawn("Mini Mushroom 2", 5, 0.01, 8000, true),
                    new HpLessTransition(0.33, "Spawn Minions")
                    ),
                new State("Spawn Minions",
                    new ReturnToSpawn(1, 20),                    
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new TimedTransition(4000, "Spawn Minions 1")
                    ),
                new State("Spawn Minions 1",
                    new SetAltTexture(0, 0, 0, false),
                    new Follow(3, 3),
                    new Wander(0.6),
                    new Shoot(radius: 15, count: 1, projectileIndex: 1, coolDownOffset: 0, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 2, shootAngle: 10, coolDownOffset: 0, coolDown: 2000, predictive: 1),
                    new Shoot(radius: 15, count: 2, projectileIndex: 3, shootAngle: 30, coolDownOffset: 0, coolDown: 2000, predictive: 1),
                    new Spawn("Mini Mushroom", 5, 0.01, 4000, true),
                    new Spawn("Mini Mushroom 2", 5, 0.01, 8000, true),
                    new Shoot(radius: 12, count: 12, projectileIndex: 0, shootAngle: 45, coolDownOffset: 900, coolDown: 1000)
                    )
                ),
            new Threshold(0.001,
                new ItemLoot("Potion of Life", 0.5),
                new ItemLoot("Potion of Mana", 0.5),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Speed", 1),
                new ItemLoot("Potion of Attack", 0.5)
                ),
            new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Fungal Cloth", 0.0014)
                ),
            new Threshold(0.001,
                new TierLoot(11, ItemType.Armor, 0.01),
                new TierLoot(12, ItemType.Armor, 0.03),
                new TierLoot(12, ItemType.Weapon, 0.01),
                new TierLoot(11, ItemType.Weapon, 0.03),
                new TierLoot(5, ItemType.Ability, 0.01),
                new TierLoot(4, ItemType.Ability, 0.03),
                new TierLoot(5, ItemType.Ring, 0.03),
                new TierLoot(4, ItemType.Ring, 0.01),
                new ItemLoot("Magical Mushroom Staff", 0.01),
                new ItemLoot("Mushroom Loop", 0.01),

                new ItemLoot("Magic Dust", 0.5)
                )
            )
        .Init("Mushroom Anchor",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible)
                )
            )
        .Init("Mushroom Bomb",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new State("Start Follow Player",
                    new Flash(0xFF0000, 0.5, 5),
                    new TimedTransition(1500, "Explotes")
                    ),
                new State("Explotes",
                    new Flash(0xFF0000, 0.2, 20),
                    new TimedTransition(1000, "fire")
                    ),
                new State("fire",
                    new Shoot(8, count: 8, projectileIndex: 0, coolDownOffset: 0, coolDown: 800),
                    new Suicide()
                    )
                )
            )
        .Init("Mini Mushroom",
            new State(
                //new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new State("Start Follow Player",
                    new Follow(1, 8, 0, 1100),
                    new TimedTransition(1100, "Explotes")
                    ),
                new State("Explotes",
                    new Flash(0xFF0000, 0.5, 5),
                    new Shoot(8, count: 8, projectileIndex: 0, coolDownOffset: 0, coolDown: 800),
                    new TimedTransition(400, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Mini Mushroom 2",
            new State(
                //new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new State("Start Follow Player",
                    new Follow(1, 10, 0, 1100),
                    new TimedTransition(1100, "Explotes")
                    ),
                new State("Explotes",
                    new Flash(0x353535, 0.25, 5),
                    new Shoot(12, count: 8, projectileIndex: 0, coolDown: 800),
                    new TimedTransition(400, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        ;
    }
}
