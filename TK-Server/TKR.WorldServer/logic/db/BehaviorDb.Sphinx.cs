using TKR.Shared.resources;
using TKR.WorldServer.logic.loot;
using TKR.WorldServer.logic.behaviors;
using TKR.WorldServer.logic.transitions;

namespace TKR.WorldServer.logic
{
    partial class BehaviorDb
    {
        private _ Sphinx = () => Behav()
        .Init("Grand Sphinx",
            new State(
                new ScaleHP2(20),
                new DropPortalOnDeath("Tomb of the Ancients Portal", 1),
                new State("Spawned",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Reproduce("Horrid Reaper", 30, 4, coolDown: 100),
                    new TimedTransition(500, "Attack1")
                    ),
                new State("Attack1",
                    new Prioritize(
                        new Wander(0.5)
                        ),
                    new Shoot(12, count: 1, coolDown: 600),
                    new Shoot(12, count: 3, shootAngle: 10, coolDown: 800),
                    new Shoot(12, count: 1, shootAngle: 130, coolDown: 700),
                    new Shoot(12, count: 1, shootAngle: 230, coolDown: 800),
                    new TimedTransition(6000, "TransAttack2")
                    ),
                new State("TransAttack2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Wander(0.5),
                    new Flash(0x00FF0C, .25, 8),
                    new Taunt("You hide behind rocks like cowards but you cannot hide from this!"),
                    new TimedTransition(2000, "Attack2")
                    ),
                new State("Attack2",
                    new Prioritize(
                        new Wander(0.5)
                        ),
                    new Shoot(12, count: 8, shootAngle: 10, fixedAngle: 0, rotateAngle: 70, coolDown: 1200,
                        projectileIndex: 1),
                    new Shoot(12, count: 8, shootAngle: 10, fixedAngle: 180, rotateAngle: 70, coolDown: 1000,
                        projectileIndex: 1),
                    new TimedTransition(6200, "TransAttack3")
                    ),
                new State("TransAttack3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Wander(0.5),
                    new Flash(0x00FF0C, .25, 8),
                    new TimedTransition(2000, "Attack3")
                    ),
                new State("Attack3",
                    new Prioritize(
                        new Wander(0.5)
                        ),
                    new Shoot(20, count: 9, fixedAngle: 360 / 9, projectileIndex: 2, coolDown: 1200),
                    new TimedTransition(6000, "TransAttack1"),
                    new State("Shoot1",
                        new Shoot(20, count: 2, shootAngle: 4, projectileIndex: 2, coolDown: 700),
                        new TimedRandomTransition(1000, false,
                            "Shoot1",
                            "Shoot2"
                            )
                        ),
                    new State("Shoot2",
                        new Shoot(20, count: 8, shootAngle: 5, projectileIndex: 2, coolDown: 800),
                        new TimedRandomTransition(1000, false,
                            "Shoot1",
                            "Shoot2"
                            )
                        )
                    ),
                new State("TransAttack1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Wander(0.5),
                    new Flash(0x00FF0C, .25, 8),
                    new TimedTransition(2000, "Attack1"),
                    new HpLessTransition(0.15, "Order")
                    ),
                new State("Order",
                    new Wander(0.5),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Order(30, "Horrid Reaper", "Die"),
                    new TimedTransition(1900, "Attack1")
                    )
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.01,
                new ItemLoot("Talisman Fragment", 0.009),
                new ItemLoot("Dojigiri", 0.0014, threshold: 0.005)
                ),
            new Threshold(0.001,
                new ItemLoot("Potion of Vitality", 1),
                new ItemLoot("Potion of Wisdom", 1),
                new ItemLoot("Potion of Vitality", 1),
                new ItemLoot("Glowing Talisman", 0.005),
                new ItemLoot("Potion of Wisdom", 1),
                new TierLoot(10, ItemType.Weapon, .12),
                new TierLoot(11, ItemType.Weapon, .09),
                new TierLoot(4, ItemType.Ability, .07),
                new TierLoot(5, ItemType.Ability, .03),
                new TierLoot(11, ItemType.Armor, .09),
                new TierLoot(12, ItemType.Armor, .06),
                new TierLoot(5, ItemType.Ring, .03),

                new ItemLoot("Magic Dust", 0.5)
                )
            )
        .Init("Horrid Reaper",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new State("Move",
                    new Prioritize(
                        new StayCloseToSpawn(3, 10),
                        new Wander(0.5)
                        ),
                    new EntityNotExistsTransition("Grand Sphinx", 50, "Die"), //Just to be sure
                    new TimedRandomTransition(2000, true, "Attack")
                    ),
                new State("Attack",
                    new Shoot(8, count: 6, fixedAngle: 360 / 6, coolDown: 700),
                    new PlayerWithinTransition(2, "Follow"),
                    new TimedRandomTransition(5000, true, "Move")
                    ),
                new State("Follow",
                    new Prioritize(
                        new Follow(0.7, 10, 3)
                        ),
                    new Shoot(7, count: 1, coolDown: 700),
                    new TimedRandomTransition(5000, true, "Move")
                    ),
                new State("Die",
                    new Taunt(0.99, "OOaoaoAaAoaAAOOAoaaoooaa!!!"),
                    new Decay(1000)
                    )
                )
            )
        ;
    }
}
