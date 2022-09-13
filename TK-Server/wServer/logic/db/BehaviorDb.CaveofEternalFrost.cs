using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ CaveofEternalFrost = () => Behav()
        .Init("Yeti Check",
            new State(
                new State("Check",
                    new EntityNotExistsTransition("Abominable Snowman", 200, "Remove")
                    ),
                new State("Remove",
                    new OpenGate("Ice Cave Wall GATE", 7)
                    )
                )
            )
        .Init("Ice Wall Spawner",
            new State(
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("Fire",
                    new Spawn("Ice Cave Wall", 1, 1, coolDown: 999999),
                    new EntityNotExistsTransition("Cursed Snowman Switch", 30, "Kill Wall")
                    ),
                new State("Kill Wall",
                    new RemoveEntity(10, "Ice Cave Wall"),
                    new TimedTransition(100, "dead")
                    ),
                new State("dead",
                    new Suicide()
                    )
                )
            )
        .Init("Ice Wall Spawner 1",
            new State(
                new State("idle",
                    new EntityNotExistsTransition("Cursed Snowman Switch", 30, "dead"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("dead",
                    new Suicide()
                    )
                )
            )
        .Init("Snowman Check",
            new State(
                new State("Check",
                    new EntityNotExistsTransition("Corrupt Snowman Switch", 200, "Remove")
                    ),
                new State("Remove",
                    new OpenGate("Ice Cave Wall S", 7)
                    )
                )
            )
        .Init("Snowman Check 2",
            new State(
                new State("Check",
                    new EntityNotExistsTransition("Cursed Snowman Switch", 200, "Remove")
                    ),
                new State("Remove",
                    new OpenGate("Ice Cave Wall S2", 7)
                    )
                )
            )
        .Init("Yeti Chest Check",
            new State(
                new State("Check",
                    new EntityNotExistsTransition("Yeti Chest", 200, "Remove")
                    ),
                new State("Remove",
                    new OpenGate("Ice Cave Wall C", 7)
                    )
                )
            )
        .Init("Dungeon Snowy Turret",
            new State(
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new PlayerWithinTransition(10, "Shoot", true)
                    ),
                new State("Shoot",
                    new Shoot(15, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 600, predictive: 1.2)
                    )
                )
            )
        .Init("Snowy Turret",
            new State(
                new State("Shoot",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Shoot(15, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000),
                    new TimedTransition(1200, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
        .Init("Snowy Turret 1",
            new State(
                new State("Shoot",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Shoot(15, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000),
                    new TimedTransition(1200, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
        .Init("Snowy Turret 2",
            new State(
                new State("Shoot",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Shoot(15, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000),
                    new TimedTransition(1200, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
        .Init("Snowy Turret 3",
            new State(
                new State("Shoot",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Shoot(15, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000),
                    new TimedTransition(1200, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
        .Init("Snowy Turret 4",
            new State(
                new State("Shoot",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Shoot(15, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000),
                    new TimedTransition(1200, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
        .Init("Snowy Turret Toss",
            new State(
                new State("Check",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("Shoot",
                    new InvisiToss("Snowy Turret", 3, angle: 0, coolDown: 2800, coolDownOffset: 0),
                    new InvisiToss("Snowy Turret 1", 3, angle: 72, coolDown: 2800, coolDownOffset: 0),
                    new InvisiToss("Snowy Turret 2", 3, angle: 144, coolDown: 2800, coolDownOffset: 0),
                    new InvisiToss("Snowy Turret 3", 3, angle: 216, coolDown: 2800, coolDownOffset: 0),
                    new InvisiToss("Snowy Turret 4", 3, angle: 288, coolDown: 2800, coolDownOffset: 0),

                    new InvisiToss("Snowy Turret", 3, angle: 36, coolDown: 2800, coolDownOffset: 1400),
                    new InvisiToss("Snowy Turret 1", 3, angle: 108, coolDown: 2800, coolDownOffset: 1400),
                    new InvisiToss("Snowy Turret 2", 3, angle: 180, coolDown: 2800, coolDownOffset: 1400),
                    new InvisiToss("Snowy Turret 3", 3, angle: 252, coolDown: 2800, coolDownOffset: 1400),
                    new InvisiToss("Snowy Turret 4", 3, angle: 324, coolDown: 2800, coolDownOffset: 1400),
                    new EntityNotExistsTransition("Primordial Quetzalcoatl", 50, "die")
                    ),
                new State("Shoot1",
                    new InvisiToss("Snowy Turret", 2, angle: 0, coolDown: 99999, coolDownOffset: 0),
                    new InvisiToss("Snowy Turret 1", 2, angle: 72, coolDown: 99999, coolDownOffset: 0)
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
        .Init("Primordial Quetzalcoatl",
            new State(
                new ScaleHP2(35),
                new State("Check",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new PlayerWithinTransition(8, "Remove1")
                    ),
                new State("Remove1",
                    new Spawn("Snowy Turret Toss", coolDown: 99999),
                    new TimedTransition(600, "Remove2")
                    ),
                new State("Remove",
                    new ReturnToSpawn(2, 1),
                    new Shoot(12, 1, projectileIndex: 9, coolDown: 1000),
                    new Shoot(12, 2, projectileIndex: 11, shootAngle: 10, coolDown: 1000),
                    new Shoot(12, 3, projectileIndex: 12, shootAngle: 15, coolDown: 1000),
                    new TimedTransition(1200, "Remove2")
                    ),
                new State("Remove2",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new Wander(0.4),
                    new Charge(10, 8, coolDown: 3000),
                    new Shoot(12, 4, projectileIndex: 2, coolDown: 2000),
                    new Shoot(12, 1, projectileIndex: 9, coolDown: 1000),
                    new Shoot(12, 2, projectileIndex: 11, shootAngle: 10, coolDown: 1000),
                    new Shoot(12, 3, projectileIndex: 12, shootAngle: 15, coolDown: 1000),
                    new OrderOnce(10, "Snowy Turret Toss", "Shoot"),
                    new Taunt("ahahahaHAHAHAHAH"),
                    new TimedTransition(1800, "Ring")
                    ),
                new State("Ring",
                    new StayCloseToSpawn(3, 15),
                    new Shoot(12, 2, projectileIndex: 9, fixedAngle: 0, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(12, 2, projectileIndex: 9, fixedAngle: 10, coolDown: 2000, coolDownOffset: 200),
                    new Shoot(12, 2, projectileIndex: 9, fixedAngle: -10, coolDown: 2000, coolDownOffset: 200),
                    new Shoot(12, 2, projectileIndex: 11, fixedAngle: 20, coolDown: 2000, coolDownOffset: 400),
                    new Shoot(12, 2, projectileIndex: 11, fixedAngle: -20, coolDown: 2000, coolDownOffset: 400),
                    new Shoot(12, 2, projectileIndex: 11, fixedAngle: 30, coolDown: 2000, coolDownOffset: 600),
                    new Shoot(12, 2, projectileIndex: 11, fixedAngle: -30, coolDown: 2000, coolDownOffset: 600),
                    new Shoot(12, 2, projectileIndex: 11, fixedAngle: 40, coolDown: 2000, coolDownOffset: 800),
                    new Shoot(12, 2, projectileIndex: 11, fixedAngle: -40, coolDown: 2000, coolDownOffset: 800),
                    new Shoot(12, 2, projectileIndex: 11, fixedAngle: 50, coolDown: 2000, coolDownOffset: 1000),
                    new Shoot(12, 2, projectileIndex: 11, fixedAngle: -50, coolDown: 2000, coolDownOffset: 1000),
                    new Shoot(12, 2, projectileIndex: 12, fixedAngle: 60, coolDown: 2000, coolDownOffset: 1200),
                    new Shoot(12, 2, projectileIndex: 12, fixedAngle: -60, coolDown: 2000, coolDownOffset: 1200),
                    new Shoot(12, 2, projectileIndex: 12, fixedAngle: 70, coolDown: 2000, coolDownOffset: 1400),
                    new Shoot(12, 2, projectileIndex: 12, fixedAngle: -70, coolDown: 2000, coolDownOffset: 1400),
                    new Shoot(12, 2, projectileIndex: 12, fixedAngle: 80, coolDown: 2000, coolDownOffset: 1600),
                    new Shoot(12, 2, projectileIndex: 12, fixedAngle: -80, coolDown: 2000, coolDownOffset: 1600),
                    new Shoot(12, 2, projectileIndex: 12, fixedAngle: 90, coolDown: 2000, coolDownOffset: 1800),
                    new Shoot(12, 2, projectileIndex: 12, fixedAngle: -90, coolDown: 2000, coolDownOffset: 1800),
                    new Wander(0.4),
                    new TimedTransition(1800, "Remove")
                     )
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.01,
                new ItemLoot("Fossilized Horns", 0.001)
                ),
            new Threshold(0.001,
                new ItemLoot("Deep Freeze", 0.009),
                new ItemLoot("Ice Age", 0.009),
                new TierLoot(13, ItemType.Weapon, 0.05),
                new TierLoot(13, ItemType.Armor, 0.05),
                new TierLoot(6, ItemType.Ring, 0.05),
                new TierLoot(6, ItemType.Ability, 0.07),
                new ItemLoot("Potion of Vitality", 1),
                new ItemLoot("Potion of Mana", 1),
                new ItemLoot("Potion of Vitality", 1),
                new ItemLoot("Potion of Mana", 1),
                new ItemLoot("Magic Dust", 0.5)
                )
            )
        .Init("Abominable Snowman",
            new State(
                new ScaleHP2(20),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new PlayerWithinTransition(10, "attack")
                    ),
                new State("attack",
                    new Spawn("Ice Wall Spawner 1", 1, 1, coolDown: 999999),
                    new HpLessTransition(0.5, "freedps"),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: 0, coolDown: 2400, coolDownOffset: 100),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -5, coolDown: 2400, coolDownOffset: 200),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -10, coolDown: 2400, coolDownOffset: 300),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -15, coolDown: 2400, coolDownOffset: 400),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -20, coolDown: 2400, coolDownOffset: 500),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -25, coolDown: 2400, coolDownOffset: 600),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -30, coolDown: 2400, coolDownOffset: 700),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -35, coolDown: 2400, coolDownOffset: 800),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -40, coolDown: 2400, coolDownOffset: 900),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -45, coolDown: 2400, coolDownOffset: 1000),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -50, coolDown: 2400, coolDownOffset: 1100),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -55, coolDown: 2400, coolDownOffset: 1200),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -60, coolDown: 2400, coolDownOffset: 1300),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -65, coolDown: 2400, coolDownOffset: 1400),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -70, coolDown: 2400, coolDownOffset: 1500),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -75, coolDown: 2400, coolDownOffset: 1600),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -80, coolDown: 2400, coolDownOffset: 1700),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -85, coolDown: 2400, coolDownOffset: 1800),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -90, coolDown: 2400, coolDownOffset: 1900),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -95, coolDown: 2400, coolDownOffset: 2000),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -100, coolDown: 2400, coolDownOffset: 2100),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -105, coolDown: 2400, coolDownOffset: 2200),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -110, coolDown: 2400, coolDownOffset: 2300),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: -115, coolDown: 2400, coolDownOffset: 2400),                  
                                  
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 0, coolDown: 7200, coolDownOffset: 100),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 5, coolDown: 7200, coolDownOffset: 200),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 10, coolDown: 7200, coolDownOffset: 300),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 15, coolDown: 7200, coolDownOffset: 400),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 20, coolDown: 7200, coolDownOffset: 500),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 25, coolDown: 7200, coolDownOffset: 600),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 30, coolDown: 7200, coolDownOffset: 700),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 35, coolDown: 7200, coolDownOffset: 800),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 40, coolDown: 7200, coolDownOffset: 900),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 45, coolDown: 7200, coolDownOffset: 1000),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 50, coolDown: 7200, coolDownOffset: 1100),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 55, coolDown: 7200, coolDownOffset: 1200),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 60, coolDown: 7200, coolDownOffset: 1300),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 65, coolDown: 7200, coolDownOffset: 1400),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 70, coolDown: 7200, coolDownOffset: 1500),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 75, coolDown: 7200, coolDownOffset: 1600),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 80, coolDown: 7200, coolDownOffset: 1700),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 85, coolDown: 7200, coolDownOffset: 1800),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 90, coolDown: 7200, coolDownOffset: 1900),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 95, coolDown: 7200, coolDownOffset: 2000),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 100, coolDown: 7200, coolDownOffset: 2100),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 105, coolDown: 7200, coolDownOffset: 2200),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 110, coolDown: 7200, coolDownOffset: 2300),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 115, coolDown: 7200, coolDownOffset: 2400),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 120, coolDown: 7200, coolDownOffset: 2500),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 125, coolDown: 7200, coolDownOffset: 2600),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 130, coolDown: 7200, coolDownOffset: 2700),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 135, coolDown: 7200, coolDownOffset: 2800),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 140, coolDown: 7200, coolDownOffset: 2900),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 145, coolDown: 7200, coolDownOffset: 3000),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 150, coolDown: 7200, coolDownOffset: 3100),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 155, coolDown: 7200, coolDownOffset: 3200),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 160, coolDown: 7200, coolDownOffset: 3300),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 165, coolDown: 7200, coolDownOffset: 3400),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 170, coolDown: 7200, coolDownOffset: 3500),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 175, coolDown: 7200, coolDownOffset: 3600),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 180, coolDown: 7200, coolDownOffset: 3700),
                                                  
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 175, coolDown: 7200, coolDownOffset: 3800),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 170, coolDown: 7200, coolDownOffset: 3900),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 165, coolDown: 7200, coolDownOffset: 4000),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 160, coolDown: 7200, coolDownOffset: 4100),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 155, coolDown: 7200, coolDownOffset: 4200),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 150, coolDown: 7200, coolDownOffset: 4300),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 145, coolDown: 7200, coolDownOffset: 4400),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 140, coolDown: 7200, coolDownOffset: 4500),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 135, coolDown: 7200, coolDownOffset: 4600),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 130, coolDown: 7200, coolDownOffset: 4700),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 125, coolDown: 7200, coolDownOffset: 4800),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 120, coolDown: 7200, coolDownOffset: 4900),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 115, coolDown: 7200, coolDownOffset: 5000),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 110, coolDown: 7200, coolDownOffset: 5100),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 105, coolDown: 7200, coolDownOffset: 5200),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 100, coolDown: 7200, coolDownOffset: 5300),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 95, coolDown: 7200, coolDownOffset: 5400),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 90, coolDown: 7200, coolDownOffset: 5500),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 85, coolDown: 7200, coolDownOffset: 5600),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 80, coolDown: 7200, coolDownOffset: 5700),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 75, coolDown: 7200, coolDownOffset: 5800),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 70, coolDown: 7200, coolDownOffset: 5900),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 65, coolDown: 7200, coolDownOffset: 6000),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 60, coolDown: 7200, coolDownOffset: 6100),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 55, coolDown: 7200, coolDownOffset: 6200),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 50, coolDown: 7200, coolDownOffset: 6300),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 45, coolDown: 7200, coolDownOffset: 6400),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 40, coolDown: 7200, coolDownOffset: 6500),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 35, coolDown: 7200, coolDownOffset: 6600),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 30, coolDown: 7200, coolDownOffset: 6700),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 25, coolDown: 7200, coolDownOffset: 6800),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 20, coolDown: 7200, coolDownOffset: 6900),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 15, coolDown: 7200, coolDownOffset: 7000),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 10, coolDown: 7200, coolDownOffset: 7100),
                    new Shoot(30, 4, projectileIndex: 5, fixedAngle: 5, coolDown: 7200, coolDownOffset: 7200),
                    new TimedTransition(10000, "attack1")
                    ),
                new State("freedps",
                    new Taunt("HAHAHAH"),
                    new ChangeSize(5, 210),
                    new HpLessTransition(0.4, "attack3")
                    ),
                new State("attack1",
                    new HpLessTransition(0.5, "freedps"),
                    new Spawn("Ice Wall Spawner 1", 1, 1, coolDown: 999999),
                    new TossObject2("Frozen Elf", 3, coolDown: 3000, randomToss: true),
                    new TossObject2("Frozen Elf", 2, coolDown: 2500, randomToss: true),
                    new TossObject2("Frozen Elf", 4, coolDown: 3200, randomToss: true),
                    new TossObject2("Frozen Elf", 8, coolDown: 2600, randomToss: true),
                    new Follow(2, 1, 10),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: 0, coolDown: 2400, coolDownOffset: 100),
                    new TimedTransition(10000, "attack2.1")
                    ),
                new State("attack2.1",
                    new HpLessTransition(0.5, "freedps"),
                    new Orbit(4, 8, 10, "Ice Wall Spawner 1", speedVariance: 0, radiusVariance: 0),
                    new TossObject2("Frozen Elf", 3, coolDown: 3000, randomToss: true),
                    new TossObject2("Frozen Elf", 2, coolDown: 2500, randomToss: true),
                    new TossObject2("Frozen Elf", 4, coolDown: 3200, randomToss: true),
                    new TossObject2("Frozen Elf", 8, coolDown: 2600, randomToss: true),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: 0, coolDown: 2400, coolDownOffset: 100),
                    new TimedTransition(10000, "attack3")
                    ),
                 new State("attack3",
                    new Orbit(3, 8, 10, "Ice Wall Spawner 1", speedVariance: 0, radiusVariance: 0, orbitClockwise: true),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: 0, coolDown: 2400, coolDownOffset: 100),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -5, coolDown: 2400, coolDownOffset: 200),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -10, coolDown: 2400, coolDownOffset: 300),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -15, coolDown: 2400, coolDownOffset: 400),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -20, coolDown: 2400, coolDownOffset: 500),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -25, coolDown: 2400, coolDownOffset: 600),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -30, coolDown: 2400, coolDownOffset: 700),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -35, coolDown: 2400, coolDownOffset: 800),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -40, coolDown: 2400, coolDownOffset: 900),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -45, coolDown: 2400, coolDownOffset: 1000),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -50, coolDown: 2400, coolDownOffset: 1100),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -55, coolDown: 2400, coolDownOffset: 1200),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -60, coolDown: 2400, coolDownOffset: 1300),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -65, coolDown: 2400, coolDownOffset: 1400),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -70, coolDown: 2400, coolDownOffset: 1500),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -75, coolDown: 2400, coolDownOffset: 1600),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -80, coolDown: 2400, coolDownOffset: 1700),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -85, coolDown: 2400, coolDownOffset: 1800),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -90, coolDown: 2400, coolDownOffset: 1900),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -95, coolDown: 2400, coolDownOffset: 2000),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -100, coolDown: 2400, coolDownOffset: 2100),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -105, coolDown: 2400, coolDownOffset: 2200),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -110, coolDown: 2400, coolDownOffset: 2300),
                    new Shoot(30, 5, projectileIndex: 6, fixedAngle: -115, coolDown: 2400, coolDownOffset: 2400),
                    new TimedTransition(10000, "attack4")
                     ),
                  new State("attack4",
                    new HpLessTransition(0.5, "freedps"),
                    new Orbit(4, 8, 10, "Ice Wall Spawner 1", speedVariance: 0, radiusVariance: 0),
                    new TossObject2("Frozen Elf", 3, coolDown: 3000, randomToss: true),
                    new TossObject2("Frozen Elf", 2, coolDown: 2500, randomToss: true),
                    new TossObject2("Frozen Elf", 4, coolDown: 3200, randomToss: true),
                    new TossObject2("Frozen Elf", 8, coolDown: 2600, randomToss: true),
                    new Shoot(30, 3, projectileIndex: 4, fixedAngle: 0, coolDown: 2400, coolDownOffset: 100),
                    new TimedTransition(10000, "attack1")
                    )
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.01,
                new TierLoot(14, ItemType.Weapon, 0.05),
                new TierLoot(14, ItemType.Armor, 0.05),
                new TierLoot(6, ItemType.Ring, 0.05),
                new TierLoot(6, ItemType.Ability, 0.07),
                new ItemLoot("Greater Potion of Vitality", 0.5),
                new ItemLoot("Greater Potion of Mana", 0.5),
                new ItemLoot("Magic Dust", 0.5),
                new ItemLoot("Visage of the Frozen", 0.001),
                new ItemLoot("Queen's Guardian Signet", 0.001),
                new ItemLoot("Magic Dust", 0.5)
                ),
             new Threshold(0.03,
                new ItemLoot("PermaFrost GreatShield", 0.001),
                new ItemLoot("Winter Solstice", 0.001)
                )
            )
        .Init("Yeti Chest",
            new State(
                new ScaleHP2(30),
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new PlayerWithinTransition(20, "prepare")
                    ),
                new State("prepare",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new TimedTransition(5000, "attack")
                    ),
                new State("attack")
                ),
           new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.01,
                new TierLoot(14, ItemType.Weapon, 0.05),
                new TierLoot(14, ItemType.Armor, 0.05),
                new TierLoot(6, ItemType.Ring, 0.05),
                new TierLoot(6, ItemType.Ability, 0.07),
                new ItemLoot("Greater Potion of Vitality", 0.5),
                new ItemLoot("Greater Potion of Mana", 0.5),
                new ItemLoot("Magic Dust", 0.5),
                new ItemLoot("Visage of the Frozen", 0.0016),
                new ItemLoot("Queen's Guardian Signet", 0.0016),
                new ItemLoot("Magic Dust", 0.5)
                ),
             new Threshold(0.03,
                 new ItemLoot("PermaFrost GreatShield", 0.0005),
                 new ItemLoot("Axe of the Frozen Tundra", 0.0005)
                 )
            )
        .Init("Queen of Ice",
            new State(
                new ScaleHP2(20),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new PlayerWithinTransition(20, "prepare")
                    ),
                new State("prepare",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Flash(0xCC1A1A, 0.5, 12),
                    new Taunt("You've killed my beloved Yeti!"),
                    new TimedTransition(5000, "scream")
                    ),
                new State("scream",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Flash(0xCC1A1A, 0.5, 12),
                    new Shoot(20, 8, projectileIndex: 0, coolDownOffset: 1240),
                    new Shoot(20, 17, projectileIndex: 1, coolDownOffset: 1570),
                    new Shoot(20, 29, projectileIndex: 2, coolDownOffset: 3150),
                    new TimedTransition(10000, "Protection")
                    ),
                new State("attack1",
                    new Taunt("Hello friend. You seem to be lost. LET ME SHOW YOU THE WAY OUT!"),
                    new Chase(8),
                    new Shoot(8, 8, projectileIndex: 1, shootAngle: 10, coolDown: 2000),
                    new Shoot(20, 6, projectileIndex: 2, coolDown: 600),
                    new HpLessTransition(.85, "return to spawn")
                    ),
                new State("return to spawn",
                    new ReturnToSpawn(1.3, 0),
                    new Taunt("You have damaged me!"),
                    new TimedTransition(5000, "Protection")
                    ),
                new State("Protection",
                    new ReturnToSpawn(1.3, 0),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("Guardians! Protect me!"),
                    new TossObject("Ice Tower", 8, angle: 0, coolDown: 99999),
                    new TossObject("Ice Tower 1", 8, angle: 60, coolDown: 99999),
                    new TossObject("Ice Tower 2", 8, angle: 120, coolDown: 99999),
                    new TossObject("Ice Tower 3", 8, angle: 180, coolDown: 99999),
                    new TossObject("Ice Tower 4", 8, angle: 240, coolDown: 99999),
                    new TossObject("Ice Tower 5", 8, angle: 300, coolDown: 99999),
                    new EntityExistsTransition("Ice Tower", 10, "waiting")
                    ),
                new State("waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Shoot(12, 1, projectileIndex: 1, predictive: .4, coolDown: 400),
                    new Shoot(8, 1, projectileIndex: 0, fixedAngle: 0, coolDown: 100),
                    new Shoot(8, 1, projectileIndex: 0, fixedAngle: 60, coolDown: 100),
                    new Shoot(8, 1, projectileIndex: 0, fixedAngle: 120, coolDown: 100),
                    new Shoot(8, 1, projectileIndex: 0, fixedAngle: 180, coolDown: 100),
                    new Shoot(8, 1, projectileIndex: 0, fixedAngle: 240, coolDown: 100),
                    new Shoot(8, 1, projectileIndex: 0, fixedAngle: 300, coolDown: 100),
                    new Shoot(10, 6, projectileIndex: 4, coolDown: 400),
                    new EntitiesNotExistsTransition(100, "tentacles", "Ice Tower", "Ice Tower 1", "Ice Tower 2", "Ice Tower 3", "Ice Tower 4", "Ice Tower 5")
                    ),
                new State("tentacles",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new TossObject2("Evil Snowman", 5, coolDown: 5000, randomToss: true),
                    new TossObject2("Evil Snowman", 5, coolDown: 5000, randomToss: true),
                    new TossObject2("Evil Snowman", 5, coolDown: 5000, randomToss: true),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, predictive: 1.2, coolDown: 1200),
                    new Shoot(30, 10, projectileIndex: 2, fixedAngle: 0, coolDown: 800, coolDownOffset: 100),
                    new Shoot(30, 10, projectileIndex: 2, fixedAngle: 5, coolDown: 800, coolDownOffset: 200),
                    new Shoot(30, 10, projectileIndex: 2, fixedAngle: 10, coolDown: 800, coolDownOffset: 300),
                    new Shoot(30, 10, projectileIndex: 2, fixedAngle: 15, coolDown: 800, coolDownOffset: 400),
                    new Shoot(30, 10, projectileIndex: 2, fixedAngle: 20, coolDown: 800, coolDownOffset: 500),
                    new Shoot(30, 10, projectileIndex: 2, fixedAngle: 25, coolDown: 800, coolDownOffset: 600),
                    new Shoot(30, 10, projectileIndex: 2, fixedAngle: 30, coolDown: 800, coolDownOffset: 700),
                    new Shoot(30, 10, projectileIndex: 2, fixedAngle: 35, coolDown: 800, coolDownOffset: 800),
                    new TimedTransition(30000, "scream2")
                    ),
                new State("scream2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Taunt("ARHHHHH"),
                    new Flash(0x5279FD, 0.5, 12),
                    new ChangeSize(5, 160),
                    new TimedTransition(10000, "attackoftheminions")
                    ),
                new State("attackoftheminions",
                    new Charge(speed: 1.4, range: 8, coolDown: 1500),
                    new Shoot(12, 6, projectileIndex: 2, shootAngle: 10, coolDown: 600),
                    new Shoot(2, 30, projectileIndex: 1, coolDown: 500),
                    new Reproduce("Evil Snowman", 5, 5, coolDown: 5000),
                    new TossObject("Frozen Elf", 4, angle: 0, coolDown: 5000),
                    new TossObject("Frozen Elf", 8, angle: 60, coolDown: 5000),
                    new TossObject("Frozen Elf", 4, angle: 120, coolDown: 5000),
                    new TossObject("Frozen Elf", 8, angle: 180, coolDown: 5000),
                    new TossObject("Frozen Elf", 4, angle: 270, coolDown: 5000),
                    new HpLessTransition(.7, "healing")
                    ),
                new State("healing",
                    new ReturnToSpawn(1.3, 0),
                    new Taunt("THE COLD MAKES ME STRONGER!!!!"),
                    new HealSelf(coolDown: 5000, amount: 25000),
                    new HpLessTransition(.6, "clonesprepare")
                    ),
                new State("clonesprepare",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("I HOPE YOU CAME PREPARED"),
                    new TimedTransition(5000, "clones")
                    ),
                new State("clones",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new TossObject("Queen of Ice Clone", 6, 45, coolDown: 99999),
                    new TossObject("Queen of Ice Clone", 6, 135, coolDown: 99999),
                    new TossObject("Queen of Ice Clone", 6, 225, coolDown: 99999),
                    new TossObject("Queen of Ice Clone", 6, 315, coolDown: 99999),
                    new EntityExistsTransition("Queen of Ice Clone", 10, "waiting1")
                    ),
                new State("waiting1",
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 15, predictive: 1, coolDown: 1000),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new EntitiesNotExistsTransition(10, "prepareattack2", "Queen of Ice Clone", "Queen of Ice Clone", "Queen of Ice Clone", "Queen of Ice Clone")
                    ),
                new State("prepareattack2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ReturnToSpawn(1.3, 0),
                    new TimedTransition(5000, "attack2")
                    ),
                new State("attack2",
                    new HpLessTransition(.45, "guards2"),
                    new Taunt("I’VE GOT PLENTY MORE TRICKS UP MY SLEEVE!"),
                    new Shoot(3, 40, projectileIndex: 3, coolDown: 300),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 216, coolDown: 300),
                    new Grenade(radius: 4, damage: 90, range: 6, fixedAngle: 36, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 90, range: 12, fixedAngle: 36, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.1")
                    ),
                new State("attack2.1",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(3, 40, projectileIndex: 3, coolDown: 300),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 252, coolDown: 300),
                    new Grenade(radius: 4, damage: 90, range: 12, fixedAngle: 72, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 90, range: 6, fixedAngle: 72, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.2")
                    ),
                new State("attack2.2",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(3, 40, projectileIndex: 3, coolDown: 300),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 288, coolDown: 300),
                    new Grenade(radius: 4, damage: 90, range: 12, fixedAngle: 108, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 90, range: 6, fixedAngle: 108, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.3")
                    ),
                new State("attack2.3",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(3, 40, projectileIndex: 3, coolDown: 300),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 324, coolDown: 300),
                    new Grenade(radius: 4, damage: 90, range: 12, fixedAngle: 144, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 90, range: 6, fixedAngle: 144, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.4")
                    ),
                new State("attack2.4",
                    new HpLessTransition(.45, "guards2"),
                    new Taunt("I’VE GOT PLENTY MORE TRICKS UP MY SLEEVE!"),
                    new Shoot(3, 40, projectileIndex: 3, coolDown: 300),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 0, coolDown: 300),
                    new Grenade(radius: 4, damage: 90, range: 12, fixedAngle: 180, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 90, range: 6, fixedAngle: 180, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.5")
                    ),
                new State("attack2.5",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(3, 40, projectileIndex: 3, coolDown: 300),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 36, coolDown: 300),
                    new Grenade(radius: 4, damage: 90, range: 12, fixedAngle: 216, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 90, range: 6, fixedAngle: 216, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.6")
                    ),
                new State("attack2.6",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(3, 40, projectileIndex: 3, coolDown: 300),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 72, coolDown: 300),
                    new Grenade(radius: 4, damage: 90, range: 12, fixedAngle: 252, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 90, range: 6, fixedAngle: 252, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.7")
                    ),
                new State("attack2.7",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(3, 40, projectileIndex: 3, coolDown: 300),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 108, coolDown: 300),
                    new Grenade(radius: 4, damage: 90, range: 12, fixedAngle: 288, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 90, range: 6, fixedAngle: 288, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.8")
                    ),
                new State("attack2.8",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(3, 40, projectileIndex: 3, coolDown: 300),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 144, coolDown: 300),
                    new Grenade(radius: 4, damage: 90, range: 12, fixedAngle: 324, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 90, range: 6, fixedAngle: 324, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.9")
                    ),
                new State("attack2.9",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(3, 40, projectileIndex: 3, coolDown: 300),
                    new Shoot(15, 3, projectileIndex: 0, coolDown: 800),
                    new Grenade(radius: 4, damage: 90, range: 12, fixedAngle: 360, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 90, range: 8, fixedAngle: 360, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2")
                    ),
                new State("guards2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("Guardians! Protect me!"),
                    new TossObject("Ice Tower", 8, angle: 0, coolDown: 99999),
                    new TossObject("Ice Tower 1", 8, angle: 60, coolDown: 99999),
                    new TossObject("Ice Tower 2", 8, angle: 120, coolDown: 99999),
                    new TossObject("Ice Tower 3", 8, angle: 180, coolDown: 99999),
                    new TossObject("Ice Tower 4", 8, angle: 240, coolDown: 99999),
                    new TossObject("Ice Tower 5", 8, angle: 300, coolDown: 99999),
                    new EntityExistsTransition("Ice Tower", 10, "waiting2")
                    ),
                new State("waiting2",
                    new Shoot(12, 1, projectileIndex: 1, predictive: .4, coolDown: 400),
                    new Shoot(8, 1, projectileIndex: 0, fixedAngle: 0, coolDown: 100),
                    new Shoot(8, 1, projectileIndex: 0, fixedAngle: 60, coolDown: 100),
                    new Shoot(8, 1, projectileIndex: 0, fixedAngle: 120, coolDown: 100),
                    new Shoot(8, 1, projectileIndex: 0, fixedAngle: 180, coolDown: 100),
                    new Shoot(8, 1, projectileIndex: 0, fixedAngle: 240, coolDown: 100),
                    new Shoot(8, 1, projectileIndex: 0, fixedAngle: 300, coolDown: 100),
                    new Shoot(10, 6, projectileIndex: 4, coolDown: 400),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new EntitiesNotExistsTransition(100, "attack4", "Ice Tower", "Ice Tower 1", "Ice Tower 2", "Ice Tower 3", "Ice Tower 4", "Ice Tower 5")
                    ),
                new State("attack4",
                    new GroundTransform("lava", relativeX: +10, relativeY: +10, persist: false),
                    new GroundTransform("lava", relativeX: +11, relativeY: +11, persist: false),
                    new GroundTransform("lava", relativeX: +12, relativeY: +12, persist: false),
                    new GroundTransform("lava", relativeX: -10, relativeY: -10, persist: false),
                    new GroundTransform("lava", relativeX: -11, relativeY: -11, persist: false),
                    new GroundTransform("lava", relativeX: -12, relativeY: -12, persist: false),
                    new Grenade(radius: 4, damage: 40, range: 7, fixedAngle: 0, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 40, range: 7, fixedAngle: 90, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 40, range: 7, fixedAngle: 180, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 40, range: 7, fixedAngle: 270, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 40, range: 7, fixedAngle: 45, coolDown: 4000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 40, range: 7, fixedAngle: 135, coolDown: 4000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 40, range: 7, fixedAngle: 225, coolDown: 4000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 40, range: 7, fixedAngle: 315, coolDown: 4000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0x5279FD),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: 0, coolDown: 5200),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: 7, coolDown: 5200, coolDownOffset: 200),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: 14, coolDown: 5200, coolDownOffset: 400),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: 21, coolDown: 5200, coolDownOffset: 600),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: 28, coolDown: 5200, coolDownOffset: 800),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: 35, coolDown: 5200, coolDownOffset: 1000),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: 42, coolDown: 5200, coolDownOffset: 1200),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: 49, coolDown: 5200, coolDownOffset: 1400),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: 56, coolDown: 5200, coolDownOffset: 1600),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: 63, coolDown: 5200, coolDownOffset: 1800),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: 70, coolDown: 5200, coolDownOffset: 2000),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: 77, coolDown: 5200, coolDownOffset: 2200),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: 82, coolDown: 5200, coolDownOffset: 2400),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: 89, coolDown: 5200, coolDownOffset: 2600),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: -82, coolDown: 5200, coolDownOffset: 2800),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: -77, coolDown: 5200, coolDownOffset: 3000),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: -70, coolDown: 5200, coolDownOffset: 3200),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: -63, coolDown: 5200, coolDownOffset: 3400),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: -56, coolDown: 5200, coolDownOffset: 3600),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: -49, coolDown: 5200, coolDownOffset: 3800),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: -42, coolDown: 5200, coolDownOffset: 4000),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: -35, coolDown: 5200, coolDownOffset: 4200),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: -28, coolDown: 5200, coolDownOffset: 4400),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: -21, coolDown: 5200, coolDownOffset: 4600),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: -14, coolDown: 5200, coolDownOffset: 4800),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: -7, coolDown: 5200, coolDownOffset: 5000),
                    new Shoot(20, 8, projectileIndex: 2, fixedAngle: -0, coolDown: 5200, coolDownOffset: 5200),
                    new HpLessTransition(.25, "attack5")
                    ),
                new State("attack5",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ChangeSize(5, 140),
                    new TimedTransition(5000, "attack5.1")
                    ),
                new State("attack5.1",
                    new TossObject("Guardian of the Ice Queen", 2, 180, coolDown: 999999),
                    new TossObject("Protector of the Ice Queen", 2, 0, coolDown: 999999),
                    new EntityExistsTransition("Guardian of the Ice Queen", 10, "waiting3")
                    ),
                new State("waiting3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new EntitiesNotExistsTransition(100, "dead", "Guardian of the Ice Queen", "Protector of the Ice Queen")
                    ),
                new State("dead",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Taunt("NOOOOO!"),
                    new TimedTransition(5000, "dead1")
                    ),
                new State("dead1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Shoot(15, 30, projectileIndex: 0, shootAngle: 12, coolDown: 5000),
                    new TimedTransition(200, "suicide")
                    ),
                new State("suicide",
                    new Suicide()
                    )
                ),
                new Threshold(0.01,
                LootTemplates.DustLoot()
                    ),
            new Threshold(0.01,
                new TierLoot(14, ItemType.Weapon, 0.05),
                new TierLoot(14, ItemType.Armor, 0.05),
                new TierLoot(6, ItemType.Ring, 0.05),
                new TierLoot(6, ItemType.Ability, 0.07),
                new ItemLoot("Greater Potion of Speed", 0.5),
                new ItemLoot("Greater Potion of Life", 0.5),
                new ItemLoot("Chilled UnderGarments", 0.0016),
                new ItemLoot("Queen's Crystalized Rage", 0.0016),
                new ItemLoot("Magic Dust", 0.5),
                new ItemLoot("Special Dust", 0.005)
                ),
             new Threshold(0.03,
                 new ItemLoot("Trinity of the Frozen Core", 0.0005),
                 new ItemLoot("Eternal Queen's Friend", 0.0005)
                ),
             new Threshold(0.05,
                 new ItemLoot("Living Heart of Ice", 0.000125)
                 )
            )
        .Init("Corrupt Snowman Switch", //finished
            new State(
                new ScaleHP2(20),
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new PlayerWithinTransition(20, "prepare")
                    ),
                new State("prepare",
                    new Taunt("huh..."),
                    new TimedTransition(2500, "changesize")
                    ),
                new State("changesize",
                    new Flash(0xCC1A1A, 0.5, 12),
                    new ChangeSize(5, 210),
                    new TimedTransition(3600, "attack1")
                    ),
                new State("attack1",
                    new Wander(0.8),
                    new Shoot(15, 8, shootAngle: 10, projectileIndex: 0, predictive: .9, coolDown: 400),
                    new Shoot(15, 2, shootAngle: 10, projectileIndex: 3, predictive: .9, coolDown: 800),
                    new TimedTransition(6000, "explode")
                    ),
                new State("explode",
                    new Follow(3, 12, 1),
                    new TimedTransition(1000, "explode1")
                    ),
                new State("explode1",
                    new Shoot(15, 20, projectileIndex: 1, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(15, 20, projectileIndex: 1, coolDown: 2000, coolDownOffset: 400),
                    new Shoot(15, 20, projectileIndex: 1, coolDown: 2000, coolDownOffset: 800),
                    new TimedTransition(1000, "attack1")
                    )
                )
            )
        .Init("Cursed Snowman Switch", //finished
            new State(
                new ScaleHP2(20),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new PlayerWithinTransition(20, "prepare")
                    ),
                new State("prepare",
                    new Taunt("I see you have obtained 1 of the 2 keys.."),
                    new Taunt("This time.. it won't be so easy!"),
                    new TimedTransition(5000, "prepare1")
                    ),
                new State("prepare1",
                    new Taunt("There's no escaping..."),
                    new OrderOnce(40, "Ice Wall Spawner", "Fire"),
                    new TimedTransition(5000, "attack1")
                    ),
                new State("attack1",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new HpLessTransition(.6, "rage"),
                    new Follow(1.5, 1, 15, duration: 6),
                    new ReplaceTile("Black Water Frozen 1", "Black Water Frozen", 30),
                    new Shoot(8, 4, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 800),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 1, predictive: .8, coolDown: 600),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 3, predictive: .8, coolDown: 600),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 4, predictive: .8, coolDown: 600),
                    new Shoot(20, 12, projectileIndex: 0, coolDown: 1000),
                    new TimedTransition(4000, "attack1.1")
                    ),
                 new State("attack1.1",
                    new HpLessTransition(.6, "rage"),
                    new Follow(1.5, 1, 15, duration: 6),
                    new ReplaceTile("Black Water Frozen", "Black Water Frozen 1", 30),
                    new Shoot(8, 4, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 800),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 1, predictive: .8, coolDown: 600),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 3, predictive: .8, coolDown: 600),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 4, predictive: .8, coolDown: 600),
                    new Shoot(20, 12, projectileIndex: 0, coolDown: 1000),
                    new TimedTransition(4000, "attack1.2")
                    ),
                 new State("attack1.2",
                    new HpLessTransition(.6, "rage"),
                    new Follow(1.5, 1, 15, duration: 6),
                    new ReplaceTile("Black Water Frozen 1", "Black Water Frozen", 30),
                    new Shoot(8, 4, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 800),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 1, predictive: .8, coolDown: 600),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 3, predictive: .8, coolDown: 600),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 4, predictive: .8, coolDown: 600),
                    new Shoot(20, 12, projectileIndex: 0, coolDown: 1000),
                    new TimedTransition(4000, "attack1")
                    ),
                new State("rage",
                    new ReplaceTile("Black Water Frozen", "Black Water Frozen 1", 30),
                    new ReturnToSpawn(4, 0),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Shoot(8, 4, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 800),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 1, predictive: .8, coolDown: 600),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 3, predictive: .8, coolDown: 600),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 4, predictive: .8, coolDown: 600),
                    new TossObject("Frozen Tower", 8, 45, coolDown: 999999),
                    new TossObject("Frozen Tower", 8, 135, coolDown: 999999),
                    new TossObject("Frozen Tower", 8, 225, coolDown: 999999),
                    new TossObject("Frozen Tower", 8, 315, coolDown: 999999),
                    new TimedTransition(15000, "freedps")
                    ),
                new State("freedps",
                    new Shoot(25, 12, predictive: 1, coolDown: 200),
                    new Taunt("HAHAHAHAHAHAH"),
                    new HpLessTransition(.4, "attack2")
                    ),
                new State("attack2",
                    new HpLessTransition(.1, "granted"),
                    new ReplaceTile("Black Water Frozen 1", "Black Water Frozen", 30),
                    new Follow(1.5, 15, 1, duration: 8),
                    new Shoot(8, 4, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 800),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 1, predictive: .8, coolDown: 600),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 3, predictive: .8, coolDown: 600),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 4, predictive: .8, coolDown: 600),
                    new Shoot(20, 12, projectileIndex: 0, coolDown: 1000),
                    new TimedTransition(4000, "attack2.1")
                    ),
                new State("attack2.1",
                    new HpLessTransition(.1, "granted"),
                    new ReplaceTile("Black Water Frozen", "Black Water Frozen 1", 30),
                    new Follow(1.5, 15, 1, duration: 8),
                    new Shoot(8, 4, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 800),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 1, predictive: .8, coolDown: 600),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 3, predictive: .8, coolDown: 600),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 4, predictive: .8, coolDown: 600),
                    new Shoot(20, 12, projectileIndex: 0, coolDown: 1000),
                    new TimedTransition(4000, "attack2.2")
                    ),
                 new State("attack2.2",
                    new HpLessTransition(.1, "granted"),
                    new ReplaceTile("Black Water Frozen 1", "Black Water Frozen", 30),
                    new Follow(1.5, 15, 1, duration: 8),
                    new Shoot(8, 4, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 800),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 1, predictive: .8, coolDown: 600),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 3, predictive: .8, coolDown: 600),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 4, predictive: .8, coolDown: 600),
                    new Shoot(20, 12, projectileIndex: 0, coolDown: 1000),
                    new TimedTransition(4000, "attack2")
                    ),
                new State("granted",
                    new ReturnToSpawn(3),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Taunt("NOOOO"),
                    new Shoot(20, 30, projectileIndex: 0, coolDown: 5000),
                    new Suicide()
                    )
                )
            )
        .Init("Guardian of the Ice Queen",
            new State(
                new ScaleHP2(20),
                new State("prepare",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new PlayerWithinTransition(20, "prepareattack1")
                    ),
                new State("prepareattack1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("I will sacrifice my life for you my Queen!"),
                    new ChangeSize(2, 180),
                    new TimedTransition(10000, "attack1")
                    ),
                new State("attack1",
                    new Follow(4, 15, duration: 5, coolDown: 2000),
                    new Shoot(12, 2, shootAngle: 10, predictive: 1.2, projectileIndex: 0, coolDown: 200),
                    new Shoot(7, 12, projectileIndex: 1, coolDown: 800),
                    new Grenade(2, 20, range: 12, fixedAngle: 180, coolDown: 1000, ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0x5279FD),
                    new Grenade(2, 20, range: 12, fixedAngle: 160, coolDown: 1500, ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0x5279FD),
                    new Grenade(2, 20, range: 12, fixedAngle: 200, coolDown: 2000, ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0x5279FD),
                    new HpLessTransition(.6, "prepareattack2")
                    ),
                new State("prepareattack2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ReturnToSpawn(1.3, 0),
                    new Taunt("YOU NEED TO DIE!"),
                    new Flash(0xCC1A1A, 0.5, 12),
                    new TimedTransition(5000, "attack2")
                    ),
                new State("attack2",
                    new Charge(3, 8, coolDown: 2000),
                    new Shoot(2, 24, projectileIndex: 0, coolDown: 500),
                    new Grenade(2, 30, 8, fixedAngle: 0, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 22.5, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 45, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 67.5, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 90, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 112.5, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 135, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 157.5, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 180, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 202.5, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 225, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 247.5, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 270, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 292.5, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 315, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new Grenade(2, 30, 8, fixedAngle: 337.5, 2000, ConditionEffectIndex.Paralyzed, 800, color: 0x5279FD),
                    new HpLessTransition(.3, "prepareattack3")
                    ),
                new State("prepareattack3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("THAT'S IT!!"),
                    new TimedTransition(3000, "attack3")
                    ),
                new State("attack3",
                    new Chase(3),
                    new Shoot(12, 2, shootAngle: 10, predictive: 1.2, projectileIndex: 1, coolDown: 200),
                    new Shoot(7, 12, projectileIndex: 0, coolDown: 800),
                    new Grenade(2, 20, range: 7, fixedAngle: 180, coolDown: 1000, ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0x5279FD),
                    new Grenade(2, 20, range: 7, fixedAngle: 160, coolDown: 1500, ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0x5279FD),
                    new Grenade(2, 20, range: 7, fixedAngle: 200, coolDown: 2000, ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0x5279FD)
                    )
                )
            )
        .Init("Protector of the Ice Queen",
            new State(
                new ScaleHP2(20),
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                new State("wait",
                    new EntityExistsTransition("Guardian of the Ice Queen", 10, "waiting")
                    ),
                new State("waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new EntitiesNotExistsTransition(100, "prepare", "Guardian of the Ice Queen")
                    ),
                new State("prepare",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new PlayerWithinTransition(20, "prepareattack1")
                    ),
                new State("prepareattack1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("I will protect you with my LIFE! NOW DIE"),
                    new ChangeSize(2, 180),
                    new TimedTransition(10000, "attack1")
                    ),
                new State("attack1",
                    new TossObject("Frozen Elf", 5, angle: 180, coolDown: 500),
                    new TossObject("Frozen Elf", 5, angle: 190, coolDown: 1000),
                    new TossObject("Frozen Elf", 5, angle: 160, coolDown: 1500),
                    new TossObject("Frozen Elf", 5, angle: 200, coolDown: 2000),
                    new TossObject("Frozen Elf", 5, angle: 170, coolDown: 2500),
                    new Shoot(12, 8, shootAngle: 10, predictive: 1, projectileIndex: 0, coolDown: 1000),
                    new HpLessTransition(.6, "attack2")
                    ),
                new State("attack2",
                    new Chase(2),
                    new Shoot(12, 8, shootAngle: 10, predictive: 1, projectileIndex: 0, coolDown: 600),
                    new Shoot(20, 1, projectileIndex: 1, predictive: 1, coolDown: 1000),
                    new HpLessTransition(.3, "attack3")
                    ),
                new State("attack3",
                    new ReturnToSpawn(1.3, 0),
                    new Shoot(12, 8, shootAngle: 10, predictive: 1, projectileIndex: 0, coolDown: 600),
                    new Shoot(20, 1, projectileIndex: 1, predictive: 1, coolDown: 1000),
                    new TossObject("Frozen Elf", 5, angle: 180, coolDown: 500),
                    new TossObject("Frozen Elf", 5, angle: 190, coolDown: 1000),
                    new TossObject("Frozen Elf", 5, angle: 160, coolDown: 1500),
                    new TossObject("Frozen Elf", 5, angle: 200, coolDown: 2000),
                    new TossObject("Frozen Elf", 5, angle: 170, coolDown: 2500)
                    )
                )
            )
        .Init("Queen of Ice Clone", // finished
            new State(
                new ScaleHP2(20),
                new Prioritize(
                    new Orbit(1, 6.0, 25, "Queen of Ice")
                    ),
                new Shoot(10, 4, projectileIndex: 0, shootAngle: 5, predictive: .6, coolDown: 400)
                )
            )
        .Init("Ice Tower",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Shoot(4, 2, shootAngle: 120, projectileIndex: 0, fixedAngle: 180, coolDown: 50)
                    )
                )
            )
        .Init("Ice Tower 1",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Shoot(4, 2, shootAngle: 120, projectileIndex: 0, fixedAngle: 240, coolDown: 50)
                    )
                )
            )
        .Init("Ice Tower 2",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Shoot(4, 2, shootAngle: 120, projectileIndex: 0, fixedAngle: 300, coolDown: 50)
                    )
                )
            )
        .Init("Ice Tower 3",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Shoot(4, 2, shootAngle: 120, projectileIndex: 0, fixedAngle: 0, coolDown: 50)
                    )
                )
            )
        .Init("Ice Tower 4",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Shoot(4, 2, shootAngle: 120, projectileIndex: 0, fixedAngle: 60, coolDown: 50)
                    )
                )
            )
        .Init("Ice Tower 5",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Shoot(4, 2, shootAngle: 120, projectileIndex: 0, fixedAngle: 120, coolDown: 50)
                    )
                )
            )
        .Init("Evil Snowman",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Taunt("attack!"),
                    new Chase(9, 15, coolDown: 1500),
                    new Shoot(8, 6, projectileIndex: 0, predictive: 0.8, coolDown: 500),
                    new HealSelf(amount: 2000, coolDown: 5000)
                    )
                )
            )
        .Init("Cursed Polar Bear",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Wander(0.5),
                    new Shoot(10, 3, projectileIndex: 0, predictive: 1, shootAngle: 20, coolDown: 1000),
                    new Shoot(20, 1, projectileIndex: 1, predictive: 1.2, coolDown: 1500)
                    )
                )
            )
        .Init("Enraged Cub",
            new State(
                new ScaleHP2(20),
                new State("Grr",
                    new Wander(.25),
                    new Shoot(9, 2, projectileIndex: 0, predictive: 0.6, shootAngle: 30, coolDown: 800),
                    new Decay(15000)
                    )
                )
            )
        .Init("Guardian's Frigid Squire",
            new State(
                new ScaleHP2(20),
                new State("attack1",
                    new Wander(.25),
                    new Shoot(20, 2, projectileIndex: 0, shootAngle: 5, predictive: .9, coolDown: 600),
                    new Shoot(15, 8, projectileIndex: 1, predictive: 1, coolDown: 1200)
                    )
                )
            )
        .Init("Frozen Elf",
            new State(
                new ScaleHP2(20),
                new State("within",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new PlayerWithinTransition(8, "explodeprep")
                    ),
                new State("explodeprep",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Taunt("explode!"),
                    new Follow(1.5, 1, 10),
                    new Flash(0xCC1A1A, 0.5, 12),
                    new TimedTransition(650, "explode")
                    ),
                new State("explode",
                    new Shoot(4, 12, projectileIndex: 0, coolDown: 600),
                    new TimedTransition(250, "suicide")
                    ),
                new State("suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Frozen Tower",
            new State(
                new ScaleHP2(20),
                new State("idle",
                    new PlayerWithinTransition(20, "attack")
                    ),
                new State("attack",
                    new Shoot(12, 3, shootAngle: 15, projectileIndex: 0, coolDown: 1000)
                    )
                ));
    }
}