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
                    new TossObject2("Mushroom Bomb", 6, 0, coolDownOffset: 100, coolDown: 999999),
                    new TossObject2("Mushroom Bomb", 6, 72, coolDownOffset: 400, coolDown: 999999),
                    new TossObject2("Mushroom Bomb", 6, 144, coolDownOffset: 700, coolDown: 999999),
                    new TossObject2("Mushroom Bomb", 6, 216, coolDownOffset: 1000, coolDown: 999999),
                    new TossObject2("Mushroom Bomb", 6, 288, coolDownOffset: 1300, coolDown: 999999),
                    new Shoot(radius: 15, count: 20, projectileIndex: 0, shootAngle: 20, coolDown: 1500, predictive: 0.1),
                    new HpLessTransition(0.75, "Armored Shoot")
                    ),
                new State("Armored Shoot",
                    new SetAltTexture(1, 1, 0, false),
                    new Wander(0.1),
                    new Shoot(radius: 25, count: 6, projectileIndex: 1, shootAngle: 25, coolDown: 1000, predictive: 0.3),
                    new Spawn("Mini Mushroom", 100, 0.05, 3000, true),
                    new HpLessTransition(0.50, "Spawn Minions")
                    ),
                new State("Spawn Minions",
                    new Charge(speed: 2, range: 12, coolDown: 1200),
                    new Shoot(radius: 12, count: 12, projectileIndex: 0, shootAngle: 45, coolDownOffset: 900, coolDown: 1000),
                    new Shoot(radius: 12, count: 2, projectileIndex: 1, shootAngle: 16, coolDownOffset: 900, coolDown: 1000, predictive: 1.5),
                    new HpLessTransition(.25, "Rage")
                    ),
                new State("Rage",
                    new Wander(0.6),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Flash(0xFF0000, 0.10, 4),
                    new Spawn("Mini Mushroom", 5, 0.01, 4000, true),
                    new Spawn("Mini Mushroom 2", 5, 0.01, 8000, true),
                    new Shoot(20, 5, projectileIndex: 1, shootAngle: 15, predictive: 0.9, coolDown: 800),
                    new Shoot(20, 1, projectileIndex: 0, fixedAngle: 0, predictive: 1.1, coolDown: 1000),
                    //new Shoot(20, 8, shootAngle: 45, projectileIndex: 2, fixedAngle: 25, coolDown: 1000, coolDownOffset: 1500),
                    new TransformOnDeath("Mini Mushroom", min: 3, max: 5)
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
                new TierLoot(14, ItemType.Armor, 0.01),
                new TierLoot(13, ItemType.Armor, 0.03),
                new TierLoot(14, ItemType.Weapon, 0.01),
                new TierLoot(13, ItemType.Weapon, 0.03),
                new TierLoot(6, ItemType.Ability, 0.01),
                new TierLoot(5, ItemType.Ability, 0.03),
                new TierLoot(5, ItemType.Ring, 0.03),
                new TierLoot(6, ItemType.Ring, 0.01),
                new ItemLoot("Magical Mushroom Staff", 0.01),
                new ItemLoot("Mushroom Loop", 0.01),

                new ItemLoot("Magic Dust", 0.5)
                )
            )
        .Init("Mushroom Bomb",
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
                    new Decay(100)
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
                    new Decay(100)
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
                    new Decay(100)
                    )
                )
            )
        ;
    }
}
