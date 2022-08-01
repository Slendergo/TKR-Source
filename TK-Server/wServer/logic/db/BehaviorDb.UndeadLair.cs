using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ UndeadLair = () => Behav()
        .Init("Septavius the Ghost God",
            new State(
                new StayCloseToSpawn(3, 10),
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new ScaleHP2(20),
                new DropPortalOnDeath("Hideout of Septavius Portal", probability: 1, timeout: 70),
                new State("Waiting Player",
                    new PlayerWithinTransition(15, "Start", false)
                    ),
                new State("Start",
                    new InvisiToss("invisible Spawner", 4, 90, coolDown: 9999999),
                    new Taunt("Hello Warrior, What are you looking for?"),
                    new Flash(0x00FF00, 1, 3),
                    new TimedTransition(3000, "Start Shooting")
                    ),
                new State("Start Shooting",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                     //new RingAttack(20, 3, 0, projectileIndex: 0, 0.15, 0, coolDown: 200),
                     new Shoot(30, 4, projectileIndex: 0, fixedAngle: 0, coolDown: 1600, coolDownOffset: 200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 15, coolDown: 1600, coolDownOffset: 400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 30, coolDown: 1600, coolDownOffset: 600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 55, coolDown: 1600, coolDownOffset: 800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 60, coolDown: 1600, coolDownOffset: 1000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 75, coolDown: 1600, coolDownOffset: 1200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 80, coolDown: 1600, coolDownOffset: 1400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 95, coolDown: 1600, coolDownOffset: 1600),

                    new Shoot(20, 5, shootAngle: 5, projectileIndex: 4, coolDown: 1000, coolDownOffset: 500),
                    new HpLessTransition(0.75, "Second Phase")
                    ),
                new State("Second Phase",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new Taunt("My Minion's will Destroy you!"),
                    new TimedTransition(3000, "Second Phase Start")
                    ),
                new State("Second Phase Start",
                   // new RingAttack(20, 3, 0, projectileIndex: 0, 0.15, 0, coolDown: 200),
                   new Shoot(30, 4, projectileIndex: 0, fixedAngle: 0, coolDown: 1600, coolDownOffset: 200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 15, coolDown: 1600, coolDownOffset: 400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 30, coolDown: 1600, coolDownOffset: 600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 55, coolDown: 1600, coolDownOffset: 800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 60, coolDown: 1600, coolDownOffset: 1000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 75, coolDown: 1600, coolDownOffset: 1200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 80, coolDown: 1600, coolDownOffset: 1400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 95, coolDown: 1600, coolDownOffset: 1600),
                    new Shoot(20, 5, shootAngle: 5, projectileIndex: 4, coolDown: 1000, coolDownOffset: 500),
                    new Reproduce("Lair Skeleton Mage 1", 50, 3, coolDown: 1500),
                    new Reproduce("Lair Skeleton King 1", 50, 3, 1000),
                    new ReproduceGroup("Lair Ghosts", 50, 3, coolDown: 1000),
                    new HpLessTransition(0.50, "Third Phase")
                    ),
                new State("Third Phase",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new Flash(0xFF0000, 1, 3),
                    new Taunt("No! What have you done to my Minions!"),
                    new TimedTransition(3000, "Third Phase Start")
                    ),
                new State("Third Phase Start",
                    //new RingAttack(20, 4, 0, projectileIndex: 0, 0.15, 0, coolDown: 200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 0, coolDown: 1600, coolDownOffset: 200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 15, coolDown: 1600, coolDownOffset: 400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 30, coolDown: 1600, coolDownOffset: 600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 55, coolDown: 1600, coolDownOffset: 800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 60, coolDown: 1600, coolDownOffset: 1000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 75, coolDown: 1600, coolDownOffset: 1200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 80, coolDown: 1600, coolDownOffset: 1400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 95, coolDown: 1600, coolDownOffset: 1600),
                    new Shoot(20, 5, shootAngle: 5, projectileIndex: 4, coolDown: 1000, coolDownOffset: 500),
                    new RingAttack(20, 2, 0, projectileIndex: 2, 0.25, 0.30, coolDown: 200),
                    new HpLessTransition(0.25, "Rage")
                    ),
                new State("Rage",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new Flash(0xFF0000, 0.3, 10),
                    new Taunt("You... will... DIE!"),
                    new TimedTransition(3000, "Rage Start")
                    ),
                new State("Rage Start",
                    new Follow(1, 20, 1),
                    new Shoot(20, 8, projectileIndex: 5, coolDown: 1500),
                    new Shoot(20, 5, shootAngle: 5, projectileIndex: 0, coolDown: 500, coolDownOffset: 500),
                    new Shoot(20, 3, shootAngle: 15, projectileIndex: 1, coolDown: 1000, coolDownOffset: 1500),
                    new Shoot(20, 2, shootAngle: 25, projectileIndex: 2, coolDown: 1000, coolDownOffset: 500)
                    )
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Cape of Septavius", 0.0014)
                ),
            new Threshold(0.01,
                new TierLoot(11, ItemType.Weapon, 0.07),
                new TierLoot(11, ItemType.Armor, 0.07),
                new TierLoot(4, ItemType.Ring, 0.1),
                new TierLoot(5, ItemType.Ring, 0.07),
                new TierLoot(4, ItemType.Ability, 0.1),
                new TierLoot(5, ItemType.Ability, 0.07),
                new ItemLoot("Potion of Wisdom", 1),
                new ItemLoot("Potion of Wisdom", 1),
                new ItemLoot("Doom Bow", 0.01),
                new ItemLoot("Edictum Praetoris", 0.015),
                new ItemLoot("Memento Mori", 0.015),
                new ItemLoot("Toga Picta", 0.015),
                new ItemLoot("Interregnum", 0.015),
                new ItemLoot("Tormentor’s Wrath", 0.01),
                new ItemLoot("Undead Lair Key", 0.001, 0, 0.03),

                new ItemLoot("Magic Dust", 0.5)
                )
            )
        .Init("invisible Spawner",
            new State(
                new State("idle",
                    new EntityNotExistsTransition("Septavius the Ghost God", 30, "drop portal")
                    ),
                new State("drop portal",
                    new RealmPortalDrop()
                    )
                )
            )
        .Init("Ghost Mage of Septavius",
            new State(
                new Prioritize(
                    new Protect(0.625, "Septavius the Ghost God", protectionRange: 6),
                    new Follow(0.75, range: 7)
                    ),
                new Wander(0.25),
                new Shoot(8, 1, coolDown: 1000)
                )
            )
        .Init("Ghost Rogue of Septavius",
            new State(
                new Follow(0.75, range: 1),
                new Wander(0.25),
                new Shoot(8, 1, coolDown: 1000)
                )
            )
        .Init("Ghost Warrior of Septavius",
            new State(
                new Follow(0.75, range: 1),
                new Wander(0.25),
                new Shoot(8, 1, coolDown: 1000)
                )
            )
        .Init("Lair Ghost Archer",
            new State(
                new Prioritize(
                    new Protect(0.625, "Septavius the Ghost God", protectionRange: 6),
                    new Follow(0.75, range: 7)
                    ),
                new Wander(0.25),
                new Shoot(8, 1, coolDown: 1000)
                )
            )
        .Init("Lair Ghost Knight",
            new State(
                new Follow(0.75, range: 1),
                new Wander(0.25),
                new Shoot(8, 1, coolDown: 1000)
                )
            )
        .Init("Lair Ghost Mage",
            new State(
                new Prioritize(
                    new Protect(0.625, "Septavius the Ghost God", protectionRange: 6),
                    new Follow(0.75, range: 7)
                    ),
                new Wander(0.25),
                new Shoot(8, 1, coolDown: 1000)
                )
            )
        .Init("Lair Ghost Paladin",
            new State(
                new Follow(0.75, range: 1),
                new Wander(0.25),
                new Shoot(8, 1, coolDown: 1000),
                new HealSelf(coolDown: 5000)
                )
            )
        .Init("Lair Ghost Rogue",
            new State(
                new Follow(0.75, range: 1),
                new Wander(0.25),
                new Shoot(8, 1, coolDown: 1000)
                ),
            new ItemLoot("Magic Potion", 0.25),
            new ItemLoot("Health Potion", 0.25)
            )
        .Init("Lair Ghost Warrior",
            new State(
                new Follow(0.75, range: 1),
                new Wander(0.25),
                new Shoot(8, 1, coolDown: 1000)
                ),
            new ItemLoot("Magic Potion", 0.25),
            new ItemLoot("Health Potion", 0.25)
            )

        .Init("Lair Skeleton",
            new State(
                new Shoot(6),
                new Prioritize(
                    new Follow(1, range: 1),
                    new Wander(0.4)
                    )
                ),
            new ItemLoot("Magic Potion", 0.05),
            new ItemLoot("Health Potion", 0.05)
            )
        .Init("Lair Skeleton King",
            new State(
                new Shoot(10, 3, shootAngle: 10),
                new Prioritize(
                    new Follow(1, range: 7),
                    new Wander(0.4)
                    )
                ),
            new TierLoot(5, ItemType.Armor, 0.2),
            new Threshold(0.5,
                new TierLoot(6, ItemType.Weapon, 0.2),
                new TierLoot(7, ItemType.Weapon, 0.1),
                new TierLoot(8, ItemType.Weapon, 0.05),
                new TierLoot(6, ItemType.Armor, 0.1),
                new TierLoot(7, ItemType.Armor, 0.05),
                new TierLoot(3, ItemType.Ring, 0.1),
                new TierLoot(3, ItemType.Ability, 0.1)
                )
            )
        .Init("Lair Skeleton Mage",
            new State(
                new Shoot(10),
                new Prioritize(
                    new Follow(1, range: 7),
                    new Wander(0.4)
                    )
                ),
            new ItemLoot("Magic Potion", 0.05),
            new ItemLoot("Health Potion", 0.05)
            )
        .Init("Lair Skeleton King 1",
            new State(
                new Shoot(10, 3, shootAngle: 10),
                new Prioritize(
                    new Follow(1, range: 7),
                    new Wander(0.4)
                    )
                )
            )
        .Init("Lair Skeleton Mage 1",
            new State(
                new Shoot(10),
                new Prioritize(
                    new Follow(1, range: 7),
                    new Wander(0.4)
                    )
                )
            )
        .Init("Lair Skeleton Swordsman",
            new State(
                new Shoot(5),
                new Prioritize(
                    new Follow(1, range: 1),
                    new Wander(0.4)
                    )
                ),
            new ItemLoot("Magic Potion", 0.05),
            new ItemLoot("Health Potion", 0.05)
            )
        .Init("Lair Skeleton Veteran",
            new State(
                new Shoot(5),
                new Prioritize(
                    new Follow(1, range: 1),
                    new Wander(0.4)
                    )
                ),
            new ItemLoot("Magic Potion", 0.05),
            new ItemLoot("Health Potion", 0.05)
            )
        .Init("Lair Mummy",
            new State(
                new Shoot(10),
                new Prioritize(
                    new Follow(0.9, range: 7),
                    new Wander(0.4)
                    )
                ),
            new ItemLoot("Magic Potion", 0.05),
            new ItemLoot("Health Potion", 0.05)
            )
        .Init("Lair Mummy King",
            new State(
                new Shoot(10),
                new Prioritize(
                    new Follow(0.9, range: 7),
                    new Wander(0.4)
                    )
                ),
            new ItemLoot("Magic Potion", 0.05),
            new ItemLoot("Health Potion", 0.05)
            )
        .Init("Lair Mummy Pharaoh",
            new State(
                new Shoot(10),
                new Prioritize(
                    new Follow(0.9, range: 7),
                    new Wander(0.4)
                    )
                ),
            new TierLoot(5, ItemType.Armor, 0.2),
            new Threshold(0.5,
                new TierLoot(6, ItemType.Weapon, 0.2),
                new TierLoot(7, ItemType.Weapon, 0.1),
                new TierLoot(8, ItemType.Weapon, 0.05),
                new TierLoot(6, ItemType.Armor, 0.1),
                new TierLoot(7, ItemType.Armor, 0.05),
                new TierLoot(3, ItemType.Ring, 0.1),
                new TierLoot(3, ItemType.Ability, 0.1)
                )
            )

        .Init("Lair Big Brown Slime",
            new State(
                new Shoot(10, 3, shootAngle: 10, coolDown: 500),
                new Wander(0.1),
                new TransformOnDeath("Lair Little Brown Slime", 1, 6, 1)
                // new SpawnOnDeath("Lair Little Brown Slime", 1.0, 6)
                )
            )
        .Init("Lair Little Brown Slime",
            new State(
                new Shoot(10, 3, shootAngle: 10, coolDown: 500),
                new Protect(0.1, "Lair Big Brown Slime", acquireRange: 5, protectionRange: 2),
                new Wander(0.1)
                ),
            new ItemLoot("Magic Potion", 0.05),
            new ItemLoot("Health Potion", 0.05)
            )
        .Init("Lair Big Black Slime",
            new State(
                new Shoot(10, coolDown: 1000),
                new Wander(0.1),
                new TransformOnDeath("Lair Little Black Slime", 1, 4, 1)
                //new SpawnOnDeath("Lair Medium Black Slime", 1.0, 4)
                )
            )
        .Init("Lair Medium Black Slime",
            new State(
                new Shoot(10, coolDown: 1000),
                new Wander(0.1),
                new TransformOnDeath("Lair Little Black Slime", 1, 4, 1)
                // new SpawnOnDeath("Lair Little Black Slime", 1.0, 4)
                )
            )
        .Init("Lair Little Black Slime",
            new State(
                new Shoot(10, coolDown: 1000),
                new Wander(0.1)
                ),
            new ItemLoot("Magic Potion", 0.05),
            new ItemLoot("Health Potion", 0.05)
            )

        .Init("Lair Construct Giant",
            new State(
                new Prioritize(
                    new Follow(0.8, range: 7),
                    new Wander(0.4)
                    ),
                new Shoot(10, 3, shootAngle: 20, coolDown: 1000),
                new Shoot(10, projectileIndex: 1, coolDown: 1000)
                ),
            new TierLoot(5, ItemType.Armor, 0.2),
            new Threshold(0.5,
                new TierLoot(6, ItemType.Weapon, 0.2),
                new TierLoot(7, ItemType.Weapon, 0.1),
                new TierLoot(8, ItemType.Weapon, 0.05),
                new TierLoot(6, ItemType.Armor, 0.1),
                new TierLoot(7, ItemType.Armor, 0.05),
                new TierLoot(3, ItemType.Ring, 0.1),
                new TierLoot(3, ItemType.Ability, 0.1)
                )
            )
        .Init("Lair Construct Titan",
            new State(
                new Prioritize(
                    new Follow(0.8, range: 7),
                    new Wander(0.4)
                    ),
                new Shoot(10, 3, shootAngle: 20, coolDown: 1000),
                new Shoot(10, 3, shootAngle: 20, projectileIndex: 1, coolDownOffset: 100, coolDown: 2000)
                ),
            new TierLoot(5, ItemType.Armor, 0.2),
            new Threshold(0.5,
                new TierLoot(6, ItemType.Weapon, 0.2),
                new TierLoot(7, ItemType.Weapon, 0.1),
                new TierLoot(8, ItemType.Weapon, 0.05),
                new TierLoot(6, ItemType.Armor, 0.1),
                new TierLoot(7, ItemType.Armor, 0.05),
                new TierLoot(3, ItemType.Ring, 0.1),
                new TierLoot(3, ItemType.Ability, 0.1)
                )
            )

        .Init("Lair Brown Bat",
            new State(
                new Wander(0.1),
                new Charge(3, 8, 2000),
                new Shoot(3, coolDown: 1000)
                ),
            new ItemLoot("Magic Potion", 0.05),
            new ItemLoot("Health Potion", 0.05)
            )
        .Init("Lair Ghost Bat",
            new State(
                new Wander(0.1),
                new Charge(3, 8, 2000),
                new Shoot(3, coolDown: 1000)
                ),
            new ItemLoot("Magic Potion", 0.05),
            new ItemLoot("Health Potion", 0.05)
            )

        .Init("Lair Reaper",
            new State(
                new Shoot(3),
                new Follow(1.3, range: 1),
                new Wander(0.1)
                ),
            new TierLoot(5, ItemType.Armor, 0.2),
            new Threshold(0.5,
                new TierLoot(6, ItemType.Weapon, 0.2),
                new TierLoot(7, ItemType.Weapon, 0.1),
                new TierLoot(8, ItemType.Weapon, 0.05),
                new TierLoot(6, ItemType.Armor, 0.1),
                new TierLoot(7, ItemType.Armor, 0.05),
                new TierLoot(3, ItemType.Ring, 0.1),
                new TierLoot(3, ItemType.Ability, 0.1)
                )
            )
        .Init("Lair Vampire",
            new State(
                new Shoot(10, coolDown: 500),
                new Shoot(3, coolDown: 1000),
                new Follow(1.3, range: 1),
                new Wander(0.1)
                ),
            new ItemLoot("Magic Potion", 0.05),
            new ItemLoot("Health Potion", 0.05)
            )
        .Init("Lair Vampire King",
            new State(
                new Shoot(10, coolDown: 500),
                new Shoot(3, coolDown: 1000),
                new Follow(1.3, range: 1),
                new Wander(0.1)
                ),
            new TierLoot(5, ItemType.Armor, 0.2),
            new Threshold(0.5,
                new TierLoot(6, ItemType.Weapon, 0.2),
                new TierLoot(7, ItemType.Weapon, 0.1),
                new TierLoot(8, ItemType.Weapon, 0.05),
                new TierLoot(6, ItemType.Armor, 0.1),
                new TierLoot(7, ItemType.Armor, 0.05),
                new TierLoot(3, ItemType.Ring, 0.1),
                new TierLoot(3, ItemType.Ability, 0.1)
                )
            )

        .Init("Lair Grey Spectre",
            new State(
                new Wander(0.1),
                new Shoot(10, coolDown: 1000),
                new Grenade(2.5, 50, 8, coolDown: 1000)
                )
            )
        .Init("Lair Blue Spectre",
            new State(
                new Wander(0.1),
                new Shoot(10, coolDown: 1000),
                new Grenade(2.5, 70, 8, coolDown: 1000)
                )
            )
        .Init("Lair White Spectre",
            new State(
                new Wander(0.1),
                new Shoot(10, coolDown: 1000),
                new Grenade(2.5, 90, 8, coolDown: 1000)
                ),
            new Threshold(0.5,
                new TierLoot(4, ItemType.Ability, 0.15)
                )
            )
        .Init("Lair Burst Trap",
            new State(
                new State("FinnaBustANut",
                    new PlayerWithinTransition(3, "Aaa")
                    ),
                new State("Aaa",
                    new Shoot(8.4, count: 12, projectileIndex: 0),
                    new Suicide()
                    )))
        .Init("Lair Blast Trap",
            new State(
                new State("FinnaBustANut",
                    new PlayerWithinTransition(3, "Aaa")
                    ),
                new State("Aaa",
                    new Shoot(25, projectileIndex: 0, count: 12, coolDown: 3000),
                    new Suicide()
                    )))
        ;
    }
}
