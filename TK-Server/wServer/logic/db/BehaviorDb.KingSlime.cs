using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ KingSlime = () => Behav()
        .Init("King Slime",
            new State(
                new ConditionalEffect(ConditionEffectIndex.ParalyzeImmune),
                new ScaleHP2(20),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(25, "Prepare")
                    ),
                new State("Prepare",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xBB0000, 0.5, 3),
                    new TimedTransition(1500, "attack")
                    ),
                new State("attack",
                    new Prioritize(
                        new Follow(0.4, 10, 1, 30, 1),
                        new Wander(0.4)
                        ),
                    new Taunt("U cant kill me, im the King."),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, predictive: 0.2, coolDown: 700),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: 10, predictive: 0.2, coolDown: 600),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: 20, predictive: 0.2, coolDown: 700), //Escopeta
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: -10, predictive: 0.2, coolDown: 500),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: -20, predictive: 0.2, coolDown: 400),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, predictive: 0.6, coolDown: 700),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: 30, predictive: 0.6, coolDown: 600),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: 40, predictive: 0.6, coolDown: 700), //Escopeta
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: -30, predictive: 0.6, coolDown: 500),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: -40, predictive: 0.6, coolDown: 400),
                    new HpLessTransition(0.50, "attack2")
                    ),
                new State("attack2",
                    new Prioritize(
                        new Follow(0.4, 10, 1, 30, 1),
                        new Wander(0.6)
                        ),
                    new Taunt("Do not make me angry."),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, predictive: 0.2, coolDown: 700),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: 10, predictive: 0.2, coolDown: 600),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: 20, predictive: 0.2, coolDown: 700), //Escopeta
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: -10, predictive: 0.2, coolDown: 500),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: -20, predictive: 0.2, coolDown: 400),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, predictive: 0.6, coolDown: 700),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: 30, predictive: 0.6, coolDown: 600),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: 40, predictive: 0.6, coolDown: 700), //Escopeta
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: -30, predictive: 0.6, coolDown: 500),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: -40, predictive: 0.6, coolDown: 400),
                    new Shoot(15, 36, shootAngle: 10, projectileIndex: 1, angleOffset: 10, predictive: 0, coolDown: 6000), //aura slow
                    new HpLessTransition(0.15, "attackrage")
                    ),
                new State("attackrage",
                    new Prioritize(
                        new Follow(0.5, 10, 1, 30, 1),
                        new Wander(0.7)
                        ),
                    new Taunt("AAAAAAAAAAAAAAAAGGGGGGGGGH!"),
                    new Flash(0xBB0000, 0.5, 10000),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, predictive: 0.2, coolDown: 700),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: 10, predictive: 0.2, coolDown: 600),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: 20, predictive: 0.2, coolDown: 700), //Escopeta
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: -10, predictive: 0.2, coolDown: 500),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: -20, predictive: 0.2, coolDown: 400),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, predictive: 0.6, coolDown: 700),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: 30, predictive: 0.6, coolDown: 600),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: 40, predictive: 0.6, coolDown: 700), //Escopeta
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: -30, predictive: 0.6, coolDown: 500),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 0, angleOffset: -40, predictive: 0.6, coolDown: 400),
                    new Shoot(15, 36, shootAngle: 10, projectileIndex: 1, angleOffset: 10, predictive: 0, coolDown: 5000), //aura slow
                    new Shoot(15, 4, shootAngle: 90, projectileIndex: 2, angleOffset: 90, predictive: 1, coolDown: 4000)
                    )
                ),
            new Threshold(0.001,
                new ItemLoot("Potion of Defense", 1),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Speed", 1),
                new ItemLoot("Potion of Defense", 0.5),
                new ItemLoot("Potion of Attack", 0.5),
                new ItemLoot("Potion of Speed", 0.5)
                ),
            new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.001,
                new TierLoot(10, ItemType.Armor, 0.12),
                new TierLoot(11, ItemType.Armor, 0.09),
                new TierLoot(10, ItemType.Weapon, 0.12),
                new TierLoot(11, ItemType.Weapon, 0.09),
                new TierLoot(5, ItemType.Ring, 0.03),
                new TierLoot(3, ItemType.Ability, 0.12),
                new TierLoot(4, ItemType.Ability, 0.07),
                new ItemLoot("Sticky Slime Armor", 0.01),

                new ItemLoot("Magic Dust", 0.5)
                ),
            new Threshold(0.04,
                new ItemLoot("Slime Slayer", 0.001),
                new ItemLoot("Strange Poison", 0.001, threshold: 0.05)
                )
            );
    }
}
