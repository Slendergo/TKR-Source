using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ LostHalls = () => Behav()

        #region PILLARS

        .Init("LH Pillar spawner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Waiting",
                    new TimedTransition(100000, "Main")
                    ),
                new State("Main",
                    new TimedRandomTransition(1, false, "Orange 1", "Green 1", "Black 1", "Red 1", "Purple 1")
                    ),
                // PURPLE
                new State("Purple 1",
                    new EntitiesNotExistsTransition(1, "Purple 2", "Marble Colossus Pillar Purple", "Marble Colossus Pillar Orange", "Marble Colossus Pillar Green", "Marble Colossus Pillar Black", "Marble Colossus Pillar Red"),
                    new TimedTransition(10, "Waiting")
                    ),
                new State("Purple 2",
                    new Spawn("Marble Colossus Pillar Purple", 1, 1, 1000),
                    new TimedTransition(1, "Waiting")
                    ),
                // ORANGE
                new State("Orange 1",
                    new EntitiesNotExistsTransition(1, "Orange 2", "Marble Colossus Pillar Purple", "Marble Colossus Pillar Orange", "Marble Colossus Pillar Green", "Marble Colossus Pillar Black", "Marble Colossus Pillar Red"),
                    new TimedTransition(10, "Waiting")
                    ),
                new State("Orange 2",
                    new Spawn("Marble Colossus Pillar Orange", 1, 1, 1000),
                    new TimedTransition(1, "Waiting")
                    ),
                // GREEN
                new State("Green 1",
                    new EntitiesNotExistsTransition(1, "Green 2", "Marble Colossus Pillar Purple", "Marble Colossus Pillar Orange", "Marble Colossus Pillar Green", "Marble Colossus Pillar Black", "Marble Colossus Pillar Red"),
                    new TimedTransition(10, "Waiting")
                    ),
                new State("Green 2",
                    new Spawn("Marble Colossus Pillar Green", 1, 1, 1000),
                    new TimedTransition(1, "Waiting")
                    ),
                // RED
                new State("Red 1",
                    new EntitiesNotExistsTransition(1, "Red 2", "Marble Colossus Pillar Purple", "Marble Colossus Pillar Orange", "Marble Colossus Pillar Green", "Marble Colossus Pillar Black", "Marble Colossus Pillar Red"),
                    new TimedTransition(10, "Waiting")
                    ),
                new State("Red 2",
                    new Spawn("Marble Colossus Pillar Red", 1, 1, 1000),
                    new TimedTransition(1, "Waiting")
                    ),
                //BLACK
                new State("Black 1",
                    new EntitiesNotExistsTransition(1, "Black 2", "Marble Colossus Pillar Purple", "Marble Colossus Pillar Orange", "Marble Colossus Pillar Green", "Marble Colossus Pillar Black", "Marble Colossus Pillar Red"),
                    new TimedTransition(10, "Waiting")
                    ),
                new State("Black 2",
                    new Spawn("Marble Colossus Pillar Black", 1, 1, 1000),
                    new TimedTransition(1, "Waiting")
                    )
                )
            )
        .Init("Marble Colossus Pillar Red",
            new State(
                new ScaleHP(3000, 0, true, 10, 1),
                new Shoot(15, 8, projectileIndex: 0, predictive: 1, coolDownOffset: 2000, coolDown: 2000)
                )
            )
        .Init("Marble Colossus Pillar Purple",
            new State(
                new ScaleHP(3000, 0, true, 10, 1),
                new Shoot(15, 8, projectileIndex: 0, predictive: 1, coolDownOffset: 2000, coolDown: 2000)
                )
            )
        .Init("Marble Colossus Pillar Black",
            new State(
                new ScaleHP(3000, 0, true, 10, 1),
                new Shoot(15, 8, projectileIndex: 0, predictive: 1, coolDownOffset: 2000, coolDown: 2000)
                )
            )
        .Init("Marble Colossus Pillar Orange",
            new State(
                new ScaleHP(3000, 0, true, 10, 1),
                new Shoot(15, 8, projectileIndex: 0, predictive: 1, coolDownOffset: 2000, coolDown: 2000)
                )
            )
        .Init("Marble Colossus Pillar Green",
            new State(
                new ScaleHP(3000, 0, true, 10, 1),
                new Shoot(15, 8, projectileIndex: 0, predictive: 1, coolDownOffset: 3000, coolDown: 2000)
                )
            )

        #endregion PILLARS

        #region Rocks

        .Init("LH Colossus Rock 1",
            new State(
                new State("Waiting"),
                new State("Move",
                    new MoveTo2(2.5f, 2.5f, 1),
                    new TimedTransition(800, "Charging")
                    ),
                new State("Move 2",
                    new MoveTo2(-2.5f, -2.5f, 1),
                    new TimedTransition(800, "Charging")
                    ),
                new State("Charging",
                    new Flash(0x353535, 0.25, 12),
                    new TimedTransition(200, "Explote")
                    ),
                new State("Explote",
                    new Shoot(30, 8, projectileIndex: 0, coolDownOffset: 0, coolDown: 103000),
                    new Suicide()
                    )
                )
            )
        .Init("LH Colossus Rock 2",
            new State(
                new State("Move",
                    new MoveTo2(-2.5f, 0, 1),
                    new TimedTransition(800, "Charging")
                    ),
                new State("Move 2",
                    new MoveTo2(2.5f, 0, 1),
                    new TimedTransition(800, "Charging")
                    ),
                new State("Charging",
                    new Flash(0x353535, 0.25, 12),
                    new TimedTransition(200, "Explote")
                    ),
                new State("Explote",
                    new Shoot(30, 8, projectileIndex: 0, coolDownOffset: 0, coolDown: 103000),
                    new Suicide()
                    )
                )
            )
        .Init("LH Colossus Rock 3",
            new State(
                new State("Move",
                    new MoveTo2(2.5f, -2.5f, 1),
                    new TimedTransition(800, "Charging")
                    ),
                new State("Move 2",
                    new MoveTo2(-2.5f, 2.5f, 1),
                    new TimedTransition(800, "Charging")
                    ),
                new State("Charging",
                    new Flash(0x353535, 0.25, 12),
                    new TimedTransition(200, "Explote")
                    ),
                new State("Explote",
                    new Shoot(30, 8, projectileIndex: 0, coolDownOffset: 0, coolDown: 103000),
                    new Suicide()
                    )
                )
            )
        .Init("LH Colossus Rock 4",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Shoot And Move 1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Orbit(1, 1, 30, "Marble Colossus", orbitClockwise: true),
                    new Shoot(30, 2, 180, projectileIndex: 0, rotateAngle: 20, coolDown: 200),
                    new EntitiesNotExistsTransition(30, "Stopped", "Marble Colossus")
                    ),
                new State("Stopped",
                    new Suicide()
                    )
                )
            )
        .Init("LH Colossus Rock 5",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Shoot And Move 1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Orbit(1, 1, 30, "Marble Colossus", orbitClockwise: true),
                    new Shoot(30, 2, 180, projectileIndex: 0, rotateAngle: 20, coolDown: 200),
                    new EntitiesNotExistsTransition(30, "Stopped", "Marble Colossus")
                    ),
                new State("Stopped",
                    new Suicide()
                    )
                )
            )
        .Init("LH Colossus Rock 6",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Shoot And Move 1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Orbit(1, 1, 30, "Marble Colossus", orbitClockwise: true),
                    new Shoot(30, 2, 180, projectileIndex: 0, rotateAngle: 20, coolDown: 200),
                    new EntitiesNotExistsTransition(30, "Stopped", "Marble Colossus")
                    ),
                new State("Stopped",
                    new Suicide()
                    )
                )
            )
        .Init("LH Colossus Rock 7",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Shoot And Move",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Orbit(1, 9, 30, "Marble Colossus ORBIT", orbitClockwise: true),
                    new Shoot(30, 2, 180, projectileIndex: 0, rotateAngle: 20, coolDown: 200),
                    new EntitiesNotExistsTransition(30, "Stopped", "Marble Colossus")
                    ),
                new State("Shoot And Move Closer",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Orbit(1, 1, 30, "Marble Colossus", orbitClockwise: true),
                    new Shoot(30, 2, 180, projectileIndex: 0, rotateAngle: 20, coolDown: 200),
                    new EntitiesNotExistsTransition(30, "Stopped", "Marble Colossus")
                    ),
                new State("Stopped",
                    new Suicide()
                    )
                )
            )
        .Init("LH Colossus Rock 8",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Shoot And Move",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Orbit(1, 9, 30, "Marble Colossus ORBIT", orbitClockwise: true),
                    new Shoot(30, 2, 180, projectileIndex: 0, rotateAngle: 20, coolDown: 200),
                    new EntitiesNotExistsTransition(30, "Stopped", "Marble Colossus")
                    ),
                new State("Shoot And Move Closer",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Orbit(1, 1, 30, "Marble Colossus", orbitClockwise: true),
                    new Shoot(30, 2, 180, projectileIndex: 0, rotateAngle: 20, coolDown: 200),
                    new EntitiesNotExistsTransition(30, "Stopped", "Marble Colossus")
                    ),
                new State("Stopped",
                    new Suicide()
                    )
                )
            )
        .Init("LH Colossus Rock 9",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Shoot And Move",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Orbit(1, 9, 30, "Marble Colossus ORBIT", orbitClockwise: true),
                    new Shoot(30, 2, 180, projectileIndex: 0, rotateAngle: 20, coolDown: 200),
                    new EntitiesNotExistsTransition(30, "Stopped", "Marble Colossus")
                    ),
                new State("Shoot And Move Closer",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Orbit(1, 1, 30, "Marble Colossus", orbitClockwise: true),
                    new Shoot(30, 2, 180, projectileIndex: 0, rotateAngle: 20, coolDown: 200),
                    new EntitiesNotExistsTransition(30, "Stopped", "Marble Colossus")
                    ),
                new State("Stopped",
                    new Suicide()
                    )
                )
            )

        #endregion Rocks

        #region Marble Colossus

        .Init("Marble Colossus ORBIT",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                )
            )
        .Init("Marble Colossus",
            new State(
                new ScaleHP2(20),
                new State("Waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(10, "Changing Skin")
                    ),
                new State("Changing Skin",
                    //new PlayerTextTransition("Phase 12.1", "skip", 30),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new SetAltTexture(0, 4, 1500),
                    new Spawn("Marble Colossus ORBIT", maxChildren: 1),
                    new TimedTransition(11000, "Phase 1")

                    ),
                new State("Phase 1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Taunt("Look upon my mighty bulwark."),
                    new Shoot(30, 12, projectileIndex: 0, fixedAngle: 0, coolDownOffset: 0, coolDown: 103000, seeInvis: true),
                    new Shoot(30, 12, projectileIndex: 0, fixedAngle: 15, coolDownOffset: 3000, coolDown: 106000, seeInvis: true),
                    new Shoot(30, 12, projectileIndex: 0, fixedAngle: 0, coolDownOffset: 6000, coolDown: 109000, seeInvis: true),
                    new TimedTransition(9000, "Phase 2")
                    ),
                new State("Phase 2",
                    new Follow(0.6, acquireRange: 30, range: 0),
                    new Taunt("You doubt my strength? FATUUS! I will destroy you!"),
                    new OrderOnce(30, "LH Pillar spawner", "Main"),
                    new Shoot(30, 8, projectileIndex: 2, coolDown: 4000, seeInvis: true),
                    new Shoot(30, 5, projectileIndex: 1, predictive: 0, shootAngle: 10, coolDown: 1500, seeInvis: true),
                    new HpLessTransition(0.90, "Phase 3")
                    ),
                new State("Phase 3",
                    new ReturnToSpawn(1.3, 0),
                    new Taunt("I cast you off!"),
                    new TimedTransition(1000, "Phase 3.1")
                    ),
                new State("Phase 3.1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Spawn("LH Colossus Rock 1", 1, 1, 1000000),
                    new OrderOnce(1, "LH Colossus Rock 1", "Move"),
                    new Spawn("LH Colossus Rock 2", 1, 1, 1000000),
                    new OrderOnce(1, "LH Colossus Rock 2", "Move"),
                    new Spawn("LH Colossus Rock 3", 1, 1, 1000000),
                    new OrderOnce(1, "LH Colossus Rock 3", "Move"),
                    new TimedTransition(1150, "Phase 3.2")
                    ),
                new State("Phase 3.2",
                    new Spawn("LH Colossus Rock 1", 1, 1, 1000000),
                    new OrderOnce(1, "LH Colossus Rock 1", "Move 2"),
                    new Spawn("LH Colossus Rock 2", 1, 1, 1000000),
                    new OrderOnce(1, "LH Colossus Rock 2", "Move 2"),
                    new Spawn("LH Colossus Rock 3", 1, 1, 1000000),
                    new OrderOnce(1, "LH Colossus Rock 3", "Move 2"),
                    new TimedTransition(1150, "Phase 3.3"),
                    new HpLessTransition(.85, "Phase 4")
                    ),
                new State("Phase 3.3",
                    new Spawn("LH Colossus Rock 1", 1, 1, 1000000),
                    new OrderOnce(1, "LH Colossus Rock 1", "Move"),
                    new Spawn("LH Colossus Rock 2", 1, 1, 1000000),
                    new OrderOnce(1, "LH Colossus Rock 2", "Move"),
                    new Spawn("LH Colossus Rock 3", 1, 1, 1000000),
                    new OrderOnce(1, "LH Colossus Rock 3", "Move"),
                    new TimedTransition(1150, "Phase 3.2"),
                    new HpLessTransition(.85, "Phase 4")
                    ),
                new State("Phase 4",
                    new Taunt("Your fervent attacks are no match for my strength! BEGONE!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                    new Shoot(radius: 20, count: 16, shootAngle: 22.5, projectileIndex: 4, coolDown: 3000, coolDownOffset: 200, seeInvis: true),
                    new Shoot(20, 3, shootAngle: 15, projectileIndex: 5, coolDown: 4000, coolDownOffset: 300, seeInvis: true),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 0, coolDown: 2600, coolDownOffset: 200),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 2, coolDown: 2600, coolDownOffset: 400),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 4, coolDown: 2600, coolDownOffset: 600),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 6, coolDown: 2600, coolDownOffset: 800),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 8, coolDown: 2600, coolDownOffset: 1000),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 10, coolDown: 2600, coolDownOffset: 1200),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 12, coolDown: 2600, coolDownOffset: 1400),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 14, coolDown: 2600, coolDownOffset: 1600),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 16, coolDown: 2600, coolDownOffset: 1800),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 18, coolDown: 2600, coolDownOffset: 2000),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 20, coolDown: 2600, coolDownOffset: 2200),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 22, coolDown: 2600, coolDownOffset: 2400),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 24, coolDown: 2600, coolDownOffset: 2600),                  
                    new HpLessTransition(0.7, "Phase 5")
                    ),
                new State("Phase 5",
                    new Taunt("INSOLENTIA! Darkness will consume you!"),
                    new OrderOnce(30, "LH Pillar spawner", "Main"),
                    new Prioritize(
                        new Orbit(1, 4, 25, target: "Marble Colossus ORBIT", orbitClockwise: true)),
                    new Shoot(30, 2, shootAngle: 180, projectileIndex: 7, rotateAngle: 20, coolDown: 200, seeInvis: true),
                    new Shoot(30, 8, shootAngle: 45, projectileIndex: 6, coolDown: 2000, seeInvis: true),
                    new HpLessTransition(0.65, "Phase 6")
                    ),
                new State("Phase 6",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new ReturnToSpawn(1.3, 10),
                    new Taunt("Brace for your demise!"),
                    new Spawn("LH Colossus Rock 1", 1, 1, 1000000),
                    new OrderOnce(1, "LH Colossus Rock 1", "Move"),
                    new Spawn("LH Colossus Rock 2", 1, 1, 1000000),
                    new OrderOnce(1, "LH Colossus Rock 2", "Move"),
                    new Spawn("LH Colossus Rock 3", 1, 1, 1000000),
                    new OrderOnce(1, "LH Colossus Rock 3", "Move"),
                    new Shoot(30, 2, shootAngle: 180, projectileIndex: 8, rotateAngle: 20, coolDown: 200, seeInvis: true),
                    new Shoot(30, 8, shootAngle: 45, projectileIndex: 6, coolDown: 2000, seeInvis: true),
                    new HpLessTransition(0.6, "Phase 7")
                    ),
                new State("Phase 7",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 25000),
                    new Taunt("Futility!"),
                    new Grenade(radius: 2, damage: 0, range: 30, coolDown: 800, effect: ConditionEffectIndex.Quiet, effectDuration: 1000, color: 0xffffff),
                    new Shoot(30, 39, shootAngle: 9.230, projectileIndex: 9, defaultAngle: 0, fixedAngle: 0, coolDown: 3000, coolDownOffset: 1000, seeInvis: true),
                    new Shoot(30, 39, shootAngle: 9.230, projectileIndex: 10, defaultAngle: 10, fixedAngle: 5, coolDown: 3000, seeInvis: true),
                    new TimedTransition(25000, "Phase 8")
                    ),
                new State("Phase 8",
                    new Taunt("Call of voice, for naught. Plea of mercy, for naught. None may enter this chamber and live!"),
                    new Follow(0.6, 30, 0),
                    new Spawn("LH Colossus Rock 4", maxChildren: 1),
                    new Spawn("LH Colossus Rock 5", maxChildren: 1),
                    new OrderOnce(1, "LH Colossus Rock 4", "Shoot And Move 1"),
                    new OrderOnce(1, "LH Colossus Rock 5", "Shoot And Move 1"),
                    new Shoot(30, 18, shootAngle: 20, projectileIndex: 11, coolDown: 2000, coolDownOffset: 200, seeInvis: true),
                    new HpLessTransition(0.57, "Phase 8.1")
                    ),
                new State("Phase 8.1",
                    new Taunt("SANGUIS! OSSE! CARO! Feel it rend from your body!"),
                    new Follow(0.6, 30, 0),
                    new Shoot(20, 3, shootAngle: 15, projectileIndex: 12, coolDown: 3000, seeInvis: true),
                    new Shoot(30, 18, shootAngle: 20, projectileIndex: 11, coolDown: 2000, coolDownOffset: 200, seeInvis: true),
                    new HpLessTransition(0.55, "Phase 8.2")
                    ),
                new State("Phase 8.2",
                    new Taunt("PESTIS! The darkness consumes!!"),
                    new OrderOnce(20, "LH Colossus Rock 4", "Stopped"),
                    new OrderOnce(20, "LH Colossus Rock 5", "Stopped"),
                    new Follow(0.6, 30, 0),
                    new Shoot(20, 3, shootAngle: 15, projectileIndex: 12, coolDown: 3000, seeInvis: true),
                    new Shoot(30, 18, shootAngle: 20, projectileIndex: 11, coolDown: 2000, coolDownOffset: 200, seeInvis: true),
                    new HpLessTransition(0.53, "Phase 9")
                    ),
                new State("Phase 9",
                    new OrderOnce(30, "LH Pillar spawner", "Main"),
                    new Taunt("Enough! Pillars, serve your purpose"),
                    new Orbit(1, 6, 30, "Marble Colossus ORBIT", orbitClockwise: true),
                    new Shoot(20, 4, shootAngle: 15, projectileIndex: 13, coolDown: 1000, seeInvis: true),
                    new HpLessTransition(0.50, "Phase 10")
                    ),
                new State("Phase 10",
                    new Taunt("Perish, blights upon this realm!"),
                    new ReturnToSpawn(3, 10),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 25000),
                    new Spawn("LH Colossus Rock 7", maxChildren: 1),
                    new OrderOnce(1, "LH Colossus Rock 7", "Shoot And Move"),
                    new Spawn("LH Colossus Rock 8", maxChildren: 1),
                    new OrderOnce(1, "LH Colossus Rock 8", "Shoot And Move"),
                    new Spawn("LH Colossus Rock 9", maxChildren: 1),
                    new OrderOnce(1, "LH Colossus Rock 9", "Shoot And Move"),
                    new Shoot(30, 39, shootAngle: 9.230, projectileIndex: 10, defaultAngle: 10, fixedAngle: 5, coolDown: 3000, seeInvis: true),
                    new Shoot(10, 3, shootAngle: 15, projectileIndex: 12, coolDown: 2000, seeInvis: true),
                    new TimedTransition(25000, "Phase 11")
                    ),
                new State("Phase 11",
                    new OrderOnce(30, "LH Pillar spawner", "Main"),
                    new Taunt("You have seen your last glimpse of sunlight!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 25000),
                    new OrderOnce(20, "LH Colossus Rock 7", "Shoot And Move Closer"),
                    new OrderOnce(20, "LH Colossus Rock 8", "Shoot And Move Closer"),
                    new OrderOnce(20, "LH Colossus Rock 9", "Shoot And Move Closer"),
                    new Shoot(30, 39, shootAngle: 9.230, projectileIndex: 10, defaultAngle: 10, fixedAngle: 5, coolDown: 3000, seeInvis: true),
                    new Shoot(10, 3, shootAngle: 15, projectileIndex: 12, coolDown: 2000, seeInvis: true),
                    new Shoot(20, 3, shootAngle: 15, projectileIndex: 5, coolDown: 4000, coolDownOffset: 300, seeInvis: true),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 0, coolDown: 2600, coolDownOffset: 200),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 2, coolDown: 2600, coolDownOffset: 400),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 4, coolDown: 2600, coolDownOffset: 600),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 6, coolDown: 2600, coolDownOffset: 800),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 8, coolDown: 2600, coolDownOffset: 1000),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 10, coolDown: 2600, coolDownOffset: 1200),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 12, coolDown: 2600, coolDownOffset: 1400),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 14, coolDown: 2600, coolDownOffset: 1600),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 16, coolDown: 2600, coolDownOffset: 1800),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 18, coolDown: 2600, coolDownOffset: 2000),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 20, coolDown: 2600, coolDownOffset: 2200),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 22, coolDown: 2600, coolDownOffset: 2400),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 24, coolDown: 2600, coolDownOffset: 2600),


                    new TimedTransition(25000, "Phase 12")
                    ),
                new State("Phase 12",
                    new Taunt("PATI! The prohibited arts allow untold power!"),
                    new OrderOnce(5, "LH Colossus Rock 7", "Stopped"),
                    new OrderOnce(5, "LH Colossus Rock 8", "Stopped"),
                    new OrderOnce(5, "LH Colossus Rock 9", "Stopped"),
                    new Charge(3, 30, 3100),
                    new TimedTransition(3100, "Phase 12.1"),
                    new HpLessTransition(0.4, "Phase 13")
                    ),
                new State("Phase 12.1",
                    new Shoot(30, 16, shootAngle: 22.5, fixedAngle: 0, angleOffset: 30, projectileIndex: 14, coolDown: 4000, coolDownOffset: 500, seeInvis: true),
                    new Shoot(30, 16, shootAngle: 22.5, fixedAngle: 0, angleOffset: 30, projectileIndex: 14, coolDown: 4000, coolDownOffset: 600, seeInvis: true),
                    new Shoot(30, 16, shootAngle: 22.5, fixedAngle: 0, angleOffset: 30, projectileIndex: 14, coolDown: 4000, coolDownOffset: 700, seeInvis: true),
                    new Shoot(30, 16, shootAngle: 22.5, fixedAngle: 0, angleOffset: 30, projectileIndex: 14, coolDown: 4000, coolDownOffset: 800, seeInvis: true),
                    new Shoot(30, 16, shootAngle: 22.5, fixedAngle: 0, angleOffset: 30, projectileIndex: 14, coolDown: 4000, coolDownOffset: 900, seeInvis: true),
                    new Shoot(30, 16, shootAngle: 22.5, fixedAngle: 0, angleOffset: 30, projectileIndex: 14, coolDown: 4000, coolDownOffset: 1000, seeInvis: true),
                    new Shoot(30, 16, shootAngle: 22.5, fixedAngle: 0, angleOffset: 30, projectileIndex: 14, coolDown: 4000, coolDownOffset: 1100, seeInvis: true),
                    new Shoot(30, 16, shootAngle: 22.5, fixedAngle: 0, angleOffset: 30, projectileIndex: 14, coolDown: 4000, coolDownOffset: 1200, seeInvis: true),
                    new TimedTransition(3000, "Phase 12.2"),
                    new HpLessTransition(0.4, "Phase 13")
                    ),
                new State("Phase 12.2",
                    new Charge(3, 30, 3100),
                    new TimedTransition(3100, "Phase 12.1"),
                    new HpLessTransition(0.4, "Phase 13")
                    ),
                new State("Phase 13",
                    new ReturnToSpawn(1.3, 0),
                    new Taunt("It is my duty to protect these catacombs! You dare threaten my purpose?"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                    new TimedTransition(1000, "Phase 13.1")
                    ),
                new State("Phase 13.1",                    // 0 DERECHA
                                                           // 180 IZQUIERDA
                                                           // -90 ARRIBA
                                                           // 90 ABAJO
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                    new Shoot(10, 3, shootAngle: 15, projectileIndex: 12, coolDown: 2000, seeInvis: true),
                    // ABAJO
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 90, defaultAngle: 90, fixedAngle: 90, coolDown: 4500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 100, defaultAngle: 100, fixedAngle: 100, coolDown: 4500, coolDownOffset: 500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 80, defaultAngle: 80, fixedAngle: 80, coolDown: 4500, coolDownOffset: 500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 110, defaultAngle: 110, fixedAngle: 110, coolDown: 4500, coolDownOffset: 1000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 70, defaultAngle: 70, fixedAngle: 70, coolDown: 4500, coolDownOffset: 1000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 120, defaultAngle: 120, fixedAngle: 120, coolDown: 4500, coolDownOffset: 1500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 60, defaultAngle: 60, fixedAngle: 60, coolDown: 4500, coolDownOffset: 1500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 130, defaultAngle: 130, fixedAngle: 130, coolDown: 4500, coolDownOffset: 2000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 50, defaultAngle: 50, fixedAngle: 50, coolDown: 4500, coolDownOffset: 2000, seeInvis: true),
                    //new Shoot(30, 1, projectileIndex: 15, shootAngle: 135, defaultAngle: 135, fixedAngle: 135, coolDown: 4500, coolDownOffset: 2500),
                    //IZQUIERDA
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 180, defaultAngle: 180, fixedAngle: 180, coolDown: 4500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 190, defaultAngle: 190, fixedAngle: 190, coolDown: 4500, coolDownOffset: 500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 170, defaultAngle: 170, fixedAngle: 170, coolDown: 4500, coolDownOffset: 500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 200, defaultAngle: 200, fixedAngle: 200, coolDown: 4500, coolDownOffset: 1000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 160, defaultAngle: 160, fixedAngle: 160, coolDown: 4500, coolDownOffset: 1000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 210, defaultAngle: 210, fixedAngle: 210, coolDown: 4500, coolDownOffset: 1500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 150, defaultAngle: 150, fixedAngle: 150, coolDown: 4500, coolDownOffset: 1500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 220, defaultAngle: 220, fixedAngle: 220, coolDown: 4500, coolDownOffset: 2000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 140, defaultAngle: 140, fixedAngle: 140, coolDown: 4500, coolDownOffset: 2000, seeInvis: true),
                    //new Shoot(30, 1, projectileIndex: 15, shootAngle: 225, defaultAngle: 225, fixedAngle: 225, coolDown: 4500, coolDownOffset: 2500),
                    //DERECHA
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 0, defaultAngle: 0, fixedAngle: 0, coolDown: 4500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 10, defaultAngle: 10, fixedAngle: 10, coolDown: 4500, coolDownOffset: 500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -10, defaultAngle: -10, fixedAngle: -10, coolDown: 4500, coolDownOffset: 500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 20, defaultAngle: 20, fixedAngle: 20, coolDown: 4500, coolDownOffset: 1000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -20, defaultAngle: -20, fixedAngle: -20, coolDown: 4500, coolDownOffset: 1000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 30, defaultAngle: 30, fixedAngle: 30, coolDown: 4500, coolDownOffset: 1500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -30, defaultAngle: -30, fixedAngle: -30, coolDown: 4500, coolDownOffset: 1500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 40, defaultAngle: 40, fixedAngle: 40, coolDown: 4500, coolDownOffset: 2000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -40, defaultAngle: -40, fixedAngle: -40, coolDown: 4500, coolDownOffset: 2000, seeInvis: true),
                    //ABAJO
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -90, defaultAngle: -90, fixedAngle: -90, coolDown: 4500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -100, defaultAngle: -100, fixedAngle: -100, coolDown: 4500, coolDownOffset: 500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -80, defaultAngle: -80, fixedAngle: -80, coolDown: 4500, coolDownOffset: 500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -110, defaultAngle: -110, fixedAngle: -110, coolDown: 4500, coolDownOffset: 1000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -70, defaultAngle: -70, fixedAngle: -70, coolDown: 4500, coolDownOffset: 1000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -120, defaultAngle: -120, fixedAngle: -120, coolDown: 4500, coolDownOffset: 1500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -60, defaultAngle: -60, fixedAngle: -60, coolDown: 4500, coolDownOffset: 1500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -130, defaultAngle: -130, fixedAngle: -130, coolDown: 4500, coolDownOffset: 2000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -50, defaultAngle: -50, fixedAngle: -50, coolDown: 4500, coolDownOffset: 2000, seeInvis: true),

                    // ARRIBA DERECHA
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -45, defaultAngle: -45, fixedAngle: -45, coolDown: 4500, coolDownOffset: 2500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -55, defaultAngle: -55, fixedAngle: -55, coolDown: 4500, coolDownOffset: 3000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -35, defaultAngle: -35, fixedAngle: -35, coolDown: 4500, coolDownOffset: 3000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -65, defaultAngle: -65, fixedAngle: -65, coolDown: 4500, coolDownOffset: 3500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -25, defaultAngle: -25, fixedAngle: -25, coolDown: 4500, coolDownOffset: 3500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -75, defaultAngle: -75, fixedAngle: -75, coolDown: 4500, coolDownOffset: 4000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -15, defaultAngle: -15, fixedAngle: -15, coolDown: 4500, coolDownOffset: 4000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -10, defaultAngle: -10, fixedAngle: -10, coolDown: 4500, coolDownOffset: 4250, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: -80, defaultAngle: -80, fixedAngle: -80, coolDown: 4500, coolDownOffset: 4250, seeInvis: true),
                    //ABAJO DERECHA
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 45, defaultAngle: 45, fixedAngle: 45, coolDown: 4500, coolDownOffset: 2500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 55, defaultAngle: 55, fixedAngle: 55, coolDown: 4500, coolDownOffset: 3000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 35, defaultAngle: 35, fixedAngle: 35, coolDown: 4500, coolDownOffset: 3000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 65, defaultAngle: 65, fixedAngle: 65, coolDown: 4500, coolDownOffset: 3500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 25, defaultAngle: 25, fixedAngle: 25, coolDown: 4500, coolDownOffset: 3500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 75, defaultAngle: 75, fixedAngle: 75, coolDown: 4500, coolDownOffset: 4000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 15, defaultAngle: 15, fixedAngle: 15, coolDown: 4500, coolDownOffset: 4000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 10, defaultAngle: 10, fixedAngle: 10, coolDown: 4500, coolDownOffset: 4250, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 80, defaultAngle: 80, fixedAngle: 80, coolDown: 4500, coolDownOffset: 4250, seeInvis: true),
                    //ARRIBA IZQUIERDA
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 225, defaultAngle: 225, fixedAngle: 225, coolDown: 4500, coolDownOffset: 2500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 235, defaultAngle: 235, fixedAngle: 235, coolDown: 4500, coolDownOffset: 3000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 215, defaultAngle: 215, fixedAngle: 215, coolDown: 4500, coolDownOffset: 3000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 245, defaultAngle: 245, fixedAngle: 245, coolDown: 4500, coolDownOffset: 3500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 205, defaultAngle: 205, fixedAngle: 205, coolDown: 4500, coolDownOffset: 3500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 255, defaultAngle: 255, fixedAngle: 255, coolDown: 4500, coolDownOffset: 4000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 195, defaultAngle: 195, fixedAngle: 195, coolDown: 4500, coolDownOffset: 4000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 200, defaultAngle: 200, fixedAngle: 200, coolDown: 4500, coolDownOffset: 4250, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 260, defaultAngle: 260, fixedAngle: 260, coolDown: 4500, coolDownOffset: 4250, seeInvis: true),
                    //ABAJO IZQUIERDA
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 135, defaultAngle: 135, fixedAngle: 135, coolDown: 4500, coolDownOffset: 2500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 145, defaultAngle: 145, fixedAngle: 145, coolDown: 4500, coolDownOffset: 3000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 125, defaultAngle: 125, fixedAngle: 125, coolDown: 4500, coolDownOffset: 3000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 155, defaultAngle: 155, fixedAngle: 155, coolDown: 4500, coolDownOffset: 3500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 115, defaultAngle: 115, fixedAngle: 115, coolDown: 4500, coolDownOffset: 3500, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 165, defaultAngle: 165, fixedAngle: 165, coolDown: 4500, coolDownOffset: 4000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 105, defaultAngle: 105, fixedAngle: 105, coolDown: 4500, coolDownOffset: 4000, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 170, defaultAngle: 170, fixedAngle: 170, coolDown: 4500, coolDownOffset: 4250, seeInvis: true),
                    new Shoot(30, 1, projectileIndex: 15, shootAngle: 100, defaultAngle: 100, fixedAngle: 100, coolDown: 4500, coolDownOffset: 4250, seeInvis: true),
                    new HpLessTransition(0.35, "Phase 14")
                    ),
                new State("Phase 14",
                    new Taunt("Magia saps from my body… My immense physical strength STILL REMAINS!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 4000),
                    new Order(1, "LH Colossus Rock 3", "Move"),
                    new Spawn("LH Colossus Rock 1", maxChildren: 100, 0.01, 4000, true),
                    new Order(1, "LH Colossus Rock 1", "Move 2"),
                    new Spawn("LH Colossus Rock 2", maxChildren: 100, 0.01, 4000, true),
                    new Order(1, "LH Colossus Rock 2", "Move 2"),
                    new Spawn("LH Colossus Rock 3", maxChildren: 100, 0.01, 4000, true),
                    new Order(1, "LH Colossus Rock 3", "Move 2"),
                    new Grenade(radius: 2, damage: 0, range: 30, coolDown: 1000, effect: ConditionEffectIndex.Quiet, effectDuration: 1000, color: 0xffffff),
                    new Shoot(10, 3, shootAngle: 15, projectileIndex: 12, coolDown: 2000, seeInvis: true),
                    new HpLessTransition(0.30, "Phase 14.1")
                    ),
                new State("Phase 14.1",
                    new Taunt("Fear the halls!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                    new Grenade(radius: 2, damage: 0, range: 30, coolDown: 1000, effect: ConditionEffectIndex.Quiet, effectDuration: 1000, color: 0xffffff),
                    new Spawn("LH Colossus Rock 1", maxChildren: 100, 0.00, 3000),
                    new Order(1, "LH Colossus Rock 1", "Move"),
                    new Spawn("LH Colossus Rock 2", maxChildren: 100, 0.00, 3000),
                    new Order(1, "LH Colossus Rock 2", "Move"),
                    new Spawn("LH Colossus Rock 3", maxChildren: 100, 0.00, 3000),
                    new Order(1, "LH Colossus Rock 3", "Move"),
                    new Spawn("LH Colossus Rock 7", maxChildren: 1),
                    new OrderOnce(10, "LH Colossus Rock 7", "Shoot And Move Closer"),
                    new Spawn("LH Colossus Rock 8", maxChildren: 1, initialSpawn: 1),
                    new OrderOnce(10, "LH Colossus Rock 8", "Shoot And Move Closer"),
                    new Spawn("LH Colossus Rock 9", maxChildren: 1, initialSpawn: 1),
                    new OrderOnce(10, "LH Colossus Rock 9", "Shoot And Move Closer"),
                    new Shoot(10, 3, shootAngle: 15, projectileIndex: 12, coolDown: 2000, seeInvis: true),
                    new HpLessTransition(0.25, "Phase 14.2")
                    ),
                new State("Phase 14.2",
                    new Taunt("I… I am… Dying…"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                    new Grenade(radius: 2, damage: 0, range: 30, coolDown: 1000, effect: ConditionEffectIndex.Quiet, effectDuration: 1000, color: 0xffffff),
                    new Spawn("LH Colossus Rock 1", maxChildren: 100, 0.00, 3000),
                    new Order(1, "LH Colossus Rock 1", "Move"),
                    new Spawn("LH Colossus Rock 2", maxChildren: 100, 0.00, 3000),
                    new Order(1, "LH Colossus Rock 2", "Move"),
                    new Spawn("LH Colossus Rock 3", maxChildren: 100, 0.00, 3000),
                    new Order(1, "LH Colossus Rock 3", "Move"),
                    new Shoot(10, 3, shootAngle: 15, projectileIndex: 12, coolDown: 2000, seeInvis: true),
                    new HpLessTransition(0.20, "Phase 15")
                    ),
                new State("Phase 15",
                    new OrderOnce(30, "LH Pillar spawner", "Main"),
                    new Taunt("You… YOU WILL COME WITH ME!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 30000),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 0, coolDown: 2600, coolDownOffset: 200),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 2, coolDown: 2600, coolDownOffset: 400),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 4, coolDown: 2600, coolDownOffset: 600),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 6, coolDown: 2600, coolDownOffset: 800),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 8, coolDown: 2600, coolDownOffset: 1000),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 10, coolDown: 2600, coolDownOffset: 1200),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 12, coolDown: 2600, coolDownOffset: 1400),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 14, coolDown: 2600, coolDownOffset: 1600),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 16, coolDown: 2600, coolDownOffset: 1800),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 18, coolDown: 2600, coolDownOffset: 2000),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 20, coolDown: 2600, coolDownOffset: 2200),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 22, coolDown: 2600, coolDownOffset: 2400),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 24, coolDown: 2600, coolDownOffset: 2600),
                    
                    new TimedTransition(30000, "Phase 15.1")
                    ),
                new State("Phase 15.1",
                    new Taunt("I CANNOT FAIL MY PURPOSE!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 30000),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: 0, coolDown: 2600, coolDownOffset: 200),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: -2, coolDown: 2600, coolDownOffset: 400),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: -4, coolDown: 2600, coolDownOffset: 600),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: -6, coolDown: 2600, coolDownOffset: 800),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: -8, coolDown: 2600, coolDownOffset: 1000),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: -10, coolDown: 2600, coolDownOffset: 1200),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: -12, coolDown: 2600, coolDownOffset: 1400),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: -14, coolDown: 2600, coolDownOffset: 1600),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: -16, coolDown: 2600, coolDownOffset: 1800),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: -18, coolDown: 2600, coolDownOffset: 2000),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: -20, coolDown: 2600, coolDownOffset: 2200),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: -22, coolDown: 2600, coolDownOffset: 2400),
                    new Shoot(30, 8, projectileIndex: 3, fixedAngle: -24, coolDown: 2600, coolDownOffset: 2600),
                    new TimedTransition(30000, "Death")
                    ),
                new State("Death",
                    new Taunt("I feel myself… Slipping… Into the void… It is so… Dark…"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new OrderOnce(20, "LH Colossus Rock 7", "Stopped"),
                    new OrderOnce(20, "LH Colossus Rock 8", "Stopped"),
                    new OrderOnce(20, "LH Colossus Rock 9", "Stopped"),
                    new Flash(0x3405B3, 1, 4),
                    new TimedTransition(4000, "Death 2")
                    ),
                new State("Death 2",
                    new Shoot(30, 48, projectileIndex: 17, defaultAngle: 0, shootAngle: 7.5, coolDown: 100000, seeInvis: true),
                    new Shoot(30, 24, projectileIndex: 18, defaultAngle: 0, shootAngle: 15, coolDown: 100000, coolDownOffset: 50, seeInvis: true),
                    new Shoot(30, 48, projectileIndex: 16, defaultAngle: 0, shootAngle: 7.5, coolDown: 1000000, seeInvis: true), //VOID BULLET
                    new TimedTransition(50, "Death 3")
                    ),
                new State("Death 3",
                    new Suicide()
                    )
                ),
            new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
            new Threshold(0.002,
                new ItemLoot("Greater Potion of Mana", 1, 0, 0.002),
                new ItemLoot("Greater Potion of Life", 1, 0, 0.002),
                new ItemLoot("Greater Potion of Mana", 0.5, 0, 0.002),
                new ItemLoot("Greater Potion of Life", 0.5, 0, 0.002),
                new TierLoot(14, ItemType.Weapon, 0.1, 0, 0.002),
                new TierLoot(14, ItemType.Armor, 0.1, 0, 0.002),
                new TierLoot(7, ItemType.Ability, 0.1, 0, 0.002),
                new TierLoot(7, ItemType.Ring, 0.1, 0, 0.002)
                ),
            new Threshold(0.03,
                new ItemLoot("Sword of the Colossus", 0.001),
                new ItemLoot("Breastplate of New Life", 0.001),
                new ItemLoot("Marble Seal", 0.001),
                new ItemLoot("Magical Lodestone", 0.001),
             //  new ItemLoot("Bow of the Void", 0.001),
             //  new ItemLoot("Quiver of the Shadows", 0.001),
             //  new ItemLoot("Armor of Nil", 0.001),
             //  new ItemLoot("Sourcestone", 0.001),
             //  new ItemLoot("Staff of Unholy Sacrifice", 0.001),
             //  new ItemLoot("Skull of Corrupted Souls", 0.001),
             //  new ItemLoot("Ritual Robe", 0.001),
             //  new ItemLoot("Bloodshed Ring", 0.001),
                new ItemLoot("Severed Marble Hand", 0.001)
      //     ),
      // new Threshold(0.05,
      //     new ItemLoot("Marbled Concoction", 0.0008),
      //     new ItemLoot("Titan Slayer", 0.0008),
      //     new ItemLoot("Colossal Plated Hide", 0.0008),
      //     new ItemLoot("Omnipotence Ring", 0.0008)
                )
            );

        #endregion Marble Colossus
    }
}
