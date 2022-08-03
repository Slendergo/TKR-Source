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
                    new PlayerWithinTransition(20, "prepare")
                    ),
                new State("prepare",
                    new SetAltTexture(2),
                    new ChangeSize(1, 130),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 0, fixedAngle: 0, coolDown: 4600, coolDownOffset: 0),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -2, fixedAngle: 0, coolDown: 4600, coolDownOffset: 100),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -4, fixedAngle: 0, coolDown: 4600, coolDownOffset: 200),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -6, fixedAngle: 0, coolDown: 4600, coolDownOffset: 300),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -8, fixedAngle: 0, coolDown: 4600, coolDownOffset: 400),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -10, fixedAngle: 0, coolDown: 4600, coolDownOffset: 500),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -12, fixedAngle: 0, coolDown: 4600, coolDownOffset: 600),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -14, fixedAngle: 0, coolDown: 4600, coolDownOffset: 700),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -16, fixedAngle: 0, coolDown: 4600, coolDownOffset: 800),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -18, fixedAngle: 0, coolDown: 4600, coolDownOffset: 900),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -20, fixedAngle: 0, coolDown: 4600, coolDownOffset: 1000),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -22, fixedAngle: 0, coolDown: 4600, coolDownOffset: 1200),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -24, fixedAngle: 0, coolDown: 4600, coolDownOffset: 1300),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -26, fixedAngle: 0, coolDown: 4600, coolDownOffset: 1400),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -28, fixedAngle: 0, coolDown: 4600, coolDownOffset: 1500),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -30, fixedAngle: 0, coolDown: 4600, coolDownOffset: 1600),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -32, fixedAngle: 0, coolDown: 4600, coolDownOffset: 1700),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -34, fixedAngle: 0, coolDown: 4600, coolDownOffset: 1800),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -36, fixedAngle: 0, coolDown: 4600, coolDownOffset: 1900),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -38, fixedAngle: 0, coolDown: 4600, coolDownOffset: 2000),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -40, fixedAngle: 0, coolDown: 4600, coolDownOffset: 2100),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -42, fixedAngle: 0, coolDown: 4600, coolDownOffset: 2200),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -44, fixedAngle: 0, coolDown: 4600, coolDownOffset: 2300),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -46, fixedAngle: 0, coolDown: 4600, coolDownOffset: 2400),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -48, fixedAngle: 0, coolDown: 4600, coolDownOffset: 2500),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -50, fixedAngle: 0, coolDown: 4600, coolDownOffset: 2600),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -52, fixedAngle: 0, coolDown: 4600, coolDownOffset: 2700),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -54, fixedAngle: 0, coolDown: 4600, coolDownOffset: 2800),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -56, fixedAngle: 0, coolDown: 4600, coolDownOffset: 2900),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -58, fixedAngle: 0, coolDown: 4600, coolDownOffset: 3000),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -60, fixedAngle: 0, coolDown: 4600, coolDownOffset: 3100),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -62, fixedAngle: 0, coolDown: 4600, coolDownOffset: 3200),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -64, fixedAngle: 0, coolDown: 4600, coolDownOffset: 3300),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -66, fixedAngle: 0, coolDown: 4600, coolDownOffset: 3400),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -68, fixedAngle: 0, coolDown: 4600, coolDownOffset: 3500),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -70, fixedAngle: 0, coolDown: 4600, coolDownOffset: 3600),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -72, fixedAngle: 0, coolDown: 4600, coolDownOffset: 3700),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -74, fixedAngle: 0, coolDown: 4600, coolDownOffset: 3800),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -76, fixedAngle: 0, coolDown: 4600, coolDownOffset: 3900),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -78, fixedAngle: 0, coolDown: 4600, coolDownOffset: 4000),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -80, fixedAngle: 0, coolDown: 4600, coolDownOffset: 4100),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -82, fixedAngle: 0, coolDown: 4600, coolDownOffset: 4200),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -84, fixedAngle: 0, coolDown: 4600, coolDownOffset: 4300),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -86, fixedAngle: 0, coolDown: 4600, coolDownOffset: 4400),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -88, fixedAngle: 0, coolDown: 4600, coolDownOffset: 4500),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: -90, fixedAngle: 0, coolDown: 4600, coolDownOffset: 4600),

                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 0, fixedAngle: 180, coolDown: 4600, coolDownOffset: 0),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 2, fixedAngle: 180, coolDown: 4600, coolDownOffset: 100),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 4, fixedAngle: 180, coolDown: 4600, coolDownOffset: 200),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 6, fixedAngle: 180, coolDown: 4600, coolDownOffset: 300),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 8, fixedAngle: 180, coolDown: 4600, coolDownOffset: 400),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 10, fixedAngle: 180, coolDown: 4600, coolDownOffset: 500),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 12, fixedAngle: 180, coolDown: 4600, coolDownOffset: 600),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 14, fixedAngle: 180, coolDown: 4600, coolDownOffset: 700),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 16, fixedAngle: 180, coolDown: 4600, coolDownOffset: 800),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 18, fixedAngle: 180, coolDown: 4600, coolDownOffset: 900),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 20, fixedAngle: 180, coolDown: 4600, coolDownOffset: 1000),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 22, fixedAngle: 180, coolDown: 4600, coolDownOffset: 1200),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 24, fixedAngle: 180, coolDown: 4600, coolDownOffset: 1300),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 26, fixedAngle: 180, coolDown: 4600, coolDownOffset: 1400),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 28, fixedAngle: 180, coolDown: 4600, coolDownOffset: 1500),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 30, fixedAngle: 180, coolDown: 4600, coolDownOffset: 1600),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 32, fixedAngle: 180, coolDown: 4600, coolDownOffset: 1700),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 34, fixedAngle: 180, coolDown: 4600, coolDownOffset: 1800),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 36, fixedAngle: 180, coolDown: 4600, coolDownOffset: 1900),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 38, fixedAngle: 180, coolDown: 4600, coolDownOffset: 2000),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 40, fixedAngle: 180, coolDown: 4600, coolDownOffset: 2100),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 42, fixedAngle: 180, coolDown: 4600, coolDownOffset: 2200),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 44, fixedAngle: 180, coolDown: 4600, coolDownOffset: 2300),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 46, fixedAngle: 180, coolDown: 4600, coolDownOffset: 2400),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 48, fixedAngle: 180, coolDown: 4600, coolDownOffset: 2500),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 50, fixedAngle: 180, coolDown: 4600, coolDownOffset: 2600),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 52, fixedAngle: 180, coolDown: 4600, coolDownOffset: 2700),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 54, fixedAngle: 180, coolDown: 4600, coolDownOffset: 2800),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 56, fixedAngle: 180, coolDown: 4600, coolDownOffset: 2900),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 58, fixedAngle: 180, coolDown: 4600, coolDownOffset: 3000),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 60, fixedAngle: 180, coolDown: 4600, coolDownOffset: 3100),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 62, fixedAngle: 180, coolDown: 4600, coolDownOffset: 3200),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 64, fixedAngle: 180, coolDown: 4600, coolDownOffset: 3300),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 66, fixedAngle: 180, coolDown: 4600, coolDownOffset: 3400),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 68, fixedAngle: 180, coolDown: 4600, coolDownOffset: 3500),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 70, fixedAngle: 180, coolDown: 4600, coolDownOffset: 3600),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 72, fixedAngle: 180, coolDown: 4600, coolDownOffset: 3700),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 74, fixedAngle: 180, coolDown: 4600, coolDownOffset: 3800),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 76, fixedAngle: 180, coolDown: 4600, coolDownOffset: 3900),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 78, fixedAngle: 180, coolDown: 4600, coolDownOffset: 4000),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 80, fixedAngle: 180, coolDown: 4600, coolDownOffset: 4100),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 82, fixedAngle: 180, coolDown: 4600, coolDownOffset: 4200),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 84, fixedAngle: 180, coolDown: 4600, coolDownOffset: 4300),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 86, fixedAngle: 180, coolDown: 4600, coolDownOffset: 4400),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 88, fixedAngle: 180, coolDown: 4600, coolDownOffset: 4500),
                    new Shoot(20, 2, projectileIndex: 3, shootAngle: 90, fixedAngle: 180, coolDown: 4600, coolDownOffset: 4600)
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
                new ScaleHP2(15),
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

