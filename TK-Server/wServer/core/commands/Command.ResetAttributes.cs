using CA.Extensions.Concurrent;
using common;
using common.database;
using wServer.core.objects;
using wServer.core.worlds;
using wServer.networking;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class ResetAttributes : Command
        {
            public ResetAttributes() : base("resetattributes")
            {
            }

            protected override bool Process(Player player, TickTime time, string args)
            {

                if ((player.World.Id == World.Vault || player.World.IdName.Contains("Vault")) || player.World.Id == World.Nexus || player.World.IdName.Contains("Nexus"))
                {
                    player.Client.Character.Reload("node1TickMin");
                    player.Client.Character.Reload("node1TickMaj");
                    player.Client.Character.Reload("node1Med");
                    player.Client.Character.Reload("node1Big");
                    player.Client.Character.Reload("node2TickMin");
                    player.Client.Character.Reload("node2TickMaj");
                    player.Client.Character.Reload("node2Med");
                    player.Client.Character.Reload("node2Big");
                    player.Client.Character.Reload("node3TickMin");
                    player.Client.Character.Reload("node3TickMaj");
                    player.Client.Character.Reload("node3Med");
                    player.Client.Character.Reload("node3Big");
                    player.Client.Character.Reload("node4TickMin");
                    player.Client.Character.Reload("node4TickMaj");
                    player.Client.Character.Reload("node4Med");
                    player.Client.Character.Reload("node4Big");
                    player.Client.Character.Reload("node5TickMin");
                    player.Client.Character.Reload("node5TickMaj");
                    player.Client.Character.Reload("node5Med");
                    player.Client.Character.Reload("node5Big");

                    player.Client.Character.Node1TickMin = 0;
                    player.Client.Character.Node1TickMaj = 0;
                    player.Client.Character.Node1Med = 0;
                    player.Client.Character.Node1Big = 0;
                    player.Client.Character.Node2TickMin = 0;
                    player.Client.Character.Node2TickMaj = 0;
                    player.Client.Character.Node2Med = 0;
                    player.Client.Character.Node2Big = 0;
                    player.Client.Character.Node3TickMin = 0;
                    player.Client.Character.Node3TickMaj = 0;
                    player.Client.Character.Node3Med = 0;
                    player.Client.Character.Node3Big = 0;
                    player.Client.Character.Node4TickMin = 0;
                    player.Client.Character.Node4TickMaj = 0;
                    player.Client.Character.Node4Med = 0;
                    player.Client.Character.Node4Big = 0;
                    player.Client.Character.Node5TickMin = 0;
                    player.Client.Character.Node5TickMaj = 0;
                    player.Client.Character.Node5Med = 0;
                    player.Client.Character.Node5Big = 0;
                    player.SendInfo("All your Attribute Points have been reset.");
                    player.CoreServerManager.Database.ReloadAccount(player.Client.Account);
                    player.Client.Disconnect("Reset Attributes");
                    player.SendInfo("Player disconnected!");
                    return true;
                }
                player.SendError("Can only reset attributes in Vault or Nexus!");
                return false;
            }
        }
    }
}
