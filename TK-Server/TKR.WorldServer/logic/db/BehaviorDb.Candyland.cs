using TKR.Shared.resources;
using TKR.WorldServer.logic.behaviors;
using TKR.WorldServer.logic.loot;
using TKR.WorldServer.logic.transitions;

namespace TKR.WorldServer.logic
{
    partial class BehaviorDb
    {
        private _ Candyland = () => Behav()
        .Init("Candy Gnome",
            new State(
                new ScaleHP2(20),
                 new State("IniPrepare1",
                    new PlayerWithinTransition(15, "IniPrepare"),
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, false)
                    ),
                 new State("IniPrepare",
                     new Shoot(15, 3, projectileIndex: 0, shootAngle: 15, coolDown: 400, predictive: 1.4),
                     new Shoot(15, 2, projectileIndex: 1, shootAngle: 15, coolDown: 600, predictive: 1.4),
                     new Wander(0.3),
                     new TossObject2("Candy Gumball Machine", 5, angle: null, coolDown: 99999, randomToss: true),
                     new TossObject2("Candy Gumball Machine", 8, angle: null, coolDown: 99999, randomToss: true),
                    
                    new InvisiToss("CLand Spike Spawner", 1, angle: 0, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner", 1, angle: 45, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner", 1, angle: 90, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner", 1, angle: 135, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner", 1, angle: 180, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner", 1, angle: 225, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner", 1, angle: 270, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner", 1, angle: 315, coolDown: 3600, coolDownOffset: 0),

                    new InvisiToss("CLand Spike Spawner 1", 2, angle: 0, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 1", 2, angle: 45, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 1", 2, angle: 90, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 1", 2, angle: 135, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 1", 2, angle: 180, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 1", 2, angle: 225, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 1", 2, angle: 270, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 1", 2, angle: 315, coolDown: 3600, coolDownOffset: 0),

                    new InvisiToss("CLand Spike Spawner 2", 3, angle: 0, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 2", 3, angle: 45, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 2", 3, angle: 90, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 2", 3, angle: 135, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 2", 3, angle: 180, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 2", 3, angle: 225, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 2", 3, angle: 270, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 2", 3, angle: 315, coolDown: 3600, coolDownOffset: 0),

                    new InvisiToss("CLand Spike Spawner 3", 4, angle: 0, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 3", 4, angle: 45, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 3", 4, angle: 90, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 3", 4, angle: 135, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 3", 4, angle: 180, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 3", 4, angle: 225, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 3", 4, angle: 270, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 3", 4, angle: 315, coolDown: 3600, coolDownOffset: 0),

                    new InvisiToss("CLand Spike Spawner 4", 5, angle: 0, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 4", 5, angle: 45, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 4", 5, angle: 90, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 4", 5, angle: 135, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 4", 5, angle: 180, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 4", 5, angle: 225, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 4", 5, angle: 270, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 4", 5, angle: 315, coolDown: 3600, coolDownOffset: 0),

                    new InvisiToss("CLand Spike Spawner 5", 6, angle: 0, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 5", 6, angle: 45, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 5", 6, angle: 90, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 5", 6, angle: 135, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 5", 6, angle: 180, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 5", 6, angle: 225, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 5", 6, angle: 270, coolDown: 3600, coolDownOffset: 0),
                    new InvisiToss("CLand Spike Spawner 5", 6, angle: 315, coolDown: 3600, coolDownOffset: 0),
                    new TimedTransition(1000, "Ini")
                     ),
                new State("Ini",
                    new Wander(0.3),
                    new Order(30, "CLand Spike Spawner", "fire"),
                    new TimedTransition(300, "spike 1")
                    ),
                new State("spike 1",
                    new Wander(0.3),
                    new Shoot(15, 3, projectileIndex: 0, shootAngle: 8, predictive: 1.4, coolDown: 300),
                    new Order(30, "CLand Spike Spawner 1", "fire"),
                     new TimedTransition(300, "spike 2")
                    ),
                new State("spike 2",
                    new Wander(0.3),
                    new TossObject2("Candy Gumball Machine", 5, angle: null, coolDown: 99999, randomToss: true),
                    new TossObject2("Candy Gumball Machine", 8, angle: null, coolDown: 99999, randomToss: true),
                    new Order(30, "CLand Spike Spawner 2", "fire"),
                    new TimedTransition(300, "spike 3")
                    ),
                new State("spike 3",
                    new Wander(0.3),
                    new Shoot(15, 3, projectileIndex: 0, shootAngle: 8, predictive: 1.4, coolDown: 300),
                    new Order(30, "CLand Spike Spawner 3", "fire"),
                    new TimedTransition(300, "spike 4")
                    ),
                new State("spike 4",
                    new Wander(0.3),
                    new Order(30, "CLand Spike Spawner 4", "fire"),
                    new TimedTransition(300, "spike 5")
                    ),
                new State("spike 5",
                    new Wander(0.3),
                    new Shoot(15, 3, projectileIndex: 0, shootAngle: 8, predictive: 1.4, coolDown: 500),
                    new Order(30, "CLand Spike Spawner 5", "fire"),
                    new TimedTransition(1000, "IniPrepare")
                    ),
                new DropPortalOnDeath(target: "Candyland Portal", probability: 1, timeout: 30)
                ),
            new Threshold(0.001,
                new ItemLoot("Chocolate Skull", 0.00125)
                ),
            new Threshold(0.01,
                new ItemLoot("Potion of Wisdom", 1),
                new ItemLoot("Potion of Dexterity", 1),
                new ItemLoot("Potion of Vitality", 1),
                new ItemLoot(item: "Rock Candy", probability: 0.15),
                new ItemLoot(item: "Red Gumball", probability: 0.15),
                new ItemLoot(item: "Purple Gumball", probability: 0.15),
                new ItemLoot(item: "Blue Gumball", probability: 0.15),
                new ItemLoot(item: "Green Gumball", probability: 0.15),
                new ItemLoot(item: "Yellow Gumball", probability: 0.15)
                )
            )
        .Init("Candy Gumball Machine",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invincible),
                new State("flash",
                    new Flash(0xCC1A1A, 0.5, 12),
                    new TimedTransition(2000, "fire")
                    ),
                new State("fire",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible),
                    new Shoot(radius: 15, count: 8, projectileIndex: 0, coolDown: 2000),
                    new TimedTransition(100, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
         .Init("CLand Spike",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invincible),
                new State("fire",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible),
                    new Shoot(radius: 15, count: 1, projectileIndex: 0, coolDown: 2000),
                    new TimedTransition(250, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("CLand Spike Spawner",
            new State(
                new State("idle",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true)
                    ),
                new State("fire",
                    new Spawn("CLand Spike", 1, 1, coolDown: 99999, givesNoXp: true),
                    new TimedTransition(1000, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("CLand Spike Spawner 1",
            new State(
                new State("idle",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true)
                    ),
                new State("fire",
                    new Spawn("CLand Spike", 1, 1, coolDown: 99999, givesNoXp: true),
                    new TimedTransition(1000, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("CLand Spike Spawner 2",
            new State(
                new State("idle",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true)
                    ),
                new State("fire",
                    new Spawn("CLand Spike", 1, 1, coolDown: 99999, givesNoXp: true),
                    new TimedTransition(1000, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("CLand Spike Spawner 3",
            new State(
                new State("idle",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true)
                    ),
                new State("fire",
                    new Spawn("CLand Spike", 1, 1, coolDown: 99999, givesNoXp: true),
                    new TimedTransition(1000, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("CLand Spike Spawner 4",
            new State(
                new State("idle",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true)
                    ),
                new State("fire",
                    new Spawn("CLand Spike", 1, 1, coolDown: 99999, givesNoXp: true),
                    new TimedTransition(1000, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("CLand Spike Spawner 5",
            new State(
                new State("idle",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible, true)
                    ),
                new State("fire",
                    new Spawn("CLand Spike", 1, 1, coolDown: 99999, givesNoXp: true),
                    new TimedTransition(1000, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("MegaRototo",
            new State(
                new StayCloseToSpawn(.4, 6),
                new Reproduce(children: "Tiny Rototo", densityRadius: 12, densityMax: 7, coolDown: 7000),
                new State("Follow",
                    new Shoot(12, count: 4, shootAngle: 90, defaultAngle: 45, coolDown: 1400),
                    new Shoot(8, count: 4, shootAngle: 90, projectileIndex: 1, coolDown: 1400),
                    new Follow(speed: 0.45, acquireRange: 11, range: 5),
                    new TimedRandomTransition(4300, false, "FlameThrower", "StayBack")
                    ),
                new State("StayBack",
                    new StayCloseToSpawn(.4, 6),
                    new Shoot(12, count: 3, shootAngle: 16, projectileIndex: 1, predictive: 0.6, coolDown: 1000),
                    new Shoot(8, count: 3, shootAngle: 16, projectileIndex: 0, predictive: 0.9, coolDown: 400),
                    new StayBack(speed: 0.5, distance: 10, entity: null),
                    new TimedTransition(time: 2400, targetState: "Follow")
                    ),
                new State("FlameThrower",
                    new State("FB1ORFB2",
                        new StayCloseToSpawn(.4, 6),
                        new TimedRandomTransition(0, false, "FB1", "FB2")
                        ),
                    new State("FB1",
                        new StayCloseToSpawn(.4, 6),
                        new Shoot(radius: 12, count: 2, shootAngle: 16, projectileIndex: 2, coolDown: 400),
                        new Shoot(radius: 12, count: 1, projectileIndex: 3, coolDown: 200)
                        ),
                    new State("FB2",
                        new StayCloseToSpawn(.4, 6),
                        new Shoot(radius: 12, count: 2, shootAngle: 16, projectileIndex: 3, coolDown: 400),
                        new Shoot(radius: 12, count: 1, projectileIndex: 2, coolDown: 200)
                        ),
                    new TimedTransition(time: 4000, targetState: "Follow")
                    )
                ),
            new Threshold(0.01,            
                new ItemLoot(item: "Seal of the Enchanted Forest", probability: 0.005),
                 new ItemLoot(item: "Sugar Cane", probability: 0.005),
                 new ItemLoot("Chocolate Skull", 0.00125),
                 new ItemLoot(item: "Red Candy Apple", probability: 0.005),
                 new ItemLoot(item: "Green Candy Apple", probability: 0.005),
                new ItemLoot(item: "Potion of Vitality", probability: 1),
                new ItemLoot(item: "Potion of Dexterity", probability: 1),
                new ItemLoot(item: "Rock Candy", probability: 0.08),
                new ItemLoot(item: "Red Gumball", probability: 0.15),
                new ItemLoot(item: "Purple Gumball", probability: 0.15),
                new ItemLoot(item: "Blue Gumball", probability: 0.15),
                new ItemLoot(item: "Green Gumball", probability: 0.15),
                new ItemLoot(item: "Yellow Gumball", probability: 0.15),
                new TierLoot(tier: 10, type: ItemType.Weapon, probability: 0.04),
                new TierLoot(tier: 11, type: ItemType.Weapon, probability: 0.06),
                new TierLoot(tier: 12, type: ItemType.Armor, probability: 0.06),
                new TierLoot(tier: 4, type: ItemType.Ability, probability: 0.05),
                new TierLoot(tier: 5, type: ItemType.Ability, probability: 0.03),
                new TierLoot(tier: 4, type: ItemType.Ring, probability: 0.05),
                new TierLoot(tier: 5, type: ItemType.Ability, probability: 0.03)
                )
            )
        .Init("Spoiled Creampuff",
            new State(
                new Spawn(children: "Big Creampuff", maxChildren: 2, initialSpawn: 0, givesNoXp: false),
                new Reproduce(children: "Big Creampuff", densityRadius: 24, densityMax: 2, coolDown: 12500),
                new Shoot(radius: 12, count: 3, shootAngle: 10, projectileIndex: 0, predictive: 1, coolDown: 1200, coolDownOffset: 200),
                new Shoot(radius: 12, count: 3, shootAngle: 10, projectileIndex: 0, predictive: 1, coolDown: 1200, coolDownOffset: 400),
                new Shoot(radius: 12, count: 3, shootAngle: 10, projectileIndex: 0, predictive: 1, coolDown: 1200, coolDownOffset: 600),
                new Shoot(radius: 8.4, count: 5, shootAngle: 12, projectileIndex: 1, predictive: 0.6, coolDown: 400),
                new Prioritize(
                    new StayCloseToSpawn(.4, 6),
                    new Charge(speed: 4, range: 11, coolDown: 800),
                    new StayBack(speed: 1, distance: 2, entity: null)
                    ),
                new Wander(speed: 0.4)
                ),
            new Threshold(0.01,
                new ItemLoot(item: "Candy-Coated Armor", probability: 0.005),
                new ItemLoot(item: "Sugar Cane", probability: 0.005),
                new ItemLoot("Chocolate Skull", 0.00125),
                 new ItemLoot(item: "Red Candy Apple", probability: 0.005),
                 new ItemLoot(item: "Green Candy Apple", probability: 0.005),
                new ItemLoot(item: "Potion of Attack", probability: 1),
                new ItemLoot(item: "Potion of Defense", probability: 1),
                new ItemLoot(item: "Rock Candy", probability: 0.08),
                new ItemLoot(item: "Red Gumball", probability: 0.15),
                new ItemLoot(item: "Purple Gumball", probability: 0.15),
                new ItemLoot(item: "Blue Gumball", probability: 0.15),
                new ItemLoot(item: "Green Gumball", probability: 0.15),
                new ItemLoot(item: "Yellow Gumball", probability: 0.15),
                new TierLoot(tier: 10, type: ItemType.Weapon, probability: 0.04),
                new TierLoot(tier: 11, type: ItemType.Weapon, probability: 0.06),
                new TierLoot(tier: 12, type: ItemType.Armor, probability: 0.06),
                new TierLoot(tier: 4, type: ItemType.Ability, probability: 0.05),
                new TierLoot(tier: 5, type: ItemType.Ability, probability: 0.03),
                new TierLoot(tier: 4, type: ItemType.Ring, probability: 0.05),
                new TierLoot(tier: 5, type: ItemType.Ability, probability: 0.03)
                )
            )
        .Init("Desire Troll",
            new State(
                new State("BaseAttack",
                    new StayCloseToSpawn(.4, 6),
                    new Shoot(radius: 10, count: 6, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 800),
                    new Grenade(radius: 5, damage: 200, range: 8, coolDown: 700),
                    new Shoot(radius: 10, count: 3, projectileIndex: 1, predictive: 1, coolDown: 1500),
                    new State("Choose",
                        new TimedRandomTransition(3800, false, "Run", "Attack")
                        ),
                    new State("Run",
                        new StayCloseToSpawn(.4, 4),
                        new TimedTransition(time: 1200, targetState: "Choose")
                        ),
                    new State("Attack",
                        new StayCloseToSpawn(.4, 6),
                        new NewCharge(speed: 1.2f, range: 11.0f, cooldown: 1.0f),
                        new TimedTransition(time: 500, targetState: "Choose")
                        ),
                    new HpLessTransition(threshold: 0.6, targetState: "NextAttack")
                    ),
                new State("NextAttack",
                    new StayCloseToSpawn(.4, 6),
                    new Shoot(radius: 10, count: 7, shootAngle: 10, projectileIndex: 2, predictive: 0.5, angleOffset: 0.4, coolDown: 1000),
                    new Shoot(radius: 10, count: 3, projectileIndex: 1, predictive: 1, coolDown: 1000),
                    new Shoot(radius: 10, count: 5, shootAngle: 15, projectileIndex: 0, predictive: 1, angleOffset: 1, coolDown: 2000),
                    new Grenade(radius: 5, damage: 200, range: 8, coolDown: 3000),
                    new State("Choose2",
                        new TimedRandomTransition(3800, false, "Run2", "Attack2")
                        ),
                    new State("Run2",
                        new StayCloseToSpawn(.4, 6),
                        new StayBack(speed: 1.5, distance: 10, entity: null),
                        new TimedTransition(time: 1500, targetState: "Choose2"),
                        new PlayerWithinTransition(dist: 3.5, targetState: "Boom", seeInvis: false)
                        ),
                    new State("Attack2",
                        new StayCloseToSpawn(.4, 6),
                        new Charge(speed: 1.2, range: 11, coolDown: 1000),
                        new TimedTransition(time: 1000, targetState: "Choose2"),
                        new PlayerWithinTransition(dist: 3.5, targetState: "Boom", seeInvis: false)
                        ),
                    new State("Boom",
                        new StayCloseToSpawn(.4, 6),
                        new Shoot(8, count: 20, shootAngle: 18, projectileIndex: 3, coolDown: 1000),
                        new TimedTransition(time: 200, targetState: "Choose2")
                        )
                    ),
                new StayCloseToSpawn(.4, 6),
                new Prioritize(
                    new Follow(speed: 1, acquireRange: 11, range: 5)
                    ),
                new Wander(speed: 0.4)
                ),
            new Threshold(0.01,
                new ItemLoot(item: "Seal of the Enchanted Forest", probability: 0.005),
                new ItemLoot(item: "Sugar Cane", probability: 0.005),
                new ItemLoot("Chocolate Skull", 0.00125),
                 new ItemLoot(item: "Red Candy Apple", probability: 0.005),
                 new ItemLoot(item: "Green Candy Apple", probability: 0.005),
                new ItemLoot(item: "Potion of Attack", probability: 1),
                new ItemLoot(item: "Potion of Wisdom", probability: 1),
                new ItemLoot(item: "Rock Candy", probability: 0.08),
                new ItemLoot(item: "Red Gumball", probability: 0.15),
                new ItemLoot(item: "Purple Gumball", probability: 0.15),
                new ItemLoot(item: "Blue Gumball", probability: 0.15),
                new ItemLoot(item: "Green Gumball", probability: 0.15),
                new ItemLoot(item: "Yellow Gumball", probability: 0.15),
                new TierLoot(tier: 10, type: ItemType.Weapon, probability: 0.04),
                new TierLoot(tier: 11, type: ItemType.Weapon, probability: 0.06),
                new TierLoot(tier: 12, type: ItemType.Armor, probability: 0.06),
                new TierLoot(tier: 4, type: ItemType.Ability, probability: 0.05),
                new TierLoot(tier: 5, type: ItemType.Ability, probability: 0.03),
                new TierLoot(tier: 4, type: ItemType.Ring, probability: 0.05),
                new TierLoot(tier: 5, type: ItemType.Ability, probability: 0.03)
                )
            )
        .Init("Swoll Fairy",
            new State(
                new Spawn(children: "Fairy", maxChildren: 6, initialSpawn: 0, coolDown: 10000, givesNoXp: false),
                new StayCloseToSpawn(.4, 6),
                new Prioritize(
                    new Follow(speed: 0.8, acquireRange: 10, range: 5)
                    ),
                new State("Shoot",
                    new StayCloseToSpawn(.4, 6),
                    new Shoot(radius: 11, count: 3, shootAngle: 30, projectileIndex: 0, predictive: 1, coolDown: 500),
                    new TimedTransition(time: 3000, targetState: "ShootB")
                    ),
                new State("ShootB",
                    new StayCloseToSpawn(.4, 6),
                    new Shoot(radius: 11, count: 8, shootAngle: 45, projectileIndex: 1, coolDown: 600),
                    new TimedTransition(time: 3000, targetState: "Pause")
                    ),
                new State("Pause",
                    new StayCloseToSpawn(.4, 6),
                    new TimedRandomTransition(3, false, "Shoot", "ShootB")
                    )
                ),
            new Threshold(0.01,
                new ItemLoot(item: "Candy-Coated Armor", probability: 0.005),
                new ItemLoot(item: "Pixie-Enchanted Sword", probability: 0.005),
                new ItemLoot(item: "Seal of the Enchanted Forest", probability: 0.005),
                new ItemLoot("Chocolate Skull", 0.00125),
                new ItemLoot(item: "Sugar Cane", probability: 0.005),
                 new ItemLoot(item: "Red Candy Apple", probability: 0.005),
                 new ItemLoot(item: "Green Candy Apple", probability: 0.005),
                new ItemLoot(item: "Potion of Defense", probability: 1),
                new ItemLoot(item: "Potion of Wisdom", probability: 1),
                new ItemLoot(item: "Rock Candy", probability: 0.08), 
                new ItemLoot(item: "Red Gumball", probability: 0.15),
                new ItemLoot(item: "Purple Gumball", probability: 0.15),
                new ItemLoot(item: "Blue Gumball", probability: 0.15),
                new ItemLoot(item: "Green Gumball", probability: 0.15),
                new ItemLoot(item: "Yellow Gumball", probability: 0.15),
                new TierLoot(tier: 11, type: ItemType.Weapon, probability: 0.01),
                new TierLoot(tier: 5, type: ItemType.Ability, probability: 0.01),
                new TierLoot(tier: 5, type: ItemType.Ring, probability: 0.01)
                )
            )
        .Init("Gigacorn",
            new State(
                new StayCloseToSpawn(.4, 4),
                new TimedTransition(1000, "Start"),
                new Prioritize(
                    new Charge(speed: 1.4, range: 11, coolDown: 1900)
                    ),
                new State("Start",
                        new StayCloseToSpawn(.4, 4),
                        new Wander(1.2),
                        new Shoot(radius: 10, count: 2, shootAngle: 10, projectileIndex: 0, predictive: 1, coolDown: 200),
                        new Shoot(radius: 2, count: 8, projectileIndex: 0, predictive: 1, coolDown: 1500, coolDownOffset: 600),
                        new TimedTransition(time: 1500, targetState: "ShootPause")
                        ),
                    new State("ShootPause",
                        new StayCloseToSpawn(.4, 4),
                        new Shoot(radius: 4.5, count: 4, shootAngle: 10, projectileIndex: 1, predictive: 0.8, coolDown: 1200, coolDownOffset: 300),
                        new Shoot(radius: 4.5, count: 4, shootAngle: 10, projectileIndex: 1, predictive: 0.8, coolDown: 1200, coolDownOffset: 600),
                        new Shoot(radius: 4.5, count: 4, shootAngle: 10, projectileIndex: 1, predictive: 0.8, coolDown: 1200, coolDownOffset: 900),
                        new TimedTransition(time: 1200, targetState: "Start")
                        )
                    ),
            new Threshold(0.01,
                new ItemLoot(item: "Sugar Cane", probability: 0.005),
                new ItemLoot("Chocolate Skull", 0.00125),
                new ItemLoot(item: "Red Candy Apple", probability: 0.005),
                new ItemLoot(item: "Green Candy Apple", probability: 0.005),
                new ItemLoot(item: "Candy-Coated Armor", probability: 0.005),
                new ItemLoot(item: "Pixie-Enchanted Sword", probability: 0.005),
                new ItemLoot(item: "Seal of the Enchanted Forest", probability: 0.005),
                new ItemLoot(item: "Potion of Attack", probability: 1),
                new ItemLoot(item: "Potion of Wisdom", probability: 1),
                new ItemLoot(item: "Rock Candy", probability: 0.08),
                new ItemLoot(item: "Candy-Coated Armor", probability: 0.01),
                new ItemLoot(item: "Red Gumball", probability: 0.15),
                new ItemLoot(item: "Purple Gumball", probability: 0.15),
                new ItemLoot(item: "Blue Gumball", probability: 0.15),
                new ItemLoot(item: "Green Gumball", probability: 0.15),
                new ItemLoot(item: "Yellow Gumball", probability: 0.15),
                new ItemLoot(item: "Pixie-Enchanted Sword", probability: 0.01),
                new TierLoot(tier: 10, type: ItemType.Weapon, probability: 0.04),
                new TierLoot(tier: 11, type: ItemType.Weapon, probability: 0.06),
                new TierLoot(tier: 12, type: ItemType.Armor, probability: 0.06),
                new TierLoot(tier: 4, type: ItemType.Ability, probability: 0.05),
                new TierLoot(tier: 5, type: ItemType.Ability, probability: 0.03),
                new TierLoot(tier: 4, type: ItemType.Ring, probability: 0.05),
                new TierLoot(tier: 5, type: ItemType.Ability, probability: 0.03)
                )
            )
        .Init("Candyland Boss Spawner",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invincible),
                new State("Ini",
                    new NoPlayerWithinTransition(dist: 16, targetState: "Ini2")
                    ),
                new State("Ini2",
                    new TimedRandomTransition(0, false, "Creampuff", "Unicorn", "Troll", "Rototo", "Fairy", "Gumball Machine")
                    ),
                new State("Ini3",
                    new EntitiesNotExistsTransition(16, "Ini", "Spoiled Creampuff", "Gigacorn", "Desire Troll", "MegaRototo", "Swoll Fairy", "Gumball Machine")
                    ),
                new State("Creampuff",
                    new Spawn(children: "Spoiled Creampuff", maxChildren: 1, initialSpawn: 0, givesNoXp: false),
                    new TimedTransition(time: 3000, targetState: "Ini3")
                    ),
                new State("Unicorn",
                    new Spawn(children: "Gigacorn", maxChildren: 1, initialSpawn: 0, givesNoXp: false),
                    new TimedTransition(time: 3000, targetState: "Ini3")
                    ),
                new State("Troll",
                    new Spawn(children: "Desire Troll", maxChildren: 1, initialSpawn: 0, givesNoXp: false),
                    new TimedTransition(time: 3000, targetState: "Ini3")
                    ),
                new State("Rototo",
                    new Spawn(children: "MegaRototo", maxChildren: 1, initialSpawn: 0, givesNoXp: false),
                    new TimedTransition(time: 3000, targetState: "Ini3")
                    ),
                new State("Fairy",
                    new Spawn(children: "Swoll Fairy", maxChildren: 1, initialSpawn: 0, givesNoXp: false),
                    new TimedTransition(time: 3000, targetState: "Ini3")
                    ),
                new State("Gumball Machine",
                    new Spawn(children: "Gumball Machine", maxChildren: 1, initialSpawn: 0, givesNoXp: false),
                    new TimedTransition(time: 3000, targetState: "Ini3")
                    )
                )
            )
        .Init("Gumball Machine",
            new State(),
            new Threshold(0.01,
                new ItemLoot(item: "Rock Candy", probability: 0.15),
                new ItemLoot(item: "Red Gumball", probability: 0.15),
                new ItemLoot(item: "Purple Gumball", probability: 0.15),
                new ItemLoot(item: "Blue Gumball", probability: 0.15),
                new ItemLoot(item: "Green Gumball", probability: 0.15),
                new ItemLoot(item: "Yellow Gumball", probability: 0.15)
                )
            )
        .Init("Fairy",
            new State(
                new StayCloseToSpawn(speed: 1, range: 13),
                new Prioritize(
                    new Protect(speed: 1.2, protectee: "Beefy Fairy", acquireRange: 15, protectionRange: 8, reprotectRange: 6),
                    new Orbit(speed: 1.2, radius: 4, acquireRange: 7)
                    ),
                new Wander(speed: 0.6),
                new Shoot(radius: 10, count: 2, shootAngle: 30, projectileIndex: 0, predictive: 1, coolDown: 2000),
                new Shoot(radius: 10, count: 1, projectileIndex: 0, predictive: 1, coolDown: 2000, coolDownOffset: 1000)
                )
            )
        .Init("Big Creampuff",
            new State(
                new Spawn(children: "Small Creampuff", maxChildren: 4, initialSpawn: 0, givesNoXp: false),
                new Shoot(radius: 10, count: 1, projectileIndex: 0, predictive: 1, coolDown: 1400),
                new Shoot(radius: 4.4, count: 5, shootAngle: 12, projectileIndex: 1, predictive: 0.6, coolDown: 800),
                new Prioritize(
                    new Charge(speed: 1.4, range: 11, coolDown: 4200),
                    new StayBack(speed: 1, distance: 4, entity: null),
                    new StayBack(speed: 0.5, distance: 7, entity: null)
                    ),
                new StayCloseToSpawn(.4, 6),
                new Wander(speed: 0.4)
                )
            )
        .Init("Small Creampuff",
            new State(
                new Shoot(radius: 5, count: 3, shootAngle: 12, projectileIndex: 1, predictive: 0.6, coolDown: 1000),
                new StayCloseToSpawn(speed: 1.3, range: 13),
                new Prioritize(
                    new Charge(speed: 1.3, range: 13, coolDown: 2500),
                    new Protect(speed: 0.8, protectee: "Big Creampuff", acquireRange: 15, protectionRange: 7, reprotectRange: 6)
                    ),
                new Wander(speed: 0.6)
                )
            )
        .Init("Tiny Rototo",
            new State(
                new Prioritize(
                    new Orbit(speed: 1.2, radius: 4, acquireRange: 10, target: "MegaRototo"),
                    new Protect(speed: 0.8, protectee: "Rototo", acquireRange: 15, protectionRange: 7, reprotectRange: 6)
                    ),
                new State("Main",
                    new State("Unaware",
                        new Prioritize(
                            new Orbit(speed: 1.4, radius: 2.6, acquireRange: 8, target: "Rototo", speedVariance: 0.2, radiusVariance: 0.2, orbitClockwise: true),
                            new Wander(speed: 0.35)
                            ),
                        new PlayerWithinTransition(dist: 3.4, targetState: "Attack"),
                        new HpLessTransition(threshold: 0.999, targetState: "Attack")
                        ),
                    new State("Attack",
                        new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 1, defaultAngle: 45, coolDown: 1400),
                        new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 0, coolDown: 1400),
                        new Prioritize(
                            new Follow(speed: 0.8, acquireRange: 8, range: 3, duration: 3000, coolDown: 2000),
                            new Charge(speed: 1.35, range: 11, coolDown: 1000),
                            new Wander(speed: 0.35)
                            )
                        )
                    )
                )
            )
        .Init("Butterfly",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invincible),
                new StayCloseToSpawn(speed: 0.3, range: 6),
                new State("Moving",
                    new Wander(speed: 0.25),
                    new PlayerWithinTransition(dist: 6, targetState: "Follow")
                    ),
                new State("Follow",
                    new Prioritize(
                        new StayBack(speed: 0.23, distance: 1.2, entity: null),
                        new Orbit(speed: 1.2, radius: 1.6, acquireRange: 3)
                        ),
                    new Follow(speed: 1.2, acquireRange: 7, range: 3),
                    new Wander(speed: 0.2),
                    new NoPlayerWithinTransition(dist: 4, targetState: "Moving")
                    )
                )
            )
        ;
    }
}
