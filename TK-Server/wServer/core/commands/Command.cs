using System;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        protected Command(string name, int permLevel = 0, string alias = null, bool listCommand = true)
        {
            CommandName = name;
            PermissionLevel = permLevel;
            ListCommand = listCommand;
            Alias = alias;
        }

        public string Alias { get; private set; }
        public string CommandName { get; private set; }
        public bool ListCommand { get; private set; }
        public int PermissionLevel { get; private set; }

        public bool Execute(Player player, TickData time, string args, bool bypassPermission = false)
        {
            if (!bypassPermission && !HasPermission(player))
            {
                player.SendError("No permission!");
                return false;
            }

            try { return Process(player, time, args); }
            catch (Exception e)
            {
                common.logger.Log.Error($"Error when executing the command: {CommandName}", e);
                return false;
            }
        }

        public bool HasPermission(Player player) => GetPermissionLevel(player) >= PermissionLevel;

        protected abstract bool Process(Player player, TickData time, string args);

        private static int GetPermissionLevel(Player player) => player.Client.Account.Rank;
    }
}
