using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ SoulBosses = () => Behav()

        .Init("Soul Death",
            new State(
                new SetNoXP(),
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("First",
                    new TimedTransition(150, "Second")
                    ),
                new State("Second",
                    new SetAltTexture(1, 1),
                    new TimedTransition(300, "Third")
                    ),
                new State("Third",
                    new SetAltTexture(2, 2),
                    new TimedTransition(300, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )

        #region Soul of Life Boss (TOMB) - Done

        .Init("The Bes Nuttiest Geb Sarcophagus",
            new State(
                new OnDeathBehavior(new SwirlingMistDeathParticles()),
                new ScaleHP2(20),
                new State("Waiting Player",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(15, "Start")
                    ),
                new State("Start",
                    new Taunt("May the strength of the gods be with you ... you will need it."),
                    new TimedTransition(2000, "Start Spawning")
                    ),
                new State("Start Spawning",
                    new Spawn("Jackal Lord", 3, 0.5, coolDown: 1000, true),
                    new EntityNotExistsTransition("Jackal Lord", 30, "Weakness")
                    ),
                new State("Weakness",
                    new TimedTransition(500, "Weakness 2")
                    ),
                new State("Weakness 2",
                    new Shoot(50, 20, projectileIndex: 0, coolDown: 3000),
                    new TimedTransition(6000, "Vulnerable")
                    ),
                new State("Vulnerable",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Shoot(7, 4, shootAngle: 15, projectileIndex: 2, coolDown: 1000),
                    new Shoot(30, 5, shootAngle: 25, projectileIndex: 1, coolDown: 2500),
                    new Shoot(30, 5, shootAngle: 25, projectileIndex: 1, coolDown: 2750, coolDownOffset: 2500),
                    new Shoot(30, 5, shootAngle: 25, projectileIndex: 1, coolDown: 2900, coolDownOffset: 2750),
                    new HpLessTransition(0.8, "Hard")
                    ),
                new State("Hard",
                    new Shoot(7, 4, shootAngle: 15, projectileIndex: 2, coolDown: 1000),
                    new Shoot(30, 5, shootAngle: 25, projectileIndex: 1, coolDown: 2500),
                    new Shoot(30, 5, shootAngle: 25, projectileIndex: 1, coolDown: 2750, coolDownOffset: 2500),
                    new Shoot(30, 3, shootAngle: 15, projectileIndex: 3, coolDown: 1500),
                    new HpLessTransition(0.5, "Fast Shots")
                    ),
                new State("Fast Shots",
                    new Spawn("Jackal Lord", 3, 0.5, coolDown: 1000, true),
                    new Shoot(7, 4, shootAngle: 15, projectileIndex: 2, coolDown: 1000),
                    new Shoot(30, 5, shootAngle: 25, projectileIndex: 1, coolDown: 2500),
                    new Shoot(30, 5, shootAngle: 25, projectileIndex: 1, coolDown: 2750, coolDownOffset: 2500),
                    new Shoot(30, 3, shootAngle: 35, projectileIndex: 4, coolDown: 750),
                    new TransformOnDeath("Soul of Life Mob", 1, 1, 1)
                    )
                )
            )

        .Init("Soul of Life Opener",
            new State(
                new State("Waiting",
                    new EntitiesNotExistsTransition(100, "OpenDoor", "Tomb Defender", "Tomb Attacker", "Tomb Support", "Tomb Defender Statue", "Tomb Attacker Statue", "Tomb Support Statue")
                    ),
                new State("OpenDoor",
                    new OpenGate(145, 145, 12, 14),
                    new Suicide()
                    )
                )
            )

        .Init("Soul of Life Mob",
            new State(
                new TransformOnDeath("Soul Death", 1, 1, 1),
                new ScaleHP2(10),
                new State("Poison",
                    new Grenade(3, 25, 15, coolDown: 1500, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1000),
                    new HpLessTransition(0.5, "Rage")
                    ),
                new State("Rage",
                    new SetAltTexture(1, 1),
                    new Grenade(3, 25, 15, coolDown: 1000, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1000)
                    )
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.007,
                new ItemLoot("Soul of Life", 1)
                ),
            new Threshold(0.03,
                new ItemLoot("Bow of the Havens", 0.0014),
                new ItemLoot("Mummified Rod", 0.0014),
                new ItemLoot("Shield of The Ancient's", 0.0014),
                new ItemLoot("Soul of Mana", 0.1),
                new ItemLoot("Pharaoh’s Scripture", 0.0014)
                ),
            new Threshold(0.01,
                new TierLoot(10, ItemType.Weapon, 0.25),
                new TierLoot(11, ItemType.Weapon, 0.2),
                new TierLoot(4, ItemType.Ring, 0.25),
                new TierLoot(5, ItemType.Ability, 0.2),
                new TierLoot(11, ItemType.Armor, 0.2)
                )
            )

        #endregion Soul of Life Boss (TOMB) - Done

        #region Soul of Mana Boss (OT) - Done

        .Init("Soul of Mana Opener",
            new State(
                new State("Waiting",
                    new EntityNotExistsTransition("Thessal the Mermaid Goddess", 100, "Done")
                    ),
                new State("Done",
                    new OpenGate(27, 27, 41, 43),
                    new Suicide()
                    )
                )
            )

        .Init("Undead Thessal",
            new State(
                new OnDeathBehavior(new SwirlingMistDeathParticles()),
                new ScaleHP2(20),
                new State("Waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(15, "Taunt")
                    ),
                new State("Taunt",
                    new Taunt("Do you want me to take you to the depths? Where there are things that nobody knows."),
                    new TimedTransition(2000, "Shot")
                    ),
                new State("Shot",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Shoot(20, 3, shootAngle: 15, projectileIndex: 0, coolDown: 500),
                    new HpLessTransition(0.8, "Chase")
                    ),
                new State("Chase",
                    new Follow(1, 15, 1),
                    new Wander(1),
                    new Shoot(20, 8, projectileIndex: 1, coolDown: 2000),
                    new Shoot(20, 3, shootAngle: 15, projectileIndex: 0, coolDown: 500),
                    new HpLessTransition(0.6, "Back")
                    ),
                new State("Back",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new ReturnToSpawn(1.5),
                    new TimedTransition(2000, "Back V2")
                    ),
                new State("Back V2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Grenade(3, 65, range: 15, coolDown: 1000, effect: ConditionEffectIndex.Quiet, effectDuration: 1000),
                    new Shoot(15, 1, projectileIndex: 2, coolDown: 1500),
                    new Shoot(15, 2, shootAngle: 25, projectileIndex: 2, coolDownOffset: 500, coolDown: 1500),
                    new HpLessTransition(0.3, "Stay Back")
                    ),
                new State("Stay Back",
                    new StayBack(0.7, 7),
                    new Shoot(15, 1, projectileIndex: 3, coolDown: 1250),
                    new Shoot(15, 2, shootAngle: 25, projectileIndex: 3, coolDownOffset: 500, coolDown: 1250),
                    new Shoot(20, 3, shootAngle: 15, projectileIndex: 0, coolDown: 500),
                    new TransformOnDeath("Soul of Mana Mob", 1, 1, 1)
                    )
                )
            )

        .Init("Soul of Mana Mob",
            new State(
                new TransformOnDeath("Soul Death", 1, 1, 1),
                new ScaleHP2(10),
                new State("Moving",
                    new Wander(1),
                    new StayBack(1, 6),
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 1500),
                    new Shoot(20, 2, shootAngle: 25, projectileIndex: 1, coolDown: 1500),
                    new HpLessTransition(0.5, "Rage")
                    ),
                new State("Rage",
                    new SetAltTexture(1, 1),
                    new Wander(1.2),
                    new StayBack(1.2, 6),
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 1000),
                    new Shoot(20, 2, shootAngle: 25, projectileIndex: 1, coolDown: 1000)
                    )
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.005,
                new ItemLoot("Soul of Mana", 1)
                ),
            new Threshold(0.03,
                new ItemLoot("Soul of Mana", 0.1),
                new ItemLoot("Neptune’s Trident", 0.0014),
                new ItemLoot("Heart of the Sea", 0.0014)
                ),
            new Threshold(0.01,
                new TierLoot(11, ItemType.Armor, 0.2),
                new TierLoot(10, ItemType.Armor, 0.25),
                new TierLoot(4, ItemType.Ability, 0.25),
                new TierLoot(5, ItemType.Ring, 0.2),
                new TierLoot(10, ItemType.Weapon, 0.25),
                new ItemLoot("Coral Bow", 0.01),
                new ItemLoot("Coral Venom Trap", 0.01),
                new ItemLoot("Coral Silk Armor", 0.01),
                new ItemLoot("Coral Ring", 0.01)
                )
            )

        #endregion Soul of Mana Boss (OT) - Done

        #region Soul of Attack Boss (Puppet) - Done

        .Init("Soul of Attack Opener",
            new State(
                new State("Waiting",
                    new EntitiesNotExistsTransition(100, "Done", "The Puppet Master", "Puppet Loot Chest")
                    ),
                new State("Done",
                    new OpenGate(51, 51, 56, 58),
                    new Suicide()
                    )
                )
            )

        .Init("Undead Puppet Master",
            new State(
                new ScaleHP2(20),
                new OnDeathBehavior(new SwirlingMistDeathParticles()),
                new State("Waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(9, "Taunt")
                    ),
                new State("Taunt",
                    new Taunt("Do you want to see a magic trick?"),
                    new MoveTo(1, 62, 57),
                    new TimedTransition(2000, "Start")
                    ),
                new State("Start",
                    new Spawn("Puppet Knight 2", 10, 0.1, coolDown: 3000),
                    new Spawn("Puppet Priest 2", 15, 0.1, coolDown: 3000),
                    new Spawn("Puppet Wizard 2", 10, 0.1, coolDown: 3000),
                    new Shoot(20, 1, projectileIndex: 2, coolDown: 1000),
                    new RingAttack(50, 6, 0, projectileIndex: 0, 0.03, fixedAngle: 0, coolDown: 1000),
                    new EntitiesNotExistsTransition(30, "Last Phase", "Puppet Knight 2", "Puppet Priest", "Puppet Wizard 2")
                    ),
                new State("Last Phase",
                    new Wander(0.5),
                    new StayBack(0.5, 7),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Shoot(20, 3, shootAngle: 15, projectileIndex: 1, coolDown: 1000),
                    new Shoot(20, 1, projectileIndex: 2, coolDown: 1000),
                    new TransformOnDeath("Soul of Attack Mob", 1, 1, 1)
                    )
                )
            )

        .Init("Puppet Wizard 2",
            new State(
                new Prioritize(
                    new Orbit(0.37, 4, 20, "Undead Puppet Master"),
                    new Wander(0.4)
                    ),
                new Shoot(8.4, count: 10, projectileIndex: 0, coolDown: 2650)
                )
            )

        .Init("Puppet Priest 2",
            new State(
                new Orbit(0.37, 4, 20, "Undead Puppet Master"),
                new HealGroup(8, "Master", coolDown: 4500, healAmount: 75)
                )
            )

        .Init("Puppet Knight 2",
            new State(
                new Prioritize(
                    new Follow(0.58, 8, 1),
                    new Wander(0.2)
                    ),
                new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1750),
                new Shoot(8, count: 1, projectileIndex: 1, coolDown: 2000)
                )
            )

        .Init("Soul of Attack Mob",
            new State(
                new TransformOnDeath("Soul Death", 1, 1, 1),
                new ScaleHP2(10),
                new Wander(0.4),
                new StayBack(1.1, 7),
                new State("Shot",
                    new Shoot(20, 8, projectileIndex: 0, coolDown: 1000),
                    new Grenade(2, 50, 20, coolDown: 2000),
                    new HpLessTransition(0.5, "Rage")
                    ),
                new State("Rage",
                    new SetAltTexture(1, 1),
                    new Shoot(20, 8, projectileIndex: 0, coolDown: 1000),
                    new Grenade(3, 50, 20, coolDown: 2000, effect: ConditionEffectIndex.Confused, effectDuration: 1000)
                    )
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.005,
                new ItemLoot("Soul of Attack", 1)
                ),
            new Threshold(0.03,
                new ItemLoot("Harlequin Armor", 0.0014),
                new ItemLoot("Soul of Attack", 0.1),
                new ItemLoot("Laughing Gas", 0.0014)
                ),
            new Threshold(0.01,
                new TierLoot(10, ItemType.Weapon, 0.15),
                new TierLoot(11, ItemType.Weapon, 0.1),
                new TierLoot(4, ItemType.Ability, 0.15),
                new TierLoot(4, ItemType.Ring, 0.15),
                new TierLoot(11, ItemType.Armor, 0.1)
                )
            )

        #endregion Soul of Attack Boss (Puppet) - Done

        #region Soul of Defense Boss (TCave) - Done

        .Init("Soul of Defense Mob",
            new State(
                new TransformOnDeath("Soul Death", 1, 1, 1),
                new ScaleHP2(10),
                new Wander(0.4),
                new Follow(1.1, 20, 1),
                new State("First",
                    new Shoot(20, 2, projectileIndex: 0, coolDown: 1000),
                    new Shoot(20, 1, projectileIndex: 1, coolDown: 750),
                    new HpLessTransition(0.5, "Second")
                    ),
                new State("Second",
                    new SetAltTexture(1, 1),
                    new Shoot(20, 8, projectileIndex: 0, coolDown: 750),
                    new Shoot(20, 1, projectileIndex: 1, coolDown: 500)
                    )
                ),
            new Threshold(0.005,
                new ItemLoot("Soul of Defense", 1)
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Soul of Defense", 0.1),
                new ItemLoot("Golden Coat", 0.0014)
                ),
            new Threshold(0.01,
                new TierLoot(10, ItemType.Armor, 0.15),
                new TierLoot(11, ItemType.Armor, 0.1),
                new TierLoot(5, ItemType.Ring, 0.1),
                new TierLoot(5, ItemType.Ability, 0.1),
                new ItemLoot("Sword of Golden Fragments", 0.005),
                new ItemLoot("Jewel-Encrusted Helmet", 0.005),
                new ItemLoot("Luminous Body Armor", 0.005),
                new ItemLoot("Ring of Golden Shine", 0.005)
                )
            )

        .Init("Soul of Defense Opener",
            new State(
                new State("Waiting",
                    new EntityNotExistsTransition("Golden Oryx Effigy", 30, "Open")
                    ),
                new State("Open",
                    new OpenGate(26, 26, 18, 20),
                    new Suicide()
                    )
                )
            )

        .Init("Stanley, the Golden Knight",
            new State(
                new ScaleHP2(20),
                new State("Start",
                    new PlayerWithinTransition(15, "Taunt")
                    ),
                new State("Taunt",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("NOOO... you won't win this time! I have my protectors!"),
                    new TimedTransition(5000, "Toss 1")
                    ),
                new State("Toss 1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("Gallant Warrior", 5, angle: 0, coolDown: 999999),
                    new TimedTransition(1000, "Toss 2")
                    ),
                new State("Toss 2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("Gallant Knight", 5, angle: 120, coolDown: 999999),
                    new TimedTransition(1000, "Toss 3")
                    ),
                new State("Toss 3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("Gallant Paladin", 5, angle: 240, coolDown: 999999),
                    new TimedTransition(1000, "Attack")
                    ),
                new State("Attack",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntitiesNotExistsTransition(20, "Prepare Attack", "Gallant Warrior", "Gallant Paladin", "Gallant Knight"),

                    new Shoot(20, 1, fixedAngle: 0, projectileIndex: 3, coolDown: 2550, coolDownOffset: 100),
                    new Shoot(20, 1, fixedAngle: 0, projectileIndex: 4, coolDown: 2550, coolDownOffset: 150),
                    new Shoot(20, 1, fixedAngle: 0, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 2, fixedAngle: 0, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 1, fixedAngle: 0, projectileIndex: 6, coolDown: 2550, coolDownOffset: 250),
                    new Shoot(20, 1, fixedAngle: 0, projectileIndex: 7, coolDown: 2550, coolDownOffset: 300),

                    new Shoot(20, 1, fixedAngle: 90, projectileIndex: 3, coolDown: 2550, coolDownOffset: 100),
                    new Shoot(20, 1, fixedAngle: 90, projectileIndex: 4, coolDown: 2550, coolDownOffset: 150),
                    new Shoot(20, 1, fixedAngle: 90, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 2, fixedAngle: 90, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 1, fixedAngle: 90, projectileIndex: 6, coolDown: 2550, coolDownOffset: 250),
                    new Shoot(20, 1, fixedAngle: 90, projectileIndex: 7, coolDown: 2550, coolDownOffset: 300),

                    new Shoot(20, 1, fixedAngle: 180, projectileIndex: 3, coolDown: 2550, coolDownOffset: 100),
                    new Shoot(20, 1, fixedAngle: 180, projectileIndex: 4, coolDown: 2550, coolDownOffset: 150),
                    new Shoot(20, 1, fixedAngle: 180, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 2, fixedAngle: 180, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 1, fixedAngle: 180, projectileIndex: 6, coolDown: 2550, coolDownOffset: 250),
                    new Shoot(20, 1, fixedAngle: 180, projectileIndex: 7, coolDown: 2550, coolDownOffset: 300),

                    new Shoot(20, 1, fixedAngle: 270, projectileIndex: 3, coolDown: 2550, coolDownOffset: 100),
                    new Shoot(20, 1, fixedAngle: 270, projectileIndex: 4, coolDown: 2550, coolDownOffset: 150),
                    new Shoot(20, 1, fixedAngle: 270, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 2, fixedAngle: 270, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 1, fixedAngle: 270, projectileIndex: 6, coolDown: 2550, coolDownOffset: 250),
                    new Shoot(20, 1, fixedAngle: 270, projectileIndex: 7, coolDown: 2550, coolDownOffset: 300),

                    //vertical
                    new Shoot(20, 1, fixedAngle: 45, projectileIndex: 3, coolDown: 2550, coolDownOffset: 1300),
                    new Shoot(20, 1, fixedAngle: 45, projectileIndex: 4, coolDown: 2550, coolDownOffset: 1350),
                    new Shoot(20, 1, fixedAngle: 45, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1400),
                    new Shoot(20, 2, fixedAngle: 45, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1450),
                    new Shoot(20, 1, fixedAngle: 45, projectileIndex: 6, coolDown: 2550, coolDownOffset: 1500),
                    new Shoot(20, 1, fixedAngle: 45, projectileIndex: 7, coolDown: 2550, coolDownOffset: 1550),

                    new Shoot(20, 1, fixedAngle: 135, projectileIndex: 3, coolDown: 2550, coolDownOffset: 1300),
                    new Shoot(20, 1, fixedAngle: 135, projectileIndex: 4, coolDown: 2550, coolDownOffset: 1350),
                    new Shoot(20, 1, fixedAngle: 135, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1400),
                    new Shoot(20, 2, fixedAngle: 135, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1450),
                    new Shoot(20, 1, fixedAngle: 135, projectileIndex: 6, coolDown: 2550, coolDownOffset: 1500),
                    new Shoot(20, 1, fixedAngle: 135, projectileIndex: 7, coolDown: 2550, coolDownOffset: 1550),

                    new Shoot(20, 1, fixedAngle: 225, projectileIndex: 3, coolDown: 2550, coolDownOffset: 1300),
                    new Shoot(20, 1, fixedAngle: 225, projectileIndex: 4, coolDown: 2550, coolDownOffset: 1350),
                    new Shoot(20, 1, fixedAngle: 225, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1400),
                    new Shoot(20, 2, fixedAngle: 225, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1450),
                    new Shoot(20, 1, fixedAngle: 225, projectileIndex: 6, coolDown: 2550, coolDownOffset: 1500),
                    new Shoot(20, 1, fixedAngle: 225, projectileIndex: 7, coolDown: 2550, coolDownOffset: 1550),

                    new Shoot(20, 1, fixedAngle: 315, projectileIndex: 3, coolDown: 2550, coolDownOffset: 1300),
                    new Shoot(20, 1, fixedAngle: 315, projectileIndex: 4, coolDown: 2550, coolDownOffset: 1350),
                    new Shoot(20, 1, fixedAngle: 315, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1400),
                    new Shoot(20, 2, fixedAngle: 315, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1450),
                    new Shoot(20, 1, fixedAngle: 315, projectileIndex: 6, coolDown: 2550, coolDownOffset: 1500),
                    new Shoot(20, 1, fixedAngle: 315, projectileIndex: 7, coolDown: 2550, coolDownOffset: 1550)
                    ),
                new State("Prepare Attack",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("That... is... ENOUGH"),
                    new ChangeSize(5, 160),
                    new TimedTransition(4000, "Attack 2")
                    ),
                new State("Attack 2",

                    new Shoot(12, 3, projectileIndex: 1, shootAngle: 10, coolDown: 400),

                    new Grenade(radius: 2, damage: 100, range: 6, fixedAngle: 45, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0xFFFF00),
                    new Grenade(radius: 2, damage: 100, range: 6, fixedAngle: 135, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0xFFFF00),
                    new Grenade(radius: 2, damage: 100, range: 6, fixedAngle: 225, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0xFFFF00),
                    new Grenade(radius: 2, damage: 100, range: 6, fixedAngle: 315, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0xFFFF00),

                    new Grenade(radius: 2, damage: 100, range: 6, fixedAngle: 0, coolDown: 4000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0xFFFF00),
                    new Grenade(radius: 2, damage: 100, range: 6, fixedAngle: 90, coolDown: 4000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0xFFFF00),
                    new Grenade(radius: 2, damage: 100, range: 6, fixedAngle: 180, coolDown: 4000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0xFFFF00),
                    new Grenade(radius: 2, damage: 100, range: 6, fixedAngle: 270, coolDown: 4000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500, color: 0xFFFF00),

                    new Shoot(20, 1, fixedAngle: 0, projectileIndex: 3, coolDown: 2550, coolDownOffset: 100),
                    new Shoot(20, 1, fixedAngle: 0, projectileIndex: 4, coolDown: 2550, coolDownOffset: 150),
                    new Shoot(20, 1, fixedAngle: 0, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 2, fixedAngle: 0, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 1, fixedAngle: 0, projectileIndex: 6, coolDown: 2550, coolDownOffset: 250),
                    new Shoot(20, 1, fixedAngle: 0, projectileIndex: 7, coolDown: 2550, coolDownOffset: 300),

                    new Shoot(20, 1, fixedAngle: 90, projectileIndex: 3, coolDown: 2550, coolDownOffset: 100),
                    new Shoot(20, 1, fixedAngle: 90, projectileIndex: 4, coolDown: 2550, coolDownOffset: 150),
                    new Shoot(20, 1, fixedAngle: 90, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 2, fixedAngle: 90, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 1, fixedAngle: 90, projectileIndex: 6, coolDown: 2550, coolDownOffset: 250),
                    new Shoot(20, 1, fixedAngle: 90, projectileIndex: 7, coolDown: 2550, coolDownOffset: 300),

                    new Shoot(20, 1, fixedAngle: 180, projectileIndex: 3, coolDown: 2550, coolDownOffset: 100),
                    new Shoot(20, 1, fixedAngle: 180, projectileIndex: 4, coolDown: 2550, coolDownOffset: 150),
                    new Shoot(20, 1, fixedAngle: 180, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 2, fixedAngle: 180, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 1, fixedAngle: 180, projectileIndex: 6, coolDown: 2550, coolDownOffset: 250),
                    new Shoot(20, 1, fixedAngle: 180, projectileIndex: 7, coolDown: 2550, coolDownOffset: 300),

                    new Shoot(20, 1, fixedAngle: 270, projectileIndex: 3, coolDown: 2550, coolDownOffset: 100),
                    new Shoot(20, 1, fixedAngle: 270, projectileIndex: 4, coolDown: 2550, coolDownOffset: 150),
                    new Shoot(20, 1, fixedAngle: 270, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 2, fixedAngle: 270, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 200),
                    new Shoot(20, 1, fixedAngle: 270, projectileIndex: 6, coolDown: 2550, coolDownOffset: 250),
                    new Shoot(20, 1, fixedAngle: 270, projectileIndex: 7, coolDown: 2550, coolDownOffset: 300),

                    //vertical
                    new Shoot(20, 1, fixedAngle: 45, projectileIndex: 3, coolDown: 2550, coolDownOffset: 1300),
                    new Shoot(20, 1, fixedAngle: 45, projectileIndex: 4, coolDown: 2550, coolDownOffset: 1350),
                    new Shoot(20, 1, fixedAngle: 45, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1400),
                    new Shoot(20, 2, fixedAngle: 45, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1450),
                    new Shoot(20, 1, fixedAngle: 45, projectileIndex: 6, coolDown: 2550, coolDownOffset: 1500),
                    new Shoot(20, 1, fixedAngle: 45, projectileIndex: 7, coolDown: 2550, coolDownOffset: 1550),

                    new Shoot(20, 1, fixedAngle: 135, projectileIndex: 3, coolDown: 2550, coolDownOffset: 1300),
                    new Shoot(20, 1, fixedAngle: 135, projectileIndex: 4, coolDown: 2550, coolDownOffset: 1350),
                    new Shoot(20, 1, fixedAngle: 135, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1400),
                    new Shoot(20, 2, fixedAngle: 135, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1450),
                    new Shoot(20, 1, fixedAngle: 135, projectileIndex: 6, coolDown: 2550, coolDownOffset: 1500),
                    new Shoot(20, 1, fixedAngle: 135, projectileIndex: 7, coolDown: 2550, coolDownOffset: 1550),

                    new Shoot(20, 1, fixedAngle: 225, projectileIndex: 3, coolDown: 2550, coolDownOffset: 1300),
                    new Shoot(20, 1, fixedAngle: 225, projectileIndex: 4, coolDown: 2550, coolDownOffset: 1350),
                    new Shoot(20, 1, fixedAngle: 225, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1400),
                    new Shoot(20, 2, fixedAngle: 225, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1450),
                    new Shoot(20, 1, fixedAngle: 225, projectileIndex: 6, coolDown: 2550, coolDownOffset: 1500),
                    new Shoot(20, 1, fixedAngle: 225, projectileIndex: 7, coolDown: 2550, coolDownOffset: 1550),

                    new Shoot(20, 1, fixedAngle: 315, projectileIndex: 3, coolDown: 2550, coolDownOffset: 1300),
                    new Shoot(20, 1, fixedAngle: 315, projectileIndex: 4, coolDown: 2550, coolDownOffset: 1350),
                    new Shoot(20, 1, fixedAngle: 315, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1400),
                    new Shoot(20, 2, fixedAngle: 315, shootAngle: 20, projectileIndex: 5, coolDown: 2550, coolDownOffset: 1450),
                    new Shoot(20, 1, fixedAngle: 315, projectileIndex: 6, coolDown: 2550, coolDownOffset: 1500),
                    new Shoot(20, 1, fixedAngle: 315, projectileIndex: 7, coolDown: 2550, coolDownOffset: 1550)
                     )
                ),
            new Threshold(0.005,
                new ItemLoot("Soul of Defense", 1)
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Golden Coat", 0.0014)
                ),
             new Threshold(0.05,
                new TierLoot(10, ItemType.Armor, 0.15),
                new TierLoot(11, ItemType.Armor, 0.1),
                new TierLoot(5, ItemType.Ring, 0.1),
                new TierLoot(5, ItemType.Ability, 0.1),
                new ItemLoot("Sword of Golden Fragments", 0.005),
                new ItemLoot("Jewel-Encrusted Helmet", 0.005),
                new ItemLoot("Luminous Body Armor", 0.005),
                new ItemLoot("Ring of Golden Shine", 0.005)
                )
            )
        .Init("Gallant Warrior",
            new State(
                new ScaleHP2(20),
                new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new PlayerWithinTransition(15, "Taunt")
                    ),
                new State("Taunt",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new TimedTransition(2500, "Attack")
                    ),
                new State("Attack",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(12, 3, projectileIndex: 1, shootAngle: 25, coolDown: 1000),
                    new Prioritize(
                        new Orbit(speed: 0.5, radius: 5, acquireRange: 6, target: "Stanley, the Golden Knight", speedVariance: 0, radiusVariance: 0))
                    )
                )
            )
       .Init("Gallant Paladin",
            new State(
                new ScaleHP2(20),
                new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new PlayerWithinTransition(15, "Taunt")
                    ),
                new State("Taunt",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new TimedTransition(1500, "Attack")
                    ),
                new State("Attack",
                    new Shoot(8, 3, projectileIndex: 2, shootAngle: 25, coolDown: 1000),
                    new Prioritize(
                        new Orbit(speed: 0.5, radius: 5, acquireRange: 6, target: "Stanley, the Golden Knight", speedVariance: 0, radiusVariance: 0))
                    )
                )
            )
        .Init("Gallant Knight",
            new State(
                new ScaleHP2(20),
                new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new PlayerWithinTransition(15, "Taunt")
                    ),
                new State("Taunt",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new TimedTransition(500, "Attack")
                    ),
                new State("Attack",
                    new Shoot(8, 3, projectileIndex: 0, shootAngle: 25, coolDown: 1000),
                    new Prioritize(
                        new Orbit(speed: 0.5, radius: 5, acquireRange: 6, target: "Stanley, the Golden Knight", speedVariance: 0, radiusVariance: 0))
                    )
                )
            )

        #endregion Soul of Defense Boss (TCave) - Done

        #region Soul of Speed Boss (Snake Pit) - Done

        .Init("Soul of Speed Opener",
            new State(
                new State("Waiting",
                    new EntityNotExistsTransition("Stheno the Snake Queen", 50, "Open")
                    ),
                new State("Open",
                    new OpenGate("Brown Wall Candles Light", 2),
                    new Suicide()
                    )
                )
            )

        .Init("Soul of Speed Mob",
            new State(
                new TransformOnDeath("Soul Death", 1, 1, 1),
                new ScaleHP2(10),
                new Wander(0.4),
                new Follow(1.1, 15, 0.5),
                new State("No Rage",
                    new Shoot(20, 8, projectileIndex: 0, coolDown: 1000),
                    new HpLessTransition(0.5, "Rage")
                    ),
                new State("Rage",
                    new SetAltTexture(1, 1),
                    new Shoot(20, 16, projectileIndex: 1, coolDown: 1000)
                    )
                )
            )

        .Init("Undead Stheno",
            new State(
                new ScaleHP2(20),
                new RealmPortalDrop(),
                new State("Waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(10, "Start")
                    ),
                new State("Start",
                    new Taunt("this WON'T be easy this time!"),
                    new Flash(0xAA00FF, 5, 5),
                    new TimedTransition(5000, "Start 2")
                    ),
                new State("Start 2",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(15, 3, shootAngle: 10, projectileIndex: 0, fixedAngle: 0, rotateAngle: 50, coolDown: 300),
                    new Shoot(15, 3, shootAngle: 10, projectileIndex: 0, fixedAngle: 180, rotateAngle: 50, coolDown: 300),

                    new Grenade(2, 50, 3, fixedAngle: 0, coolDown: 3700, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000, color: 0xAA00FF),
                    new Grenade(2, 50, 3, fixedAngle: 180, coolDown: 3700, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000, color: 0xAA00FF),

                    new Grenade(3, 50, 10, fixedAngle: 0, coolDown: 1000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000, color: 0xAA00FF),
                    new Grenade(3, 50, 10, fixedAngle: 72, coolDown: 1000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000, color: 0xAA00FF),
                    new Grenade(3, 50, 10, fixedAngle: 144, coolDown: 1000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000, color: 0xAA00FF),
                    new Grenade(3, 50, 10, fixedAngle: 216, coolDown: 1000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000, color: 0xAA00FF),
                    new Grenade(3, 50, 10, fixedAngle: 288, coolDown: 1000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000, color: 0xAA00FF),

                    new Grenade(3, 50, 10, fixedAngle: 36, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000, color: 0xAA00FF),
                    new Grenade(3, 50, 10, fixedAngle: 108, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000, color: 0xAA00FF),
                    new Grenade(3, 50, 10, fixedAngle: 180, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000, color: 0xAA00FF),
                    new Grenade(3, 50, 10, fixedAngle: 252, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000, color: 0xAA00FF),
                    new Grenade(3, 50, 10, fixedAngle: 324, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000, color: 0xAA00FF),

                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 0, coolDown: 3700, coolDownOffset: 100),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 5, coolDown: 3700, coolDownOffset: 200),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 10, coolDown: 3700, coolDownOffset: 300),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 15, coolDown: 3700, coolDownOffset: 400),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 20, coolDown: 3700, coolDownOffset: 500),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 25, coolDown: 3700, coolDownOffset: 600),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 30, coolDown: 3700, coolDownOffset: 700),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 35, coolDown: 3700, coolDownOffset: 800),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 40, coolDown: 3700, coolDownOffset: 900),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 45, coolDown: 3700, coolDownOffset: 1000),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 50, coolDown: 3700, coolDownOffset: 1100),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 55, coolDown: 3700, coolDownOffset: 1200),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 60, coolDown: 3700, coolDownOffset: 1300),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 65, coolDown: 3700, coolDownOffset: 1400),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 70, coolDown: 3700, coolDownOffset: 1500),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 75, coolDown: 3700, coolDownOffset: 1600),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 80, coolDown: 3700, coolDownOffset: 1700),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 85, coolDown: 3700, coolDownOffset: 1800),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 90, coolDown: 3700, coolDownOffset: 1900),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 95, coolDown: 3700, coolDownOffset: 2000),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 100, coolDown: 3700, coolDownOffset: 2100),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 105, coolDown: 3700, coolDownOffset: 2200),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 110, coolDown: 3700, coolDownOffset: 2300),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 115, coolDown: 3700, coolDownOffset: 2400),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 120, coolDown: 3700, coolDownOffset: 2500),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 125, coolDown: 3700, coolDownOffset: 2600),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 130, coolDown: 3700, coolDownOffset: 2700),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 135, coolDown: 3700, coolDownOffset: 2800),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 140, coolDown: 3700, coolDownOffset: 2900),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 145, coolDown: 3700, coolDownOffset: 3000),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 150, coolDown: 3700, coolDownOffset: 3100),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 155, coolDown: 3700, coolDownOffset: 3200),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 160, coolDown: 3700, coolDownOffset: 3300),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 165, coolDown: 3700, coolDownOffset: 3400),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 170, coolDown: 3700, coolDownOffset: 3500),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 175, coolDown: 3700, coolDownOffset: 3600),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 180, coolDown: 3700, coolDownOffset: 3700),

                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 180, coolDown: 3700, coolDownOffset: 100),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 185, coolDown: 3700, coolDownOffset: 200),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 190, coolDown: 3700, coolDownOffset: 300),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 195, coolDown: 3700, coolDownOffset: 400),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 200, coolDown: 3700, coolDownOffset: 500),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 205, coolDown: 3700, coolDownOffset: 600),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 210, coolDown: 3700, coolDownOffset: 700),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 215, coolDown: 3700, coolDownOffset: 800),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 220, coolDown: 3700, coolDownOffset: 900),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 225, coolDown: 3700, coolDownOffset: 1000),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 230, coolDown: 3700, coolDownOffset: 1100),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 235, coolDown: 3700, coolDownOffset: 1200),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 240, coolDown: 3700, coolDownOffset: 1300),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 245, coolDown: 3700, coolDownOffset: 1400),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 250, coolDown: 3700, coolDownOffset: 1500),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 255, coolDown: 3700, coolDownOffset: 1600),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 260, coolDown: 3700, coolDownOffset: 1700),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 265, coolDown: 3700, coolDownOffset: 1800),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 270, coolDown: 3700, coolDownOffset: 1900),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 275, coolDown: 3700, coolDownOffset: 2000),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 280, coolDown: 3700, coolDownOffset: 2100),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 285, coolDown: 3700, coolDownOffset: 2200),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 290, coolDown: 3700, coolDownOffset: 2300),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 295, coolDown: 3700, coolDownOffset: 2400),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 300, coolDown: 3700, coolDownOffset: 2500),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 305, coolDown: 3700, coolDownOffset: 2600),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 310, coolDown: 3700, coolDownOffset: 2700),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 315, coolDown: 3700, coolDownOffset: 2800),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 320, coolDown: 3700, coolDownOffset: 2900),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 325, coolDown: 3700, coolDownOffset: 3000),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 330, coolDown: 3700, coolDownOffset: 3100),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 335, coolDown: 3700, coolDownOffset: 3200),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 340, coolDown: 3700, coolDownOffset: 3300),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 345, coolDown: 3700, coolDownOffset: 3400),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 350, coolDown: 3700, coolDownOffset: 3500),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 355, coolDown: 3700, coolDownOffset: 3600),
                    new Shoot(30, 5, projectileIndex: 1, shootAngle: 20, fixedAngle: 360, coolDown: 3700, coolDownOffset: 3700)
                         )
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.05,
                new ItemLoot("Soul of Speed", 1)
                ),
            new Threshold(0.03,
                new ItemLoot("Queen's Scale", 0.0014),
                new ItemLoot("Soul of Speed", 0.1),
                new ItemLoot("Snake Bane Quiver", 0.0014)
                ),
            new Threshold(0.01,
                new ItemLoot("Hebikira", 0.01),
                new ItemLoot("Snake Queen’s Hide", 0.01),
                new ItemLoot("Star of Stheno", 0.01),
                new ItemLoot("Spirit of Snakes", 0.01),
                new TierLoot(6, ItemType.Ability, 0.1),
                new TierLoot(13, ItemType.Weapon, 0.1),
                new TierLoot(13, ItemType.Armor, 0.1),
                new TierLoot(6, ItemType.Ring, 0.15)
                )
            )

        #endregion Soul of Speed Boss (Snake Pit) - Done

        #region Soul of Dexterity Boss (Sprite World) - Done

        .Init("Soul of Dexterity Opener",
            new State(
                new State("Waiting",
                    new EntityNotExistsTransition("Limon the Sprite God", 100, "Open")
                    ),
                new State("Open",
                    new OpenGate("All Black Wall", 2),
                    new Suicide()
                    )
                )
            )

        .Init("Soul of Dexterity Mob",
            new State(
                new ScaleHP2(10),
                new TransformOnDeath("Soul Death", 1, 1, 1),
                new Wander(0.4),
                new Charge(4, 10, 1000),
                new State("First",
                    new State("Shooting 1",
                        new Shoot(20, 3, shootAngle: 15, projectileIndex: 0, coolDown: 1000),
                        new PlayerWithinTransition(2, "Player 1")
                        ),
                    new State("Player 1",
                        new Shoot(20, 8, projectileIndex: 1, coolDown: 1000),
                        new NoPlayerWithinTransition(2, "Shooting 1")
                        ),
                    new HpLessTransition(0.5, "Rage")
                    ),
                new State("Rage",
                    new SetAltTexture(1, 1),
                    new State("Shooting 2",
                        new Shoot(20, 3, shootAngle: 15, projectileIndex: 0, coolDown: 750),
                        new PlayerWithinTransition(2, "Player 2")
                        ),
                    new State("Player 2",
                        new Shoot(20, 8, projectileIndex: 1, coolDown: 750),
                        new NoPlayerWithinTransition(2, "Shooting 2")
                        )
                    )
                ),
            new Threshold(0.005,
                new ItemLoot("Soul of Dexterity", 1)
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Soul of Dexterity", 0.1),
                new ItemLoot("Chromatic Extinction", 0.0014)
                ),
            new Threshold(0.01,
                new TierLoot(4, ItemType.Ring, 0.15),
                new TierLoot(5, ItemType.Ring, 0.1),
                new TierLoot(11, ItemType.Armor, 0.1),
                new TierLoot(11, ItemType.Weapon, 0.1),
                new TierLoot(5, ItemType.Armor, 0.1)
                )
            )

        .Init("Undead Limon",
            new State(
                new ScaleHP2(20),
                new TransformOnDeath("Soul of Dexterity Mob", 1, 1, 1),
                new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(10, "Taunt")
                    ),
                new State("Taunt",
                    new Taunt("I love stars, do you want to see them?"),
                    new TimedTransition(2000, "Prepare")
                    ),
                new State("Prepare",
                    new TossObject(child: "Limon Element 1 v2", range: 9.5, angle: 315, coolDown: 1000000),
                    new TossObject(child: "Limon Element 2 v2", range: 9.5, angle: 225, coolDown: 1000000),
                    new TossObject(child: "Limon Element 3 v2", range: 9.5, angle: 135, coolDown: 1000000),
                    new TossObject(child: "Limon Element 4 v2", range: 9.5, angle: 45, coolDown: 1000000),
                    new TossObject(child: "Limon Element 1 v2", range: 14, angle: 315, coolDown: 1000000),
                    new TossObject(child: "Limon Element 2 v2", range: 14, angle: 225, coolDown: 1000000),
                    new TossObject(child: "Limon Element 3 v2", range: 14, angle: 135, coolDown: 1000000),
                    new TossObject(child: "Limon Element 4 v2", range: 14, angle: 45, coolDown: 1000000),
                    new TimedTransition(3000, "Start")
                    ),
                new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Shoot(20, 3, shootAngle: 10, projectileIndex: 0, coolDown: 2000),
                    new Shoot(20, 16, projectileIndex: 1, coolDown: 250),
                    new HpLessTransition(0.75, "Spawn"),
                    new HpLessTransition(0.5, "Spawn"),
                    new State("Player",
                        new PlayerWithinTransition(3, "Found Player")
                        ),
                    new State("Found Player",
                        new Shoot(20, 12, projectileIndex: 2, coolDown: 250),
                        new NoPlayerWithinTransition(3, "Player")
                        ),
                    new State("Spawn",
                        new Spawn(children: "Magic Sprite", maxChildren: 10, initialSpawn: 0, coolDown: 500),
                        new Spawn(children: "Ice Sprite", maxChildren: 10, initialSpawn: 0, coolDown: 500)
                        )
                    )
                )
            )

        .Init("Limon Element 1 v2",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new EntityNotExistsTransition(target: "Undead Limon", dist: 999, targetState: "Suicide"),
                new State("Setup",
                    new TimedTransition(time: 2000, targetState: "Attacking1")
                    ),
                new State("Attacking1",
                    new Shoot(radius: 999, fixedAngle: 180, defaultAngle: 180, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 90, defaultAngle: 90, coolDown: 100),
                    new TimedTransition(time: 6000, targetState: "Attacking2")
                    ),
                new State("Attacking2",
                    new Shoot(radius: 999, fixedAngle: 180, defaultAngle: 180, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 90, defaultAngle: 90, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 135, defaultAngle: 135, coolDown: 100),
                    new TimedTransition(time: 6000, targetState: "Attacking3")
                    ),
                new State("Attacking3",
                    new Shoot(radius: 999, fixedAngle: 180, defaultAngle: 180, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 90, defaultAngle: 90, coolDown: 100),
                    new TimedTransition(time: 6000, targetState: "Setup")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Limon Element 2 v2",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new EntityNotExistsTransition(target: "Undead Limon", dist: 999, targetState: "Suicide"),
                new State("Setup",
                    new TimedTransition(time: 2000, targetState: "Attacking1")
                    ),
                new State("Attacking1",
                    new Shoot(radius: 999, fixedAngle: 90, defaultAngle: 90, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 0, defaultAngle: 0, coolDown: 100),
                    new TimedTransition(time: 6000, targetState: "Attacking2")
                    ),
                new State("Attacking2",
                    new Shoot(radius: 999, fixedAngle: 90, defaultAngle: 90, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 0, defaultAngle: 0, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 45, defaultAngle: 45, coolDown: 100),
                    new TimedTransition(time: 6000, targetState: "Attacking3")
                    ),
                new State("Attacking3",
                    new Shoot(radius: 999, fixedAngle: 90, defaultAngle: 90, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 0, defaultAngle: 0, coolDown: 100),
                    new TimedTransition(time: 6000, targetState: "Setup")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Limon Element 3 v2",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new EntityNotExistsTransition(target: "Undead Limon", dist: 999, targetState: "Suicide"),
                new State("Setup",
                    new TimedTransition(time: 2000, targetState: "Attacking1")
                    ),
                new State("Attacking1",
                    new Shoot(radius: 999, fixedAngle: 0, defaultAngle: 0, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 270, defaultAngle: 270, coolDown: 100),
                    new TimedTransition(time: 6000, targetState: "Attacking2")
                    ),
                new State("Attacking2",
                    new Shoot(radius: 999, fixedAngle: 0, defaultAngle: 0, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 270, defaultAngle: 270, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 315, defaultAngle: 315, coolDown: 100),
                    new TimedTransition(time: 6000, targetState: "Attacking3")
                    ),
                new State("Attacking3",
                    new Shoot(radius: 999, fixedAngle: 0, defaultAngle: 0, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 270, defaultAngle: 270, coolDown: 100),
                    new TimedTransition(time: 6000, targetState: "Setup")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Limon Element 4 v2",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new EntityNotExistsTransition(target: "Undead Limon", dist: 999, targetState: "Suicide"),
                new State("Setup",
                    new TimedTransition(time: 2000, targetState: "Attacking1")
                    ),
                new State("Attacking1",
                    new Shoot(radius: 999, fixedAngle: 270, defaultAngle: 270, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 180, defaultAngle: 180, coolDown: 100),
                    new TimedTransition(time: 6000, targetState: "Attacking2")
                    ),
                new State("Attacking2",
                    new Shoot(radius: 999, fixedAngle: 270, defaultAngle: 270, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 180, defaultAngle: 180, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 225, defaultAngle: 225, coolDown: 100),
                    new TimedTransition(time: 6000, targetState: "Attacking3")
                    ),
                new State("Attacking3",
                    new Shoot(radius: 999, fixedAngle: 270, defaultAngle: 270, coolDown: 100),
                    new Shoot(radius: 999, fixedAngle: 180, defaultAngle: 180, coolDown: 100),
                    new TimedTransition(time: 6000, targetState: "Setup")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )

        #endregion Soul of Dexterity Boss (Sprite World) - Done

        #region Soul of Vitality Boss (Abyss) - Done

        .Init("Soul of Vitality Mob",
            new State(
                new Wander(0.4),
                new StayBack(1.1, 6),
                new State("Start",
                    new RingAttack(20, 3, 0, projectileIndex: 0, 0.03, 0, coolDown: 100),
                    new Shoot(20, 3, shootAngle: 15, projectileIndex: 1, coolDown: 1500),
                    new HpLessTransition(0.5, "Rage")
                    ),
                new State("Rage",
                    new SetAltTexture(1, 1),
                    new RingAttack(20, 3, 0, projectileIndex: 0, 0.03, 0, coolDown: -1),
                    new Shoot(20, 3, shootAngle: 15, projectileIndex: 1, coolDown: 1000)
                    )
                ),
            new Threshold(0.005,
                new ItemLoot("Soul of Vitality", 1)
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Soul of Vitality", 0.1),
                new ItemLoot("Cadaverous Knife", 0.0014)
                ),
            new Threshold(0.01,
                new ItemLoot("Abyssal Sword", 0.02),
                new ItemLoot("Seal of the Underworld", 0.02),
                new ItemLoot("Archdemon’s Remains", 0.02),
                new ItemLoot("Demon’s Sigil", 0.02),
                new TierLoot(10, ItemType.Armor, 0.15),
                new TierLoot(11, ItemType.Armor, 0.1),
                new TierLoot(4, ItemType.Ring, 0.15),
                new TierLoot(4, ItemType.Ability, 0.15),
                new TierLoot(11, ItemType.Weapon, 0.1)
                )
            )

        .Init("Undead Malphas",
            new State(
                new ScaleHP2(20),
                new State("Waiting",
                    new RealmPortalDrop(),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new PlayerWithinTransition(10, "Start")
                    ),
                new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("Hell, a good place to rest ..."),
                    new TimedTransition(2000, "Wave 1")
                    ),
                new State("Wave 1",
                    new Shoot(15, 5, projectileIndex: 1, coolDown: 1000),

                    new Shoot(8, 1, projectileIndex: 0, coolDown: 1000, predictive: 1.5, coolDownOffset: 100),
                    new Shoot(8, 2, shootAngle: 10, projectileIndex: 4, predictive: 1.5, coolDown: 1000, coolDownOffset: 100),
                    new Shoot(8, 2, shootAngle: 20, projectileIndex: 5, predictive: 1.5, coolDown: 1000, coolDownOffset: 100),

                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 0, coolDown: 7200, coolDownOffset: 100),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 5, coolDown: 7200, coolDownOffset: 200),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 10, coolDown: 7200, coolDownOffset: 300),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 15, coolDown: 7200, coolDownOffset: 400),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 20, coolDown: 7200, coolDownOffset: 500),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 25, coolDown: 7200, coolDownOffset: 600),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 30, coolDown: 7200, coolDownOffset: 700),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 35, coolDown: 7200, coolDownOffset: 800),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 40, coolDown: 7200, coolDownOffset: 900),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 45, coolDown: 7200, coolDownOffset: 1000),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 50, coolDown: 7200, coolDownOffset: 1100),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 55, coolDown: 7200, coolDownOffset: 1200),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 60, coolDown: 7200, coolDownOffset: 1300),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 65, coolDown: 7200, coolDownOffset: 1400),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 70, coolDown: 7200, coolDownOffset: 1500),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 75, coolDown: 7200, coolDownOffset: 1600),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 80, coolDown: 7200, coolDownOffset: 1700),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 85, coolDown: 7200, coolDownOffset: 1800),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 90, coolDown: 7200, coolDownOffset: 1900),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 95, coolDown: 7200, coolDownOffset: 2000),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 100, coolDown: 7200, coolDownOffset: 2100),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 105, coolDown: 7200, coolDownOffset: 2200),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 110, coolDown: 7200, coolDownOffset: 2300),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 115, coolDown: 7200, coolDownOffset: 2400),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 120, coolDown: 7200, coolDownOffset: 2500),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 125, coolDown: 7200, coolDownOffset: 2600),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 130, coolDown: 7200, coolDownOffset: 2700),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 135, coolDown: 7200, coolDownOffset: 2800),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 140, coolDown: 7200, coolDownOffset: 2900),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 145, coolDown: 7200, coolDownOffset: 3000),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 150, coolDown: 7200, coolDownOffset: 3100),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 155, coolDown: 7200, coolDownOffset: 3200),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 160, coolDown: 7200, coolDownOffset: 3300),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 165, coolDown: 7200, coolDownOffset: 3400),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 170, coolDown: 7200, coolDownOffset: 3500),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 175, coolDown: 7200, coolDownOffset: 3600),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 180, coolDown: 7200, coolDownOffset: 3700),
                                  
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 175, coolDown: 7200, coolDownOffset: 3800),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 170, coolDown: 7200, coolDownOffset: 3900),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 165, coolDown: 7200, coolDownOffset: 4000),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 160, coolDown: 7200, coolDownOffset: 4100),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 155, coolDown: 7200, coolDownOffset: 4200),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 150, coolDown: 7200, coolDownOffset: 4300),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 145, coolDown: 7200, coolDownOffset: 4400),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 140, coolDown: 7200, coolDownOffset: 4500),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 135, coolDown: 7200, coolDownOffset: 4600),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 130, coolDown: 7200, coolDownOffset: 4700),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 125, coolDown: 7200, coolDownOffset: 4800),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 120, coolDown: 7200, coolDownOffset: 4900),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 115, coolDown: 7200, coolDownOffset: 5000),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 110, coolDown: 7200, coolDownOffset: 5100),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 105, coolDown: 7200, coolDownOffset: 5200),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 100, coolDown: 7200, coolDownOffset: 5300),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 95, coolDown: 7200, coolDownOffset: 5400),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 90, coolDown: 7200, coolDownOffset: 5500),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 85, coolDown: 7200, coolDownOffset: 5600),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 80, coolDown: 7200, coolDownOffset: 5700),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 75, coolDown: 7200, coolDownOffset: 5800),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 70, coolDown: 7200, coolDownOffset: 5900),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 65, coolDown: 7200, coolDownOffset: 6000),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 60, coolDown: 7200, coolDownOffset: 6100),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 55, coolDown: 7200, coolDownOffset: 6200),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 50, coolDown: 7200, coolDownOffset: 6300),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 45, coolDown: 7200, coolDownOffset: 6400),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 40, coolDown: 7200, coolDownOffset: 6500),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 35, coolDown: 7200, coolDownOffset: 6600),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 30, coolDown: 7200, coolDownOffset: 6700),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 25, coolDown: 7200, coolDownOffset: 6800),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 20, coolDown: 7200, coolDownOffset: 6900),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 15, coolDown: 7200, coolDownOffset: 7000),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 10, coolDown: 7200, coolDownOffset: 7100),
                    new Shoot(30, 2, projectileIndex: 3, fixedAngle: 5, coolDown: 7200, coolDownOffset: 7200),
                    
                    new HpLessTransition(0.4, "Prepare Phase 1")
                    ),
                new State("Prepare Phase 1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("AHAHA, is that all you got?"),
                    new ChangeSize(5, 180),
                    new Flash(0xFF0000, 0.5, 5),
                    new GroundTransform("Red Earth Water", 20, -5, -1),
                    new GroundTransform("Red Earth Water", 20, -5, 0),
                    new GroundTransform("Red Earth Water", 20, -5, 1),
                    new GroundTransform("Red Earth Water", 20, -6, 1),
                    new GroundTransform("Red Earth Water", 20, -6, 0),
                    new GroundTransform("Red Earth Water", 20, -6, -1),

                    new GroundTransform("Red Earth Water", 20, -1, 5),
                    new GroundTransform("Red Earth Water", 20, 0, 5),
                    new GroundTransform("Red Earth Water", 20, 1, 5),
                    new GroundTransform("Red Earth Water", 20, 1, 6),
                    new GroundTransform("Red Earth Water", 20, 0, 6),
                    new GroundTransform("Red Earth Water", 20, -1, 6),

                    new GroundTransform("Red Earth Water", 20, 5, 1),
                    new GroundTransform("Red Earth Water", 20, 5, 0),
                    new GroundTransform("Red Earth Water", 20, 5, -1),
                    new GroundTransform("Red Earth Water", 20, 6, 1),
                    new GroundTransform("Red Earth Water", 20, 6, 0),
                    new GroundTransform("Red Earth Water", 20, 6, -1),

                    new GroundTransform("Red Earth Water", 20, 1, -5),
                    new GroundTransform("Red Earth Water", 20, 0, -5),
                    new GroundTransform("Red Earth Water", 20, -1, -5),
                    new GroundTransform("Red Earth Water", 20, 1, -6),
                    new GroundTransform("Red Earth Water", 20, 0, -6),
                    new GroundTransform("Red Earth Water", 20, -1, -6),

                    new TimedTransition(3000, "Attack 2")
                    ),
                new State("Attack 2",
                    new Shoot(15, 10, projectileIndex: 1, coolDown: 1000),

                    new TossObject("White Demon of the Abyss", 12, 45, coolDown: 99999, coolDownOffset: -1),
                    new TossObject("White Demon of the Abyss", 12, -45, coolDown: 99999, coolDownOffset: -1),
                    new TossObject("White Demon of the Abyss", 12, 135, coolDown: 99999, coolDownOffset: -1),
                    new TossObject("White Demon of the Abyss", 12, -135, coolDown: 99999, coolDownOffset: -1),

                    new Shoot(8, 1, projectileIndex: 0, coolDown: 500, predictive: 1.5, coolDownOffset: 100),
                    new Shoot(8, 2, shootAngle: 10, projectileIndex: 4, predictive: 1.5, coolDown: 500, coolDownOffset: 100),
                    new Shoot(8, 2, shootAngle: 20, projectileIndex: 5, predictive: 1.5, coolDown: 500, coolDownOffset: 100),

                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 0, coolDown: 7200, coolDownOffset: 100),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 5, coolDown: 7200, coolDownOffset: 200),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 10, coolDown: 7200, coolDownOffset: 300),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 15, coolDown: 7200, coolDownOffset: 400),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 20, coolDown: 7200, coolDownOffset: 500),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 25, coolDown: 7200, coolDownOffset: 600),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 30, coolDown: 7200, coolDownOffset: 700),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 35, coolDown: 7200, coolDownOffset: 800),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 40, coolDown: 7200, coolDownOffset: 900),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 45, coolDown: 7200, coolDownOffset: 1000),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 50, coolDown: 7200, coolDownOffset: 1100),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 55, coolDown: 7200, coolDownOffset: 1200),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 60, coolDown: 7200, coolDownOffset: 1300),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 65, coolDown: 7200, coolDownOffset: 1400),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 70, coolDown: 7200, coolDownOffset: 1500),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 75, coolDown: 7200, coolDownOffset: 1600),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 80, coolDown: 7200, coolDownOffset: 1700),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 85, coolDown: 7200, coolDownOffset: 1800),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 90, coolDown: 7200, coolDownOffset: 1900),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 95, coolDown: 7200, coolDownOffset: 2000),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 100, coolDown: 7200, coolDownOffset: 2100),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 105, coolDown: 7200, coolDownOffset: 2200),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 110, coolDown: 7200, coolDownOffset: 2300),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 115, coolDown: 7200, coolDownOffset: 2400),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 120, coolDown: 7200, coolDownOffset: 2500),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 125, coolDown: 7200, coolDownOffset: 2600),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 130, coolDown: 7200, coolDownOffset: 2700),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 135, coolDown: 7200, coolDownOffset: 2800),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 140, coolDown: 7200, coolDownOffset: 2900),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 145, coolDown: 7200, coolDownOffset: 3000),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 150, coolDown: 7200, coolDownOffset: 3100),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 155, coolDown: 7200, coolDownOffset: 3200),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 160, coolDown: 7200, coolDownOffset: 3300),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 165, coolDown: 7200, coolDownOffset: 3400),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 170, coolDown: 7200, coolDownOffset: 3500),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 175, coolDown: 7200, coolDownOffset: 3600),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 180, coolDown: 7200, coolDownOffset: 3700),
                                 
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 175, coolDown: 7200, coolDownOffset: 3800),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 170, coolDown: 7200, coolDownOffset: 3900),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 165, coolDown: 7200, coolDownOffset: 4000),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 160, coolDown: 7200, coolDownOffset: 4100),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 155, coolDown: 7200, coolDownOffset: 4200),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 150, coolDown: 7200, coolDownOffset: 4300),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 145, coolDown: 7200, coolDownOffset: 4400),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 140, coolDown: 7200, coolDownOffset: 4500),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 135, coolDown: 7200, coolDownOffset: 4600),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 130, coolDown: 7200, coolDownOffset: 4700),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 125, coolDown: 7200, coolDownOffset: 4800),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 120, coolDown: 7200, coolDownOffset: 4900),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 115, coolDown: 7200, coolDownOffset: 5000),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 110, coolDown: 7200, coolDownOffset: 5100),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 105, coolDown: 7200, coolDownOffset: 5200),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 100, coolDown: 7200, coolDownOffset: 5300),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 95, coolDown: 7200, coolDownOffset: 5400),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 90, coolDown: 7200, coolDownOffset: 5500),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 85, coolDown: 7200, coolDownOffset: 5600),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 80, coolDown: 7200, coolDownOffset: 5700),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 75, coolDown: 7200, coolDownOffset: 5800),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 70, coolDown: 7200, coolDownOffset: 5900),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 65, coolDown: 7200, coolDownOffset: 6000),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 60, coolDown: 7200, coolDownOffset: 6100),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 55, coolDown: 7200, coolDownOffset: 6200),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 50, coolDown: 7200, coolDownOffset: 6300),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 45, coolDown: 7200, coolDownOffset: 6400),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 40, coolDown: 7200, coolDownOffset: 6500),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 35, coolDown: 7200, coolDownOffset: 6600),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 30, coolDown: 7200, coolDownOffset: 6700),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 25, coolDown: 7200, coolDownOffset: 6800),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 20, coolDown: 7200, coolDownOffset: 6900),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 15, coolDown: 7200, coolDownOffset: 7000),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 10, coolDown: 7200, coolDownOffset: 7100),
                    new Shoot(30, 3, projectileIndex: 3, fixedAngle: 5, coolDown: 7200, coolDownOffset: 7200),

                    new HpLessTransition(0.05, "Dead")
                    ),
                new State("Dead",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("Aghh, well done warrior."),
                    new Flash(0xFF0000, 0.5, 5),
                    new TimedTransition(4000, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                       )
                ),
            new Threshold(0.005,
                new ItemLoot("Soul of Vitality", 1)
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Cadaverous Knife", 0.0014)
                ),
            new Threshold(0.01,
                new ItemLoot("Abyssal Sword", 0.002),
                new ItemLoot("Archdemon’s Remains", 0.002),
                new ItemLoot("Seal of the Underworld", 0.002),
                new ItemLoot("Demon’s Sigil", 0.002),
                new TierLoot(13, ItemType.Armor, 0.15),
                new TierLoot(6, ItemType.Ring, 0.15),
                new TierLoot(6, ItemType.Ability, 0.15),
                new TierLoot(13, ItemType.Weapon, 0.1)
                )
            )

        #endregion Soul of Vitality Boss (Abyss) - Done

        #region Soul of Wisdom Boss (UDL) - Done


        .Init("Soul of Wisdom Mob",
            new State(
                new Wander(0.4),
                new StayBack(1.1, 6),
                new ScaleHP2(10),
                new TransformOnDeath("Soul Death", 1, 1, 1),
                new State("First",
                    new HpLessTransition(0.5, "Rage"),
                    new Shoot(20, 8, shootAngle: 15, projectileIndex: 0, coolDown: 1500)
                    ),
                new State("Rage",
                    new SetAltTexture(1, 1),
                    new Shoot(20, 8, shootAngle: 15, projectileIndex: 0, coolDown: 1500),
                    new Grenade(3, 50, 10, coolDown: 1500, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1000)
                    )
                ),
            new Threshold(0.03,
                new ItemLoot("Cape of Septavius", 0.0014),
                new ItemLoot("Soul of Wisdom", 0.1),
                new ItemLoot("Soul-Stealing Trap", 0.0014)
                ),
            new Threshold(0.005,
                new ItemLoot("Soul of Wisdom", 1)
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.01,
                new ItemLoot("Doom Bow", 0.013),
                new ItemLoot("Edictum Praetoris", 0.02),
                new ItemLoot("Memento Mori", 0.02),
                new ItemLoot("Toga Picta", 0.02),
                new ItemLoot("Interregnum", 0.02),
                new ItemLoot("Tormentor’s Wrath", 0.015),
                new ItemLoot("Undead Lair Key", 0.015, 0, 0.03),
                new TierLoot(4, ItemType.Ability, 0.01),
                new TierLoot(5, ItemType.Ability, 0.005),
                new TierLoot(9, ItemType.Armor, 0.01),
                new TierLoot(9, ItemType.Weapon, 0.01)
                )
            )
        .Init("Undead Septavius",
            new State(
                new ScaleHP2(20),
                new State("Waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(10, "Taunt")
                    ),
                new State("Taunt",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Taunt("You thought this was the end? It's only the beginning!"),
                    new TimedTransition(200, "Shoot 1")
                    ),
                new State("Shoot 1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new ChangeSize(10, 130),
                        new Flash(0x00ff00, 0.5, 5),
                        new TimedTransition(5000, "Start 1")
                        ),
                    new State("Start 1",
                        new HpLessTransition(0.5, "Rage"),
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),

                        new Shoot(12, 3, projectileIndex: 0, shootAngle: 15, coolDown: 2000, coolDownOffset: 300),
                        new Shoot(12, 3, projectileIndex: 0, shootAngle: 15, coolDown: 2000, coolDownOffset: 600),
                        new Shoot(12, 3, projectileIndex: 0, shootAngle: 15, coolDown: 2000, coolDownOffset: 900),

                        new Shoot(12, 5, projectileIndex: 2, coolDown: 700),

                        new Shoot(20, 8, projectileIndex: 1, fixedAngle: 0, shootAngle: 45, coolDown: 3000, coolDownOffset: 200),
                        new Shoot(20, 8, projectileIndex: 1, fixedAngle: 10, shootAngle: 45, coolDown: 3000, coolDownOffset: 600),
                        new Shoot(20, 8, projectileIndex: 1, fixedAngle: 20, shootAngle: 45, coolDown: 3000, coolDownOffset: 1000),
                        new Shoot(20, 8, projectileIndex: 1, fixedAngle: 30, shootAngle: 45, coolDown: 3000, coolDownOffset: 1400),
                        new Shoot(20, 8, projectileIndex: 1, fixedAngle: 40, shootAngle: 45, coolDown: 3000, coolDownOffset: 1800),
                        new Shoot(20, 8, projectileIndex: 1, fixedAngle: 50, shootAngle: 45, coolDown: 3000, coolDownOffset: 2200),
                        new Shoot(20, 8, projectileIndex: 1, fixedAngle: 60, shootAngle: 45, coolDown: 3000, coolDownOffset: 2600)
                        ),
                new State("Rage",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Flash(0xFF0000, 0.5, 5),
                        new TimedTransition(2500, "Start 2")
                        ),
                    new State("Start 2",
                        new HpLessTransition(0.25, "Rage 2"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                        new Chase(9, 12),

                        new Shoot(8, 5, shootAngle: 10, projectileIndex: 1, coolDown: 500, predictive: 2),
                        new Shoot(20, 5, projectileIndex: 2, coolDown: 1000),
                        new Shoot(20, 2, shootAngle: 25, projectileIndex: 0, coolDown: 800)
                        ),
                new State("Rage 2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Flash(0xFF0000, 0.5, 5),
                        new ChangeSize(5, 150),
                        new TimedTransition(2500, "Start 4")
                        ),
                    new State("Start 4",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                        new TossObject2("Lair Skeleton Mage 1", 3, angle: 0, coolDown: 5000),
                        new TossObject2("Lair Skeleton Mage 1", 3, angle: 120, coolDown: 5000),
                        new TossObject2("Lair Skeleton Mage 1", 3, angle: 240, coolDown: 5000),
                        new Reproduce("Lair Skeleton King 1", 50, 3, 5000),
                        new Chase(12, 12),

                        new Shoot(8, 7, shootAngle: 10, projectileIndex: 1, coolDown: 300, predictive: 2),
                        new Shoot(20, 5, projectileIndex: 2, coolDown: 800),
                        new Shoot(20, 4, shootAngle: 25, projectileIndex: 0, coolDown: 600)
                        )
                ),

            new Threshold(0.03,
                new ItemLoot("Cape of Septavius", 0.0028),
                new ItemLoot("Potion of Wisdom", 1),
                new ItemLoot("Potion of Wisdom", 1),
                new ItemLoot("Soul-Stealing Trap", 0.0028)
                ),
            new Threshold(0.005,
                new ItemLoot("Soul of Wisdom", 1)
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.01,
                new ItemLoot("Special Dust", 0.01),
                new ItemLoot("Doom Bow", 0.026),
                new ItemLoot("Edictum Praetoris", 0.02),
                new ItemLoot("Memento Mori", 0.04),
                new ItemLoot("Toga Picta", 0.04),
                new ItemLoot("Interregnum", 0.04),
                new ItemLoot("Tormentor’s Wrath", 0.03),
                new ItemLoot("Undead Lair Key", 0.5, 0, 0.03),
                new TierLoot(6, ItemType.Ability, 0.1),
                new TierLoot(13, ItemType.Armor, 0.1),
                new TierLoot(13, ItemType.Weapon, 0.1)
                         )
            )
#endregion Soul of Wisdom Boss (UDL) - Done

        ;
    }
}
