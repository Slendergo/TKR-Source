using TKR.Shared.resources;
using TKR.WorldServer.logic.loot;
using TKR.WorldServer.logic.behaviors;
using TKR.WorldServer.logic.transitions;

namespace TKR.WorldServer.logic
{
    partial class BehaviorDb
    {
        private _ CrawlingDepths = () => Behav()
        .Init("Son of Arachna",
            new State(
                new ScaleHP2(20),
                new DropPortalOnDeath("Realm Portal", 1, 0, 0, 0, 120),
                new EntitiesNotExistsTransition(50, "Attack 2", "Yellow Son of Arachna Giant Egg Sac", "Blue Son of Arachna Giant Egg Sac", "Red Son of Arachna Giant Egg Sac", "Silver Son of Arachna Giant Egg Sac"),
                new State("Idle",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(9, "MakeWeb")
                    ),
                new State("MakeWeb",
                    new TossObject("Epic Arachna Web Spoke 1", range: 10, angle: 0, coolDown: 100000),
                    new TossObject("Epic Arachna Web Spoke 7", range: 6, angle: 0, coolDown: 100000),
                    new TossObject("Epic Arachna Web Spoke 2", range: 10, angle: 60, coolDown: 100000),
                    new TossObject("Epic Arachna Web Spoke 3", range: 10, angle: 120, coolDown: 100000),
                    new TossObject("Epic Arachna Web Spoke 8", range: 6, angle: 120, coolDown: 100000),
                    new TossObject("Epic Arachna Web Spoke 4", range: 10, angle: 180, coolDown: 100000),
                    new TossObject("Epic Arachna Web Spoke 5", range: 10, angle: 240, coolDown: 100000),
                    new TossObject("Epic Arachna Web Spoke 9", range: 6, angle: 240, coolDown: 100000),
                    new TossObject("Epic Arachna Web Spoke 6", range: 10, angle: 300, coolDown: 100000),
                    new EntitiesNotExistsTransition(30, "Attack 2", "Yellow Son of Arachna Giant Egg Sac", "Blue Son of Arachna Giant Egg Sac", "Red Son of Arachna Giant Egg Sac", "Silver Son of Arachna Giant Egg Sac"),
                    new TimedTransition(3500, "Attack")
                    ),
                // BUCLE INVULNERABLE
                new State("Attack",
                    //new Taunt("Check 1"),
                    new EntitiesNotExistsTransition(25, "Attack 2", "Yellow Son of Arachna Giant Egg Sac", "Blue Son of Arachna Giant Egg Sac", "Red Son of Arachna Giant Egg Sac", "Silver Son of Arachna Giant Egg Sac"),
                    new Shoot(15, projectileIndex: 0, count: 8, shootAngle: 15, coolDown: 1200),
                    new Shoot(16, projectileIndex: 1, coolDown: 2200),
                    new TimedTransition(2200, "Follow")
                    ),
                    new State("Follow",
                        //new Taunt("Check 2"),
                        new EntitiesNotExistsTransition(25, "Attack 2", "Yellow Son of Arachna Giant Egg Sac", "Blue Son of Arachna Giant Egg Sac", "Red Son of Arachna Giant Egg Sac", "Silver Son of Arachna Giant Egg Sac"),
                        new Prioritize(
                            new StayCloseToSpawn(.6, 10),
                            new StayAbove(.6, 1),
                            new StayBack(.6, distance: 8),
                            new Wander(0.3)
                            ),
                        new TimedTransition(1500, "Return")
                        ),
                    new State("Return",
                        //new Taunt("Check 3"),
                        new EntitiesNotExistsTransition(25, "Attack 2", "Yellow Son of Arachna Giant Egg Sac", "Blue Son of Arachna Giant Egg Sac", "Red Son of Arachna Giant Egg Sac", "Silver Son of Arachna Giant Egg Sac"),
                        new StayCloseToSpawn(.4, 1),
                        new TimedTransition(2500, "Follow")
                        ),
                // BUCLE VULNERABLE
                new State("Attack 2",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(15, projectileIndex: 0, count: 8, coolDown: 2200, shootAngle: 45, fixedAngle: 0),
                    new Shoot(15, projectileIndex: 1, coolDown: 3000),
                    new TimedTransition(1500, "Follow 2")
                    ),
                    new State("Follow 2",
                        new Prioritize(
                            new StayCloseToSpawn(1, 10),
                            new StayAbove(1, 1),
                            new StayBack(1, distance: 8),
                            new Wander(0.3)
                            ),
                        new TimedTransition(1500, "Return 2")
                        ),
                    new State("Return 2",
                        new StayCloseToSpawn(.4, 1),
                        new ReturnToSpawn(.4),
                        new TimedTransition(2500, "Follow 2")             
                        )
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Doku No Ken", 0.00125, 0, 0.03),
                new ItemLoot("Arachne Carapace", 0.00125, 0, 0.03)
                ),
            new Threshold(0.05,
                new ItemLoot("Bone Flinger", 0.00125),
                new ItemLoot("Venomous Quiver", 0.00125)
                ),
            new Threshold(0.01,
                new TierLoot(12, ItemType.Weapon, 0.2),
                new TierLoot(6, ItemType.Ability, 0.2),
                new TierLoot(13, ItemType.Armor, 0.2),
                new TierLoot(6, ItemType.Ring, 0.2),
                new ItemLoot("Potion of Mana", 1),
                new ItemLoot("Potion of Mana", 1),
                new ItemLoot("Spotted Venom Extract", 0.00125),
                new ItemLoot("The Crawling Depths Key", 0.01, 0, 0.03)
                )
            )
        .Init("Crawling Depths Egg Sac",
            new State(
                new State("CheckOrDeath",
                    new PlayerWithinTransition(2, "Urclose"),
                    new TransformOnDeath("Crawling Spider Hatchling", 5, 7)
                    ),
                new State("Urclose",
                    new Spawn("Crawling Spider Hatchling", 6),
                    new Suicide()
                    )))
        .Init("Crawling Spider Hatchling",
            new State(
                new Prioritize(
                    new Wander(0.3)
                    ),
                new Shoot(7, count: 1, shootAngle: 0, coolDown: 650),
                new Shoot(7, count: 1, shootAngle: 0, projectileIndex: 1, predictive: 1, coolDown: 850)
                )
            )
        .Init("Crawling Grey Spotted Spider",
            new State(
                new Prioritize(
                    new Charge(3, 8, 1050),
                    new Wander(0.3)
                    ),
                new Shoot(10, count: 1, shootAngle: 0, coolDown: 500)
                ),
            new ItemLoot("Healing Ichor", 0.2),
            new ItemLoot("Health Potion", 0.3)
            )
        .Init("Crawling Grey Spider",
            new State(
                new Prioritize(
                    new Charge(3, 8, 1050),
                    new Wander(0.3)
                    ),
                new Shoot(9, count: 1, shootAngle: 0, coolDown: 850)
                ),
            new ItemLoot("Healing Ichor", 0.2),
            new ItemLoot("Health Potion", 0.3)
            )
        .Init("Crawling Red Spotted Spider",
            new State(
                new Prioritize(
                    new Wander(0.3)
                    ),
                new Shoot(8, count: 1, shootAngle: 0, coolDown: 750)
                ),
            new ItemLoot("Healing Ichor", 0.2),
            new ItemLoot("Health Potion", 0.3)
            )
        .Init("Crawling Green Spider",
            new State(
                new Prioritize(
                    new Follow(2, 11, 1),
                    new Wander(0.3)
                    ),
                new Shoot(8, count: 3, shootAngle: 10, coolDown: 400)
                ),
            new ItemLoot("Healing Ichor", 0.2),
            new ItemLoot("Health Potion", 0.3)
            )
        .Init("Yellow Son of Arachna Giant Egg Sac",
            new State(
                new ScaleHP2(20),
                new TransformOnDeath("Yellow Egg Summoner"),
                new State("Spawn",
                    new Spawn("Crawling Green Spider", 2),
                    new EntityNotExistsTransition("Crawling Green Spider", 20, "Spawn2")
                    ),
                new State("Spawn2",
                    new Spawn("Crawling Grey Spider", 2),
                    new EntityNotExistsTransition("Crawling Grey Spider", 20, "Spawn3")
                    ),
                new State("Spawn3",
                    new Spawn("Crawling Red Spotted Spider", 2),
                    new EntityNotExistsTransition("Crawling Red Spotted Spider", 20, "Spawn4")
                    ),
                new State("Spawn4",
                    new Spawn("Crawling Spider Hatchling", 2),
                    new EntityNotExistsTransition("Crawling Spider Hatchling", 20, "Spawn5")
                    ),
                new State("Spawn5",
                    new Spawn("Crawling Grey Spotted Spider", 2),
                    new EntityNotExistsTransition("Crawling Grey Spotted Spider", 20, "Spawn")
                    )),
            new Threshold(0.15,
                new ItemLoot("Potion of Mana", 0.1),
                new ItemLoot("Potion of Dexterity", 1),
                new ItemLoot("Potion of Defense", 1)
                ),
            new Threshold(0.03,
                new ItemLoot("Doku No Ken", 0.00165, 0, 0.03)
                )
            )
        .Init("Blue Son of Arachna Giant Egg Sac",
            new State(
                new ScaleHP2(20),
                new State("DeathSpawn",
                    new TransformOnDeath("Crawling Spider Hatchling", 5, 7)
                    )
                ),
            new Threshold(0.15,
                new ItemLoot("Potion of Mana", 0.1),
                new ItemLoot("Potion of Wisdom", 1),
                new ItemLoot("Potion of Vitality", 1)
                ),
            new Threshold(0.03,
                new ItemLoot("Doku No Ken", 0.00165, 0, 0.03)
                )
            )
        .Init("Red Son of Arachna Giant Egg Sac",
            new State(
                new ScaleHP2(20),
                new State("DeathSpawn",
                    new TransformOnDeath("Crawling Red Spotted Spider", 3, 3)
                    )
                ),
            new Threshold(0.15,
                new ItemLoot("Potion of Mana", 0.1),
                new ItemLoot("Potion of Dexterity", 1),
                new ItemLoot("Potion of Defense", 1)
                ),
            new Threshold(0.03,
                new ItemLoot("Doku No Ken", 0.00165, 0, 0.03)
                )
            )
        .Init("Silver Son of Arachna Giant Egg Sac",
            new State(
                new ScaleHP2(20),
                new State("DeathSpawn",
                    new TransformOnDeath("Crawling Grey Spider", 3, 3)
                    )
                ),
            new Threshold(0.15,
                new ItemLoot("Potion of Mana", 0.1),
                new ItemLoot("Potion of Wisdom", 1),
                new ItemLoot("Potion of Vitality", 1)
                ),
            new Threshold(0.03,
                new ItemLoot("Doku No Ken", 0.00165, 0, 0.03)
                )
            )
        .Init("Silver Egg Summoner",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invincible)
                )
            )
        .Init("Yellow Egg Summoner",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invincible)
                )
            )
        .Init("Red Egg Summoner",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invincible)
                )
            )
        .Init("Blue Egg Summoner",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invincible)
                )
            )
        .Init("Epic Arachna Web Spoke 1",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 180, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 120, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 240, coolDown: 150)
                )
            )
        .Init("Epic Arachna Web Spoke 2",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 240, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 180, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 300, coolDown: 150)
                )
            )
        .Init("Epic Arachna Web Spoke 3",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 300, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 240, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 0, coolDown: 150)
                )
            )
        .Init("Epic Arachna Web Spoke 4",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 0, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 60, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 300, coolDown: 150)
                )
            )
        .Init("Epic Arachna Web Spoke 5",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 60, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 0, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 120, coolDown: 150)
                )
            )
        .Init("Epic Arachna Web Spoke 6",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 120, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 60, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 180, coolDown: 150)
                )
            )
        .Init("Epic Arachna Web Spoke 7",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 180, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 120, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 240, coolDown: 150)
                )
            )
        .Init("Epic Arachna Web Spoke 8",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 360, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 240, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 300, coolDown: 150)
                )
            )
        .Init("Epic Arachna Web Spoke 9",
            new State(
                new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                new Shoot(200, count: 1, fixedAngle: 0, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 60, coolDown: 150),
                new Shoot(200, count: 1, fixedAngle: 120, coolDown: 150)
                )
            );
    }
}
