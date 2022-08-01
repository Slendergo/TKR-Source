using common.database;
using System;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class FixStacks : Command
        {
            public FixStacks() : base("fixstacks", permLevel: 0)
            {
            }

            protected override bool Process(Player player, TickTime time, string args)
            {
                var inventory = player.Inventory;
                var fixedStacks = 0;

                for (int i = 4; i < 12; i++)
                {
                    if (inventory[i] != null && inventory[i].ObjectId == "Magic Dust")
                    {
                        if (inventory.Data[i] == null)
                        {
                            inventory.Data[i] = new ItemData()
                            {
                                ObjectId = null,
                                Stack = 1,
                                MaxStack = 5
                            };
                            fixedStacks ++;
                            player.SendInfo($"Successfully fixed ItemData in Slot {i}");
                        }
                    }
                }
                if (fixedStacks < 1)
                    player.SendError("Nothing to fix.");
                return true;
            }
        }
    }
}
