using wServer.logic.behaviors;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Pets = () => Behav()
        .InitMany("Black Cat", "Snowman", _ =>
            new State(
                new PetFollow()
                )
            );
    }
}
