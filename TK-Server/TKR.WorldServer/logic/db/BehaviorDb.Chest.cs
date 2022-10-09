using TKR.Shared.resources;
using TKR.WorldServer.logic.behaviors;
using TKR.WorldServer.logic.loot;
using TKR.WorldServer.logic.transitions;

namespace TKR.WorldServer.logic
{
    partial class BehaviorDb
    {
        private _ Chest = () => Behav()
        .Init("Quest Chest",
           new State(
                new ScaleHP2(20),
                new State("Idle",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                    new TimedTransition(5000, "UnsetEffect")
                    ),
                new State("UnsetEffect",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable)
                    )
                ),
            new Threshold(0.01,
                new ItemLoot("Harlequin Armor", 0.0014, threshold: 0.03),
                new TierLoot(12, ItemType.Weapon, 0.05),
                new TierLoot(5, ItemType.Ability, 0.045),
                new TierLoot(12, ItemType.Armor, 0.05),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Attack", 0.5),
                new ItemLoot("Prism of Dancing Swords", 0.015),
                new ItemLoot("Large Jester Argyle Cloth", 0.1),
                new ItemLoot("Small Jester Argyle Cloth", 0.1),
                new ItemLoot("Theatre Key", 0.01, 0, 0.03)
                    )
                )
        .Init("Epic Quest Chest",
            new State(
                new ScaleHP2(20),
                new State("Idle",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                    new TimedTransition(5000, "UnsetEffect")
                    ),
                new State("UnsetEffect",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable)
                    )
                ),
            new Threshold(0.01,
                new ItemLoot("Harlequin Armor", 0.0014, threshold: 0.03),
                new TierLoot(12, ItemType.Weapon, 0.05),
                new TierLoot(5, ItemType.Ability, 0.045),
                new TierLoot(12, ItemType.Armor, 0.05),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Attack", 0.5),
                new ItemLoot("Prism of Dancing Swords", 0.015),
                new ItemLoot("Large Jester Argyle Cloth", 0.1),
                new ItemLoot("Small Jester Argyle Cloth", 0.1),
                new ItemLoot("Theatre Key", 0.01, 0, 0.03)
                )
            );
    }
}
