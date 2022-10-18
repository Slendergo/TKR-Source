using System;
using TKR.Shared;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.objects.containers;
using TKR.WorldServer.logic.loot;

namespace TKR.WorldServer.core.commands
{
    public abstract partial class Command
    {
        internal class CreateLootbags : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "lootbags";

            protected override bool Process(Player player, TickTime time, string args)
            {
                foreach (var item in player.GameServer.Resources.GameData.Items)
                    if (/*item.Value.Mythical || item.Value.Legendary || */item.Value.SlotType == 26) // talisman
                    {
                        var container = new Container(player.GameServer, Loot.BAG_ID_TO_TYPE[item.Value.BagType], 60000, true);
                        container.Inventory[0] = item.Value;
                        container.Move(player.X + (float)((Random.Shared.NextDouble() * 2 - 1) * 8), player.Y + (float)((Random.Shared.NextDouble() * 2 - 1) * 8));
                        container.SetDefaultSize(75);
                        player.World.EnterWorld(container);
                    }
                return true;
            }
        }

        internal class CreateLootbagsB : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "lootbagsb";

            protected override bool Process(Player player, TickTime time, string args)
            {
                foreach (var item in player.GameServer.Resources.GameData.Items)
                    if (/*item.Value.Mythical || item.Value.Legendary || */item.Value.SlotType == 26) // talisman
                    {
                        var container = new Container(player.GameServer, Loot.BOOSTED_BAG_ID_TO_TYPE[item.Value.BagType], 60000, true);
                        container.Inventory[0] = item.Value;
                        container.Move(player.X + (float)((Random.Shared.NextDouble() * 2 - 1) * 8), player.Y + (float)((Random.Shared.NextDouble() * 2 - 1) * 8));
                        container.SetDefaultSize(75);
                        player.World.EnterWorld(container);
                    }
                return true;
            }
        }

        internal class Announce : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "announce";

            protected override bool Process(Player player, TickTime time, string args)
            {
                player.GameServer.ChatManager.Announce(player, args);
                return true;
            }
        }

        internal class ServerAnnounce : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "sannounce";

            protected override bool Process(Player player, TickTime time, string args)
            {
                player.GameServer.ChatManager.ServerAnnounce(args);
                return true;
            }
        }
    }
}
