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
                new ConditionalEffect(ConditionEffectIndex.ParalyzeImmune),
                new ConditionalEffect(ConditionEffectIndex.DazedImmune),
                new ConditionalEffect(ConditionEffectIndex.PetrifyImmune),
                new ConditionalEffect(ConditionEffectIndex.StasisImmune),
                new State("Check Player",
                    new PlayerWithinTransition(15, "Start")
                    ),
                new State("Start",
                    new Taunt("Hello Warrior!, want some of me to enjoy a good tour?"),
                    new TimedTransition(3000, "Start To Shoot")
                    ),
                new State("Start To Shoot",
                    new Taunt("Well, I have some extra shrooms!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Shoot(radius: 25, count: 20, projectileIndex: 0, shootAngle: 20, coolDown: 1500, predictive: 0.1),
                    new HpLessTransition(0.75, "Armored Shoot")
                    ),
                new State("Armored Shoot",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new SetAltTexture(1, 1, 0, false),
                    new Wander(0.1),
                    new Shoot(radius: 25, count: 6, projectileIndex: 1, shootAngle: 25, coolDown: 1000, predictive: 0.3),
                    new Spawn("Mini Mushroom", 100, 0.05, 3000, true),
                    new HpLessTransition(0.50, "Spawn Minions")
                    ),
                new State("Spawn Minions",
                    new Taunt("Well, you're getting so far, did you think it was true what i said before?"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Charge(speed: 5, range: 12, coolDown: 3000),
                    new Shoot(radius: 25, count: 12, projectileIndex: 0, shootAngle: 45, coolDownOffset: 900, coolDown: 1000),
                    new Shoot(radius: 25, count: 2, projectileIndex: 1, shootAngle: 16, coolDownOffset: 900, coolDown: 1000, predictive: 1.5),
                    new HpLessTransition(.25, "Rage")
                    ),
                new State("Rage",
                    new Taunt("No! You can't get me!"),
                    new Wander(1.6),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Flash(0xFF0000, 0.10, 4),
                    new Spawn("Mini Mushroom", 5, 0.01, 4000, true),
                    new Spawn("Mini Mushroom 2", 5, 0.01, 8000, true),
                    new Shoot(20, 5, projectileIndex: 1, shootAngle: 15, predictive: 0.9, coolDown: 800),
                    new Shoot(20, 1, projectileIndex: 0, fixedAngle: 0, predictive: 1.1, coolDown: 1000),
                    //new Shoot(20, 8, shootAngle: 45, projectileIndex: 2, fixedAngle: 25, coolDown: 1000, coolDownOffset: 1500),
                    new TransformOnDeath("Mini Mushroom", min: 7, max: 11)
                    )
                ),
            new Threshold(0.001,
                new ItemLoot("Potion of Life", 0.5),
                new ItemLoot("Potion of Mana", 0.5),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Speed", 1),
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
        .Init("Mini Mushroom",
            new State(
                //new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new State("Start Follow Player",
                    new Follow(2.5, 20, 0, 1100),
                    new TimedTransition(1100, "Explotes")
                    ),
                new State("Explotes",
                    new Flash(0xFF0000, 0.5, 5),
                    new Shoot(20, count: 8, shootAngle: 45, projectileIndex: 0, coolDownOffset: 0, coolDown: 10000),
                    new Decay(100)
                    )
                )
            )
        .Init("Mini Mushroom 2",
            new State(
                //new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new State("Start Follow Player",
                    new Follow(2.5, 20, 0, 1100),
                    new TimedTransition(1100, "Explotes")
                    ),
                new State("Explotes",
                    new Flash(0x353535, 0.25, 5),
                    new Shoot(20, count: 8, shootAngle: 45, projectileIndex: 0, coolDownOffset: 0, coolDown: 10000),
                    new Decay(100)
                    )
                )
            )
        ;
    }
}
