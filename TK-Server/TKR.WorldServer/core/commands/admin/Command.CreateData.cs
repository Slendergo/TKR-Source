using System;
using TKR.Shared;
using TKR.Shared.database.character.inventory;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.commands
{
    public abstract partial class Command
    {
        internal class CreateData : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "createdata";
            public override string Alias => "cd";

            protected override bool Process(Player player, TickTime time, string args)
            {
                var invSlot = Convert.ToInt32(args);
                var inventory = player.Inventory;

                if (inventory.Data[invSlot] == null)
                {
                    inventory.Data[invSlot] = new ItemData()
                    {
                        ObjectId = "Hello!"
                    };
                    player.SendInfo($"Successfully created a ItemData in Slot {invSlot}, Data: {inventory.Data[invSlot].GetData()}");
                    return true;
                }
                else
                {
                    player.SendError($"Slot {invSlot} already have an ItemData! {inventory.Data[invSlot].GetData()}");
                    return false;
                }
            }
        }
    }
}
