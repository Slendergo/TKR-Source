using wServer.logic.behaviors;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Specialized = () => Behav()
        .Init("Spirit Prism Bomb",
            new State(
                new State("Idle",
                    new TimedTransition(1000, "Explode")
                    ),
                new State("Explode",
                    new Prioritize(
                        new StayCloseToSpawn(3, 3)
                        ),
                    new State("Explode 1",
                        new JumpToRandomOffset(-2, 2, -2, 2),
                        new ChangeSize(100, 0),

                        new EnemyAOE(1, false, 40, 90, false, 0xFF9933),
                        new TimedTransition(100, "Explode 2")
                        ),
                    new State("Explode 2",
                        new JumpToRandomOffset(-2, 2, -2, 2),

                        new EnemyAOE(1, false, 40, 90, false, 0xFF9933),
                        new TimedTransition(100, "Explode 3")
                        ),
                    new State("Explode 3",
                        new JumpToRandomOffset(-2, 2, -2, 2),

                        new EnemyAOE(1, false, 40, 90, false, 0xFF9933),
                        new TimedTransition(100, "Explode 4")
                        ),
                    new State("Explode 4",
                        new JumpToRandomOffset(-2, 2, -2, 2),

                        new EnemyAOE(1, false, 40, 90, false, 0xFF9933),
                        new TimedTransition(100, "Explode 5")
                        ),
                    new State("Explode 5",
                        new JumpToRandomOffset(-2, 2, -2, 2),

                        new EnemyAOE(1, false, 40, 90, false, 0xFF9933),
                        new TimedTransition(100, "Explode 6")
                        ),
                    new State("Explode 6",
                        new JumpToRandomOffset(-2, 2, -2, 2),

                        new EnemyAOE(1, false, 40, 90, false, 0xFF9933),
                        new Decay(0)
                        )
                    )
                )
            );
    }
}
