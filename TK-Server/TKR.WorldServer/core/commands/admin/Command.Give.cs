using TKR.Shared;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;

namespace TKR.WorldServer.core.commands
{
    public abstract partial class Command
    {
        internal class Give : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "give";

            protected override bool Process(Player player, TickTime time, string args)
            {
                /*if(player.Rank == 60)
                {
                    player.SendError("This feature is disabled!");
                    return true;
                }*/

                var gameData = player.GameServer.Resources.GameData;

                /*if (player.Client.Account.Rank == 60)
                {
                    player.SendInfo("This command is disabled due to game altering bug!");
                    return false;
                }*/

                // allow both DisplayId and Id for query
                if (!gameData.DisplayIdToObjectType.TryGetValue(args, out ushort objType))
                {
                    if (!gameData.IdToObjectType.TryGetValue(args, out objType))
                    {
                        player.SendError("Unknown item type!");
                        return false;
                    }
                }

                if (!gameData.Items.ContainsKey(objType))
                {
                    player.SendError("Not an item!");
                    return false;
                }

                var item = gameData.Items[objType];

                var availableSlot = player.Inventory.GetAvailableInventorySlot(item);
                if (availableSlot != -1)
                {
                    player.Inventory[availableSlot] = item;
                    return true;
                }

                player.SendError("Not enough space in inventory!");
                return false;
            }
        }
    }
}