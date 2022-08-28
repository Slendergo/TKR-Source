using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ TikiTiki = () => Behav()
        .Init("Tiki Tiki",
            new State(
                new ScaleHP2(35),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new PlayerWithinTransition(25, "Prepare")
                    ),
                new State("Prepare",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new TimedTransition(1500, "attack")
                    ),
                new State("attack",
                    new Taunt("Tiki Tiki."),

                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 0, fixedAngle: 0, coolDown: 400),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 10, fixedAngle: 10, coolDown: 400),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 20, fixedAngle: 20, coolDown: 400),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -10, fixedAngle: -20, coolDown: 400),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -20, fixedAngle: -10, coolDown: 400),

                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 180, fixedAngle: 180, coolDown: 600),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 170, fixedAngle: 170, coolDown: 600),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 160, fixedAngle: 160, coolDown: 600),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 190, fixedAngle: 190, coolDown: 600),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 200, fixedAngle: 200, coolDown: 600),

                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 90, fixedAngle: 90, coolDown: 800),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 80, fixedAngle: 80, coolDown: 800),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 70, fixedAngle: 70, coolDown: 800),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 100, fixedAngle: 100, coolDown: 800),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 110, fixedAngle: 110, coolDown: 800),

                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -90, fixedAngle: -90, coolDown: 1000),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -80, fixedAngle: -80, coolDown: 1000),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -70, fixedAngle: -70, coolDown: 1000),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 100, fixedAngle: -100, coolDown: 1000),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 110, fixedAngle: -110, coolDown: 1000),

                    new Shoot(15, 36, shootAngle: 10, projectileIndex: 3, angleOffset: 10, coolDown: 10000, coolDownOffset: 10000), //aura stun
                    new Shoot(15, 36, shootAngle: 10, projectileIndex: 1, angleOffset: 10, predictive: 0, coolDown: 10000, coolDownOffset: 10000), //aura molesta
                    new HpLessTransition(0.50, "attack2")
                    ),
                new State("attack2",
                    new Taunt("Tiki Tiki Tiki..."),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 0, fixedAngle: 0, coolDown: 300),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 10, fixedAngle: 10, coolDown: 300),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 20, fixedAngle: 20, coolDown: 300),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -10, fixedAngle: -20, coolDown: 300),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -20, fixedAngle: -10, coolDown: 300),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 40, fixedAngle: 40, coolDown: 300),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -40, fixedAngle: -40, coolDown: 300),

                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 180, fixedAngle: 180, coolDown: 500),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 170, fixedAngle: 170, coolDown: 500),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 160, fixedAngle: 160, coolDown: 500),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 190, fixedAngle: 190, coolDown: 500),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 200, fixedAngle: 200, coolDown: 500),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 140, fixedAngle: 140, coolDown: 300),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 220, fixedAngle: 220, coolDown: 300),

                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 90, fixedAngle: 90, coolDown: 700),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 80, fixedAngle: 80, coolDown: 700),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 70, fixedAngle: 70, coolDown: 700),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 100, fixedAngle: 100, coolDown: 700),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 110, fixedAngle: 110, coolDown: 700),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 50, fixedAngle: 50, coolDown: 300),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 130, fixedAngle: 130, coolDown: 300),

                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -90, fixedAngle: -90, coolDown: 900),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -80, fixedAngle: -80, coolDown: 900),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -70, fixedAngle: -70, coolDown: 900),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -100, fixedAngle: -100, coolDown: 900),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -110, fixedAngle: -110, coolDown: 900),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -50, fixedAngle: -50, coolDown: 300),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -130, fixedAngle: -130, coolDown: 300),

                    new Shoot(15, 36, shootAngle: 10, projectileIndex: 3, angleOffset: 10, coolDown: 9000, coolDownOffset: 9000), //aura stun
                    new Shoot(15, 36, shootAngle: 10, projectileIndex: 1, angleOffset: 10, predictive: 0, coolDown: 9000, coolDownOffset: 9000), //aura molesta
                    new HpLessTransition(0.15, "attackrage")
                    ),
                new State("attackrage",
                    new ChangeSize(100, 500),
                    new Taunt("TIKI TIKIIII!!!!!"),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 0, fixedAngle: 0, coolDown: 200),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 10, fixedAngle: 10, coolDown: 200),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 20, fixedAngle: 20, coolDown: 200),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -10, fixedAngle: -20, coolDown: 200),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -20, fixedAngle: -10, coolDown: 200),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 40, fixedAngle: 40, coolDown: 200),
                    new Shoot(15, projectileIndex: 2, count: 1, shootAngle: 50, fixedAngle: 50, coolDown: 1000),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -40, fixedAngle: -40, coolDown: 200),
                    new Shoot(15, projectileIndex: 2, count: 1, shootAngle: -50, fixedAngle: -50, coolDown: 1000),

                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 180, fixedAngle: 180, coolDown: 400),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 170, fixedAngle: 170, coolDown: 400),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 160, fixedAngle: 160, coolDown: 400),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 190, fixedAngle: 190, coolDown: 400),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 200, fixedAngle: 200, coolDown: 400),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 140, fixedAngle: 140, coolDown: 400),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 220, fixedAngle: 220, coolDown: 400),
                    new Shoot(15, projectileIndex: 2, count: 1, shootAngle: 150, fixedAngle: 150, coolDown: 1000),
                    new Shoot(15, projectileIndex: 2, count: 1, shootAngle: 230, fixedAngle: 230, coolDown: 1000),

                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 90, fixedAngle: 90, coolDown: 600),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 80, fixedAngle: 80, coolDown: 600),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 70, fixedAngle: 70, coolDown: 600),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 100, fixedAngle: 100, coolDown: 600),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 110, fixedAngle: 110, coolDown: 600),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 50, fixedAngle: 50, coolDown: 600),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: 130, fixedAngle: 130, coolDown: 600),
                    new Shoot(15, projectileIndex: 2, count: 1, shootAngle: 60, fixedAngle: 60, coolDown: 1000),
                    new Shoot(15, projectileIndex: 2, count: 1, shootAngle: 140, fixedAngle: 140, coolDown: 1000),

                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -90, fixedAngle: -90, coolDown: 800),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -80, fixedAngle: -80, coolDown: 800),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -70, fixedAngle: -70, coolDown: 800),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -100, fixedAngle: -100, coolDown: 800),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -110, fixedAngle: -110, coolDown: 800),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -50, fixedAngle: -50, coolDown: 800),
                    new Shoot(15, projectileIndex: 0, count: 1, shootAngle: -130, fixedAngle: -130, coolDown: 800),
                    new Shoot(15, projectileIndex: 2, count: 1, shootAngle: -60, fixedAngle: -60, coolDown: 1000),
                    new Shoot(15, projectileIndex: 2, count: 1, shootAngle: -140, fixedAngle: -140, coolDown: 1000),

                    new Shoot(15, 36, shootAngle: 10, projectileIndex: 3, angleOffset: 10, coolDown: 8000, coolDownOffset: 8000), //aura stun
                    new Shoot(15, 36, shootAngle: 10, projectileIndex: 1, angleOffset: 10, predictive: 0, coolDown: 8000, coolDownOffset: 8000), //aura molesta
                    new HpLessTransition(0.05, "dead")
                    ),
                new State("dead",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("Todo"),
                    new Suicide()
                    )
                ),
            new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Talisman Fragment", 0.0005),
                new ItemLoot("Tiki's Breastplate", 0.000014)
                ),
            new Threshold(0.001,
                new ItemLoot("Potion of Defense", 1),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Speed", 1),
                new ItemLoot("Potion of Defense", 1),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Speed", 1)
                ),
            new Threshold(0.005,
                new TierLoot(11, ItemType.Armor, 0.09),
                new TierLoot(10, ItemType.Armor, 0.12),
                new TierLoot(11, ItemType.Weapon, 0.09),
                new TierLoot(10, ItemType.Weapon, 0.12),
                new TierLoot(4, ItemType.Ring, 0.07),
                new TierLoot(4, ItemType.Ability, 0.07),
                new ItemLoot("Hunter's Cloak", 0.0015),
                new ItemLoot("Tribesmen's Shank", 0.001),
                new ItemLoot("Leaf Staff", 0.001),

                new ItemLoot("Magic Dust", 0.5)
                )
            );
    }
}
