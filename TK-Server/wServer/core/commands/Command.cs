using common;
using System;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        public virtual RankingType RankRequirement => RankingType.Regular;
        public virtual string Alias { get; }
        public abstract string CommandName { get; }
        public virtual bool ListAsCommand => RankRequirement != RankingType.Admin;

        public bool Execute(Player player, TickTime time, string args)
        {
            if (!HasPermission(player))
            {
                player.SendError("You dont have the required permission to use this command!");
                return false;
            }

            try
            {
                return Process(player, time, args);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error when executing the command: {CommandName}", e);
                return false;
            }
        }

        public bool HasPermission(Player player) => player.Client.Account.Rank >= RankRequirement;

        protected abstract bool Process(Player player, TickTime time, string args);
    }
}
