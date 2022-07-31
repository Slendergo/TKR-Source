using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Elementals = () => Behav()

        #region Water Elemental

        .Init("Water Elemental Minion",
            new State(
                new ScaleHP2(5),
                new State("Start",
                    new Shoot(15, 4, projectileIndex: 0, coolDown: 1000, coolDownOffset: 1000),
                    new Prioritize(
                        new Orbit(1, 6, 20, "Water Elemental", orbitClockwise: true)),
                    new EntityNotExistsTransition("Water Elemental", 200, "Die")
                    ),
                new State("Suicide",
                    new Shoot(15, 8, projectileIndex: 0, coolDown: 1000, coolDownOffset: 1000),
                    new Orbit(1, 6, 20, "Water Elemental", orbitClockwise: true),
                    new TimedTransition(1000, "Die")
                    ),
                new State("Die",
                    new Suicide()
                    )
                )
            )

        .Init("Water Elemental",
            new State(
                new ScaleHP2(15),
                new State("Waiting Player",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(10, "Start", false)
                    ),
                new State("Start",
                    new TimedTransition(3000, "One Now")
                    ),
                new State("One Now",
                    new Spawn("Water Elemental Minion", 1, 1, 99999),
                    new Taunt("One..."),
                    new Shoot(15, 3, shootAngle: 25, projectileIndex: 0, coolDown: 1500),
                    new EntitiesNotExistsTransition(30, "Two Now", "Water Elemental Minion")
                    ),
                new State("Two Now",
                    new Taunt("Two..."),
                    new TimedTransition(1500, "Two Spawn")
                    ),
                new State("Two Spawn",
                    new SetAltTexture(1, 1),
                    new Spawn("Water Elemental Minion", 2, 1, 1000),
                    new Shoot(15, 3, shootAngle: 25, projectileIndex: 0, coolDown: 1500),
                    new EntitiesNotExistsTransition(30, "Three Now", "Water Elemental Minion")
                    ),
                new State("Three Now",
                    new Taunt("Three..."),
                    new TimedTransition(1500, "Three Spawn")
                    ),
                new State("Three Spawn",
                    new Spawn("Water Elemental Minion", 3, 1, 1000),
                    new Shoot(15, 8, projectileIndex: 0, coolDown: 4000, coolDownOffset: 1000),
                    new Shoot(15, 3, shootAngle: 25, projectileIndex: 0, coolDown: 1500),
                    new EntitiesNotExistsTransition(30, "Four Now", "Water Elemental Minion")
                    ),
                new State("Four Now",
                    new Taunt("Four..."),
                    new TimedTransition(1500, "Four Spawn")
                    ),
                new State("Four Spawn",
                    new SetAltTexture(2, 2),
                    new Spawn("Water Elemental Minion", 4, 1, 1000),
                    new Shoot(15, 8, projectileIndex: 0, coolDown: 4000, coolDownOffset: 1000),
                    new Shoot(15, 3, shootAngle: 25, projectileIndex: 0, coolDown: 1500),
                    new EntitiesNotExistsTransition(30, "Rage", "Water Elemental Minion")
                    ),
                new State("Rage",
                    new Taunt("..."),
                    new TimedTransition(1500, "Shoot")
                    ),
                new State("Shoot",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Wander(0.4),
                    new StayCloseToSpawn(1, 8),
                    new Taunt("FIVE!"),
                    new ChangeSize(25, 200),
                    new Shoot(15, 8, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000),
                    new Shoot(15, 5, shootAngle: 25, projectileIndex: 0, coolDown: 1000),
                    new Shoot(15, 5, projectileIndex: 2, shootAngle: 25, coolDown: 3000, coolDownOffset: 3000)
                    )
                ),
            new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Water Fragment", 0.0015, threshold: 0.03)
                ),
            new Threshold(0.001,
                new ItemLoot("Thorn", 0.01),
                new ItemLoot("Massacre", 0.01),
                new ItemLoot("Frozen Water Armor", 0.0125),
                new ItemLoot("Rising Tide Orb", 0.015),
                new ItemLoot("Potion of Wisdom", 1),
                new ItemLoot("Potion of Speed", 1),
                new ItemLoot("Potion of Wisdom", 0.5),
                new ItemLoot("Potion of Speed", 0.5),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Defense", 1),
                new ItemLoot("Potion of Attack", 0.5),
                new ItemLoot("Potion of Defense", 0.5),
                new TierLoot(5, ItemType.Ability, 0.07),
                new TierLoot(11, ItemType.Armor, 0.3),
                new TierLoot(12, ItemType.Armor, 0.1),
                new TierLoot(10, ItemType.Weapon, 0.3),
                new TierLoot(11, ItemType.Weapon, 0.1),
                new TierLoot(5, ItemType.Ring, 0.1),

                new ItemLoot("Magic Dust", 0.5)
                )
            )

        #endregion Water Elemental

        #region Earth Elemental

        .Init("Earth Elemental",
            new State(
                new ScaleHP2(20),
                new StayCloseToSpawn(0.4, 8),
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new State("WaitingPlayer",
                    new PlayerWithinTransition(20, "Start")
                    ),
                new State("Start",
                    new Taunt("Hello Warrior, I come from another galaxy to destroy your planet, I need all the earth of this place!"),
                    new Flash(0x42b9f5, 1, 3),
                    new TimedTransition(3000, "Shoots")
                    ),
                new State("Shoots",
                    new SetAltTexture(0),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Wander(0.4),
                    new Shoot(20, 5, projectileIndex: 0, shootAngle: 15, coolDown: 1500),
                    new HpLessTransition(0.75, "Shoots Two Charge")
                    ),
                new State("Shoots Two Charge",
                    new ReturnToSpawn(0.5, 1),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Flash(0xFF0000, 1, 3),
                    new TimedTransition(3000, "Shoots Two")
                    ),
                new State("Shoots Two",
                    new SetAltTexture(0),
                    new Wander(0.4),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Shoot(20, 5, projectileIndex: 0, shootAngle: 15, coolDown: 1500),
                    new Shoot(20, 16, projectileIndex: 1, coolDown: 1000),
                    new HpLessTransition(0.50, "Shoots Three Charge")
                    ),
                new State("Shoots Three Charge",
                    new ReturnToSpawn(0.5, 1),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Flash(0xFF0000, 1, 3),
                    new TimedTransition(3000, "Shoots Three")
                    ),
                new State("Shoots Three",
                    new SetAltTexture(0),
                    new Wander(0.4),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Shoot(20, 5, projectileIndex: 0, shootAngle: 15, coolDown: 1500),
                    new Shoot(20, 32, projectileIndex: 2, coolDown: 4000),
                    new Shoot(20, 1, projectileIndex: 3, coolDown: 2000)
                    )
                ),
            new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Earth Fragment", 0.0015, threshold: 0.03)
                ),

            new Threshold(0.001,
                new ItemLoot("Forbidden Jungle’s Seal", 0.015),
                new ItemLoot("Shield of the Forest", 0.01),
                new ItemLoot("Cometfell Katana", 0.01),
                new ItemLoot("Forest’s Call", 0.015),
                new ItemLoot("Potion of Wisdom", 1),
                new ItemLoot("Potion of Speed", 1),
                new ItemLoot("Potion of Wisdom", 0.5),
                new ItemLoot("Potion of Speed", 0.5),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Defense", 1),
                new ItemLoot("Potion of Attack", 0.5),
                new ItemLoot("Potion of Defense", 0.5),
                new TierLoot(5, ItemType.Ability, 0.07),
                new TierLoot(11, ItemType.Armor, 0.3),
                new TierLoot(12, ItemType.Armor, 0.1),
                new TierLoot(10, ItemType.Weapon, 0.3),
                new TierLoot(11, ItemType.Weapon, 0.1),
                new TierLoot(5, ItemType.Ring, 0.1),

                new ItemLoot("Magic Dust", 0.5)
                )
            )

        #endregion Earth Elemental

        #region Wind Elemental

        .Init("Wind Elemental",
            new State(
                new ScaleHP2(20),
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new State("Check Player",
                    new PlayerWithinTransition(15, "Start", false)
                    ),
                new State("Start",
                    new Flash(0x696969, 1, 3),
                    new TimedTransition(3000, "Start Shooting")
                    ),
                new State("Start Shooting",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Wander(0.4),
                    new Follow(1, 1, 6, 2000, 3000),
                    new StayCloseToSpawn(1, 7),
                    new Shoot(20, 4, projectileIndex: 1, shootAngle: 15, coolDown: 1000),
                    new HpLessTransition(0.75, "Second Phase Charge")
                    ),
                new State("Second Phase Charge",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1500),
                    new ReturnToSpawn(1),
                    new Taunt("You will test the power of the wind!"),
                    new Flash(0x696969, 0.3, 5),
                    new TimedTransition(1500, "Second Phase")
                    ),
                new State("Second Phase",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Wander(0.4),
                    new Follow(1, 1, 6, 2000, 3000),
                    new StayCloseToSpawn(1, 7),
                    new Shoot(20, 3, projectileIndex: 0, shootAngle: 15, coolDown: 1000),
                    new Shoot(20, 5, projectileIndex: 1, shootAngle: 20, coolDown: 2000),
                    new HpLessTransition(0.50, "Third Phase Charge")
                    ),
                new State("Third Phase Charge",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1500),
                    new ReturnToSpawn(1),
                    new Taunt("Tornadoes! Help me!"),
                    new TimedTransition(1500, "Third Phase")
                    ),
                new State("Third Phase",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Spawn("Wind Elemental Tornado", 4, 0.5, coolDown: 100, true),
                    new Shoot(20, 3, projectileIndex: 0, shootAngle: 15, coolDown: 1000),
                    new Shoot(20, 5, projectileIndex: 1, shootAngle: 20, coolDown: 1000)
                    )
                ),
            new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.01,
                new ItemLoot("Staff of Zephyrs", 0.009, threshold: 0.01),
                new ItemLoot("Cyclone Orb", 0.009, threshold: 0.01),
                new ItemLoot("Gale Robe", 0.009, threshold: 0.01),
                new ItemLoot("Wind Charm", 0.009, threshold: 0.01)
                ),
            new Threshold(0.03,
                new ItemLoot("Wind Fragment", 0.0015, threshold: 0.03),
                new ItemLoot("Quiver of Thunder", 0.0033)
                ),
            new Threshold(0.001,
                new ItemLoot("Potion of Speed", 1),
                new ItemLoot("Potion of Dexterity", 1),
                new ItemLoot("Potion of Speed", 0.5),
                new ItemLoot("Potion of Dexterity", 0.5),
                new ItemLoot("Potion of Vitality", 1),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Vitality", 0.5),
                new ItemLoot("Potion of Attack", 0.5),
                new TierLoot(5, ItemType.Ability, 0.07),
                new TierLoot(11, ItemType.Armor, 0.3),
                new TierLoot(12, ItemType.Armor, 0.1),
                new TierLoot(10, ItemType.Weapon, 0.3),
                new TierLoot(11, ItemType.Weapon, 0.1),
                new TierLoot(5, ItemType.Ring, 0.1),

                new ItemLoot("Magic Dust", 0.5)

                )
            )

        .Init("Wind Elemental Tornado",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Orbit",
                    new Orbit(1.4, 6, 20, "Wind Elemental", 1, 1),
                    new Shoot(20, 4, projectileIndex: 0, coolDown: 2500),
                    new EntityNotExistsTransition("Wind Elemental", 20, "Die")
                    ),
                new State("Die",
                    new ChangeSize(10, 0),
                    new TimedTransition(2000, "Die Two")
                    ),
                new State("Die Two",
                    new Suicide()
                    )
                )
            )

        #endregion Wind Elemental

        #region Fire Elemental

        .Init("Fire Elemental",
            new State(
                new StayCloseToSpawn(1, 7),
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new ScaleHP2(15),
                new State("Check Player",
                    new Spawn("Fire Elemental Rotation", 1, 1, 99999999),
                    new PlayerWithinTransition(20, "Start", false)
                    ),
                new State("Start",
                    new Taunt("You will feel the pain of burning alive!"),
                    new TimedTransition(1500, "Start Shooting")
                    ),
                new State("Start Shooting",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new ChangeSize(10, 150),
                    new Shoot(20, 8, projectileIndex: 2, coolDown: 1500),
                    new Shoot(20, 4, projectileIndex: 2, coolDown: 1000),
                    new HpLessTransition(0.75, "Second Phase")
                    ),
                new State("Second Phase",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("You will see the power of the FIRE!"),
                    new TimedTransition(1500, "Second Phase Start")
                    ),
                new State("Second Phase Start",
                    new Orbit(.6, 7, 20, "Fire Elemental Rotation"),
                    new Shoot(20, 5, shootAngle: 15, projectileIndex: 1, coolDown: 750),
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 2500, coolDownOffset: 1250),
                    new RingAttack(20, 3, 0, projectileIndex: 2, 0.03, 0, 1500),
                    new Grenade(3, 15, 5, coolDown: 2500, effect: ConditionEffectIndex.Bleeding, effectDuration: 1000),
                    new Grenade(5, 15, 10, coolDown: 3500, effect: ConditionEffectIndex.Bleeding, effectDuration: 1250),
                    new HpLessTransition(0.33, "Third Phase Charge")
                    ),
                new State("Third Phase Charge",
                    new ChangeSize(10, 200),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new ReturnToSpawn(1.5),
                    new Taunt("AAAAARRRRRRRRRRGH!"),
                    new TimedTransition(1500, "Rage")
                    ),
                new State("Rage",
                    new ReplaceTile("Fire Tile Semi-Lava Right", "Fire Tile Lava Left", 50),
                    new ReplaceTile("Fire Tile Semi-Lava Left", "Fire Tile Lava Right", 50),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Wander(0.4),
                    new Shoot(20, 4, shootAngle: 15, projectileIndex: 1, coolDown: 2000),
                    new Shoot(20, 8, projectileIndex: 1, coolDown: 5000, coolDownOffset: 2500),
                    new Shoot(20, 5, shootAngle: 15, projectileIndex: 2, coolDown: 2500),
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 2000),
                    new Shoot(20, 8, projectileIndex: 2, coolDown: 5000, coolDownOffset: 2500)
                    )
                ),
            new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.03,
                new ItemLoot("Fire Fragment", 0.0015),
                new ItemLoot("Phoenix Ashes Orb", 0.0033)
                ),
            new Threshold(0.001,
                new ItemLoot("Potion of Speed", 1),
                new ItemLoot("Potion of Defense", 1),
                new ItemLoot("Potion of Speed", 0.5),
                new ItemLoot("Potion of Defense", 0.5),
                new ItemLoot("Potion of Wisdom", 1),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Wisdom", 0.5),
                new ItemLoot("Potion of Attack", 0.5),
                new TierLoot(5, ItemType.Ability, 0.07),
                new TierLoot(11, ItemType.Armor, 0.3),
                new TierLoot(12, ItemType.Armor, 0.1),
                new TierLoot(10, ItemType.Weapon, 0.3),
                new TierLoot(11, ItemType.Weapon, 0.1),
                new TierLoot(5, ItemType.Ring, 0.1),

                new ItemLoot("Magic Dust", 0.5)
                )
            )

        #endregion Fire Elemental

        ;
    }
}
