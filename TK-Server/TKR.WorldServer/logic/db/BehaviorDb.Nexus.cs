using TKR.Shared.resources;
using TKR.WorldServer.logic.behaviors;
using TKR.WorldServer.logic.loot;
using TKR.WorldServer.logic.transitions;

namespace TKR.WorldServer.logic
{
    partial class BehaviorDb
    {



        private _ Nexus = () => Behav()
            .Init("Crier",
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
                        new Taunt("Want a new look? Check out the styles in the market!"),
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
                        new EngineStageTransition(1, "1"),
                        new EngineStageTransition(2, "2"),
                        new EngineStageTransition(3, "3")
                        ),
                    new State("1",
                        new EngineTransform("Engine Stage 1"),
                        new Decay()
                        ),
                    new State("2",
                        new EngineTransform("Engine Stage 2"),
                        new Decay()
                        ),
                    new State("3",
                        new EngineTransform("Engine Stage 3"),
                        new Decay()
                        )
                    )
                )
            .Init("Engine Stage 1",
                new State(
                    new State("Idle",
                        new EngineStageTransition(0, "0"),
                        new EngineStageTransition(2, "2"),
                        new EngineStageTransition(3, "3")
                        ),
                    new State("0",
                        new EngineTransform("Engine"),
                        new Decay()
                        ),
                    new State("2",
                        new EngineTransform("Engine Stage 2"),
                        new Decay()
                        ),
                    new State("3",
                        new EngineTransform("Engine Stage 3"),
                        new Decay()
                        )
                    )
                )
            .Init("Engine Stage 2",
                new State(
                    new State("Idle",
                        new EngineStageTransition(0, "0"),
                        new EngineStageTransition(1, "1"),
                        new EngineStageTransition(3, "3")
                        ),
                    new State("0",
                        new EngineTransform("Engine"),
                        new Decay()
                        ),
                    new State("1",
                        new EngineTransform("Engine Stage 1"),
                        new Decay()
                        ),
                    new State("3",
                        new EngineTransform("Engine Stage 3"),
                        new Decay()
                        )
                    )
                )
            .Init("Engine Stage 3",
                new State(
                    new State("Idle",
                        new EngineStageTransition(0, "0"),
                        new EngineStageTransition(1, "1"),
                        new EngineStageTransition(2, "2")
                        ),
                    new State("0",
                        new EngineTransform("Engine"),
                        new Decay()
                        ),
                    new State("1",
                        new EngineTransform("Engine Stage 1"),
                        new Decay()
                        ),
                    new State("2",
                        new EngineTransform("Engine Stage 2"),
                        new Decay()
                        )
                    )
                );
    }
}