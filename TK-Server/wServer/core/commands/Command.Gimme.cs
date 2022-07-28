using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Gimme : Command
        {
            public Gimme() : base("gimme", permLevel: 60, alias: "give")
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                /*if(player.Rank == 60)
                {
                    player.SendError("This feature is disabled!");
                    return true;
                }*/

                var gameData = player.CoreServerManager.Resources.GameData;

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

                if (player.Client.Account.Rank < 110 && (item.DisplayName.Equals("Boshy Gun")
                    || item.DisplayName.Equals("Boshy Shotgun")
                    || item.DisplayName.Equals("Oryx's Arena Key")
                    || item.DisplayName.Equals("Ring of the Talisman's Kingdom")
                    || item.DisplayName.Equals("Excalibur")
                    || item.DisplayName.Equals("Crown")))
                {
                    player.SendError("Insufficient rank for that item.");
                    return false;
                }

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
