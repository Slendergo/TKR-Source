using TKR.WorldServer.logic.behaviors;

namespace TKR.WorldServer.logic
{
    partial class BehaviorDb
    {
        private _ Pets = () => Behav()
        .Init("Admin Orb",
            new State(
                new PetFollow()
                )
            )
        .Init("Snorlax",
            new State(
                new PetFollow()
                )
            )
        .Init("Prisoner",
            new State(
                new PetFollow()
                )
            )
        .Init("Wagon",
            new State(
                new PetFollow()
                )
            )
        .Init("Kirby",
            new State(
                new PetFollow()
                )
            )
        .Init("Drip Camel",
            new State(
                new PetFollow()
                )
            )
        .Init("Evil Wolf",
            new State(
                new PetFollow()
                )
            )
        .Init("Flying Lion",
            new State(
                new PetFollow()
                )
            )
        .Init("Shrek Pet",
            new State(
                new PetFollow()
                )
            )
        .Init("Donkey Pet",
            new State(
                new PetFollow()
                )
            )
        .Init("Fat Purple Bunny",
            new State(
                new PetFollow()
                )
            )
        .Init("Bullet Bill",
            new State(
                new PetFollow()
                )
            )
        .Init("Tiny Rogue",
            new State(
                new PetFollow()
                )
            )
        .Init("Bean Warrior",
            new State(
                new PetFollow()
                )
            )
        .Init("Annoyed Slime",
            new State(
                new PetFollow()
                )
            )
        .Init("Giant Rubber Duck",
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
