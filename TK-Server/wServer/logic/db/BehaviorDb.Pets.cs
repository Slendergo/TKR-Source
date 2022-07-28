using wServer.logic.behaviors;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Pets = () => Behav()
        .Init("Bdwubz Bee",
            new State(
                new PetFollow()

                )
            )

        .Init("Black Cat",
            new State(
                new PetFollow()
                )
            );
    }
}
