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
                    new Shoot(20, 10, projectileIndex: 3, coolDown: 1000),
                    new Shoot(20, 2, projectileIndex: 2, shootAngle: 45, coolDown: 600),
                    new Shoot(20, 3, projectileIndex: 0, shootAngle: 72, coolDown: 800),
                    new Shoot(20, 5, projectileIndex: 4, shootAngle: 30, coolDown: 1200)
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

