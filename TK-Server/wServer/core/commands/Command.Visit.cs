using CA.Extensions.Concurrent;
using System;
using System.Linq;
using wServer.core.objects;
using wServer.core.worlds;
using wServer.core.worlds.logic;
using wServer.networking.packets.outgoing;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Visit : Command
        {
            public Visit() : base("visit", permLevel: 100)
            {
            }

            protected override bool Process(Player player, TickData time, string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    player.SendInfo("Usage: /visit <player name>");
                    return true;
                }

                var target = player.CoreServerManager.ConnectionManager.Clients
                    .KeyWhereAsParallel(_ => _.Account != null && _.Account.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    .SingleOrDefault();

                if (target?.Player?.Owner == null || !target.Player.CanBeSeenBy(player))
                {
                    player.SendError("Player not found!");
                    return false;
                }

                if (target?.Player?.Owner is Vault)
                {
                    player.SendError("He's in the Vault.");
                    return false;
                }

                var owner = target.Player.Owner;

                if ((owner.Id == World.Vault || owner.Name.Contains("Vault")) && player.Rank < 110)
                {
                    player.SendError("Only rank 110 accounts can visit other players' vault.");
                    return false;
                }

                player.Client.Reconnect(new Reconnect()
                {
                    Host = "",
                    GameId = owner.Id,
                    Name = owner.SBName
                });
                return true;
            }
        }
    }
}
