using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ GardenofHorror = () => Behav()
        .Init("Plantera",
            new State(
                new ScaleHP2(15),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(10, "taunt1")
                    ),
                new State("taunt1",
                    new Taunt("Fluh? Why have you come here?"),
                    new TimedTransition(3000, "taunt2")
                    ),
                new State("taunt2",
                    new Taunt("To disturb the peace of my garden no doubt!"),
                    new TimedTransition(3000, "taunt3")
                    ),
                new State("taunt3",
                    new Taunt("WE will never let that happen!"),
                    new TimedTransition(3000, "prepare")
                    ),
                new State("prepare",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new SetAltTexture(2),
                    new TimedTransition(2000, "wander")
                    ),
                new State("wander",
                    new Wander(0.8),
                    new Shoot(5, 1, projectileIndex: 0, shootAngle: 10, predictive: .8, coolDown: 1200),
                    new Shoot(5, 4, projectileIndex: 3, fixedAngle: 90, shootAngle: 10, coolDown: 3000),
                    new Shoot(5, 4, projectileIndex: 3, fixedAngle: 270, shootAngle: 10, coolDown: 3000),
                    new Shoot(5, 4, projectileIndex: 3, fixedAngle: 0, shootAngle: 0, coolDown: 4000),
                    new Shoot(5, 4, projectileIndex: 3, fixedAngle: 180, shootAngle: 0, coolDown: 4000),
                    new TimedTransition(8000, "charge")
                    ),
                new State("charge",
                    new Flash(0xFF0000, 0.5, 2),
                    new TimedTransition(500, "charge2")
                    ),
                new State("charge2",
                    new Charge(speed: 9, range: 15, coolDown: 3000), //wanna offset this so it happens 1 second after the reproduce but its whatever
                    new Shoot(20, 12, shootAngle: 20, projectileIndex: 0, predictive: 1, coolDown: 3000),
                    new Shoot(20, 1, shootAngle: 0, projectileIndex: 3, predictive: 0, coolDown: 500),
                    new HpLessTransition(0.5, "return")
                    ),
                new State("return",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Taunt("Little ones... papa is hurt!"),
                    new ReturnToSpawn(1),
                    new TimedTransition(3000, "shell")
                    ),
                new State("shell",
                    new SetAltTexture(0),
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new ConditionalEffect(ConditionEffectIndex.Armored, true),

                    new Shoot(20, 5, shootAngle: 15, projectileIndex: 0, predictive: 0, coolDown: 1000),

                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 0,  coolDown: 2000),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 15, coolDown: 2200),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 30, coolDown: 2400),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 45, coolDown: 2600),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 60, coolDown: 2800),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 90, coolDown: 3000),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 90, coolDown: 3200),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 105, coolDown: 3400),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 120, coolDown: 3600),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 135, coolDown: 3800),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 150, coolDown: 4000),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 165, coolDown: 4200),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 180, coolDown: 4400),

                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 180, coolDown: 2000),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 195, coolDown: 2200),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 210, coolDown: 2400),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 225, coolDown: 2600),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 240, coolDown: 2800),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 255, coolDown: 3000),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 270, coolDown: 3200),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 285, coolDown: 3400),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 300, coolDown: 3600),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 315, coolDown: 3800),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 330, coolDown: 4000),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 345, coolDown: 4200),
                    new Shoot(5, 1, projectileIndex: 3, fixedAngle: 360, coolDown: 4400)
                    )
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.01,
                new TierLoot(12, ItemType.Weapon, 0.2),
                new TierLoot(13, ItemType.Weapon, 0.15),
                new TierLoot(12, ItemType.Armor, 0.2),
                new TierLoot(13, ItemType.Armor, 0.15),
                new TierLoot(6, ItemType.Ring, 0.07),
                new TierLoot(6, ItemType.Ability, 0.07),
                new ItemLoot("Blossom Bow", 0.01),
                new ItemLoot("Rose Buckler", 0.01),
                new ItemLoot("Lotus Flower", 0.01),
                new ItemLoot("Potion of Speed", 1),
                new ItemLoot("Potion of Life", 0.15),
                new ItemLoot("Potion of Mana", 0.15),
                new ItemLoot("Potion of Wisdom", 1)
                )
            )
        .Init("Piranha Plant",
            new State(
                new State("attack",
                    new Prioritize(
                    new Orbit(2, 6, 20, "Plantera", orbitClockwise: true)),
                    new Shoot(10, 3, projectileIndex: 0, shootAngle: 10, predictive: .8, coolDown: 1200)
                    )
                )
            )
        .Init("Raged Piranha Plant",
            new State(
                new Wander(0.35),
                new ScaleHP2(15),
                new State("attack",
                new Taunt("FLUHH"),
                new Chase(6),
                new Shoot(12, 2, projectileIndex: 0, shootAngle: 10, predictive: 1.2, coolDown: 800)
                    )
                ));
    }
}

