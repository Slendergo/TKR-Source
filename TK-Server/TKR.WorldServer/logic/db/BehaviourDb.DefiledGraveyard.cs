using TKR.Shared.resources;
using System.Runtime.InteropServices;
using TKR.WorldServer.logic.loot;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.logic.behaviors;
using TKR.WorldServer.logic.transitions;
using TKR.WorldServer.networking.packets.outgoing;

namespace TKR.WorldServer.logic
{
    partial class BehaviorDb
    {
        private _ MortomusHideout = () => Behav()
        .Init("Mortomus, Keeper of Souls",
            new State(
                new ScaleHP2(30),
                new State("idle",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true),
                    new PlayerWithinTransition(20, "talk")
                    ),
                new State("talk",
                    new Taunt("Prove your worth, kill my minions"),
                    new EntitiesNotExistsTransition(200, "prepare", "Crawling Devourer", "Cursed Mermaid", "Undead Blood Bat")
                    ),
                new State("prepare",
                    new Taunt("Who goes there and how have you found this place? Cereberus must have failed me!"),
                    new Taunt("I've hidden here for a millenia. Why have you come to this place! Deal with them, my hallowed knights!"),
                    new TimedRandomTransition(10000, false, "prepare 1", "prepare 1.1", "prepare 1.2")
                    ),
                new State("prepare 1",
                    new TossObject("Haunted Guard", 8, 270, coolDown: 999999),
                    new TossObject("Haunted Knight", 4, 315, coolDown: 999999),
                    new TossObject("Haunted Warrior", 4, 225, coolDown: 999999),
                    new TossObject("Haunted Knight", 4, 90, coolDown: 999999),
                    new TossObject("Haunted Warrior", 4, 180, coolDown: 999999),
                    new TimedTransition(5000, "phase 1")
                    ),
                new State("prepare 1.1",
                    new TossObject("Haunted Guard", 8, 270, coolDown: 999999),
                    new TossObject("Haunted Knight", 4, 315, coolDown: 999999),
                    new TossObject("Haunted Warrior", 4, 225, coolDown: 999999),
                    new TossObject("Haunted Knight", 4, 90, coolDown: 999999),
                    new TimedTransition(5000, "phase 1")
                    ),
                new State("prepare 1.2",
                    new TossObject("Haunted Guard", 8, 270, coolDown: 999999),
                    new TossObject("Haunted Knight", 4, 315, coolDown: 999999),
                    new TossObject("Haunted Warrior", 4, 225, coolDown: 999999),
                    new TimedTransition(5000, "phase 1")
                    ),
                new State("phase 1",
                    new Shoot(15, 12, projectileIndex: 2, fixedAngle: 0, coolDown: 600, rotateAngle: 3),
                    new EntitiesNotExistsTransition(40, "phase 2", "Haunted Guard", "Haunted Knight", "Haunted Warrior")
                    ),
                new State("phase 2",
                    new Taunt("The souls in this place grant me power! Have a taste of the weary souls that have been lost, eternally wandering this place!"),
                    new TimedTransition(5000, "phase 3")
                    ),
                new State("phase 3",
                    new Shoot(15, 12, projectileIndex: 2, fixedAngle: 0, coolDown: 600, rotateAngle: 3),
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 0, coolDown: 3600, coolDownOffset: 0),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 5, coolDown: 3600, coolDownOffset: 100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 10, coolDown: 3600, coolDownOffset: 200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 15, coolDown: 3600, coolDownOffset: 300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 20, coolDown: 3600, coolDownOffset: 400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 25, coolDown: 3600, coolDownOffset: 500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 30, coolDown: 3600, coolDownOffset: 600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 35, coolDown: 3600, coolDownOffset: 700),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 40, coolDown: 3600, coolDownOffset: 800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 45, coolDown: 3600, coolDownOffset: 900),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 50, coolDown: 3600, coolDownOffset: 1000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 55, coolDown: 3600, coolDownOffset: 1100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 60, coolDown: 3600, coolDownOffset: 1200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 65, coolDown: 3600, coolDownOffset: 1300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 70, coolDown: 3600, coolDownOffset: 1400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 75, coolDown: 3600, coolDownOffset: 1500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 80, coolDown: 3600, coolDownOffset: 1600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 85, coolDown: 3600, coolDownOffset: 1700),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 90, coolDown: 3600, coolDownOffset: 1800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 95, coolDown: 3600, coolDownOffset: 1900),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 100, coolDown: 3600, coolDownOffset: 2000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 105, coolDown: 3600, coolDownOffset: 2100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 110, coolDown: 3600, coolDownOffset: 2200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 115, coolDown: 3600, coolDownOffset: 2300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 120, coolDown: 3600, coolDownOffset: 2400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 125, coolDown: 3600, coolDownOffset: 2500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 130, coolDown: 3600, coolDownOffset: 2600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 135, coolDown: 3600, coolDownOffset: 2700),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 140, coolDown: 3600, coolDownOffset: 2800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 145, coolDown: 3600, coolDownOffset: 2900),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 150, coolDown: 3600, coolDownOffset: 3000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 155, coolDown: 3600, coolDownOffset: 3100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 160, coolDown: 3600, coolDownOffset: 3200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 165, coolDown: 3600, coolDownOffset: 3300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 170, coolDown: 3600, coolDownOffset: 3400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 175, coolDown: 3600, coolDownOffset: 3500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 180, coolDown: 3600, coolDownOffset: 3600),
                    new HpLessTransition(0.60, "attack2.1")
                    ),
                new State("attack2.1",
                    new Shoot(15, 12, projectileIndex: 2, fixedAngle: 0, coolDown: 600, rotateAngle: 3),
                    new Taunt("HAHAHA! Good, good! Glimpse into the power of a dark magician!"),
                    new TossObject2("Undead Blood Bat", 4, angle: 45, coolDown: 999999),
                    new TossObject2("Undead Blood Bat", 4, angle: 135, coolDown: 999999),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 0, coolDown: 3600, coolDownOffset: 0),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 5, coolDown: 3600, coolDownOffset: 100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 10, coolDown: 3600, coolDownOffset: 200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 15, coolDown: 3600, coolDownOffset: 300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 20, coolDown: 3600, coolDownOffset: 400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 25, coolDown: 3600, coolDownOffset: 500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 30, coolDown: 3600, coolDownOffset: 600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 35, coolDown: 3600, coolDownOffset: 700),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 40, coolDown: 3600, coolDownOffset: 800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 45, coolDown: 3600, coolDownOffset: 900),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 50, coolDown: 3600, coolDownOffset: 1000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 55, coolDown: 3600, coolDownOffset: 1100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 60, coolDown: 3600, coolDownOffset: 1200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 65, coolDown: 3600, coolDownOffset: 1300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 70, coolDown: 3600, coolDownOffset: 1400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 75, coolDown: 3600, coolDownOffset: 1500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 80, coolDown: 3600, coolDownOffset: 1600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 85, coolDown: 3600, coolDownOffset: 1700),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 90, coolDown: 3600, coolDownOffset: 1800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 95, coolDown: 3600, coolDownOffset: 1900),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 100, coolDown: 3600, coolDownOffset: 2000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 105, coolDown: 3600, coolDownOffset: 2100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 110, coolDown: 3600, coolDownOffset: 2200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 115, coolDown: 3600, coolDownOffset: 2300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 120, coolDown: 3600, coolDownOffset: 2400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 125, coolDown: 3600, coolDownOffset: 2500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 130, coolDown: 3600, coolDownOffset: 2600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 135, coolDown: 3600, coolDownOffset: 2700),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 140, coolDown: 3600, coolDownOffset: 2800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 145, coolDown: 3600, coolDownOffset: 2900),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 150, coolDown: 3600, coolDownOffset: 3000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 155, coolDown: 3600, coolDownOffset: 3100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 160, coolDown: 3600, coolDownOffset: 3200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 165, coolDown: 3600, coolDownOffset: 3300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 170, coolDown: 3600, coolDownOffset: 3400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 175, coolDown: 3600, coolDownOffset: 3500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 180, coolDown: 3600, coolDownOffset: 3600),

                    new Shoot(30, 1, projectileIndex: 12, fixedAngle: 0, coolDown: 200, rotateAngle: -7),
                    new Shoot(30, 1, projectileIndex: 12, fixedAngle: 180, coolDown: 200, rotateAngle: -7),                   
                    new HpLessTransition(0.30, "attack3")
                    ),
                new State("attack3",
                    new Shoot(15, 12, projectileIndex: 2, fixedAngle: 0, coolDown: 600, rotateAngle: 3),
                    new Taunt("Souls, come to me! Bestow onto me enough power to relinquish our foes from this domain!"),
                    new TossObject2("Mortomus Ball", 4, null, coolDown: 8000, coolDownOffset: 0, randomToss: true),
                    new TossObject2("Mortomus Ball 1", 6, null, coolDown: 8000, coolDownOffset: 2000, randomToss: true),
                    new TossObject2("Mortomus Ball 2", 8, null, coolDown: 8000, coolDownOffset: 4000, randomToss: true),
                    new TossObject2("Mortomus Ball 3", 10, null, coolDown: 8000, coolDownOffset: 6000, randomToss: true),
                    new TossObject2("Mortomus Ball", 4, null, coolDown: 8000, coolDownOffset: 6000, randomToss: true),
                    new TossObject2("Mortomus Ball 1", 6, null, coolDown: 8000, coolDownOffset: 4000, randomToss: true),
                    new TossObject2("Mortomus Ball 2", 8, null, coolDown: 8000, coolDownOffset: 2000, randomToss: true),
                    new TossObject2("Mortomus Ball 3", 10, null, coolDown: 8000, coolDownOffset: 0, randomToss: true),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 0, coolDown: 3600, coolDownOffset: 0),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 5, coolDown: 3600, coolDownOffset: 100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 10, coolDown: 3600, coolDownOffset: 200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 15, coolDown: 3600, coolDownOffset: 300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 20, coolDown: 3600, coolDownOffset: 400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 25, coolDown: 3600, coolDownOffset: 500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 30, coolDown: 3600, coolDownOffset: 600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 35, coolDown: 3600, coolDownOffset: 700),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 40, coolDown: 3600, coolDownOffset: 800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 45, coolDown: 3600, coolDownOffset: 900),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 50, coolDown: 3600, coolDownOffset: 1000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 55, coolDown: 3600, coolDownOffset: 1100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 60, coolDown: 3600, coolDownOffset: 1200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 65, coolDown: 3600, coolDownOffset: 1300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 70, coolDown: 3600, coolDownOffset: 1400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 75, coolDown: 3600, coolDownOffset: 1500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 80, coolDown: 3600, coolDownOffset: 1600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 85, coolDown: 3600, coolDownOffset: 1700),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 90, coolDown: 3600, coolDownOffset: 1800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 95, coolDown: 3600, coolDownOffset: 1900),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 100, coolDown: 3600, coolDownOffset: 2000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 105, coolDown: 3600, coolDownOffset: 2100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 110, coolDown: 3600, coolDownOffset: 2200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 115, coolDown: 3600, coolDownOffset: 2300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 120, coolDown: 3600, coolDownOffset: 2400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 125, coolDown: 3600, coolDownOffset: 2500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 130, coolDown: 3600, coolDownOffset: 2600),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 135, coolDown: 3600, coolDownOffset: 2700),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 140, coolDown: 3600, coolDownOffset: 2800),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 145, coolDown: 3600, coolDownOffset: 2900),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 150, coolDown: 3600, coolDownOffset: 3000),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 155, coolDown: 3600, coolDownOffset: 3100),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 160, coolDown: 3600, coolDownOffset: 3200),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 165, coolDown: 3600, coolDownOffset: 3300),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 170, coolDown: 3600, coolDownOffset: 3400),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 175, coolDown: 3600, coolDownOffset: 3500),
                    new Shoot(30, 4, projectileIndex: 0, fixedAngle: 180, coolDown: 3600, coolDownOffset: 3600),

                    new Shoot(30, 1, projectileIndex: 12, fixedAngle: 0, coolDown: 200, rotateAngle: -7),
                    new Shoot(30, 1, projectileIndex: 12, fixedAngle: 180, coolDown: 200, rotateAngle: -7),
                    new HpLessTransition(0.03, "dead")
                    ),               
                new State("dead",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, false),
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
            new Threshold(0.001,
                new TierLoot(12, ItemType.Weapon, 0.05),
                new TierLoot(13, ItemType.Armor, 0.05),
                new TierLoot(5, ItemType.Ring, 0.05),
                new TierLoot(6, ItemType.Ability, 0.07),
                new ItemLoot("Potion of Wisdom", 1),
                new ItemLoot("Potion of Speed", 1),
                new ItemLoot("Potion of Vitality", 1),
                new ItemLoot("Potion of Dexterity", 1),
                new ItemLoot("Potion of Life", 1),
                new ItemLoot("Potion of Mana", 1),
                new ItemLoot("Potion of Defense", 1),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Old Cleric's Cloak", 0.00033),
                new ItemLoot("Mortomus' Shovel", 0.0009, threshold: 0.03),
                new ItemLoot("Groundkeeper's Lantern", 0.005),
                new ItemLoot("Scepter of Whispers", 0.005),
                new ItemLoot("Magic Dust", 0.5)
                )
            )
        .Init("Undead Blood Bat",
            new State(
                new ScaleHP2(4),
                new State("idle",
                    new PlayerWithinTransition(10, "attack1", true)
                    ),
                new State("attack1",
                    new Wander(0.3),
                    new Shoot(15, 1, projectileIndex: 0, predictive: 1, coolDown: 1000, coolDownOffset: 0),
                    new Shoot(15, 2, shootAngle: 10, projectileIndex: 1, predictive: 1, coolDown: 1000, coolDownOffset: 0),
                    new Shoot(15, 3, shootAngle: 15, projectileIndex: 2, predictive: 1, coolDown: 1000, coolDownOffset: 0),
                    new TimedTransition(3000, "attack")
                    ),
                new State("attack",
                    new Charge(7, range: 8, coolDown: 600),
                    new Wander(0.6),
                    new Shoot(15, 1, projectileIndex: 0, predictive: 1, coolDown: 1000, coolDownOffset: 0),
                    new Shoot(15, 2, shootAngle: 10, projectileIndex: 1, predictive: 1, coolDown: 1000, coolDownOffset: 0),
                    new Shoot(15, 3, shootAngle: 15, projectileIndex: 2, predictive: 1, coolDown: 1000, coolDownOffset: 0)
                    )
                )
            )
        .Init("Mortomus Ball",
            new State(
                new ScaleHP2(4),
                new SetNoXP(),
                new State("attack",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true),
                    new Shoot(12, 12, projectileIndex: 0, predictive: 1, coolDown: 100),
                    new TimedTransition(3000, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
        .Init("Mortomus Ball 1",
            new State(
                new ScaleHP2(4),
                new SetNoXP(),
                new State("attack",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true),
                    new Shoot(12, 12, projectileIndex: 0, predictive: 1, coolDown: 100),
                    new TimedTransition(3000, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
        .Init("Mortomus Ball 2",
            new State(
                new ScaleHP2(4),
                new SetNoXP(),
                new State("attack",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true),
                    new Shoot(12, 12, projectileIndex: 0, predictive: 1, coolDown: 100),
                    new TimedTransition(3000, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
        .Init("Mortomus Ball 3",
            new State(
                new ScaleHP2(4),
                new SetNoXP(),
                new State("attack",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true),
                    new Shoot(12, 12, projectileIndex: 0, predictive: 1, coolDown: 100),
                    new TimedTransition(3000, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
        .Init("Haunted Knight",
            new State(
                new ScaleHP2(5),

                new State("taunt",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible),
                    new TimedTransition(5000, "attack")
                    ),
                new State("attack",
                    new Wander(0.4),
                    new Charge(8, 10, coolDown: 1000),
                    new TimedTransition(2000, "Fire")
                    ),
                new State("Fire",
                    new Wander(0.3),
                    new Shoot(8, 2, shootAngle: 10, projectileIndex: 0, predictive: .9, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(8, 2, shootAngle: 10, projectileIndex: 0, predictive: .9, coolDown: 2000, coolDownOffset: 300),
                    new Shoot(8, 2, shootAngle: 10, projectileIndex: 0, predictive: .9, coolDown: 2000, coolDownOffset: 600),
                    new TimedTransition(2000, "attack")
                    )
                )
            )
        .Init("Haunted Warrior",
            new State(
                new State("taunt",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible),
                    new TimedTransition(5000, "attack")
                    ),
                new ScaleHP2(20),
                new State("attack",
                    new Follow(1, 12),
                    new Wander(0.4),
                    new StayBack(1, 4),
                    new HealEntity(30, "Haunted Guard", 1000, coolDown: 2000),
                    new HealEntity(30, "Haunted Knight", 1000, coolDown: 2000),
                    new Shoot(8, 3, shootAngle: 10, projectileIndex: 0, predictive: .9, coolDown: 800)
                    )
                )
            )
        .Init("Haunted Guard",
            new State(
                new State("taunt",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible),
                    new TimedTransition(5000, "attack")
                    ),
                new ScaleHP2(20),
                new State("attack",
                    new Wander(0.3),
                    new Prioritize(
                        new Protect(1.5, "Mortomus, Keeper of Souls", 10, 5)),
                    new Grenade(5, 140, 12, null, 1000, ConditionEffectIndex.Weak, 1000, 0x017E52)
                    )
                )
            )
        .Init("Crawling Devourer",
            new State(
                new ScaleHP2(20),
                new State("attack",
                    new Shoot(15, 13, projectileIndex: 0, predictive: 1.2, coolDown: 700)
                    )
                )
            )
        .Init("Cursed Mermaid",
            new State(
                new ScaleHP2(5),
                 new State("attack",
                     new Wander(.3),
                     new Grenade(radius: 2, damage: 110, range: 12, coolDown: 800, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1000, color: 0xffffff),
                     new HpLessTransition(0.5, "charge")
                     ),
                 new State("charge",
                     new Follow(3, 10),
                     new Shoot(8, 3, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 700)
                    )
                ));
    }
}