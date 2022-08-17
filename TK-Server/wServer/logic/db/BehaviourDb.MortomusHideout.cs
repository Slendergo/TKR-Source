using common.resources;
using System.Runtime.InteropServices;
using wServer.core.objects;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using wServer.networking.packets.outgoing;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ MortomusHideout = () => Behav()
        .Init("Mortomus, Keeper of Souls",
            new State(
                new ScaleHP2(30),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new PlayerWithinTransition(20, "prepare")
                    ),
                new State("prepare",
                    new Taunt("AT LAST!"),
                    new Taunt("Allow me to introduce myself!"),
                    new TimedTransition(5000, "phase 1")
                    ),
                new State("phase 1",
                    new TossObject("Haunted Guard", 8, 0, coolDown: 999999),
                    new TossObject("Haunted Knight", 4, 320, coolDown: 999999),
                    new TossObject("Haunted Warrior", 4, 45, coolDown: 999999),
                    new Shoot(15, 12, projectileIndex: 2, coolDown: 1000)
                    //new EntitiesNotExistsTransition(40, "phase 2", "Haunted Guard", "Haunted Knight", "Haunted Warrior")
                    ),
                new State("phase 2",
                    new Taunt("Now that we’ve had a proper introduction, let me end you NOW!"),
                    new TimedTransition(5000, "phase 3")
                    ),
                new State("phase 3",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 0, coolDown: 3700, coolDownOffset: 100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 5, coolDown: 3700, coolDownOffset: 200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 10, coolDown: 3700, coolDownOffset: 300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 15, coolDown: 3700, coolDownOffset: 400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 20, coolDown: 3700, coolDownOffset: 500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 25, coolDown: 3700, coolDownOffset: 600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 30, coolDown: 3700, coolDownOffset: 700),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 35, coolDown: 3700, coolDownOffset: 800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 40, coolDown: 3700, coolDownOffset: 900),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 45, coolDown: 3700, coolDownOffset: 1000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 50, coolDown: 3700, coolDownOffset: 1100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 55, coolDown: 3700, coolDownOffset: 1200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 60, coolDown: 3700, coolDownOffset: 1300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 65, coolDown: 3700, coolDownOffset: 1400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 70, coolDown: 3700, coolDownOffset: 1500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 75, coolDown: 3700, coolDownOffset: 1600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 80, coolDown: 3700, coolDownOffset: 1700),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 85, coolDown: 3700, coolDownOffset: 1800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 90, coolDown: 3700, coolDownOffset: 1900),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 95, coolDown: 3700, coolDownOffset: 2000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 100, coolDown: 3700, coolDownOffset: 2100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 105, coolDown: 3700, coolDownOffset: 2200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 110, coolDown: 3700, coolDownOffset: 2300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 115, coolDown: 3700, coolDownOffset: 2400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 120, coolDown: 3700, coolDownOffset: 2500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 125, coolDown: 3700, coolDownOffset: 2600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 130, coolDown: 3700, coolDownOffset: 2700),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 135, coolDown: 3700, coolDownOffset: 2800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 140, coolDown: 3700, coolDownOffset: 2900),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 145, coolDown: 3700, coolDownOffset: 3000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 150, coolDown: 3700, coolDownOffset: 3100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 155, coolDown: 3700, coolDownOffset: 3200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 160, coolDown: 3700, coolDownOffset: 3300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 165, coolDown: 3700, coolDownOffset: 3400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 170, coolDown: 3700, coolDownOffset: 3500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 175, coolDown: 3700, coolDownOffset: 3600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 180, coolDown: 3700, coolDownOffset: 3700),
                    new HpLessTransition(0.80, "attack2.1")
                    ),
                new State("attack2.1",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 300),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 500),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 0, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 0, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 0, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.2")
                    ),
                new State("attack2.2",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 800),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 45, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 45, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 45, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.3")
                    ),
                new State("attack2.3",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 800),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 90, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 90, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 90, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.4")
                    ),
                new State("attack2.4",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 800),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 135, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 135, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 135, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.5")
                    ),
                new State("attack2.5",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 800),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 180, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 180, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 180, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.6")
                    ),
                new State("attack2.6",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 800),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 225, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 225, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 225, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.7")
                    ),
                new State("attack2.7",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 800),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 270, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 270, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 270, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.8")
                    ),
                new State("attack2.8",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 800),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 315, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 315, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 315, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.1")
                    ),
                new State("attack3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("HAHAHAH!"),
                    new Chase(4),
                    new Shoot(10, 2, projectileIndex: 5, shootAngle: 20, predictive: 1, coolDown: 350),
                    new Grenade(3, 65, range: 8, fixedAngle: 0, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 8, fixedAngle: 45, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 8, fixedAngle: 90, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 8, fixedAngle: 135, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 8, fixedAngle: 180, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 8, fixedAngle: 225, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 8, fixedAngle: 270, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 8, fixedAngle: 315, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(15000, "attack4")
                    ),
                new State("attack4",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ReturnToSpawn(3),
                    new TimedTransition(1000, "attack4.1")
                    ),
                new State("attack4.1",
                    new ReturnToSpawn(3),
                    new Shoot(12, 3, projectileIndex: 6, shootAngle: 15, predictive: 1, coolDown: 400),
                    new Shoot(15, 1, projectileIndex: 7, predictive: 1.2, coolDown: 800),
                    new TossObject("Haunted Bat", 4, 0, coolDown: 999999, coolDownOffset: 100),
                    new TossObject("Haunted Bat", 4, 180, coolDown: 999999, coolDownOffset: 100),
                    new TossObject("Killer Bat", 6, 45, coolDown: 999999, coolDownOffset: 500),
                    new TossObject("Killer Bat", 6, 225, coolDown: 999999, coolDownOffset: 500),
                    new TossObject("Demon Bat", 8, 90, coolDown: 999999, coolDownOffset: 1000),
                    new TossObject("Demon Bat", 8, 270, coolDown: 999999, coolDownOffset: 1000),
                    new HpLessTransition(.35, "attack5")
                    ),
                new State("attack5",
                    new RemoveEntity(100, "Haunted Bat"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("MY CHILDREN! I SUMMON YOU!"),
                    new TimedTransition(2000, "attack5.1")
                    ),
                new State("attack5.1",
                    new Wander(.25),
                    new TossObject("Undead Blood Bat", 5, 0, coolDown: 99999),
                    new TossObject("Crawling Devourer", 5, 90, coolDown: 99999),
                    new TossObject("Cursed Mermaid", 5, 180, coolDown: 99999),
                    new TossObject("Haunted Guard", 5, 270, coolDown: 99999),
                    new Shoot(10, 4, projectileIndex: 8, shootAngle: 10, predictive: 1, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 9, shootAngle: 20, predictive: .8, coolDown: 350),
                    new HpLessTransition(.15, "attack6")
                    ),
                new State("attack6",
                    new ReturnToSpawn(3),
                    new RemoveEntity(100, "Undead Blood Bat"),
                    new RemoveEntity(100, "Crawling Devourer"),
                    new RemoveEntity(100, "Cursed Mermaid"),
                    new RemoveEntity(100, "Haunted Guard"),
                    new Shoot(10, 4, projectileIndex: 10, shootAngle: 10, predictive: 1, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 11, shootAngle: 20, predictive: .8, coolDown: 350),
                    new HpLessTransition(.1, "dead")
                    ),
                new State("dead",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Taunt("M-my children!!! NOOOO!"),
                    new Shoot(20, 25, projectileIndex: 10, coolDown: 99999),
                    new TimedTransition(500, "dead1")
                    ),
                new State("dead1",
                    new Suicide()
                     )
                ),
                new Threshold(0.01,
                LootTemplates.DustLoot()
                    ),
            new Threshold(0.05,
                new TierLoot(12, ItemType.Weapon, 0.05),
                new TierLoot(13, ItemType.Armor, 0.05),
                new TierLoot(5, ItemType.Ring, 0.05),
                new TierLoot(6, ItemType.Ability, 0.07),
                new ItemLoot("Potion of Wisdom", 0.5),
                new ItemLoot("Potion of Speed", 0.5),
                new ItemLoot("Potion of Vitality", 0.5),
                new ItemLoot("Potion of Dexterity", 0.5),
                new ItemLoot("Potion of Life", 0.5),
                new ItemLoot("Potion of Mana", 0.5),
                new ItemLoot("Potion of Defense", 0.5),
                new ItemLoot("Potion of Attack", 0.5),
                new ItemLoot("Old Cleric's Cloak", 0.0016),
                new ItemLoot("Mortomus' Shovel", 0.0001),
                new ItemLoot("Ghostly Warrior's Lantern", 0.0016),
                new ItemLoot("Scepter of Whispers", 0.0016),
                new ItemLoot("Magic Dust", 0.5)
                )
            )
        .Init("Undead Blood Bat",
            new State(
                new ScaleHP2(4),
                new State("attack",
                    new Taunt("SCREEECH"),
                    new Charge(3, 10, coolDown: 1500),
                    new Shoot(2, 8, shootAngle: 30, projectileIndex: 0, predictive: .7, coolDown: 500),
                    new Shoot(6, 1, projectileIndex: 0, predictive: 1, coolDown: 250)
                    )
                )
            )
        .Init("Haunted Bat",
            new State(
                new ScaleHP2(4),
                new Taunt("SCREEECH"),
                //new OrbitBehavior("Mortomus, Keeper of Souls", speedVariability: 0, speed: 0.5, radius: 4.0, radiusVariability: 0),
                new Orbit(0.5, 4.0),
                new Shoot(30, 1, projectileIndex: 0, coolDown: 200)
                )
            )
        .Init("Killer Bat",
            new State(
                new ScaleHP2(4),
                new Taunt("SCREEECH"),
                //new OrbitBehavior("Mortomus, Keeper of Souls", speedVariability: 0, speed: 1.0, radius: 6.0, radiusVariability: 0),
                new Orbit(1, 6.0),
                new Shoot(30, 1, projectileIndex: 0, coolDown: 155)
                )
            )
        .Init("Demon Bat",
            new State(
                new ScaleHP2(4),
                 new Taunt("SCREEECH"),
                 //new OrbitBehavior("Mortomus, Keeper of Souls", speedVariability: 0, speed: 1.5, radius: 8.0, radiusVariability: 0),
                 new Orbit(1, 8.0),
                 new Shoot(30, 1, projectileIndex: 0, coolDown: 55)
                //        )
                //     )
                // .Init("Haunted Knight",
                //     new State(
                //         new ScaleHP2(5),
                //         new State("attack",
                //             new Wander(0.4),
                //             new Charge(4, 10, coolDown: 2000),
                //             new TimedTransition(2000, "Fire")
                //             ),
                //         new State("Fire",
                //             new Wander(0.3),
                //             new Shoot(8, 2, shootAngle: 10, projectileIndex: 0, predictive: .9, coolDown: 2000, coolDownOffset: 0),
                //             new Shoot(8, 2, shootAngle: 10, projectileIndex: 0, predictive: .9, coolDown: 2000, coolDownOffset: 300),
                //             new Shoot(8, 2, shootAngle: 10, projectileIndex: 0, predictive: .9, coolDown: 2000, coolDownOffset: 600),
                //             new TimedTransition(2000, "attack")
                //             )
                )
            )
        .Init("Haunted Warrior",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Follow(1, 12),
                    new StayBack(1, 4),
                    new HealEntity(30, "Haunted Guard", 1000, coolDown: 2000),
                    new HealEntity(30, "Haunted Knight", 1000, coolDown: 2000),
                    new SwirlingMistDeathParticles(),
                    new Shoot(8, 3, shootAngle: 10, projectileIndex: 0, predictive: .9, coolDown: 800)
                    )
                )
            )
        //.Init("Haunted Guard",
        //    new State(
        //        new ScaleHP2(20),
        //        new State("attack",
        //            new Wander(0.3),
        //            new Prioritize(
        //                new Protect(2, "Mortomus, Keeper of Souls", 10, 3)),
        //            new Grenade(5, 140, 12, null, 1000, ConditionEffectIndex.Weak, 1000, 0x017E52)
        //            )
        //        )
        //    )
        .Init("Crawling Devourer",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Wander(.25),
                    new SwirlingMistDeathParticles(),
                    new Shoot(6, 5, shootAngle: 12, projectileIndex: 0, predictive: 1.2, coolDown: 1000)
                    )
                )
            )
        .Init("Cursed Mermaid",
            new State(
                new ScaleHP2(5),
                 new State("attack",
                     new Wander(.15),
                     new Grenade(radius: 3, damage: 80, range: 8, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000, color: 0xffffff),
                     new Shoot(8, 3, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 1500)
                    )
                ));
    }
}