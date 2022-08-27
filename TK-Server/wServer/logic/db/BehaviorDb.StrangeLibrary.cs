using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ StrangeLibrary = () => Behav()

        #region Dungeon

        .Init("Corrupted Mage",
            new State(
                new State("See Player",
                    new PlayerWithinTransition(15, "Start")
                    ),
                new State("Start",
                    new Wander(0.6),
                    new StayBack(0.6, 5),
                    new Shoot(20, 2, shootAngle: 10, 0, coolDown: 200),
                    new Shoot(20, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000)
                    )
                )
            )
        .Init("Corrupted Priest",
            new State(
                new State("See Player",
                    new PlayerWithinTransition(15, "Start")
                    ),
                new State("Start",
                    new Wander(0.6),
                    new Protect(0.6, "Corrupted Mage", 20, 5, 5),
                    new StayBack(0.6, 5),
                    new Shoot(20, 1, 0, coolDown: 300),
                    new HealEntity(15, "Corrupted Mage", 100, coolDown: 500),
                    new HealEntity(15, "Strange Priest", 1500, coolDown: 500),
                    new Shoot(20, 2, shootAngle: 25, projectileIndex: 1, coolDown: 3000, coolDownOffset: 1500)
                    )
                )
            )
        .Init("Corrupted Mini Priest",
            new State(
                new State("Start",
                    new Wander(0.6),
                    new StayBack(0.6, 5),
                    new Shoot(20, 1, 0, coolDown: 300),
                    new HealEntity(15, "Strange Priest", 100, coolDown: 500),
                    new Shoot(20, 2, shootAngle: 25, projectileIndex: 1, coolDown: 3000, coolDownOffset: 1500)
                    )
                )
            )

        .Init("Strange Magician Spell",
            new State(
                new EntityNotExistsTransition("Strange Magician", 50, "Suicide"),
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new ChangeSize(100, 0),
                new TimedTransition(1500, "Shoot"),
                new State("Follow Player",
                    new Follow(0.5, 20, 0),
                    new PlayerWithinTransition(1, "Shoot")
                    ),
                new State("Shoot",
                    new Taunt("EXPLODE!"),
                    new TimedTransition(1000, "Shoot2")
                    ),
                new State("Shoot2",
                    new Shoot(20, 8, projectileIndex: 0, coolDown: 5000),
                    new Suicide()
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Strange Statue",
            new State(
                new MoveTo2(0.5f, 0.5f, isMapPosition: false, instant: true),
                new OnDeathBehavior(new SwirlingMistDeathParticles())
                )
            )
        .Init("Statue Check 1",
            new State(
                new State("Check",
                    new EntityNotExistsTransition("Strange Statue", 200, "Remove")
                    ),
                new State("Remove",
                    new OpenGate(59, 59, 12, 15)
                    )
                )
            )
        .Init("Player Check",
            new State(
                new State("Check Boss",
                    new EntityNotExistsTransition("Strange Priest", 20, "Check")
                    ),
                new State("Check",
                    new PlayerTextTransition("Remove", "Ameno", 10, ignoreCase: true)
                    ),
                new State("Remove",
                    new OpenGate(33, 33, 12, 15)
                    )
                )
            )
        .Init("Strange Chest",
            new State(
                new ScaleHP2(20),
                new MoveTo2(0.5f, 0.5f, isMapPosition: false, instant: true)
                ),
            new Threshold(0.06,
                new ItemLoot("Tome of Universal Theory", 0.001)
                ),
            new Threshold(0.03,
                new ItemLoot("Wand of Pain", 0.0015, threshold: 0.1)
                ),
            new Threshold(0.01,
                new ItemLoot("Head of Calamity", 0.007),
                new ItemLoot("Blood-Soaked Robe", 0.015),
                new ItemLoot("The Dance of Fire", 0.01),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Defense", 1),
                new ItemLoot("Potion of Defense", 1),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Vitality", 0.5),
                new ItemLoot("Potion of Wisdom", 0.5),
                new TierLoot(10, ItemType.Armor, 0.08),
                new TierLoot(11, ItemType.Armor, 0.06),
                new TierLoot(12, ItemType.Armor, 0.04),
                new TierLoot(10, ItemType.Weapon, 0.08),
                new TierLoot(11, ItemType.Weapon, 0.06),
                new TierLoot(12, ItemType.Weapon, 0.04),
                new TierLoot(3, ItemType.Ring, 0.12),
                new TierLoot(4, ItemType.Ring, 0.07),
                new TierLoot(3, ItemType.Ability, 0.12),
                new TierLoot(4, ItemType.Ability, 0.07)
                )
            )
        .Init("Strange Magician",
            new State(
                new ScaleHP2(20),
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new DropPortalOnDeath("Strange Door", 1, timeout: 120),
                new State("Start",
                    new PlayerWithinTransition(15, "Start Two")
                    ),
                new State("Start Two",
                    new Taunt("My knowledge will kill you. I will dominate you all!"),
                    new TimedTransition(1500, "Shoot One")
                    ),
                new State("Shoot One",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Reproduce("Strange Magician Spell", 20, 3, 3000),
                    new Flash(0xFF0000, 1, 1),
                    new Charge(speed: 3, range: 15, coolDown: 3000), //wanna offset this so it happens 1 second after the reproduce but its whatever
                    new Shoot(20, 2, shootAngle: 20, projectileIndex: 0, predictive: 1, coolDown: 500),
                    new Shoot(20, 1, shootAngle: 0, projectileIndex: 1, predictive: 0, coolDown: 1750),
                    new HpLessTransition(0.5, "Shoot Two")
                    ),
                new State("Shoot Two",
                    new Reproduce("Strange Magician Spell", 20, 3, 3000),
                    new Wander(0.4),
                    new Shoot(radius: 20, count: 1, shootAngle: 20, projectileIndex: 0, fixedAngle: 0, coolDown: 0),
                    new Shoot(radius: 20, count: 1, shootAngle: 20, projectileIndex: 0, fixedAngle: 90, coolDown: 0),
                    new Shoot(radius: 20, count: 1, shootAngle: 20, projectileIndex: 0, fixedAngle: 180, coolDown: 0),
                    new Shoot(radius: 20, count: 1, shootAngle: 20, projectileIndex: 0, fixedAngle: 270, coolDown: 0),
                    new Shoot(20, 3, shootAngle: 20, projectileIndex: 0, predictive: 0.7, coolDown: 500),
                    new Shoot(20, 1, shootAngle: 0, projectileIndex: 1, predictive: 1, coolDown: 0),
                    new HpLessTransition(0.25, "Rage")
                    ),
                new State("Rage",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                    new Taunt("ARRRG! THIS IS IMPOSIBLE!"),
                    new TimedTransition(2000, "Rage Start")
                    ),
                new State("Rage Start",
                    new Reproduce("Strange Magician Spell", 20, 3, 1500),
                    new Wander(0.5),
                    new Chase(speed: 3, sightRange: 11, range: 15, duration: 5, coolDown: 3000),
                    new Shoot(20, 2, shootAngle: 20, projectileIndex: 0, predictive: 0, coolDown: 750),
                    new Shoot(20, 2, shootAngle: 20, projectileIndex: 0, predictive: 0, coolDown: 1000),
                    new Shoot(20, 1, shootAngle: 20, projectileIndex: 1, predictive: 0, coolDown: 1200)
                    )
                ),
            new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Talisman Fragment", 0.01),
                new ItemLoot("Magicians Hide", 0.00014)
                ),
            new Threshold(0.01,
                new ItemLoot("Potion of Life", 0.18),
                new ItemLoot("Potion of Mana", 0.32)
                ),
            new Threshold(0.001,
                new ItemLoot("Potion of Life", 0.015),
                new ItemLoot("Potion of Mana", 0.02),
                new ItemLoot("Azurite Staff", 0.01),
                new ItemLoot("Mysterious Skull", 0.015),
                new ItemLoot("Wrapping Cloak", 0.01),
                new ItemLoot("Corrupted Scroll", 0.015),
                new ItemLoot("Magic Robe", 0.015),
                new ItemLoot("Sapphire Ring", 0.015),
                new ItemLoot("Potion of Dexterity", 1),
                new ItemLoot("Potion of Wisdom", 1),
                new TierLoot(8, ItemType.Armor, 0.22),
                new TierLoot(9, ItemType.Armor, 0.17),
                new TierLoot(10, ItemType.Armor, 0.12),
                new TierLoot(8, ItemType.Weapon, 0.22),
                new TierLoot(9, ItemType.Weapon, 0.17),
                new TierLoot(10, ItemType.Weapon, 0.12),
                new TierLoot(3, ItemType.Ring, 0.12),
                new TierLoot(4, ItemType.Ring, 0.07),
                new TierLoot(3, ItemType.Ability, 0.12),
                new TierLoot(4, ItemType.Ability, 0.07)
                )
            )

        #endregion Dungeon

        .Init("Ball of Fire",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Follow Player",
                    new Follow(1, 20, 1),
                    new TimedTransition(4500, "Shoot"),
                    new PlayerWithinTransition(1, "Shoot")
                    ),
                new State("Shoot",
                    new Shoot(20, 16, projectileIndex: 0, coolDown: 100),
                    new TimedTransition(1, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Strange Priest",
            new State(
                new ScaleHP2(20),
                new MoveTo2(0, 0.5f, 2, isMapPosition: false, instant: true),
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("See Player",
                    new PlayerWithinTransition(15, "Start")
                    ),
                new State("Start",
                    new Taunt("Hmmm... ill see you destroyed all my Statues..."),
                    new TimedTransition(3000, "Start 2")
                    ),
                new State("Start 2",
                    new Taunt("I thought my wizards could win against you."),
                    new TimedTransition(3000, "Start 3")
                    ),
                new State("Start 3",
                    new Taunt("Well... i think i I have to kill you myself."),
                    new TimedTransition(3000, "Start 4")
                    ),
                new State("Start 4",
                    new Taunt("As I prepare, fight with this."),
                    new TossObject("Corrupted Mage", 5, _upAngle - 45, coolDown: 100000),
                    new TossObject("Corrupted Priest", 5, _upAngle - 25, coolDown: 100000),
                    new TossObject("Corrupted Mage", 5, _downAngle + 45, coolDown: 100000),
                    new TossObject("Corrupted Priest", 5, _downAngle + 25, coolDown: 100000),
                    new ChangeSize(-45, 0),
                    new TimedTransition(1500, "Check")
                    ),
                new State("Check",
                    new EntitiesNotExistsTransition(12, "Attack 1", "Corrupted Mage", "Corrupted Priest")
                    ),
                new State("Attack 1",
                    new ChangeSize(35, 200),
                    new TimedTransition(3000, "Subestimated")
                    ),
                new State("Subestimated",
                    new Taunt("Well... i think i subestimated you."),
                    new TimedTransition(3000, "Attack 2")
                    ),
                new State("Attack 2",
                    new Taunt("Let's fight and see who have more Power."),
                    new TimedTransition(1000, "Attack 3")
                    ),
                new State("Attack 3",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new TossObject("Corrupted Mage", 5, _upAngle - 45, coolDown: 100000),
                    new TossObject("Corrupted Priest", 5, _downAngle + 45, coolDown: 100000),
                    new Spawn("Ball of Fire", 100, 0.0, coolDown: 4000, true),
                    new HpLessTransition(0.5, "Attack 4")
                    ),
                new State("Attack 4",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Taunt("How!? Priest! Help me!"),
                    new TimedTransition(1000, "Attack 5")
                    ),
                new State("Attack 5",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new TossObject("Corrupted Priest", 5, _upAngle - 45, coolDown: 100000),
                    new TossObject("Corrupted Priest", 5, _downAngle + 45, coolDown: 100000),
                    new TimedTransition(500, "Attack 6")
                    ),
                new State("Attack 6",
                    new Reproduce("Corrupted Priest", 20, 2, coolDown: 1500),
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new Spawn("Ball of Fire", 100, 0.0, coolDown: 4000, true),
                    new Shoot(20, 5, projectileIndex: 0, shootAngle: 30, coolDown: 1500),
                    new HpLessTransition(0.25, "Attack 7")
                    ),
                new State("Attack 7",
                    new Taunt("UH?! WHAT IS HAPPENING?! IM THE BEST PRIEST IN THE WORLD!!"),
                    new Spawn("Ball of Fire", 100, 0.0, coolDown: 4000, true),
                    new Reproduce("Corrupted Priest", 20, 2, coolDown: 1000),
                    new Shoot(20, 5, projectileIndex: 0, shootAngle: 30, coolDown: 1500),
                    new Grenade(5, 100, 20, coolDown: 3000),
                    new Grenade(2, 25, 20, coolDown: 4000, effect: ConditionEffectIndex.Sick, effectDuration: 1500, color: 0x3575FF00)
                    )
                ),
            new Threshold(0.03,
                new ItemLoot("Wand of Pain", .003)
                ),
            new Threshold(0.05,
                new ItemLoot("Tome of Universal Theory", .0015, threshold: 0.05)//,
                //new ItemLoot("All-Knowing Bookworm", .0015, threshold: 0.05) //NO XML
                ),
            new Threshold(0.01,
                new ItemLoot("Crafting Material 1", 0.01),
                new ItemLoot("Head of Calamity", 0.007),
                new ItemLoot("Blood-Soaked Robe", 0.015),
                new ItemLoot("The Dance of Fire", 0.01),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Defense", 1),
                new ItemLoot("Potion of Defense", 1),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Vitality", 0.5),
                new ItemLoot("Potion of Wisdom", 0.5),
                new TierLoot(11, ItemType.Armor, 0.06),
                new TierLoot(12, ItemType.Armor, 0.04),
                new TierLoot(12, ItemType.Weapon, 0.04),
                new TierLoot(5, ItemType.Ring, 0.07),
                new TierLoot(4, ItemType.Ability, 0.12),
                new TierLoot(5, ItemType.Ability, 0.07)
                )
            )
        ;
    }
}
