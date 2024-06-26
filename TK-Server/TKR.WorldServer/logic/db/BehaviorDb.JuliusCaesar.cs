using TKR.Shared.resources;
using TKR.WorldServer.logic.behaviors;
using TKR.WorldServer.logic.transitions;
using TKR.WorldServer.logic.loot;

namespace TKR.WorldServer.logic
{
    partial class BehaviorDb
    {
        private _ JuliusCaesar = () => Behav()
        .Init("Julius Caesar",
            new State(
                new ScaleHP2(35),
                new State("idle",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true), //invuln
                    new PlayerWithinTransition(15, "Prepare")
                    ),
                 new State("Prepare",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true),
                    new Taunt("Protect me"),
                    new Spawn("Roman Archer 1", 1, 1, 99999),
                    new Spawn("Roman Archer 1.1", 1, 1, 99999),
                    new TimedTransition(1000, "Prepare 1")
                    ),
                  new State("Prepare 1",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true),
                    new Spawn("Roman Archer 2", 1, 1, 99999),
                    new Spawn("Roman Archer 2.1", 1, 1, 99999),
                    new TimedTransition(1000, "Prepare 2")
                    ),
                   new State("Prepare 2",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true),
                    new Spawn("Roman Soldier 1", 1, 1, 99999),
                    new Spawn("Roman Soldier 1.1", 1, 1, 99999),
                    new TimedTransition(1000, "Prepare 3")
                    ),
                new State("Prepare 3",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true),
                    new Taunt("With your life!"),
                    new Spawn("Roman Gladiator 1", 1, 1, 99999),
                    new Spawn("Roman Gladiator 1.1", 1, 1, 99999),
                    new TimedTransition(1500, "Start")
                    ),
                new State("Start",
                    new Shoot(20, 15, projectileIndex: 7, coolDown: 500),
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new Order(20, "Roman Archer 1", "Orbit"),
                    new Order(20, "Roman Archer 1.1", "Orbit"),
                    new Order(20, "Roman Archer 2", "Orbit"),
                    new Order(20, "Roman Archer 2.1", "Orbit"),
                    new Order(20, "Roman Soldier 1", "Orbit"),
                    new Order(20, "Roman Soldier 1.1", "Orbit"),
                    new Order(20, "Roman Gladiator 1", "Orbit"),
                    new Order(20, "Roman Gladiator 1.1", "Orbit"),
                    new HpLessTransition(0.75, "Faster")
                    ),
                new State("Faster",
                    new Shoot(20, 15, projectileIndex: 7, coolDown: 500),
                    new Order(20, "Roman Archer 1", "Orbit1"),
                    new Order(20, "Roman Archer 1.1", "Orbit1"),
                    new Order(20, "Roman Archer 2", "Orbit1"),
                    new Order(20, "Roman Archer 2.1", "Orbit1"),
                    new Order(20, "Roman Soldier 1", "Orbit1"),
                    new Order(20, "Roman Soldier 1.1", "Orbit1"),
                    new Order(20, "Roman Gladiator 1", "Orbit1"),
                    new Order(20, "Roman Gladiator 1.1", "Orbit1"),
                    new HpLessTransition(0.50, "Turrets")
                    ),
                new State("Turrets",
                    new Shoot(20, 15, projectileIndex: 7, coolDown: 500),
                    new InvisiToss("Roman Pillar", 10, 0, coolDown: 999999),
                    new InvisiToss("Roman Pillar", 10, 90, coolDown: 999999),
                    new InvisiToss("Roman Pillar", 10, 180, coolDown: 999999),
                    new InvisiToss("Roman Pillar", 10, 270, coolDown: 999999),
                    new HpLessTransition(.25, "Final")
                    ),
                new State("Final",
                    new Shoot(20, 15, projectileIndex: 7, coolDown: 500),
                    new Order(20, "Roman Gladiator 1", "Orbit2"),
                    new Order(20, "Roman Gladiator 1.1", "Orbit2"),
                    new Order(20, "Roman Archer 1", "Orbit2"),
                    new Order(20, "Roman Archer 1.1", "Orbit2"),
                    new Order(20, "Roman Archer 2", "Orbit2"),
                    new Order(20, "Roman Archer 2.1", "Orbit2"),
                    new Order(20, "Roman Soldier 1", "Orbit2"),
                    new Order(20, "Roman Soldier 1.1", "Orbit2"),
                    new HpLessTransition(0.05, "remove")
                    ),
                new State("remove",
                    new Taunt("NOOOO"),
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true),
                    new RemoveEntity(20, "Roman Gladiator 1"),
                    new RemoveEntity(20, "Roman Gladiator 1.1"),
                    new RemoveEntity(20, "Roman Archer 1"),
                    new RemoveEntity(20, "Roman Archer 1.1"),
                    new RemoveEntity(20, "Roman Roman Archer 2"),
                    new RemoveEntity(20, "Roman Roman Archer 2.1"),
                    new RemoveEntity(20, "Roman Soldier 1"),
                    new RemoveEntity(20, "Roman Soldier 1.1"),
                    new TimedTransition(1000, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                ),
            new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
             new Threshold(0.01,
                 new ItemLoot("Talisman Fragment", 0.009),
                    new ItemLoot("Spear of Thiram", 0.001)
                ),
              new Threshold(0.01,
                    new ItemLoot("Gladiator's Visage", 0.001)
                ),
                    new Threshold(0.001,

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
                         new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                         new Flash(0xFF0000, 0.2, 3),
                         new TimedTransition(1000, "Kill")
                         ),
               new State("Kill",
                         new ConditionEffectBehavior(ConditionEffectIndex.Armored),
                         new Shoot(15, count: 3, projectileIndex: 0, shootAngle: 10, predictive: 1, coolDown: 1500),
                         new EntityNotExistsTransition("Julius Caesar", 30, "die")
                    ),
                new State("die",
                    new Suicide()
                        )
             )
            )
        .Init("Roman Soldier 1",
            new State(
                new State("Defense",
                    new MoveTo3(6, 0, 1, isMapPosition: false, instant: false),
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible)
                    ),
                new State("Orbit",
                    new Shoot(10, 7, projectileIndex: 0, coolDown: 1500),
                    new Orbit(3, 6, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                     ),
                new State("Orbit1",
                    new Shoot(10, 7, projectileIndex: 0, coolDown: 1500),
                    new Orbit(6, 6, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                     ),
                new State("Orbit2",
                    new Shoot(10, 7, projectileIndex: 0, coolDown: 1500),
                    new Orbit(6, 6, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0, orbitClockwise: true),
                    new EntityNotExistsTransition("Julius Caesar", 30, "die")
                    ),
                new State("die",
                    new Suicide()
                 )
            )
                 )
         .Init("Roman Soldier 1.1",
            new State(
                new State("Defense",
                    new MoveTo3(-6, 0, 1, isMapPosition: false, instant: false),
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible)
                    ),
                new State("Orbit",
                    new Shoot(10, 7, projectileIndex: 0, coolDown: 1500),
                    new Orbit(3, 6, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                     ),
                new State("Orbit1",
                    new Shoot(10, 7, projectileIndex: 0, coolDown: 1500),
                    new Orbit(6, 6, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                     ),
                new State("Orbit2",
                    new Shoot(10, 7, projectileIndex: 0, coolDown: 1500),
                    new Orbit(6, 6, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0, orbitClockwise: true),
                    new EntityNotExistsTransition("Julius Caesar", 30, "die")
                    ),
                new State("die",
                    new Suicide()
                 )
            )
                 )
        .Init("Roman Gladiator 1",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invincible),
                new State("Start",
                    new MoveTo3(8, 0, 1, isMapPosition: false, instant: false),
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible)
                    ),
                new State("Orbit",
                    new Shoot(30, 1, projectileIndex: 0, coolDown: 100),
                    new Orbit(4, 8, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                     ),
                new State("Orbit1",
                    new Shoot(30, 1, projectileIndex: 0, coolDown: 50),
                    new Orbit(8, 8, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                     ),
                new State("Orbit2",
                    new Shoot(30, 1, projectileIndex: 0, coolDown: 25),
                    new Orbit(8, 8, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0, orbitClockwise: true),
                    new EntityNotExistsTransition("Julius Caesar", 30, "die")
                    ),
                new State("die",
                    new Suicide()
                )
                    )
            )
        .Init("Roman Gladiator 1.1",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invincible),
                new EntitiesNotExistsTransition(50, "Start", "Roman Archer 1", "Roman Archer 2", "Roman Archer 3", "Roman Archer 4", "Roman Soldier 1.1"),
                new State("Start",
                    new MoveTo3(-8, 0, 1, isMapPosition: false, instant: false),
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible)
                    ),
                new State("Orbit",
                    new Shoot(30, 1, projectileIndex: 0, coolDown: 100),
                    new Orbit(4, 8, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                    ),
                new State("Orbit1",
                    new Shoot(30, 1, projectileIndex: 0, coolDown: 50),
                    new Orbit(8, 8, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                     ),
                new State("Orbit2",
                    new Shoot(30, 1, projectileIndex: 0, coolDown: 25),
                    new Orbit(8, 8, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0, orbitClockwise: true),
                    new EntityNotExistsTransition("Julius Caesar", 30, "die")
                    ),
                new State("die",
                    new Suicide()
                )
                    )
            )
            .Init("Roman Archer 1",
            new State(
                   new State("Start",
                    new MoveTo3(2, 0, 1, isMapPosition: false, instant: false),
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible)
                    ),
                new State("Orbit",
                    new Shoot(10, 2, projectileIndex: 0, shootAngle: 15, coolDown: 800),
                    new Orbit(1, 2, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                    ),
                new State("Orbit1",
                    new Shoot(10, 2, projectileIndex: 0, shootAngle: 15, coolDown: 800),
                    new Orbit(2, 2, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                     ),
                new State("Orbit2",
                    new Shoot(10, 2, projectileIndex: 0, shootAngle: 15, coolDown: 800),
                    new Orbit(2, 2, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0, orbitClockwise: true),
                    new EntityNotExistsTransition("Julius Caesar", 30, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                 )
            )
        .Init("Roman Archer 1.1",
            new State(
                    new State("Start",
                    new MoveTo3(-2, 0, 1, isMapPosition: false, instant: false),
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible)
                    ),
                new State("Orbit",
                    new Shoot(10, 2, projectileIndex: 0, shootAngle: 10, coolDown: 800),
                    new Orbit(1, 2, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                    ),
                new State("Orbit1",
                    new Shoot(10, 2, projectileIndex: 0, shootAngle: 10, coolDown: 800),
                    new Orbit(2, 2, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                     ),
                new State("Orbit2",
                    new Shoot(10, 2, projectileIndex: 0, shootAngle: 15, coolDown: 800),
                    new Orbit(2, 2, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0, orbitClockwise: true),
                    new EntityNotExistsTransition("Julius Caesar", 30, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                 )
            )
        .Init("Roman Archer 2",
            new State(
                    new State("Start",

                    new MoveTo3(4, 0, 1, isMapPosition: false, instant: false),
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible)
                    ),
                new State("Orbit",
                    new Shoot(10, 1, projectileIndex: 0, coolDown: 1300, coolDownOffset: 0),
                    new Shoot(10, 2, projectileIndex: 0, shootAngle: 10, coolDown: 1300, coolDownOffset: 700),
                    new Orbit(2, 4, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                    ),
                new State("Orbit1",
                    new Shoot(10, 1, projectileIndex: 0, coolDown: 1300, coolDownOffset: 0),
                    new Shoot(10, 2, projectileIndex: 0, shootAngle: 10, coolDown: 1300, coolDownOffset: 700),
                    new Orbit(4, 4, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                     ),
                new State("Orbit2",
                    new Shoot(10, 1, projectileIndex: 0, coolDown: 1300, coolDownOffset: 0),
                    new Shoot(10, 2, projectileIndex: 0, shootAngle: 10, coolDown: 1300, coolDownOffset: 700),
                    new Orbit(4, 4, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0, orbitClockwise: true),
                    new EntityNotExistsTransition("Julius Caesar", 30, "die")
                    ),
                new State("die",
                    new Suicide()
                 )
            )
            )
        .Init("Roman Archer 2.1",
            new State(
                    new State("Start",
                    new MoveTo3(-4, 0, 1, isMapPosition: false, instant: false),
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible)
                    ),
                new State("Orbit",
                    new Shoot(10, 1, projectileIndex: 0, coolDown: 1300, coolDownOffset: 0),
                    new Shoot(10, 2, projectileIndex: 0, shootAngle: 10, coolDown: 1300, coolDownOffset: 700),
                    new Orbit(2, 4, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                    ),
                new State("Orbit1",
                    new Shoot(10, 1, projectileIndex: 0, coolDown: 1300, coolDownOffset: 0),
                    new Shoot(10, 2, projectileIndex: 0, shootAngle: 10, coolDown: 1300, coolDownOffset: 700),
                    new Orbit(4, 4, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0)
                    ),
                new State("Orbit2",
                    new Shoot(10, 1, projectileIndex: 0, coolDown: 1300, coolDownOffset: 0),
                    new Shoot(10, 2, projectileIndex: 0, shootAngle: 10, coolDown: 1300, coolDownOffset: 700),
                    new Orbit(4, 4, 10, "Julius Caesar", speedVariance: 0, radiusVariance: 0, orbitClockwise: true),
                    new EntityNotExistsTransition("Julius Caesar", 30, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                 )
            )
        .Init("Roman Archer 3",
            new State(
                  new State("Defense",
                    new ReturnToSpawn(1),
                    new ConditionEffectBehavior(ConditionEffectIndex.Armored),
                    new Shoot(radius: 12, projectileIndex: 0, shootAngle: 10, count: 3, fixedAngle: 225, coolDown: 1000)
                    ),
                new State("Offence",
                    new Wander(0.5),
                    new Shoot(radius: 8, projectileIndex: 0, count: 3, shootAngle: 25, predictive: 10, coolDown: 1000),
                    new EntityNotExistsTransition("Julius Caesar", 30, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                 )
            )
        .Init("Roman Archer 4",
            new State(
                  new State("Defense",
                    new ReturnToSpawn(1),
                    new ConditionEffectBehavior(ConditionEffectIndex.Armored),
                    new Shoot(radius: 12, projectileIndex: 0, shootAngle:10, count: 3, fixedAngle: 315, coolDown: 1000)
                    ),
                new State("Offence",
                    new Wander(0.5),
                    new Shoot(radius: 8, projectileIndex: 0, count: 3, shootAngle: 25, predictive: 10, coolDown: 1000),
                    new EntityNotExistsTransition("Julius Caesar", 30, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                 )
            )
;

    }
}






















