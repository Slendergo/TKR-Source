using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {



        private _ Nexus = () => Behav()
            .Init("Nexus Crier",
                new State(
                    new State("Welcome",
                        new Wander(0.1),
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new PlayerWithinTransition(10, "Start")
                        ),
                    new State("Start",
                        new Taunt("Welcome to Talisman's Kingdom Reborn!"),
                        new TimedTransition(4000, "0 k")
                        ),
                    new State("0 k",
                        new Wander(0.1),
                        new TimedRandomTransition(10000, false, "1", "2", "3", "Welcome")
                        ),
                    new State("1",
                        new Taunt("Uncover the mysteries of the Talisman's King!"),
                        new TimedTransition(4000, "1 k")
                        ),
                    new State("1 k",
                        new Wander(0.1),
                        new TimedRandomTransition(10000, false, "2", "3", "Welcome")
                        ),
                    new State("2",
                        new Taunt("That generator seems to be making a lot of noise!"),
                        new TimedTransition(4000, "2 k")
                        ),
                    new State("2 k",
                        new Wander(0.1),
                        new TimedRandomTransition(10000, false, "1", "3", "Welcome")
                        ),
                    new State("3",
                        new Taunt("Want a new look? Check out the cloth bazaar!"),
                        new TimedTransition(4000, "3 k")
                        ),
                    new State("3 k",
                        new Wander(0.1),
                        new TimedRandomTransition(10000, false, "2", "3", "Welcome")
                        )
                    )
                )
            .Init("Engine",
                new State(
                    new State("Idle",
                        new EngineStateTransition(1, "1"),
                        new EngineStateTransition(2, "2"),
                        new EngineStateTransition(3, "3")
                        ),
                    new State("1",
                        new Transform("Engine Stage 1"),
                        new Decay()
                        ),
                    new State("2",
                        new Transform("Engine Stage 2"),
                        new Decay()
                        ),
                    new State("3",
                        new Transform("Engine Stage 3"),
                        new Decay()
                        )
                    )
                )
            .Init("Engine Stage 1",
                new State(
                    new State("Idle",
                        new EngineStateTransition(0, "0"),
                        new EngineStateTransition(2, "2"),
                        new EngineStateTransition(3, "3")
                        ),
                    new State("0",
                        new Transform("Engine"),
                        new Decay()
                        ),
                    new State("2",
                        new Transform("Engine Stage 2"),
                        new Decay()
                        ),
                    new State("3",
                        new Transform("Engine Stage 3"),
                        new Decay()
                        )
                    )
                )
            .Init("Engine Stage 2",
                new State(
                    new State("Idle",
                        new EngineStateTransition(0, "0"),
                        new EngineStateTransition(1, "1"),
                        new EngineStateTransition(3, "3")
                        ),
                    new State("0",
                        new Transform("Engine"),
                        new Decay()
                        ),
                    new State("1",
                        new Transform("Engine Stage 1"),
                        new Decay()
                        ),
                    new State("3",
                        new Transform("Engine Stage 3"),
                        new Decay()
                        )
                    )
                )
            .Init("Engine Stage 3",
                new State(
                    new State("Idle",
                        new EngineStateTransition(0, "0"),
                        new EngineStateTransition(1, "1"),
                        new EngineStateTransition(2, "2")
                        ),
                    new State("0",
                        new Transform("Engine"),
                        new Decay()
                        ),
                    new State("1",
                        new Transform("Engine Stage 1"),
                        new Decay()
                        ),
                    new State("2",
                        new Transform("Engine Stage 2"),
                        new Decay()
                        )
                    )
                );
    }
}