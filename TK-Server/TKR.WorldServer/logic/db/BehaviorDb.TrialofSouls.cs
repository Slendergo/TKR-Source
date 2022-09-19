using TKR.Shared.resources;
using TKR.WorldServer.logic.behaviors;
using TKR.WorldServer.logic.loot;
using TKR.WorldServer.logic.transitions;

namespace TKR.WorldServer.logic
{
    partial class BehaviorDb
    {
        private _ TrialofSouls = () => Behav()
        .Init("The Baron",
            new State(
                new CompletedTrialOfSouls(),
                new ScaleHP2(20),
                new State("Spawned",
                    new HpLessTransition(threshold: 0.99, targetState: "Choose")
                    ),
                new State("Choose",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Taunt("I deem thy worthy!"),
                    new TimedTransition(2000, "Choose 1")
                    ),
                new State("Choose 1",
                    new Taunt("Defeat me, and you shall be rewarded!"),
                     new TimedTransition(2000, "Choose 2")
                    ),
                new State("Choose 2",
                    new Taunt("Fail to do so, and I'll leave you trembling! HAHAHAH"),
                    new TimedRandomTransition(5000, false, "Phase 1", "Phase 2")
                    ),
                new State("Phase 1",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new Chase(8),
                    new Shoot(15, 3, projectileIndex: 4, coolDown: 2500),
                    new Shoot(15, 5, projectileIndex: 0, shootAngle: 10, coolDown: 1200, coolDownOffset: 0),
                    new Shoot(15, 5, projectileIndex: 1, shootAngle: 15, coolDown: 1200, coolDownOffset: 600),
                    new Order(30, "Baron Turret", "Shoot"),
                    new Order(30, "Baron Turret 1", "Shoot"),
                    new Order(30, "Baron Turret 2", "Shoot"),
                    new Order(30, "Baron Turret 3", "Shoot"),
                    new Shoot(15, 7, projectileIndex: 2, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(15, 7, projectileIndex: 6, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(15, 7, projectileIndex: 3, coolDown: 2000, coolDownOffset: 0)
                    ),
                new State("Phase 2",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new Grenade(radius: 2, damage: 0, range: 10, coolDown: 800, fixedAngle: 0, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1000, color: 0xF9F9F9),
                    new Grenade(radius: 2, damage: 0, range: 10, coolDown: 800, fixedAngle: 36, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1000, color: 0xF9F9F9),
                    new Grenade(radius: 2, damage: 0, range: 10, coolDown: 800, fixedAngle: 72, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1000, color: 0xF9F9F9),
                    new Grenade(radius: 2, damage: 0, range: 10, coolDown: 800, fixedAngle: 108, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1000, color: 0xF9F9F9),
                    new Grenade(radius: 2, damage: 0, range: 10, coolDown: 800, fixedAngle: 144, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1000, color: 0xF9F9F9),
                    new Grenade(radius: 2, damage: 0, range: 10, coolDown: 800, fixedAngle: 180, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1000, color: 0xF9F9F9),
                    new Grenade(radius: 2, damage: 0, range: 10, coolDown: 800, fixedAngle: 216, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1000, color: 0xF9F9F9),
                    new Grenade(radius: 2, damage: 0, range: 10, coolDown: 800, fixedAngle: 252, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1000, color: 0xF9F9F9),
                    new Grenade(radius: 2, damage: 0, range: 10, coolDown: 800, fixedAngle: 288, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1000, color: 0xF9F9F9),
                    new Grenade(radius: 2, damage: 0, range: 10, coolDown: 800, fixedAngle: 324, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 1000, color: 0xF9F9F9),
                    new TossObject2("Baron Turret", 2, angle: 0, coolDown: 999999),
                    new TossObject2("Baron Turret 1", 2, angle: 90, coolDown: 999999),
                    new TossObject2("Baron Turret 2", 2, angle: 180, coolDown: 999999),
                    new TossObject2("Baron Turret 3", 2, angle: 270, coolDown: 999999),
                    new Order(30, "Baron Turret", "Shoot"),
                    new Order(30, "Baron Turret 1", "Shoot"),
                    new Order(30, "Baron Turret 2", "Shoot"),
                    new Order(30, "Baron Turret 3", "Shoot"),
                    new Shoot(15, 7, projectileIndex: 2, coolDown: 1000, coolDownOffset: 0),
                    new Shoot(15, 7, projectileIndex: 6, coolDown: 1000, coolDownOffset: 0),
                    new Shoot(15, 7, projectileIndex: 3, coolDown: 1000, coolDownOffset: 0)
                    )
                ),
            new Threshold(0.001,
                new ItemLoot("Empty Vial", 1)
                )
            )
        .Init("Baron Turret 1",
            new State(
                new ScaleHP2(20),
                new State("Spawned",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("Shoot",
                    new Shoot(15, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 0),
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000),
                    new EntityNotExistsTransition("The Baron", 20, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
        .Init("Baron Turret 2",
            new State(
                new ScaleHP2(20),
                new State("Spawned",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("Shoot",
                    new Shoot(15, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 200),
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1200),
                     new EntityNotExistsTransition("The Baron", 20, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
        .Init("Baron Turret 3",
            new State(
                new ScaleHP2(20),
                new State("Spawned",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("Shoot",
                    new Shoot(15, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 400),
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1400),
                     new EntityNotExistsTransition("The Baron", 20, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
         .Init("Baron Turret",
            new State(
                new ScaleHP2(20),
                new State("Spawned",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("Shoot",
                    new Shoot(15, 1, projectileIndex: 0, coolDown: 2000, coolDownOffset: 600),
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1600),
                     new EntityNotExistsTransition("The Baron", 20, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
        ;
    }
}
