using wServer.logic.behaviors;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ MarbleSeal = () => Behav()
        .Init("Eye of Marble",
            new State(
                new State("Buff",
                    new MarbleSeal(6, 100, 1000),
                    new TimedTransition(6000, "Suicide")
                    ),
                new State("Suicide",
                    new Decay(0)
                    )
                )
            );
    };
}
