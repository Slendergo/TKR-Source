using TKR.Shared;
using System;
using System.Linq;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.worlds.logic;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.networking.packets.outgoing;

namespace TKR.WorldServer.core.commands
{
    public abstract partial class Command
    {
        internal class Visit : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "visit";

            protected override bool Process(Player player, TickTime time, string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    player.SendInfo("Usage: /visit <player name>");
                    return true;
                }

                var target = player.GameServer.ConnectionManager.Clients.Keys.SingleOrDefault(_ => _.Account != null && _.Account.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                if (target?.Player?.World == null || !target.Player.CanBeSeenBy(player))
                {
                    player.SendError("Player not found!");
                    return false;
                }

                var owner = target.Player.World;

                if ((owner is VaultWorld || owner.IdName.Contains("Vault")) && !player.IsAdmin)
                {
                    player.SendError("Only admins can visit other players' vault.");
                    return false;
                }

                player.Client.Reconnect(new Reconnect()
                {
                    Host = "",
                    GameId = owner.Id,
                    Name = owner.DisplayName
                });
                return true;
            }
        }
    }
}
