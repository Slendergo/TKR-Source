using TKR.Shared.resources;
using TKR.WorldServer.logic.loot;
using TKR.WorldServer.logic.behaviors;
using TKR.WorldServer.logic.transitions;

namespace TKR.WorldServer.logic
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
        .Init("Snowball Roll",
            new State(
                new SetAltTexture(0, 3, 100, true),
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Move",
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 200),
                    new MoveLine(1, 90),
                    new OnGroundTransition("Snow Replace Snowball", "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Snowball Roll 1",
            new State(
                new SetAltTexture(0, 3, 100, true),
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Move",
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 200),
                    new MoveLine(1, 180),
                    new OnGroundTransition("Snow Replace Snowball", "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Snowball Roll 2",
            new State(
                new SetAltTexture(0, 3, 100, true),
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Move",
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 200),
                    new MoveLine(1, 270),
                    new OnGroundTransition("Snow Replace Snowball", "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Snowball Roll 3",
            new State(
                new SetAltTexture(0, 3, 100, true),
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Move",
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 200),
                    new MoveLine(1, 0),
                    new OnGroundTransition("Snow Replace Snowball", "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Snowball Roll 4",
            new State(
                new SetAltTexture(0, 3, 100, true),
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Move",
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 200),
                    new MoveLine(1, 45),
                    new OnGroundTransition("Snow Replace Snowball", "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Snowball Roll 5",
            new State(
                new SetAltTexture(0, 3, 100, true),
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Move",
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 200),
                    new MoveLine(1, 135),
                    new OnGroundTransition("Snow Replace Snowball", "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Snowball Roll 6",
            new State(
                new SetAltTexture(0, 3, 100, true),
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Move",
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 200),
                    new MoveLine(1, 225),
                    new OnGroundTransition("Snow Replace Snowball", "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Snowball Roll 7",
            new State(
                new SetAltTexture(0, 3, 100, true),
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Move",
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 200),
                    new MoveLine(1, 315),
                    new OnGroundTransition("Snow Replace Snowball", "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Snowball Spawner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Check Player",
                    new PlayerWithinTransition(100, "Start")
                    ),
                new State("Start",
                    new Reproduce("Snowball Roll", 20, 1, coolDown: 600),
                    new Reproduce("Snowball Roll 1", 20, 1, coolDown: 600),
                    new Reproduce("Snowball Roll 2", 20, 1, coolDown: 600),
                    new Reproduce("Snowball Roll 3", 20, 1, coolDown: 600),
                    new Reproduce("Snowball Roll 4", 20, 1, coolDown: 600),
                    new Reproduce("Snowball Roll 5", 20, 1, coolDown: 600),
                    new Reproduce("Snowball Roll 6", 20, 1, coolDown: 600),
                    new Reproduce("Snowball Roll 7", 20, 1, coolDown: 600),
                    new EntitiesNotExistsTransition(25, "die", "Corrupt Snowman Switch")
                    ),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("die",
                    new Suicide()

                    )
                )
            )
        .Init("Snowball Spawner TP",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Check Player",
                    new PlayerWithinTransition(100, "Start")
                    ),
               new State("Start",
                    new Reproduce("Snowball Roll 4", 20, 1, coolDown: 200),
                    new Reproduce("Snowball Roll 5", 20, 1, coolDown: 200),
                    new Reproduce("Snowball Roll 6", 20, 1, coolDown: 200),
                    new Reproduce("Snowball Roll 7", 20, 1, coolDown: 200)
                    )
                )
            )
        .Init("Snowball Spawner BT",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Check Player",
                    new PlayerWithinTransition(100, "Start")
                    ),
                new State("Start",
                    new Reproduce("Snowball Roll", 20, 1, coolDown: 200),
                    new Reproduce("Snowball Roll 1", 20, 1, coolDown: 200),
                    new Reproduce("Snowball Roll 2", 20, 1, coolDown: 200),
                    new Reproduce("Snowball Roll 3", 20, 1, coolDown: 200)
                    )
                )
            )
        .Init("Snowball Spawner LF",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Check Player",
                    new PlayerWithinTransition(100, "Start")
                    ),
                new State("Start",
                    new Reproduce("Snowball Roll", 20, 1, coolDown: 200),
                    new Reproduce("Snowball Roll 1", 20, 1, coolDown: 200),
                    new Reproduce("Snowball Roll 2", 20, 1, coolDown: 200),
                    new Reproduce("Snowball Roll 3", 20, 1, coolDown: 200)
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
        .Init("Water Creator",
            new State(
                new State("idl",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("idle",
                    new Orbit(1, 8, 15, "Cursed Snowman Switch", speedVariance: 0, radiusVariance: 0),
                    new ReplaceTile("Snow", "Black Water Frozen", 1)
                    )
                )
            )
        .Init("Water Creator 1",
            new State(
                new State("idl",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("idle",
                    new Orbit(2, 8, 15, "Cursed Snowman Switch", speedVariance: 0, radiusVariance: 0),
                    new ReplaceTile("Black Water Frozen", "Snow", 1)
                    )
                )
            )
        .Init("Water Creator 2",
            new State(
                new State("idl",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("idle",
                    new Orbit(1, 8, 15, "Cursed Snowman Switch", speedVariance: 0, radiusVariance: 0),
                    new ReplaceTile("Snow", "Black Water Frozen", 1)
                    )
                )
            )
        .Init("Water Creator 3",
            new State(
                new State("idl",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("idle",
                    new Orbit(2, 8, 15, "Cursed Snowman Switch", speedVariance: 0, radiusVariance: 0),
                    new ReplaceTile("Black Water Frozen", "Snow", 1)
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
                    new Shoot(6, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(6, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 600, predictive: 1.2)
                    )
                )
            )
        .Init("Snowy Turret",
            new State(
                new State("Shoot",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Shoot(8, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(8, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000),
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
                    new Shoot(8, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(8, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000),
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
                    new Shoot(8, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(8, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000),
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
                    new Shoot(8, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(8, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000),
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
                    new Shoot(8, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(8, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000),
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
                new DropPortalOnDeath("Cave of Eternal Frost Portal", 0.8),
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
                new ItemLoot("Frozen Coin", 0.05),
                new ItemLoot("Magic Dust", 0.5)
                )
            )
        .Init("Abominable Snowman",
            new State(
                new ScaleHP2(20),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new PlayerWithinTransition(10, "prepattack")
                    ),
                 new State("prepattack",
                    new Spawn("Ice Wall Spawner 1", 1, 1, coolDown: 999999),
                    new TimedTransition(5000, "attack")
                    ),
                new State("attack",
                    new Spawn("Snowball Spawner", 1, 1, coolDown: 999999),
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
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
                    new TossObject2("Frozen Elf", 6, 60, coolDown: 3000),
                    new TossObject2("Frozen Elf", 9, 120, coolDown: 3000),
                    new TossObject2("Frozen Elf", 6, 180, coolDown: 3000),
                    new TossObject2("Frozen Elf", 9, 240, coolDown: 3000),
                    new TossObject2("Frozen Elf", 6, 300, coolDown: 3000),
                    new TossObject2("Frozen Elf", 9, 360, coolDown: 3000),
                    new Shoot(15, 3, shootAngle: 10, projectileIndex: 4, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(15, 5, shootAngle: 10, projectileIndex: 5, coolDown: 2000, coolDownOffset: 600),
                    new Shoot(15, 7, shootAngle: 10, projectileIndex: 4, coolDown: 2000, coolDownOffset: 1200),
                    new Shoot(30, 3, shootAngle: 10, projectileIndex: 4, coolDown: 1200),
                    new TimedTransition(10000, "attack2.1")
                    ),
                new State("attack2.1",
                    new HpLessTransition(0.5, "freedps"),
                    new Orbit(4, 8, 10, "Snowball Spawner", speedVariance: 0, radiusVariance: 0),
                    new Shoot(15, 3, shootAngle: 10, projectileIndex: 4, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(15, 5, shootAngle: 10, projectileIndex: 5, coolDown: 2000, coolDownOffset: 600),
                    new Shoot(15, 7, shootAngle: 10, projectileIndex: 4, coolDown: 2000, coolDownOffset: 1200),
                    new TimedTransition(10000, "attack3")
                    ),
                 new State("attack3",
                    new Orbit(3, 8, 10, "Snowball Spawner", speedVariance: 0, radiusVariance: 0, orbitClockwise: true),
                    new TossObject2("Frozen Elf", 6, 60, coolDown: 3000),
                    new TossObject2("Frozen Elf", 9, 120, coolDown: 3000),
                    new TossObject2("Frozen Elf", 6, 180, coolDown: 3000),
                    new TossObject2("Frozen Elf", 9, 240, coolDown: 3000),
                    new TossObject2("Frozen Elf", 6, 300, coolDown: 3000),
                    new TossObject2("Frozen Elf", 9, 360, coolDown: 3000),
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
                new ItemLoot("Greater Potion of Vitality", 1),
                new ItemLoot("Greater Potion of Mana", 1),
                new ItemLoot("Magic Dust", 0.5),
                new ItemLoot("Frozen Coin", 0.05),
                new ItemLoot("Glowing Talisman", 0.0014)
                ),
             new Threshold(0.03,
                new ItemLoot("Winter Solstice", 0.0003),
                new ItemLoot("Polar Vortex", 0.0003),
                new ItemLoot("Iceberg", 0.0003)
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
            new Threshold(0.03,
                new TierLoot(14, ItemType.Weapon, 0.05),
                new TierLoot(14, ItemType.Armor, 0.05),
                new TierLoot(6, ItemType.Ring, 0.05),
                new TierLoot(6, ItemType.Ability, 0.07),
                new ItemLoot("Greater Potion of Vitality", 1),
                new ItemLoot("Greater Potion of Mana", 1),
                new ItemLoot("Special Dust", 0.01)
                ),
             new Threshold(0.03,
                new ItemLoot("Winter Solstice", 0.000125),
                new ItemLoot("Polar Vortex", 0.000125),
                new ItemLoot("Iceberg", 0.000125),

                new ItemLoot("Condemned Frostbite", 0.000125),
                new ItemLoot("The Expansion", 0.000125),
                new ItemLoot("The Northern Star", 0.000125),

                new ItemLoot("Snow Angle", 0.000125),
                new ItemLoot("World of Ice", 0.000125),

                new ItemLoot("Agdluak", 0.000125),
                new ItemLoot("Absolute Zero", 0.000125),
                new ItemLoot("Cryogenic Freeze", 0.000125),
                new ItemLoot("Frozen Coin", 0.05),
                new ItemLoot("Glowing Talisman", 0.0014)
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
                    new TossObject("Queen of Ice Clone", 12, 45, coolDown: 999999),
                    new TossObject("Queen of Ice Clone 2", 12, 135, coolDown: 999999),
                    new TossObject("Queen of Ice Clone 3", 12, 225, coolDown: 999999),
                    new TossObject("Queen of Ice Clone 4", 12, 315, coolDown: 999999),
                    new Taunt("You've killed my beloved Yeti!"),
                    new TimedTransition(5000, "scream")
                    ),
                new State("scream",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Shoot(12, 3, projectileIndex: 1, shootAngle: 15, coolDown: 4000, coolDownOffset: 0),
                    new Shoot(12, 5, projectileIndex: 5, shootAngle: 15, coolDown: 4000, coolDownOffset: 0),
                    new Shoot(12, 7, projectileIndex: 6, shootAngle: 15, coolDown: 4000, coolDownOffset: 0),
                    new Shoot(12, 9, projectileIndex: 1, shootAngle: 15, coolDown: 4000, coolDownOffset: 1000),
                    new Shoot(12, 11, projectileIndex: 5, shootAngle: 15, coolDown: 4000, coolDownOffset: 1000),
                    new Shoot(12, 13, projectileIndex: 6, shootAngle: 15, coolDown: 4000, coolDownOffset: 1000),
                    new Shoot(12, 15, projectileIndex: 1, shootAngle: 15, coolDown: 4000, coolDownOffset: 2000),
                    new Shoot(12, 17, projectileIndex: 5, shootAngle: 15, coolDown: 4000, coolDownOffset: 2000),
                    new Shoot(12, 19, projectileIndex: 6, shootAngle: 15, coolDown: 4000, coolDownOffset: 2000),
                    new TimedTransition(6000, "attack1")
                    ),
                new State("attack1",
                    new OrderOnce(30, "Queen of Ice Clone", "attack"),
                    new OrderOnce(30, "Queen of Ice Clone 2", "attack"),
                    new OrderOnce(30, "Queen of Ice Clone 3", "attack"),
                    new OrderOnce(30, "Queen of Ice Clone 4", "attack"),
                    new StayCloseToSpawn(2, 4),
                    new Wander(0.3),
                    new Shoot(12, 11, projectileIndex: 1, coolDown: 800),
                    new Shoot(20, 13, projectileIndex: 2, coolDown: 1000),
                    new HpLessTransition(.85, "return to spawn")
                    ),
                new State("return to spawn",
                    new ReturnToSpawn(1.3, 0),
                    new InvisiToss("Evil Snowman", 6, 60, coolDown: 999999),
                    new InvisiToss("Evil Snowman", 6, 120, coolDown: 999999),
                    new InvisiToss("Evil Snowman", 6, 180, coolDown: 999999),
                    new InvisiToss("Evil Snowman", 6, 240, coolDown: 999999),
                    new InvisiToss("Evil Snowman", 6, 300, coolDown: 999999),
                    new InvisiToss("Evil Snowman", 6, 360, coolDown: 999999),
                    new Taunt("You have damaged me!"),
                    new TimedTransition(1400, "Protection")
                    ),
                new State("Protection",
                    new ReturnToSpawn(3, 0),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("Guardians! Protect me!"),
                    new OrderOnce(30, "Evil Snowman", "attack2"),
                    new TossObject("Ice Tower", 8, angle: 0, coolDown: 999999),
                    new TossObject("Ice Tower 1", 8, angle: 60, coolDown: 999999),
                    new TossObject("Ice Tower 2", 8, angle: 120, coolDown: 999999),
                    new TossObject("Ice Tower 3", 8, angle: 180, coolDown: 999999),
                    new TossObject("Ice Tower 4", 8, angle: 240, coolDown: 999999),
                    new TossObject("Ice Tower 5", 8, angle: 300, coolDown: 999999),
                    new EntityExistsTransition("Ice Tower", 10, "waiting")
                    ),
                new State("waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    //new Shoot(12, 2, projectileIndex: 1, shootAngle: 15, predictive: .4, coolDown: 400),
                    //new Shoot(12, 3, projectileIndex: 0, shootAngle: 20, fixedAngle: 0, coolDown: 200),
                    //new Shoot(12, 2, projectileIndex: 0, shootAngle: 21, fixedAngle: 60, coolDown: 200),
                    //new Shoot(12, 3, projectileIndex: 0, shootAngle: 15, fixedAngle: 120, coolDown: 200),
                    //new Shoot(12, 3, projectileIndex: 0, shootAngle: 16, fixedAngle: 180, coolDown: 200),
                    //new Shoot(12, 2, projectileIndex: 0, shootAngle: 13, fixedAngle: 240, coolDown: 200),
                    //new Shoot(12, 3, projectileIndex: 0, shootAngle: 22, fixedAngle: 300, coolDown: 200),
                    //new Shoot(10, 6, projectileIndex: 4, coolDown: 800),
                    new EntitiesNotExistsTransition(100, "tentacles", "Ice Tower", "Ice Tower 1", "Ice Tower 2", "Ice Tower 3", "Ice Tower 4", "Ice Tower 5")
                    ),
                new State("tentacles",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new TimedTransition(1000, "tentacles2")
                    ),
                new State("tentacles2",
                    new OrderOnce(30, "Queen of Ice Clone", "idle"),
                    new OrderOnce(30, "Queen of Ice Clone 2", "idle"),
                    new OrderOnce(30, "Queen of Ice Clone 3", "idle"),
                    new OrderOnce(30, "Queen of Ice Clone 4", "idle"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Shoot(20, 4, shootAngle: 12, projectileIndex: 3, coolDown: 800),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 0, coolDown: 3700, rotateAngle: 3, coolDownOffset: 100),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 5, coolDown: 3700, rotateAngle: 3, coolDownOffset: 200),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 10, coolDown: 3700, rotateAngle: 3, coolDownOffset: 300),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 15, coolDown: 3700, rotateAngle: 3, coolDownOffset: 400),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 20, coolDown: 3700, rotateAngle: 3, coolDownOffset: 500),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 25, coolDown: 3700, rotateAngle: 3, coolDownOffset: 600),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 30, coolDown: 3700, rotateAngle: 3, coolDownOffset: 700),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 35, coolDown: 3700, rotateAngle: 3, coolDownOffset: 800),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 40, coolDown: 3700, rotateAngle: 3, coolDownOffset: 900),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 45, coolDown: 3700, rotateAngle: 3, coolDownOffset: 1000),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 50, coolDown: 3700, rotateAngle: 3, coolDownOffset: 1100),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 55, coolDown: 3700, rotateAngle: 3, coolDownOffset: 1200),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 60, coolDown: 3700, rotateAngle: 3, coolDownOffset: 1300),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 65, coolDown: 3700, rotateAngle: 3, coolDownOffset: 1400),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 70, coolDown: 3700, rotateAngle: 3, coolDownOffset: 1500),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 75, coolDown: 3700, rotateAngle: 3, coolDownOffset: 1600),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 80, coolDown: 3700, rotateAngle: 3, coolDownOffset: 1700),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 85, coolDown: 3700, rotateAngle: 3, coolDownOffset: 1800),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 90, coolDown: 3700, rotateAngle: 3, coolDownOffset: 1900),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 95, coolDown: 3700, rotateAngle: 3, coolDownOffset: 2000),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 100, coolDown: 3700, rotateAngle: 3, coolDownOffset: 2100),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 105, coolDown: 3700, rotateAngle: 3, coolDownOffset: 2200),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 110, coolDown: 3700, rotateAngle: 3, coolDownOffset: 2300),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 115, coolDown: 3700, rotateAngle: 3, coolDownOffset: 2400),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 120, coolDown: 3700, rotateAngle: 3, coolDownOffset: 2500),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 125, coolDown: 3700, rotateAngle: 3, coolDownOffset: 2600),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 130, coolDown: 3700, rotateAngle: 3, coolDownOffset: 2700),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 135, coolDown: 3700, rotateAngle: 3, coolDownOffset: 2800),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 140, coolDown: 3700, rotateAngle: 3, coolDownOffset: 2900),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 145, coolDown: 3700, rotateAngle: 3, coolDownOffset: 3000),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 150, coolDown: 3700, rotateAngle: 3, coolDownOffset: 3100),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 155, coolDown: 3700, rotateAngle: 3, coolDownOffset: 3200),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 160, coolDown: 3700, rotateAngle: 3, coolDownOffset: 3300),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 165, coolDown: 3700, rotateAngle: 3, coolDownOffset: 3400),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 170, coolDown: 3700, rotateAngle: 3, coolDownOffset: 3500),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 175, coolDown: 3700, rotateAngle: 3, coolDownOffset: 3600),
                    new Shoot(30, 5, projectileIndex: 2, fixedAngle: 180, coolDown: 3700, rotateAngle: 3, coolDownOffset: 3700),

                    new TimedTransition(10000, "scream2")
                    ),
                new State("scream2",
                    new RemoveEntity(20, "Evil Snowman"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Taunt("ARHHHHH"),
                    new Flash(0x5279FD, 0.5, 12),
                    new ChangeSize(5, 200),
                    new TimedTransition(5000, "attackoftheminions")
                    ),
                new State("attackoftheminions",
                    new Wander(0.3),
                    new OrderOnce(30, "Queen of Ice Clone", "attack"),
                    new OrderOnce(30, "Queen of Ice Clone 2", "attack"),
                    new OrderOnce(30, "Queen of Ice Clone 3", "attack"),
                    new OrderOnce(30, "Queen of Ice Clone 4", "attack"),
                    new Shoot(12, 13, projectileIndex: 5, coolDown: 1000),
                    new HpLessTransition(.7, "healing")
                    ),
                new State("healing",
                    new ReturnToSpawn(1.3, 0),
                    new Taunt("THE COLD MAKES ME STRONGER!!!!"),
                    new HealSelf(coolDown: 5000, amount: 10, percentage: true),
                    new HpLessTransition(.6, "prepareattack2")
                    ),
                new State("prepareattack2",
                    new InvisiToss("Evil Snowman", 6, 60, coolDown: 999999),
                    new InvisiToss("Evil Snowman", 6, 120, coolDown: 999999),
                    new InvisiToss("Evil Snowman", 6, 180, coolDown: 999999),
                    new InvisiToss("Evil Snowman", 6, 240, coolDown: 999999),
                    new InvisiToss("Evil Snowman", 6, 300, coolDown: 999999),
                    new InvisiToss("Evil Snowman", 6, 360, coolDown: 999999),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new TimedTransition(1000, "attack2")
                    ),
                new State("attack2",
                    new OrderOnce(30, "Evil Snowman", "attack2"),
                    new OrderOnce(30, "Queen of Ice Clone", "idle"),
                    new OrderOnce(30, "Queen of Ice Clone 2", "idle"),
                    new OrderOnce(30, "Queen of Ice Clone 3", "idle"),
                    new OrderOnce(30, "Queen of Ice Clone 4", "idle"),
                    new HpLessTransition(.45, "guards2"),
                    new Taunt("I’VE GOT PLENTY MORE TRICKS UP MY SLEEVE!"),
                    new Shoot(13, 40, projectileIndex: 3, coolDown: 1200),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 200),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 216, coolDown: 400),
                    new Grenade(radius: 4, damage: 200, range: 6, fixedAngle: 36, coolDown: 99999, effect: ConditionEffectIndex.Darkness, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 200, range: 12, fixedAngle: 36, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.1")
                    ),
                new State("attack2.1",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(13, 40, projectileIndex: 3, coolDown: 1200),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 252, coolDown: 300),
                    new Grenade(radius: 4, damage: 200, range: 12, fixedAngle: 72, coolDown: 99999, effect: ConditionEffectIndex.Darkness, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 200, range: 6, fixedAngle: 72, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.2")
                    ),
                new State("attack2.2",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(13, 40, projectileIndex: 3, coolDown: 1200),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 288, coolDown: 300),
                    new Grenade(radius: 4, damage: 200, range: 12, fixedAngle: 108, coolDown: 99999, effect: ConditionEffectIndex.Darkness, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 200, range: 6, fixedAngle: 108, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.3")
                    ),
                new State("attack2.3",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(13, 40, projectileIndex: 3, coolDown: 1200),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 324, coolDown: 300),
                    new Grenade(radius: 4, damage: 200, range: 12, fixedAngle: 144, coolDown: 99999, effect: ConditionEffectIndex.Darkness, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 200, range: 6, fixedAngle: 144, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.4")
                    ),
                new State("attack2.4",
                    new HpLessTransition(.45, "guards2"),
                    new Taunt("I’VE GOT PLENTY MORE TRICKS UP MY SLEEVE!"),
                    new Shoot(13, 40, projectileIndex: 3, coolDown: 1200),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 0, coolDown: 300),
                    new Grenade(radius: 4, damage: 200, range: 12, fixedAngle: 180, coolDown: 99999, effect: ConditionEffectIndex.Darkness, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 200, range: 6, fixedAngle: 180, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.5")
                    ),
                new State("attack2.5",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(13, 40, projectileIndex: 3, coolDown: 1200),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 36, coolDown: 300),
                    new Grenade(radius: 4, damage: 200, range: 12, fixedAngle: 216, coolDown: 99999, effect: ConditionEffectIndex.Darkness, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 200, range: 6, fixedAngle: 216, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.6")
                    ),
                new State("attack2.6",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(13, 40, projectileIndex: 3, coolDown: 1200),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 72, coolDown: 300),
                    new Grenade(radius: 4, damage: 200, range: 12, fixedAngle: 252, coolDown: 99999, effect: ConditionEffectIndex.Darkness, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 200, range: 6, fixedAngle: 252, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.7")
                    ),
                new State("attack2.7",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(13, 40, projectileIndex: 3, coolDown: 1200),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 108, coolDown: 300),
                    new Grenade(radius: 4, damage: 200, range: 12, fixedAngle: 288, coolDown: 99999, effect: ConditionEffectIndex.Darkness, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 200, range: 6, fixedAngle: 288, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.8")
                    ),
                new State("attack2.8",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(13, 40, projectileIndex: 3, coolDown: 1200),
                    new Shoot(12, 2, projectileIndex: 1, shootAngle: 12, predictive: .5, coolDown: 100),
                    new Shoot(20, 4, shootAngle: 10, projectileIndex: 3, fixedAngle: 144, coolDown: 300),
                    new Grenade(radius: 4, damage: 200, range: 12, fixedAngle: 324, coolDown: 99999, effect: ConditionEffectIndex.Darkness, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 200, range: 6, fixedAngle: 324, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2.9")
                    ),
                new State("attack2.9",
                    new HpLessTransition(.45, "guards2"),
                    new Shoot(13, 40, projectileIndex: 3, coolDown: 1200),
                    new Shoot(15, 3, projectileIndex: 0, coolDown: 800),
                    new Grenade(radius: 4, damage: 200, range: 12, fixedAngle: 360, coolDown: 99999, effect: ConditionEffectIndex.Darkness, effectDuration: 3000, color: 0x5279FD),
                    new Grenade(radius: 4, damage: 200, range: 8, fixedAngle: 360, coolDown: 99999, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x5279FD),
                    new TimedTransition(400, "attack2")
                    ),
                new State("guards2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("Guardians! Protect me!"),
                    new OrderOnce(30, "Queen of Ice Clone", "attack"),
                    new OrderOnce(30, "Queen of Ice Clone 2", "attack"),
                    new OrderOnce(30, "Queen of Ice Clone 3", "attack"),
                    new OrderOnce(30, "Queen of Ice Clone 4", "attack"),
                    new TossObject("Ice Tower", 8, angle: 0, coolDown: 99999),
                    new TossObject("Ice Tower 1", 8, angle: 60, coolDown: 99999),
                    new TossObject("Ice Tower 2", 8, angle: 120, coolDown: 99999),
                    new TossObject("Ice Tower 3", 8, angle: 180, coolDown: 99999),
                    new TossObject("Ice Tower 4", 8, angle: 240, coolDown: 99999),
                    new TossObject("Ice Tower 5", 8, angle: 300, coolDown: 99999),
                    new EntityExistsTransition("Ice Tower", 10, "waiting2")
                    ),
                new State("waiting2",
                    new Shoot(10, 6, projectileIndex: 4, coolDown: 400),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new EntitiesNotExistsTransition(100, "attack4", "Ice Tower", "Ice Tower 1", "Ice Tower 2", "Ice Tower 3", "Ice Tower 4", "Ice Tower 5")
                    ),
                new State("attack4",
                    new GroundTransform("Lava", relativeX: +10, relativeY: +10, persist: false),
                    new GroundTransform("Lava", relativeX: +11, relativeY: +11, persist: false),
                    new GroundTransform("Lava", relativeX: +12, relativeY: +12, persist: false),
                    new GroundTransform("Lava", relativeX: -10, relativeY: -10, persist: false),
                    new GroundTransform("Lava", relativeX: -11, relativeY: -11, persist: false),
                    new GroundTransform("Lava", relativeX: -12, relativeY: -12, persist: false),
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
            new Threshold(0.001,
                new TierLoot(14, ItemType.Weapon, 0.05),
                new TierLoot(14, ItemType.Armor, 0.05),
                new TierLoot(6, ItemType.Ring, 0.05),
                new TierLoot(6, ItemType.Ability, 0.07),
                new ItemLoot("Greater Potion of Speed", 1),
                new ItemLoot("Greater Potion of Life", 1),
                new ItemLoot("Frozen Coin", 0.01)
                ),
             new Threshold(0.03,
                new ItemLoot("Condemned Frostbite", 0.0003),
                new ItemLoot("The Expansion", 0.0003),
                new ItemLoot("The Northern Star", 0.0003),
                new ItemLoot("Glowing Talisman", 0.0014)
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
                    new TimedTransition(5000, "changesize")
                    ),
                new State("changesize",
                    new Flash(0xCC1A1A, 0.5, 12),
                    new ChangeSize(5, 230),
                    new Spawn("Snowball Spawner", 1, 1, coolDown: 999999),
                    new TimedTransition(3600, "attack1")
                    ),
                new State("attack1",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new ReturnToSpawn(1, 0),
                    new StayCloseToSpawn(1, 15),
                    new Order(10, "Snowball Spawner", "Start"),
                    //new Shoot(15, 1, projectileIndex: 4, predictive: .9, coolDown: 1000),
                    //new Shoot(15, 1, projectileIndex: 5, predictive: .9, coolDown: 500),
                    //new Shoot(15, 1, projectileIndex: 6, predictive: .9, coolDown: 1500),
                    new Shoot(15, 15, projectileIndex: 0, coolDown: 1000, angleOffset: 3),
                    new Shoot(15, 1, projectileIndex: 5, coolDown: 400, coolDownOffset: 0),
                    new Shoot(15, 2, shootAngle: 20, projectileIndex: 7, coolDown: 800, coolDownOffset: 0),
                    new Shoot(15, 2, shootAngle: 30, projectileIndex: 8, coolDown: 800, coolDownOffset: 0),
                    new Shoot(15, 2, shootAngle: 40, projectileIndex: 9, coolDown: 800, coolDownOffset: 0),
                    new TimedRandomTransition(9000, false, "Orbit", "Rush", "Orbit1")
                    ),
                new State("Orbit",
                    new Orbit(6, 7, 15, "Snowball Spawner", speedVariance: 0, radiusVariance: 0),
                    new Order(10, "Snowball Spawner", "idle"),
                    new Shoot(15, 14, projectileIndex: 4, coolDown: 1000),
                    new Shoot(15, 7, projectileIndex: 5, coolDown: 800),
                    new TimedRandomTransition(10000, false, "attack1", "Rush", "Orbit1")
                    ),
                new State("Orbit1",
                    new Orbit(6, 7, 15, "Snowball Spawner", speedVariance: 0, radiusVariance: 0, true),
                    new Order(10, "Snowball Spawner", "idle"),
                    new Shoot(15, 14, projectileIndex: 4, coolDown: 1000),
                    new Shoot(15, 7, projectileIndex: 5, coolDown: 800),
                    new TimedRandomTransition(10000, false, "attack1", "Rush", "Orbit")
                    ),
                new State("Rush",
                    new Order(10, "Snowball Spawner", "idle"),
                    new Wander(0.4),
                    new Shoot(15, 10, projectileIndex: 0, shootAngle: 10, coolDown: 1600, angleOffset: 3),
                    new Shoot(15, 4, projectileIndex: 5, shootAngle: 15, coolDown: 1200),
                    new TimedRandomTransition(7000, false, "attack1", "Orbit", "Orbit1")
                    )
                ),
                new Threshold(0.01,
                LootTemplates.DustLoot()
                    ),
            new Threshold(0.001,
                new TierLoot(14, ItemType.Weapon, 0.05),
                new TierLoot(14, ItemType.Armor, 0.05),
                new TierLoot(6, ItemType.Ring, 0.05),
                new TierLoot(6, ItemType.Ability, 0.07),
                new ItemLoot("Greater Potion of Attack", 1),
                new ItemLoot("Greater Potion of Dexterity", 1),
                new ItemLoot("Greater Potion of Attack", 1),
                new ItemLoot("Greater Potion of Dexterity", 1),
                new ItemLoot("Frozen Coin", 0.05),
                new ItemLoot("Glowing Talisman", 0.0014)
                ),
             new Threshold(0.03,
                 new ItemLoot("Snow Angle", 0.0003),
                new ItemLoot("World of Ice", 0.0003)
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
                    new TimedTransition(5000, "attack1")
                    ),
                new State("attack1",
                    new Wander(0.3),
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new HpLessTransition(.6, "rage"),
                    new ReplaceTile("Black Water Frozen 1", "Black Water Frozen", 30),
                    new Shoot(18, 7, projectileIndex: 0, coolDown: 1200),
                    new Shoot(18, 9, projectileIndex: 0, coolDown: 1500),
                    new Shoot(18, 11, projectileIndex: 0, coolDown: 1700),
                    new Shoot(16, 8, shootAngle: 10, projectileIndex: 1, coolDown: 1000),
                    new Shoot(20, 12, projectileIndex: 0, coolDown: 2000),
                    new TimedTransition(4000, "attack1.1")
                    ),
                 new State("attack1.1",
                    new TossObject("Frozen Elf", 8, 0, coolDown: 8000),
                    new TossObject("Frozen Elf", 8, 90, coolDown: 5000),
                    new TossObject("Frozen Elf", 8, 180, coolDown: 8000),
                    new TossObject("Frozen Elf", 8, 270, coolDown: 5000),
                    new HpLessTransition(.6, "rage"),
                    new ReplaceTile("Black Water Frozen", "Black Water Frozen 1", 30),
                    new Shoot(18, 4, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 800),
                    new Shoot(16, 6, shootAngle: 10, projectileIndex: 1, coolDown: 1200),
                    new Shoot(20, 12, projectileIndex: 0, coolDown: 2000),
                    new TimedTransition(4000, "attack1.2")
                    ),
                 new State("attack1.2",
                    new Wander(0.3),
                    new HpLessTransition(.6, "rage"),
                    new ReplaceTile("Black Water Frozen 1", "Black Water Frozen", 30),
                    new Shoot(18, 4, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 800),
                    new Shoot(16, 6, shootAngle: 10, projectileIndex: 1, coolDown: 1000),
                    new Shoot(20, 12, projectileIndex: 0, coolDown: 2000),
                    new TimedTransition(4000, "attack1")
                    ),
                new State("rage",
                    new ReplaceTile("Black Water Frozen", "Black Water Frozen 1", 30),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Shoot(18, 4, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 800),
                    new Shoot(16, 6, shootAngle: 10, projectileIndex: 1, coolDown: 1000),
                    new TossObject("Frozen Tower", 8, 45, coolDown: 999999),
                    new TossObject("Frozen Tower", 8, 135, coolDown: 999999),
                    new TossObject("Frozen Tower", 8, 225, coolDown: 999999),
                    new TossObject("Frozen Tower", 8, 315, coolDown: 999999),
                    new TimedTransition(15000, "freedps")
                    ),
                new State("freedps",
                    new Shoot(25, 13, predictive: 1, coolDown: 200),
                    new Taunt("HAHAHAHAHAHAH"),
                    new HpLessTransition(.4, "attack2")
                    ),
                new State("attack2",
                    new Wander(0.3),
                    new HpLessTransition(.1, "granted"),
                    new ReplaceTile("Black Water Frozen 1", "Black Water Frozen", 30),
                    new Shoot(18, 4, shootAngle: 20, projectileIndex: 0, predictive: 1, coolDown: 800),
                    new Shoot(16, 6, shootAngle: 15, projectileIndex: 1, coolDown: 1000),
                    new Shoot(20, 12, projectileIndex: 0, coolDown: 1000),
                    new TimedTransition(4000, "attack2.1")
                    ),
                new State("attack2.1",
                    new TossObject("Frozen Elf", 8, 0, coolDown: 8000),
                    new TossObject("Frozen Elf", 8, 90, coolDown: 5000),
                    new TossObject("Frozen Elf", 8, 180, coolDown: 8000),
                    new TossObject("Frozen Elf", 8, 270, coolDown: 5000),
                    new HpLessTransition(.1, "granted"),
                    new ReplaceTile("Black Water Frozen", "Black Water Frozen 1", 30),
                    new Shoot(18, 4, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 800),
                    new Shoot(16, 6, shootAngle: 10, projectileIndex: 1, coolDown: 1000),
                    new Shoot(20, 12, projectileIndex: 0, coolDown: 1000),
                    new TimedTransition(4000, "attack2.2")
                    ),
                 new State("attack2.2",
                    new Wander(0.3),
                    new HpLessTransition(.1, "granted"),
                    new ReplaceTile("Black Water Frozen 1", "Black Water Frozen", 30),
                    new Shoot(18, 7, projectileIndex: 0, coolDown: 1200),
                    new Shoot(18, 9, projectileIndex: 0, coolDown: 1500),
                    new Shoot(18, 11, projectileIndex: 0, coolDown: 1700),
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
                ),
                new Threshold(0.01,
                LootTemplates.DustLoot()
                    ),
            new Threshold(0.001,
                new TierLoot(14, ItemType.Weapon, 0.05),
                new TierLoot(14, ItemType.Armor, 0.05),
                new TierLoot(6, ItemType.Ring, 0.05),
                new TierLoot(6, ItemType.Ability, 0.07),
                new ItemLoot("Greater Potion of Defense", 1),
                new ItemLoot("Greater Potion of Wisdom", 1),
                new ItemLoot("Greater Potion of Defense", 1),
                new ItemLoot("Greater Potion of Wisdom", 1),
                new ItemLoot("Magic Dust", 0.01),
                new ItemLoot("Glowing Talisman", 0.0014)
                ),
             new Threshold(0.03,
                new ItemLoot("Agdluak", 0.0003),
                new ItemLoot("Absolute Zero", 0.0003),
                new ItemLoot("Cryogenic Freeze", 0.0003)
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
                    new Wander(0.3),
                    new Shoot(12, 2, shootAngle: 10, predictive: 1.2, projectileIndex: 0, coolDown: 200),
                    new Shoot(12, 12, projectileIndex: 1, coolDown: 800),
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
                    new Charge(10, 8, coolDown: 2000),
                    new Shoot(12, 24, projectileIndex: 0, coolDown: 2000),
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
                    new Chase(10),
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
                    new TossObject("Frozen Elf", 5, angle: 180, coolDown: 10000, coolDownOffset: 0),
                    new TossObject("Frozen Elf", 5, angle: 190, coolDown: 10000, coolDownOffset: 500),
                    new TossObject("Frozen Elf", 5, angle: 160, coolDown: 10000, coolDownOffset: 1000),
                    new TossObject("Frozen Elf", 5, angle: 200, coolDown: 10000, coolDownOffset: 1500),
                    new TossObject("Frozen Elf", 5, angle: 170, coolDown: 10000, coolDownOffset: 2000),
                    new Shoot(12, 8, shootAngle: 10, predictive: 1, projectileIndex: 0, coolDown: 1000),
                    new HpLessTransition(.6, "attack2")
                    ),
                new State("attack2",
                    new Chase(10),
                    new Shoot(12, 8, shootAngle: 10, predictive: 1, projectileIndex: 0, coolDown: 600),
                    new Shoot(20, 1, projectileIndex: 1, predictive: 1, coolDown: 1000),
                    new HpLessTransition(.3, "attack3")
                    ),
                new State("attack3",
                    new ReturnToSpawn(1.3, 0),
                    new Shoot(12, 8, shootAngle: 10, predictive: 1, projectileIndex: 0, coolDown: 600),
                    new Shoot(20, 1, projectileIndex: 1, predictive: 1, coolDown: 1000),
                    new TossObject("Frozen Elf", 5, angle: 180, coolDown: 10000, coolDownOffset: 0),
                    new TossObject("Frozen Elf", 5, angle: 190, coolDown: 10000, coolDownOffset: 500),
                    new TossObject("Frozen Elf", 5, angle: 160, coolDown: 10000, coolDownOffset: 1000),
                    new TossObject("Frozen Elf", 5, angle: 200, coolDown: 10000, coolDownOffset: 1500),
                    new TossObject("Frozen Elf", 5, angle: 170, coolDown: 10000, coolDownOffset: 2000)
                    )
                )
            )
        .Init("Queen of Ice Clone", // finished
            new State(
                new ScaleHP2(20),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                new State("attack",
                new Shoot(10, 1, projectileIndex: 0, fixedAngle: 225, coolDown: 1500, coolDownOffset: 0),
                new Shoot(10, 1, projectileIndex: 1, fixedAngle: 225, coolDown: 1500, coolDownOffset: 0),
                new Shoot(10, 1, projectileIndex: 2, fixedAngle: 225, coolDown: 1500, coolDownOffset: 0),
                new EntityExistsTransition("Queen of Ice", 20, "die")
                    ),
                new State("die",
                    new Suicide()
                )
            )
        )
        .Init("Queen of Ice Clone 2", // finished
           new State(
                new ScaleHP2(20),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                new State("attack",
                new Shoot(10, 1, projectileIndex: 0, fixedAngle: 315, coolDown: 1500, coolDownOffset: 0),
                new Shoot(10, 1, projectileIndex: 1, fixedAngle: 315, coolDown: 1500, coolDownOffset: 0),
                new Shoot(10, 1, projectileIndex: 2, fixedAngle: 315, coolDown: 1500, coolDownOffset: 0),
                new EntityExistsTransition("Queen of Ice", 20, "die")
                    ),
                new State("die",
                    new Suicide()
                )
            )
        )
        .Init("Queen of Ice Clone 3", // finished
            new State(
                new ScaleHP2(20),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                new State("attack",
                new Shoot(10, 1, projectileIndex: 0, fixedAngle: 45, coolDown: 1500, coolDownOffset: 0),
                new Shoot(10, 1, projectileIndex: 1, fixedAngle: 45, coolDown: 1500, coolDownOffset: 0),
                new Shoot(10, 1, projectileIndex: 2, fixedAngle: 45, coolDown: 1500, coolDownOffset: 0),
                new EntityExistsTransition("Queen of Ice", 20, "die")
                    ),
                new State("die",
                    new Suicide()
                )
            )
        )
        .Init("Queen of Ice Clone 4", // finished
            new State(
                new ScaleHP2(20),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                new State("attack",
                new Shoot(10, 1, projectileIndex: 0, fixedAngle: 135, coolDown: 1500, coolDownOffset: 0),
                new Shoot(10, 1, projectileIndex: 1, fixedAngle: 135, coolDown: 1500, coolDownOffset: 0),
                new Shoot(10, 1, projectileIndex: 2, fixedAngle: 135, coolDown: 1500, coolDownOffset: 0),
                new EntityExistsTransition("Queen of Ice", 20, "die")
                    ),
                new State("die",
                    new Suicide()
                )
            )
        )
        .Init("Ice Tower",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Shoot(4, 2, shootAngle: 120, projectileIndex: 0, fixedAngle: 180, coolDown: 50),
                    new HpLessTransition(0.05, "explode")
                    ),
                new State("explode",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Flash(0xFFFFF, 0.5, 12),
                    new TimedTransition(1500, "die")
                    ),
                new State("die",
                    new Shoot(8, 11, coolDown: 400),
                    new Suicide()
                    )
                )
            )
        .Init("Ice Tower 1",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Shoot(4, 2, shootAngle: 120, projectileIndex: 0, fixedAngle: 240, coolDown: 200)
                    )
                )
            )
        .Init("Ice Tower 2",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Shoot(4, 2, shootAngle: 120, projectileIndex: 0, fixedAngle: 300, coolDown: 200)
                    )
                )
            )
        .Init("Ice Tower 3",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Shoot(4, 2, shootAngle: 120, projectileIndex: 0, fixedAngle: 0, coolDown: 200)
                    )
                )
            )
        .Init("Ice Tower 4",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Shoot(4, 2, shootAngle: 120, projectileIndex: 0, fixedAngle: 60, coolDown: 200)
                    )
                )
            )
        .Init("Ice Tower 5",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Shoot(4, 2, shootAngle: 120, projectileIndex: 0, fixedAngle: 120, coolDown: 200)
                    )
                )
            )
        .Init("Evil Snowman",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Orbit(2, 2, 10, "Guardian's Frigid Squire", speedVariance: 0, radiusVariance: 0),
                    new Shoot(8, 3, shootAngle: 10, projectileIndex: 0, predictive: 0.8, coolDown: 400)
                    ),
                new State("attack2",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Orbit(3, 6, 10, "Queen of Ice", speedVariance: 0, radiusVariance: 0),
                    new Shoot(12, 3, shootAngle: 10, projectileIndex: 0, predictive: 0.8, coolDown: 400),
                    new EntityExistsTransition("Queen of Ice", 20, "die")
                    ),
                new State("die",
                    new Suicide()
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
                    new Spawn("Evil Snowman", coolDown: 999999),
                    new Shoot(20, 1, projectileIndex: 0, shootAngle: 10, coolDown: 600),
                    new Shoot(15, 8, projectileIndex: 1, predictive: 1, coolDown: 1200)
                    )
                )
            )
        .Init("Frozen Elf",
            new State(
                new ScaleHP2(20),
                new State("within",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new PlayerWithinTransition(15, "explodeprep")
                    ),
                new State("explodeprep",                  
                    new Flash(0xCC1A1A, 0.5, 12),
                    new TimedTransition(1400, "explode")
                    ),
                new State("explode",
                    new Taunt("explode!"),
                    new Shoot(14, 12, projectileIndex: 0, coolDown: 200),
                    new TimedTransition(200, "suicide")
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