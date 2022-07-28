using System.Linq;
using wServer.core.objects;
using System;
using common.database;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class CreateData : Command
        {
            public CreateData() : base("createData", permLevel: 90, alias: "cd")
            {
            }

            protected override bool Process(Player player, TickData time, string args)
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
