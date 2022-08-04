using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ JuliusCaesar = () => Behav()
        .Init("Julius Caesar",
            new State(
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true), //invuln
                    new PlayerWithinTransition(15, "Start")
                    ),
                new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true), //invuln
                    new Taunt(".... it has been centuries...."),
                    new Wander(0.1),
                    new Taunt("Why would you wake me from my slumber... has Rome risen from the ashes?"),
                    new TimedTransition(5500, "FightTime")
                    ),
                new State("FightTime",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xFF0000, 0.2, 43),
                    new Taunt("Rome remains in ruins? Then I will take matters into my own hands. DIE!"),
                    new Wander(0.52),
                    new Shoot(15, count: 8, shootAngle: 45, projectileIndex: 0, coolDown: 1650, coolDownOffset: 0),
                    new Shoot(15, count: 2, shootAngle: 16, projectileIndex: 1, predictive: 0.6, coolDown: 1100, coolDownOffset: 0),
                    new Shoot(12, count: 4, shootAngle: 25, projectileIndex: 2, predictive: 0.2, coolDown: 1300, coolDownOffset: 0),
                    new HpLessTransition(0.75, "SwordAtt")
                    ),
                new State("SwordAtt",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Taunt("My sword has slain many people and you, {PLAYER} , are no exception!"),
                    new Follow(4.4, range: 22, duration: 5000, coolDown: 0),
                    new Shoot(15, count: 2, shootAngle: 16, projectileIndex: 3, predictive: 0.75, coolDown: 300, coolDownOffset: 0),
                    new Shoot(15, count: 8, shootAngle: 45, projectileIndex: 0, coolDown: 1200, coolDownOffset: 400),
                    new HpLessTransition(0.5, "Power")
                    ),
                new State("Power", //have to add false
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("This power has been bestowed upon me by the gods, fools! I cannot die."),
                    new Shoot(15, count: 4, shootAngle: 25, projectileIndex: 4, predictive: 0.6, coolDown: 800, coolDownOffset: 0),
                    new Shoot(10, count: 8, shootAngle: 45, projectileIndex: 1, coolDown: 2000, coolDownOffset: 500),
                    new TimedTransition(12000, "Killer")
                    ),
                new State("Killer",
                    new Shoot(15, count: 2, shootAngle: 30, projectileIndex: 5, predictive: 0.7, coolDown: 750, coolDownOffset: 300),
                    new Shoot(12, count: 4, shootAngle: 45, projectileIndex: 1, predictive: 0.3, coolDown: 1280, coolDownOffset: 300),
                    new Shoot(7, count: 1, shootAngle: 10, projectileIndex: 4, predictive: 1.0, coolDown: 800, coolDownOffset: 300),
                    new Shoot(15, count: 8, shootAngle: 45, projectileIndex: 1, predictive: 0.2, coolDown: 999999, coolDownOffset: 0),
                    new HpLessTransition(0.30, "Find")
                    ),
                new State("Find",
                    new ConditionalEffect(ConditionEffectIndex.Armored, true),
                    new Taunt("Where are you, insects... I shall not be beaten down by puny mortals."),
                    new Follow(speed: 5.0, range: 15, duration: 3000, coolDown: 150),
                    new Shoot(7, count: 2, shootAngle: 20, projectileIndex: 4, predictive: 0.6, coolDown: 800, coolDownOffset: 0),
                    new Shoot(12, count: 3, shootAngle: 30, projectileIndex: 6, predictive: 0.4, coolDown: 1200, coolDownOffset: 0),
                    new Shoot(15, count: 8, shootAngle: 45, projectileIndex: 1, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(15, count: 8, shootAngle: 45, projectileIndex: 3, coolDown: 2000, coolDownOffset: 500),
                    new HpLessTransition(0.15, "Enough")
                    ),
                new State("Enough",
                    new Taunt("Enough is enough.... you puny mortals mock me.  I shall make you pay."),
                    new Shoot(15, count: 10, shootAngle: 65, projectileIndex: 3, predictive: 0.2, coolDown: 2000, coolDownOffset: 250),
                    new Shoot(12, count: 4, shootAngle: 25, projectileIndex: 1, predictive: 0.5, coolDown: 1050, coolDownOffset: 400),
                    new Shoot(15, count: 1, shootAngle: 0, projectileIndex: 7, predictive: 1.2, coolDown: 200, coolDownOffset: 500),
                    new Follow(3.3, range: 35, duration: 4000, coolDown: 0),
                    new Shoot(7, count: 3, shootAngle: 17, projectileIndex: 6, predictive: 0.4, coolDown: 750, coolDownOffset: 500),
                    new HpLessTransition(0.075, "Heal")
                    ),
                new State("Heal",
                    new ReturnToSpawn(speed: 1.7),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("Did you really think you had beaten me? HAHAHA! Do not make me laugh!"),
                    new HealSelf(coolDown: 1000000, amount: 800000, percentage: false),
                    new ChangeSize(2, 200),
                    new TimedTransition(10000, "Kill all")
                    ),
                new State("Kill all",
                    new Taunt("Puny mortals... I shall show you the true power of Rome!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable,false,0),
                    new Shoot(15, count: 8, shootAngle: 45, projectileIndex: 3, coolDown: 1500, coolDownOffset: 500),
                    new Shoot(12, count: 2, shootAngle: 16, projectileIndex: 4, predictive: 1.3, coolDown: 750, coolDownOffset: 500),
                    new Shoot(7, count: 4, shootAngle: 25, projectileIndex: 1, predictive: 0.63, coolDown: 1000, coolDownOffset: 500),
                    new HpLessTransition(0.7, "kDie")
                    ),
                new State("kDie",
                    new Taunt("Okay, peasants.. You do not have the right to even gaze upon me, let alone strike me.  I'm ending this, now!"),
                    new Shoot(20, count: 8, shootAngle: 45, projectileIndex: 8, coolDown: 999999, coolDownOffset: 300),
                    new Shoot(15, count: 1, shootAngle: 0, projectileIndex: 7, predictive: 1.7, coolDown: 250, coolDownOffset: 500),
                    new Shoot(7, count: 5, shootAngle: 32, projectileIndex: 4, predictive: 0.54, coolDown: 1100, coolDownOffset: 600),
                    new HpLessTransition(0.5, "Spawn")
                    ),
                new State("Spawn",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Taunt("Minions! Come help me with these pests."),
                    new TossObject("Roman Soldier", 3, 0, 1000000, coolDownOffset: 400),
                    new TossObject("Roman Archer 1", 3, 45, 1000000, coolDownOffset: 800),
                    new TossObject("Roman Soldier", 3, 90, 1000000, coolDownOffset: 1200),
                    new TossObject("Roman Archer 2", 3, 135, 1000000, coolDownOffset: 1500),
                    new TossObject("Roman Soldier", 3, 180, 1000000, coolDownOffset: 1800),
                    new TossObject("Roman Archer 3", 3, 225, 1000000, coolDownOffset: 2100),
                    new TossObject("Roman Soldier", 3, 270, 1000000, coolDownOffset: 2400),
                    new TossObject("Roman Archer 4", 3, 315, 1000000, coolDownOffset: 2700),
                    new TossObject("Roman Gladiator", 1, 90, 1000000, coolDownOffset: 3000),
                    //new Spawn("Roman Archer", maxChildren: 1, coolDown: 900000, initialSpawn: 0.7),
                    new ReturnToSpawn(1),
                    new TimedTransition(5000, "Areuded")
                    ),
                new State("Areuded",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new EntitiesNotExistsTransition(100, "Rage", "Roman Soldier", "Roman Archer 1", "Roman Archer 2", "Roman Archer 3", "Roman Archer 4", "Roman Gladiator")
                    ),
                new State("Rage", // Already called on armored, will remove
                    new Taunt("My army...! YOU WILL PAY, YOU UNGRATEFUL PESTS!"),
                    new Flash(0xFF0000, 0.2, 3),
                    new Follow(5.4, range: 35, duration: 4000, coolDown: 100),
                    new Shoot(25, count: 8, shootAngle: 45, projectileIndex: 8, coolDown: 3000, coolDownOffset: 375),
                    new Shoot(20, count: 4, shootAngle: 25, projectileIndex: 0, predictive: 0.7, coolDown: 1600, coolDownOffset: 375),
                    new Shoot(15, count: 2, shootAngle: 16, projectileIndex: 6, predictive: 1.3, coolDown: 1200, coolDownOffset: 375),
                    new Shoot(7, count: 1, shootAngle: 0, projectileIndex: 5, predictive: 1.7, coolDown: 800, coolDownOffset: 375),
                    new Grenade(2.5, 100, 10),
                    new TossObject("Roman Pillar", 9.5, 45, 1000000),
                    new TossObject("Roman Pillar", 9.5, 135, 1000000),
                    new TossObject("Roman Pillar", 9.5, 225, 1000000),
                    new TossObject("Roman Pillar", 9.5, 315, 1000000),
                    new HpLessTransition(0.10, "Itstime")
                    ),
                new State("Itstime",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Taunt("You may have won this time... but you'll never defeat the real gods."),
                    new RemoveEntity(50, "Roman Pillar"),
                    new Shoot(100, count: 8, shootAngle: 45, projectileIndex: 8, coolDown: 1000000, coolDownOffset: 300),
                    new TimedTransition(350, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()

                    )
                ),
            new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
             new Threshold(0.05,
                    new ItemLoot("Romanian Tunic", 0.0014),
                    new ItemLoot("Spear of Thiram", 0.0014)
                ),
              new Threshold(0.03,
                    new ItemLoot("Gladiator's Visage", 0.0014)
                ),
                    new Threshold(0.001,
                        new ItemLoot("Crafting Material 1", 0.05),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Potion of Mana", 1),
                    new ItemLoot("Potion of Life", 0.5),
                    new ItemLoot("Potion of Mana",0.5)
                ),
                    new Threshold(0.001,
                //new ItemLoot("Ancient Gladiator Sword", 0.0005),
                //new ItemLoot("Helm of the Conqueror", 0.0005),

                new TierLoot(14, ItemType.Armor, 0.01),
                new TierLoot(13, ItemType.Armor, 0.03),
                new TierLoot(14, ItemType.Weapon, 0.01),
                new TierLoot(13, ItemType.Weapon, 0.03),
                new TierLoot(6, ItemType.Ability, 0.01),
                new TierLoot(5, ItemType.Ability, 0.03),
                new TierLoot(5, ItemType.Ring, 0.03),
                new TierLoot(6, ItemType.Ring, 0.01)
                    )
            )
          .Init("Roman Pillar", //this "pillar" should act like the LH pillars in a sense
             new State(
               new State("Start",
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                         new Taunt("Protect Julius Caesar!"),
                         new Flash(0xFF0000, 0.2, 3),
                         new TimedTransition(1000, "Kill")
                         ),
               new State("Kill",
                         new ConditionalEffect(ConditionEffectIndex.Armored),
                         new Shoot(12, count: 8, projectileIndex: 0, shootAngle: 45, angleOffset: 10, predictive: 1, coolDown: 2000)
                        )
             )
            )
        .Init("Roman Soldier",
            new State(
                new State("Defense",
                    new ReturnToSpawn(1),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Shoot(5, 1, projectileIndex: 2, coolDown: 500),
                    new Shoot(12, 1, fixedAngle: 90, coolDown: 4000),
                    new Shoot(12, 1, fixedAngle: 270, coolDown: 4000),
                    new Shoot(12, 1, fixedAngle: 180, coolDown: 4000, coolDownOffset: 2000),
                    new Shoot(12, 1, fixedAngle: 360, coolDown: 4000, coolDownOffset: 2000)
                    ),
                new State("Offence",
                    new Wander(0.4),
                    new Follow(1.7, range: 6, duration: 2750, coolDown: 0),
                    new Shoot(12, count: 1, projectileIndex: 0, shootAngle: 0, predictive: 1.4, coolDown: 2500),
                    new Shoot(7, count: 4, projectileIndex: 1, shootAngle: 35, predictive: 0.8, coolDown: 1400)
                 )
            )
                 )
        .Init("Roman Gladiator",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new EntitiesNotExistsTransition(50, "Anger", "Roman Archer 1", "Roman Archer 2", "Roman Archer 3", "Roman Archer 4", "Roman Soldier"),
                new State("Start",
                    new Order(50, "Roman Soldier", "Defense"),
                    new Order(50, "Roman Archer 1", "Defense"),
                    new Order(50, "Roman Archer 2", "Defense"),
                    new Order(50, "Roman Archer 3", "Defense"),
                    new Order(50, "Roman Archer 4", "Defense"),
                    new Follow(2.3, range: 11, duration: 5000, coolDown: 0),
                    new Taunt("Soldiers, HOLD!"),
                    new Shoot(15, count: 2, shootAngle: 16, projectileIndex: 0, predictive: 1.2, coolDown: 300),
                    new Shoot(15, count: 6, shootAngle: 33, projectileIndex: 1, predictive: 0.5, coolDown: 900),
                    new HpLessTransition(0.3, "Insta"),
                    new TimedTransition(10000, "Bomb")
                    ),
                new State("Bomb",
                    new Taunt("Soldiers, ATTACK!"),
                    new Order(50, "Roman Soldier", "Offence"),
                    new Order(50, "Roman Archer 1", "Offence"),
                    new Order(50, "Roman Archer 2", "Offence"),
                    new Order(50, "Roman Archer 3", "Offence"),
                    new Order(50, "Roman Archer 4", "Offence"),
                    new Wander(0.5),
                    new Flash(0xFF0000, flashRepeats: 4, flashPeriod: 0.3),
                    new Follow(2.3, range: 11, duration: 5000, coolDown: 0),
                    new Grenade(2.5, 100, 10),
                    new Shoot(15, count: 8, shootAngle: 45, projectileIndex: 1, predictive: 0.2, coolDown: 1200),
                    new HpLessTransition(0.15, "Insta"),
                    new TimedTransition(5000, "End")
                    ),
                new State("End",
                    new Wander(0.5),
                    new Flash(0xFF0000, flashRepeats: 4, flashPeriod: 0.3),
                    new Follow(2.3, range: 11, duration: 5000, coolDown: 0),
                    new Shoot(20, count: 1, shootAngle: 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1.5, coolDown: 300),
                    new Shoot(15, count: 6, shootAngle: 33, projectileIndex: 1, predictive: 0.7, coolDown: 900),
                    new TimedTransition(5000, "Start"),
                    new HpLessTransition(0.07, "Insta")
                    ),
                new State("Anger",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Wander(0.5),
                    new Taunt("MY ARMY! YOU SHALL PAY"),
                    new ChangeSize(5, 200),
                    new Flash(0xFF0000, flashRepeats: 4, flashPeriod: 0.3),
                    new Follow(0.8, range: 5, duration: 5000, coolDown: 0),
                    new Charge(2, 10, coolDown: 2000),
                    new Shoot(10, 5, shootAngle: 36, projectileIndex: 0, coolDown: 1750, coolDownOffset: 250),
                    new Shoot(10, 3, shootAngle: 15, projectileIndex: 1, coolDown: 1500, coolDownOffset: 350),
                    new Shoot(20, projectileIndex: 3, count: 8, coolDown: 2500),
                    new HpLessTransition(0.07, "Insta")
                    ),
                new State("Insta",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xFF0000, flashPeriod: 0.5, flashRepeats: 6),
                    new Shoot(20, projectileIndex: 2, count: 8),
                    new Suicide()
                )
                    )
            )
            .Init("Roman Archer 1",
            new State(
                  new State("Defense",
                    new ReturnToSpawn(1),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Shoot(radius: 12, projectileIndex: 0, shootAngle: 10, count: 3, fixedAngle: 45, coolDown: 1000)
                    ),
                new State("Offence",
                    new Wander(0.5),
                    new Shoot(radius: 8, projectileIndex: 0, count: 3, shootAngle:25, predictive: 10, coolDown: 1000)
                    )
                 )
            )
        .Init("Roman Archer 2",
            new State(
                  new State("Defense",
                    new ReturnToSpawn(1),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Shoot(radius: 12, projectileIndex: 0, shootAngle: 10, count: 3, fixedAngle: 135, coolDown: 1000)
                    ),
                new State("Offence",
                    new Wander(0.5),
                    new Shoot(radius: 8, projectileIndex: 0, count: 3, shootAngle: 25, predictive: 10, coolDown: 1000)
                    )
                 )
            )
        .Init("Roman Archer 3",
            new State(
                  new State("Defense",
                    new ReturnToSpawn(1),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Shoot(radius: 12, projectileIndex: 0, shootAngle: 10, count: 3, fixedAngle: 225, coolDown: 1000)
                    ),
                new State("Offence",
                    new Wander(0.5),
                    new Shoot(radius: 8, projectileIndex: 0, count: 3, shootAngle: 25, predictive: 10, coolDown: 1000)
                    )
                 )
            )
        .Init("Roman Archer 4",
            new State(
                  new State("Defense",
                    new ReturnToSpawn(1),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Shoot(radius: 12, projectileIndex: 0, shootAngle:10, count: 3, fixedAngle: 315, coolDown: 1000)
                    ),
                new State("Offence",
                    new Wander(0.5),
                    new Shoot(radius: 8, projectileIndex: 0, count: 3, shootAngle: 25, predictive: 10, coolDown: 1000)
                    )
                 )
            )
;

    }
}






















