using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Abyss = () => Behav()
        .Init("Archdemon Malphas",
            new State(
                new MoveTo2(0.5f, 0.5f, isMapPosition: false, instant: true),
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new ScaleHP2(10),
                new DropPortalOnDeath("Hideout of Malphas Portal", 1, 0, 0, 0, 120),
                new State("Check Player",
                    new PlayerWithinTransition(10, "Start Flashing", false)
                    ),
                new State("Start Flashing",
                    new Flash(0xFF0000, 1, 3),
                    new Taunt("My minions will end with you!"),
                    new TimedTransition(3000, "Start")
                    ),
                new State("Start",
                    new TossObject("White Demon of the Abyss", 7, 45, coolDown: 99999, coolDownOffset: -1),
                    new TossObject("White Demon of the Abyss", 7, -45, coolDown: 99999, coolDownOffset: -1),
                    new TossObject("White Demon of the Abyss", 7, 135, coolDown: 99999, coolDownOffset: -1),
                    new TossObject("White Demon of the Abyss", 7, -135, coolDown: 99999, coolDownOffset: -1),
                    new TimedTransition(1500, "Start Two")
                    ),
                new State("Start Two",
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 0, coolDown: 7400, coolDownOffset: 200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 5, coolDown: 7400, coolDownOffset: 400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 10, coolDown: 7400, coolDownOffset: 600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 15, coolDown: 7400, coolDownOffset: 800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 20, coolDown: 7400, coolDownOffset: 1000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 25, coolDown: 7400, coolDownOffset: 1200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 30, coolDown: 7400, coolDownOffset: 1400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 35, coolDown: 7400, coolDownOffset: 1600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 40, coolDown: 7400, coolDownOffset: 1800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 45, coolDown: 7400, coolDownOffset: 2000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 50, coolDown: 7400, coolDownOffset: 2200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 55, coolDown: 7400, coolDownOffset: 2400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 60, coolDown: 7400, coolDownOffset: 2600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 65, coolDown: 7400, coolDownOffset: 2800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 70, coolDown: 7400, coolDownOffset: 3000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 75, coolDown: 7400, coolDownOffset: 3200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 80, coolDown: 7400, coolDownOffset: 3400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 85, coolDown: 7400, coolDownOffset: 3600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 90, coolDown: 7400, coolDownOffset: 3800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 95, coolDown: 7400, coolDownOffset: 4000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 100, coolDown: 7400, coolDownOffset: 4200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 105, coolDown: 7400, coolDownOffset: 4400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 110, coolDown: 7400, coolDownOffset: 4600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 115, coolDown: 7400, coolDownOffset: 4800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 120, coolDown: 7400, coolDownOffset: 5000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 125, coolDown: 7400, coolDownOffset: 5200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 130, coolDown: 7400, coolDownOffset: 5400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 135, coolDown: 7400, coolDownOffset: 5600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 140, coolDown: 7400, coolDownOffset: 5800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 145, coolDown: 7400, coolDownOffset: 6000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 150, coolDown: 7400, coolDownOffset: 6200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 155, coolDown: 7400, coolDownOffset: 6400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 160, coolDown: 7400, coolDownOffset: 6600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 165, coolDown: 7400, coolDownOffset: 6800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 170, coolDown: 7400, coolDownOffset: 7000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 175, coolDown: 7400, coolDownOffset: 7200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 180, coolDown: 7400, coolDownOffset: 7400),
                    new EntitiesNotExistsTransition(20, "Second Phase Charge", "White Demon of the Abyss")
                    ),
                new State("Second Phase Charge",
                    new Taunt("No! What have you done!"),
                    new TimedTransition(1500, "Second Phase")
                    ),
                new State("Second Phase",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -0, coolDown: 7400, coolDownOffset: 200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -5, coolDown: 7400, coolDownOffset: 400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -10, coolDown: 7400, coolDownOffset: 600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -15, coolDown: 7400, coolDownOffset: 800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -20, coolDown: 7400, coolDownOffset: 1000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -25, coolDown: 7400, coolDownOffset: 1200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -30, coolDown: 7400, coolDownOffset: 1400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -35, coolDown: 7400, coolDownOffset: 1600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -40, coolDown: 7400, coolDownOffset: 1800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -45, coolDown: 7400, coolDownOffset: 2000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -50, coolDown: 7400, coolDownOffset: 2200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -55, coolDown: 7400, coolDownOffset: 2400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -60, coolDown: 7400, coolDownOffset: 2600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -65, coolDown: 7400, coolDownOffset: 2800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -70, coolDown: 7400, coolDownOffset: 3000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -75, coolDown: 7400, coolDownOffset: 3200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -80, coolDown: 7400, coolDownOffset: 3400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -85, coolDown: 7400, coolDownOffset: 3600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -90, coolDown: 7400, coolDownOffset: 3800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -95, coolDown: 7400, coolDownOffset: 4000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -100, coolDown: 7400, coolDownOffset: 4200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -105, coolDown: 7400, coolDownOffset: 4400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -110, coolDown: 7400, coolDownOffset: 4600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -115, coolDown: 7400, coolDownOffset: 4800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -120, coolDown: 7400, coolDownOffset: 5000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -125, coolDown: 7400, coolDownOffset: 5200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -130, coolDown: 7400, coolDownOffset: 5400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -135, coolDown: 7400, coolDownOffset: 5600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -140, coolDown: 7400, coolDownOffset: 5800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -145, coolDown: 7400, coolDownOffset: 6000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -150, coolDown: 7400, coolDownOffset: 6200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -155, coolDown: 7400, coolDownOffset: 6400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -160, coolDown: 7400, coolDownOffset: 6600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -165, coolDown: 7400, coolDownOffset: 6800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -170, coolDown: 7400, coolDownOffset: 7000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -175, coolDown: 7400, coolDownOffset: 7200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -180, coolDown: 7400, coolDownOffset: 7400),
                    new Shoot(20, 3, shootAngle: 15, projectileIndex: 2, coolDown: 1500),
                    new Reproduce("Malphas Missile", coolDown: 1500),
                    new HpLessTransition(0.50, "Third Phase")
                    ),
                new State("Third Phase",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1500),
                    new Taunt("I will release a part of my power."),
                    new TimedTransition(1500, "Third Phase Start")
                    ),
                new State("Third Phase Start",
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 0, coolDown: 7400, coolDownOffset: 200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 5, coolDown: 7400, coolDownOffset: 400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 10, coolDown: 7400, coolDownOffset: 600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 15, coolDown: 7400, coolDownOffset: 800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 20, coolDown: 7400, coolDownOffset: 1000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 25, coolDown: 7400, coolDownOffset: 1200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 30, coolDown: 7400, coolDownOffset: 1400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 35, coolDown: 7400, coolDownOffset: 1600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 40, coolDown: 7400, coolDownOffset: 1800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 45, coolDown: 7400, coolDownOffset: 2000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 50, coolDown: 7400, coolDownOffset: 2200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 55, coolDown: 7400, coolDownOffset: 2400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 60, coolDown: 7400, coolDownOffset: 2600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 65, coolDown: 7400, coolDownOffset: 2800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 70, coolDown: 7400, coolDownOffset: 3000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 75, coolDown: 7400, coolDownOffset: 3200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 80, coolDown: 7400, coolDownOffset: 3400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 85, coolDown: 7400, coolDownOffset: 3600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 90, coolDown: 7400, coolDownOffset: 3800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 95, coolDown: 7400, coolDownOffset: 4000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 100, coolDown: 7400, coolDownOffset: 4200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 105, coolDown: 7400, coolDownOffset: 4400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 110, coolDown: 7400, coolDownOffset: 4600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 115, coolDown: 7400, coolDownOffset: 4800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 120, coolDown: 7400, coolDownOffset: 5000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 125, coolDown: 7400, coolDownOffset: 5200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 130, coolDown: 7400, coolDownOffset: 5400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 135, coolDown: 7400, coolDownOffset: 5600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 140, coolDown: 7400, coolDownOffset: 5800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 145, coolDown: 7400, coolDownOffset: 6000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 150, coolDown: 7400, coolDownOffset: 6200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 155, coolDown: 7400, coolDownOffset: 6400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 160, coolDown: 7400, coolDownOffset: 6600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 165, coolDown: 7400, coolDownOffset: 6800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 170, coolDown: 7400, coolDownOffset: 7000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 175, coolDown: 7400, coolDownOffset: 7200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: 180, coolDown: 7400, coolDownOffset: 7400),
                    new Shoot(20, 3, shootAngle: 15, projectileIndex: 4, coolDown: 1000),
                    new HpLessTransition(0.25, "Four Phase")
                    ),
                new State("Four Phase",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1500),
                    new Flash(0xFF0000, 0.2, 5),
                    new Taunt("NOOO! IM GONNA RELEASE ALL MY POWER!!"),
                    new TimedTransition(1500, "Four Phase Start")
                    ),
                new State("Four Phase Start",
                     new Shoot(30, 4, projectileIndex: 1, fixedAngle: -0, coolDown: 7400, coolDownOffset: 200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -5, coolDown: 7400, coolDownOffset: 400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -10, coolDown: 7400, coolDownOffset: 600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -15, coolDown: 7400, coolDownOffset: 800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -20, coolDown: 7400, coolDownOffset: 1000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -25, coolDown: 7400, coolDownOffset: 1200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -30, coolDown: 7400, coolDownOffset: 1400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -35, coolDown: 7400, coolDownOffset: 1600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -40, coolDown: 7400, coolDownOffset: 1800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -45, coolDown: 7400, coolDownOffset: 2000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -50, coolDown: 7400, coolDownOffset: 2200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -55, coolDown: 7400, coolDownOffset: 2400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -60, coolDown: 7400, coolDownOffset: 2600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -65, coolDown: 7400, coolDownOffset: 2800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -70, coolDown: 7400, coolDownOffset: 3000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -75, coolDown: 7400, coolDownOffset: 3200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -80, coolDown: 7400, coolDownOffset: 3400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -85, coolDown: 7400, coolDownOffset: 3600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -90, coolDown: 7400, coolDownOffset: 3800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -95, coolDown: 7400, coolDownOffset: 4000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -100, coolDown: 7400, coolDownOffset: 4200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -105, coolDown: 7400, coolDownOffset: 4400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -110, coolDown: 7400, coolDownOffset: 4600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -115, coolDown: 7400, coolDownOffset: 4800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -120, coolDown: 7400, coolDownOffset: 5000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -125, coolDown: 7400, coolDownOffset: 5200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -130, coolDown: 7400, coolDownOffset: 5400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -135, coolDown: 7400, coolDownOffset: 5600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -140, coolDown: 7400, coolDownOffset: 5800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -145, coolDown: 7400, coolDownOffset: 6000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -150, coolDown: 7400, coolDownOffset: 6200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -155, coolDown: 7400, coolDownOffset: 6400),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -160, coolDown: 7400, coolDownOffset: 6600),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -165, coolDown: 7400, coolDownOffset: 6800),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -170, coolDown: 7400, coolDownOffset: 7000),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -175, coolDown: 7400, coolDownOffset: 7200),
                    new Shoot(30, 4, projectileIndex: 1, fixedAngle: -180, coolDown: 7400, coolDownOffset: 7400),
                    new Shoot(20, 3, shootAngle: 15, projectileIndex: 4, coolDown: 1000),
                    new Wander(0.3),
                    new Reproduce("Malphas Missile", coolDown: 2000)
                    )
                ),
            new Threshold(0.01,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.01,
                new ItemLoot("Abyssal Sword", 0.005),
                new ItemLoot("Archdemon’s Remains", 0.005),
                new ItemLoot("Seal of the Underworld", 0.007),
                new ItemLoot("Demon’s Sigil", 0.009),
                new ItemLoot("Abyss of Demons Key", 0.01, 0, 0.03),
                new ItemLoot("Demon Blade", 0.005),
                new ItemLoot("Sword of Illumination", 0.001),
                new ItemLoot("Potion of Vitality", 1, 1),
                new ItemLoot("Potion of Defense", 1, 1),
                new ItemLoot("Potion of Vitality", 1, 1),
                new ItemLoot("Potion of Defense", 1, 1),
                new TierLoot(9, ItemType.Armor, 0.1),
                new TierLoot(10, ItemType.Armor, 0.08),
                new TierLoot(11, ItemType.Armor, 0.06),
                new TierLoot(9, ItemType.Weapon, 0.1),
                new TierLoot(10, ItemType.Weapon, 0.08),
                new TierLoot(11, ItemType.Weapon, 0.06),
                new TierLoot(3, ItemType.Ring, 0.1),
                new TierLoot(4, ItemType.Ring, 0.08),
                new TierLoot(5, ItemType.Ring, 0.04),
                new TierLoot(3, ItemType.Ability, 0.1),
                new TierLoot(4, ItemType.Ability, 0.08)
                )
            )
        .Init("Malphas Missile",
            new State(
                new Follow(6, 20, 0),
                new PlayerWithinTransition(0, "Explode"),
                new State("Explode",
                    new Flash(0xFFFFFF, 0.1, 5),
                    new TimedTransition(500, "Explode v2")
                    ),
                new State("Explode v2",
                    new Shoot(10, 8),
                    new Decay(0)
                    )
                //new Flash(0xFFFFFF, 0.2, 5),
                /*new State("explode",
                new Shoot(10, 8),
                new Decay(100)
                )*/
                )
            )
        .Init("Imp of the Abyss",
            new State(
                new Wander(0.2),
                new Shoot(8, 5, 10, coolDown: 3200)
                ),
            new ItemLoot("Magic Potion", 0.1),
            new ItemLoot("Health Potion", 0.1),
            new Threshold(0.5,
                new ItemLoot("Cloak of the Red Agent", 0.01),
                new ItemLoot("Felwasp Toxin", 0.01)
                ),
            new Threshold(0.01,
                new ItemLoot("Talisman of Looting", 0.02)
                )
            )
        .Init("Demon of the Abyss",
            new State(
                new Prioritize(
                    new Follow(4, 8, 5),
                    new Wander(0.25)
                    ),
                new Shoot(8, 3, shootAngle: 10, coolDown: 5000)
                ),
            new ItemLoot("Fire Bow", 0.05),
            new Threshold(0.5,
                new ItemLoot("Mithril Armor", 0.01)
                ),
            new Threshold(0.01,
                new ItemLoot("Talisman of Looting", 0.02)
                )
            )
        .Init("Demon Warrior of the Abyss",
            new State(
                new Prioritize(
                    new Follow(5, 8, 5),
                    new Wander(0.25)
                    ),
                new Shoot(8, 3, shootAngle: 10, coolDown: 3000)
                ),
            new Threshold(0.01,
                new ItemLoot("Talisman of Looting", 0.02)
                ),
            new ItemLoot("Fire Sword", 0.025),
            new ItemLoot("Steel Shield", 0.025)
            )
        .Init("Demon Mage of the Abyss",
            new State(
                new Prioritize(
                    new Follow(4, 8, 5),
                    new Wander(0.25)
                    ),
                new Shoot(8, 3, shootAngle: 10, coolDown: 3400)
                ),
            new ItemLoot("Fire Nova Spell", 0.02),
            new Threshold(0.1,
                new ItemLoot("Wand of Dark Magic", 0.01),
                new ItemLoot("Avenger Staff", 0.01),
                new ItemLoot("Robe of the Invoker", 0.01),
                new ItemLoot("Essence Tap Skull", 0.01),
                new ItemLoot("Demonhunter Trap", 0.01)
                )
            )
        .Init("Brute of the Abyss",
            new State(
                new Prioritize(
                    new Follow(7, 8, 1),
                    new Wander(0.25)
                    ),
                new Shoot(8, 3, shootAngle: 10, coolDown: 800)
                ),
            new ItemLoot("Magic Potion", 0.1),
            new Threshold(0.1,
                new ItemLoot("Obsidian Dagger", 0.02),
                new ItemLoot("Steel Helm", 0.02)
                )
            )
        .Init("Brute Warrior of the Abyss",
            new State(
                new Prioritize(
                    new Follow(4, 8, 1),
                    new Wander(0.25)
                    ),
                new Shoot(8, 3, shootAngle: 10, coolDown: 800)
                ),
            new ItemLoot("Spirit Salve Tome", 0.02),
            new Threshold(0.5,
                new ItemLoot("Glass Sword", 0.01),
                new ItemLoot("Ring of Greater Dexterity", 0.01),
                new ItemLoot("Magesteel Quiver", 0.01)
                )
            )
        .Init("White Demon of the Abyss",
            new State(
                new Prioritize(
                    new Wander(0.2)
                    ),
                new Shoot(10, 3, 20, predictive: 1, coolDown: 1000)
                )
            )
        ;
    }
}
