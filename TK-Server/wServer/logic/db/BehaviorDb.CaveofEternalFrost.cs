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
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 600),
                    new TimedTransition(1200, "die")
                    ),
                new State("die",
                    new Suicide()
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
                    new InvisiToss("Snowy Turret", 8, angle: 0, coolDown: 2800, coolDownOffset: 0),
                    new InvisiToss("Snowy Turret 1", 8, angle: 72, coolDown: 2800, coolDownOffset: 0),
                    new InvisiToss("Snowy Turret 2", 8, angle: 144, coolDown: 2800, coolDownOffset: 0),
                    new InvisiToss("Snowy Turret 3", 8, angle: 216, coolDown: 2800, coolDownOffset: 0),
                    new InvisiToss("Snowy Turret 4", 8, angle: 288, coolDown: 2800, coolDownOffset: 0),

                    new InvisiToss("Snowy Turret", 8, angle: 36, coolDown: 2800, coolDownOffset: 1400),
                    new InvisiToss("Snowy Turret 1", 8, angle: 108, coolDown: 2800, coolDownOffset: 1400),
                    new InvisiToss("Snowy Turret 2", 8, angle: 180, coolDown: 2800, coolDownOffset: 1400),
                    new InvisiToss("Snowy Turret 3", 8, angle: 252, coolDown: 2800, coolDownOffset: 1400),
                    new InvisiToss("Snowy Turret 4", 8, angle: 324, coolDown: 2800, coolDownOffset: 1400),
                    new EntityNotExistsTransition("Primordial Quetzalcoatl", 50, "die")
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
                    new Shoot(12, 1, projectileIndex: 9, coolDown: 600),
                    new Shoot(12, 2, projectileIndex: 11, shootAngle: 10, coolDown: 600),
                    new Shoot(12, 3, projectileIndex: 12, shootAngle: 15, coolDown: 600),
                    new TimedTransition(1000, "Remove2")
                    ),
                new State("Remove2",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new Wander(0.4),
                    new Charge(10, 8, coolDown: 3000),
                    new Shoot(12, 1, projectileIndex: 9, coolDown: 600),
                    new Shoot(12, 2, projectileIndex: 11, shootAngle: 10, coolDown: 600),
                    new Shoot(12, 3, projectileIndex: 12, shootAngle: 15, coolDown: 600),
                    new OrderOnce(10, "Snowy Turret Toss", "Shoot"),
                    new Taunt("ahahahaHAHAHAHAH"),
                    new TimedTransition(5000, "Ring")
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
                    new Wander(0.5),
                    new Shoot(25, projectileIndex: 0, count: 4, shootAngle: 15, coolDown: 1200, coolDownOffset: 400),
                    new Shoot(25, projectileIndex: 0, count: 4, shootAngle: 74, coolDown: 1200, coolDownOffset: 800),
                    new Shoot(25, projectileIndex: 2, count: 1, shootAngle: 10, coolDown: 1200, coolDownOffset: 1200),
                    new HpLessTransition(.7, "attack2prepare")
                    ),
                new State("attack2prepare",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new ChangeSize(5, 180),
                    new TimedTransition(8000, "attack2")
                    ),
                new State("attack2",
                        new Follow(0.8, 15, 3),
                        new StayCloseToSpawn(2, 15),
                        new Shoot(15, projectileIndex: 0, count: 4, shootAngle: 15, coolDown: 1500, coolDownOffset: 500),
                        new Shoot(15, projectileIndex: 0, count: 4, shootAngle: 74, coolDown: 1500, coolDownOffset: 1000),
                        new Shoot(15, projectileIndex: 2, count: 1, shootAngle: 10, coolDown: 4000, coolDownOffset: 1500),
                        new Shoot(15, projectileIndex: 1, count: 1, coolDown: 900, coolDownOffset: 900),
                        new Grenade(5, 150, 10, 0, coolDown: 2000, color: 0x00ffff),
                        new Grenade(5, 150, 10, 90, coolDown: 2000, color: 0x00ffff),
                        new Grenade(5, 150, 10, 180, coolDown: 2000, color: 0x00ffff),
                        new Grenade(5, 150, 10, 270, coolDown: 2000, color: 0x00ffff),
                    new HpLessTransition(.5, "attack3prepare")
                    ),
                new State("attack3prepare",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ReturnToSpawn(3, 0),
                    new Taunt("GARR"),
                    new TimedTransition(5000, "attack3")
                    ),
                new State("attack3",
                    new Chase(10),
                    new TossObject("Guardian's Frigid Squire", 7, angle: 120, coolDown: 999999),
                    new TossObject("Guardian's Frigid Squire", 7, angle: 360, coolDown: 999999),
                    new Shoot(15, projectileIndex: 0, count: 4, shootAngle: 15, coolDown: 1500, coolDownOffset: 500),
                    new Shoot(15, projectileIndex: 0, count: 4, shootAngle: 74, coolDown: 1500, coolDownOffset: 1000),
                    new Shoot(15, projectileIndex: 2, count: 1, shootAngle: 10, coolDown: 1500, coolDownOffset: 1500),
                    new Shoot(15, projectileIndex: 1, count: 1, coolDown: 700, coolDownOffset: 700),
                    new Grenade(5, 150, 10, 0, coolDown: 2000, color: 0x00ffff),
                    new Grenade(5, 150, 10, 90, coolDown: 2000, color: 0x00ffff),
                    new Grenade(5, 150, 10, 180, coolDown: 2000, color: 0x00ffff),
                    new Grenade(5, 150, 10, 270, coolDown: 2000, color: 0x00ffff),

                    new HpLessTransition(.2, "hurt")
                    ),
                new State("hurt",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ReturnToSpawn(1.3, 0),
                    new Taunt("GARR, OK YOU WIN, ARE YOU GOING TO KILL ME?!"),
                    new PlayerTextTransition("attack4prepare", "yes", 100, ignoreCase: true),
                    new PlayerTextTransition("no", "no", 100, ignoreCase: true)
                    ),
                new State("no",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("THANK YOU! I GET OUT OF HERE NOW!"),
                    new TimedTransition(1000, "no2")
                    ),
                new State("no2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("I OPEN SHORTCUT FOR YOU!"),
                    new OpenGate("Ice Cave Wall Shortcut", 200),
                    new MoveTo(1, -5, -1),
                    new Decay(3000)
                    ),
                new State("attack4prepare",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("HOW DARE YOU!"),
                    new TimedTransition(3000, "attack4prepare2")
                    ),
                new State("attack4prepare2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Flash(0xffffff, 3000, 6),
                    new HealSelf(500, 5000),
                    new Taunt("I WON'T DIE WITHOUT A FIGHT!!"),
                    new TimedTransition(3000, "attack4")
                    ),
                new State("attack4",
                    new Chase(2, 12, duration: 8, coolDown: 2000),
                    new Shoot(25, projectileIndex: 0, count: 10, shootAngle: 45, coolDown: 1500, coolDownOffset: 500),
                    new Shoot(25, projectileIndex: 0, count: 4, shootAngle: 15, coolDown: 1000, coolDownOffset: 1000),
                    new Shoot(25, projectileIndex: 2, count: 5, shootAngle: 30, coolDown: 3000, coolDownOffset: 1500),
                    new Shoot(25, projectileIndex: 1, count: 1, coolDown: 2800, coolDownOffset: 500),
                    new Shoot(25, projectileIndex: 3, count: 1, predictive: 0.8, coolDown: 1000, coolDownOffset: 2500),
                    new Grenade(5, 150, 5, 0, coolDown: 3500, color: 0x00ffff),
                    new Grenade(5, 150, 10, 90, coolDown: 3500, color: 0x00ffff),
                    new Grenade(5, 150, 5, 180, coolDown: 3500, color: 0x00ffff),
                    new Grenade(5, 150, 10, 270, coolDown: 3500, color: 0x00ffff)
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
                new ItemLoot("Visage of the Frozen", 0.0016),
                new ItemLoot("Queen's Guardian Signet", 0.0016),
                new ItemLoot("Magic Dust", 0.5)
                ),
             new Threshold(0.03,
                new ItemLoot("PermaFrost GreatShield", 0.001),
                new ItemLoot("Axe of the Frozen Tundra", 0.001)
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
                    new TimedTransition(10000, "attack1")
                    ),
                new State("attack1",
                    new Taunt("Hello friend. You seem to be lost. LET ME SHOW YOU THE WAY OUT!"),
                    new Chase(8),
                    new Shoot(8, 8, projectileIndex: 1, shootAngle: 10, coolDown: 2000),
                    new Shoot(20, 6, projectileIndex: 2, coolDown: 600),
                    new TossObject("Evil Snowman", 5, 0, coolDown: 10000),
                    new TossObject("Evil Snowman", 8, 60, coolDown: 10000),
                    new TossObject("Evil Snowman", 3, 120, coolDown: 10000),
                    new TossObject("Evil Snowman", 4, 180, coolDown: 10000),
                    new TossObject("Evil Snowman", 8, 240, coolDown: 10000),
                    new TossObject("Evil Snowman", 5, 300, coolDown: 10000),
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
                    new EntitiesNotExistsTransition(100, "freeze", "Ice Tower", "Ice Tower 1", "Ice Tower 2", "Ice Tower 3", "Ice Tower 4", "Ice Tower 5")
                    ),
                new State("freeze",
                    new Taunt("FREEZE!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Grenade(10, 60, 10, fixedAngle: 0, 1000, ConditionEffectIndex.Paralyzed, 1200, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 22.5, 1000, ConditionEffectIndex.Paralyzed, 1200, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 45, 1000, ConditionEffectIndex.Paralyzed, 1200, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 67.5, 1000, ConditionEffectIndex.Paralyzed, 1200, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 90, 1000, ConditionEffectIndex.Paralyzed, 1200, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 112.5, 1000, ConditionEffectIndex.Paralyzed, 1200, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 135, 1000, ConditionEffectIndex.Paralyzed, 1200, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 157.5, 1000, ConditionEffectIndex.Paralyzed, 1200, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 180, 1000, ConditionEffectIndex.Paralyzed, 1200, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 202.5, 1000, ConditionEffectIndex.Paralyzed, 1200, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 225, 1000, ConditionEffectIndex.Paralyzed, 1200, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 247.5, 1000, ConditionEffectIndex.Paralyzed, 1200, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 270, 1000, ConditionEffectIndex.Paralyzed, 1200, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 292.5, 1000, ConditionEffectIndex.Paralyzed, 1200, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 315, 1000, ConditionEffectIndex.Paralyzed, 500, color: 0x5279FD),
                    new Grenade(10, 60, 10, fixedAngle: 337.5, 1000, ConditionEffectIndex.Paralyzed, 100, color: 0x5279FD),
                    new TimedTransition(10000, "tentacles")
                    ),
                new State("tentacles",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Reproduce("Evil Snowman", 15, 5, coolDown: 5000),
                    new Reproduce("Evil Snowman", 15, 5, coolDown: 5000),
                    new Reproduce("Evil Snowman", 15, 5, coolDown: 5000),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, predictive: 1.2, coolDown: 1200),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 3, coolDown: 8600),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 5, coolDown: 8600, coolDownOffset: 200),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 7, coolDown: 8600, coolDownOffset: 400),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 9, coolDown: 8600, coolDownOffset: 600),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 11, coolDown: 8600, coolDownOffset: 800),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 13, coolDown: 8600, coolDownOffset: 1000),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 15, coolDown: 8600, coolDownOffset: 1200),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 17, coolDown: 8600, coolDownOffset: 1400),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 19, coolDown: 8600, coolDownOffset: 1600),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 21, coolDown: 8600, coolDownOffset: 1800),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 23, coolDown: 8600, coolDownOffset: 2000),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 25, coolDown: 8600, coolDownOffset: 2200),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 27, coolDown: 8600, coolDownOffset: 2400),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 29, coolDown: 8600, coolDownOffset: 2600),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 31, coolDown: 8600, coolDownOffset: 2800),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 33, coolDown: 8600, coolDownOffset: 3000),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 35, coolDown: 8600, coolDownOffset: 3200),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 37, coolDown: 8600, coolDownOffset: 3400),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 39, coolDown: 8600, coolDownOffset: 3600),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 41, coolDown: 8600, coolDownOffset: 3800),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 43, coolDown: 8600, coolDownOffset: 4000),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 45, coolDown: 8600, coolDownOffset: 4200),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 47, coolDown: 8600, coolDownOffset: 4400),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 49, coolDown: 8600, coolDownOffset: 4600),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 51, coolDown: 8600, coolDownOffset: 4800),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 53, coolDown: 8600, coolDownOffset: 5000),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 55, coolDown: 8600, coolDownOffset: 5200),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 57, coolDown: 8600, coolDownOffset: 5400),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 59, coolDown: 8600, coolDownOffset: 5600),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 61, coolDown: 8600, coolDownOffset: 5800),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 63, coolDown: 8600, coolDownOffset: 6000),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 65, coolDown: 8600, coolDownOffset: 6200),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 67, coolDown: 8600, coolDownOffset: 6400),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 69, coolDown: 8600, coolDownOffset: 6600),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 71, coolDown: 8600, coolDownOffset: 6800),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 73, coolDown: 8600, coolDownOffset: 7000),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 75, coolDown: 8600, coolDownOffset: 7200),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 77, coolDown: 8600, coolDownOffset: 7400),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 79, coolDown: 8600, coolDownOffset: 7600),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 81, coolDown: 8600, coolDownOffset: 7800),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 83, coolDown: 8600, coolDownOffset: 8000),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 85, coolDown: 8600, coolDownOffset: 8200),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 87, coolDown: 8600, coolDownOffset: 8400),
                    new Shoot(25, 10, projectileIndex: 2, fixedAngle: 89, coolDown: 8600, coolDownOffset: 8600),
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
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new PlayerWithinTransition(20, "prepare")
                    ),
                new State("prepare",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Taunt("I am one of the protectors of the key! Defeat me and gain access to what lies ahead!"),
                    new TimedTransition(5000, "changesize")
                    ),
                new State("changesize",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Taunt("RAHHHH"),
                    new Flash(0xCC1A1A, 0.5, 12),
                    new ChangeSize(5, 180),
                    new TimedTransition(5000, "attack1")
                    ),
                new State("attack1",
                    new Wander(0.5),
                    new Shoot(15, 2, shootAngle: 10, projectileIndex: 0, predictive: .9, coolDown: 400),
                    new Shoot(20, 6, projectileIndex: 1, coolDown: 1000),
                    new TossObject("Cursed Polar Bear", 5, 90, coolDown: 20000),
                    new TossObject("Cursed Polar Bear", 5, 180, coolDown: 20000),
                    new HpLessTransition(.4, "attack2")
                    ),
                new State("attack2",
                    new Taunt("DIE!"),
                    new Follow(3, 15, 10, 5, coolDown: 1200),
                    new Shoot(6, 3, shootAngle: 15, projectileIndex: 2, predictive: .8, coolDown: 500),
                    new Shoot(7, 12, projectileIndex: 1, coolDown: 1000),
                    new TossObject("Frozen Elf", 5, 0, coolDown: 5000),
                    new TossObject("Frozen Elf", 5, 60, coolDown: 5500),
                    new TossObject("Frozen Elf", 5, 120, coolDown: 6000),
                    new TossObject("Frozen Elf", 5, 180, coolDown: 6500),
                    new HpLessTransition(.1, "granted")
                    ),
                new State("granted",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("I shall grant you access!"),
                    new TimedTransition(5000, "changesize1")
                    ),
                new State("changesize1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new ChangeSize(5, 130),
                    new Flash(0xCC1A1A, 0.5, 12),
                    new TimedTransition(5000, "granted1")
                    ),
                new State("granted1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Taunt("I have failed you!"),
                    new Shoot(12, 8, projectileIndex: 1, shootAngle: 45, coolDown: 5000),
                    new Suicide()
                    )
                )
            )
        .Init("Cursed Snowman Switch", //finished
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
                    new Taunt("I see you have obtained 1 of the 2 keys.."),
                    new Taunt("This time.. it won't be so easy!"),
                    new TimedTransition(10000, "attack1")
                    ),
                new State("attack1",
                    new Follow(3, 20, 5, duration: 10, coolDown: 3000),
                    new Shoot(8, 4, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 800),
                    new Shoot(6, 8, shootAngle: 5, projectileIndex: 1, predictive: .8, coolDown: 600),
                    new Shoot(20, 12, projectileIndex: 0, coolDown: 1000),
                    new HpLessTransition(.6, "rage")
                    ),
                new State("rage",
                    new ReturnToSpawn(4, 0),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Shoot(25, 32, predictive: 1, projectileIndex: 1, coolDown: 1000),
                    new Shoot(25, 23, predictive: 1, projectileIndex: 0, coolDown: 1200),
                    new TossObject("Frozen Tower", 8, 45, coolDown: 999999),
                    new TossObject("Frozen Tower", 8, 135, coolDown: 999999),
                    new TossObject("Frozen Tower", 8, 225, coolDown: 999999),
                    new TossObject("Frozen Tower", 8, 315, coolDown: 999999),
                    new TimedTransition(15000, "freedps")
                    ),
                new State("freedps",
                    new Shoot(25, 12, predictive: 1, coolDown: 400),
                    new Taunt("HAHAHAHAHAHAH"),
                    new HpLessTransition(.4, "attack2")
                    ),
                new State("attack2",
                    new Chase(2),
                    new Shoot(8, 5, shootAngle: 10, predictive: 1.1, projectileIndex: 0, coolDown: 300),
                    new Shoot(15, 1, projectileIndex: 1, predictive: 1.6, coolDown: 300),
                    new HpLessTransition(.1, "granted")
                    ),
                new State("granted",
                    new ReturnToSpawn(3),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Taunt("I have failed you my master.. the gate has been OPENNED!"),
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
                    new Chase(5, 15, coolDown: 1500),
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
                    new TossObject("Enraged Cub", range: 3, angle: 90, coolDown: 15000),
                    new TossObject("Enraged Cub", range: 3, angle: 180, coolDown: 15000),
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
                    new TossObject2("Frozen Elf", range: 3, coolDown: 1500, randomToss: true),
                    new Shoot(20, 2, projectileIndex: 0, shootAngle: 5, predictive: .9, coolDown: 200),
                    new Shoot(15, 8, projectileIndex: 1, predictive: 1, coolDown: 1000)
                    )
                )
            )
        .Init("Frozen Elf",
            new State(
                new ScaleHP2(20),
                new State("explodeprep",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Taunt("explode!"),
                    new Flash(0xCC1A1A, 0.5, 12),
                    new TimedTransition(650, "explode")
                    ),
                new State("explode",
                    new Shoot(4, 12, projectileIndex: 0, coolDown: 1000),
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