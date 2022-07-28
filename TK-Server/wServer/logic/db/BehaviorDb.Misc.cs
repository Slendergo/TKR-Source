using wServer.logic.behaviors;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Misc = () => Behav()
        .Init("White Fountain",
            new State(
                new HealPlayer(5, 1000, 100),
                new HealPlayerMP(5, 1000, 100)
                )
            )
        .Init("Winter Fountain Frozen",
            new State(
                new HealPlayer(5, 1000, 100),
                new HealPlayerMP(5, 1000, 100)
                )
            )
        .Init("Sheep",
            new State(
                new PlayerWithinTransition(15, "player_nearby"),
                new State("player_nearby",
                    new Prioritize(
                        new StayCloseToSpawn(0.1, 2),
                        new Wander(0.1)
                        ),
                    new Taunt(0.001, 1000, "baa", "baa baa")
                    )
                )
            );
    }
}
