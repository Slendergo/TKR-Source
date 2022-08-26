using wServer.logic.behaviors;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Pets = () => Behav()
        .Init("Admin Orb",
            new State(
                new PetFollow()
                )
            )
        .InitMany("Black Cat", "Snowman", _ =>
            new State(
                new PetFollow()
                )
            );
    }
}
