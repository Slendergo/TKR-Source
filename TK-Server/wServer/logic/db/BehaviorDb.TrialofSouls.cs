using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ TrialofSouls = () => Behav()
        .Init("The Baron",
            new State(
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
                    new Taunt("Fail to do so, and I'll leave you trembling HAHAHAH"),
                    new TimedRandomTransition(5000, false, "Phase 1", "Phase 2")
                    ),
                new State("Phase 1",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new Order(30, "Baron Turret", "Shoot"),
                    new Order(30, "Baron Turret 1", "Shoot"),
                    new Order(30, "Baron Turret 2", "Shoot"),
                    new Order(30, "Baron Turret 3", "Shoot"),
                    new Shoot(15, 7, projectileIndex: 2, coolDown: 1000, coolDownOffset: 0),
                    new Shoot(15, 7, projectileIndex: 6, coolDown: 1000, coolDownOffset: 0),
                    new Shoot(15, 7, projectileIndex: 3, coolDown: 1000, coolDownOffset: 0)
                    ),
                new State("Phase 2",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                    new Order(30, "Baron Turret", "Shoot"),
                    new Order(30, "Baron Turret 1", "Shoot"),
                    new Order(30, "Baron Turret 2", "Shoot"),
                    new Order(30, "Baron Turret 3", "Shoot"),
                    new Shoot(15, 7, projectileIndex: 2, coolDown: 1000, coolDownOffset: 0),
                    new Shoot(15, 7, projectileIndex: 6, coolDown: 1000, coolDownOffset: 0),
                    new Shoot(15, 7, projectileIndex: 3, coolDown: 1000, coolDownOffset: 0)
                    )
                ),
            new Threshold(0.05,
                new ItemLoot("Supreme Potion", 1)
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
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1000, predictive: 1.2)
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
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1200, predictive: 1.2)
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
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1400, predictive: 1.2)
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
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1600, predictive: 1.2)
                    )
                )
            )
        ;
    }
}
