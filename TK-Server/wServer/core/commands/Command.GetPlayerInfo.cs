using System.Text;
using common;
using wServer.core.objects;

namespace wServer.core.commands
{
	public abstract partial class Command
	{
		internal class GetPlayerInfo : Command
		{
			public GetPlayerInfo() : base("getpinfo", permLevel: 0, alias: "gpi")
			{
			}

			protected override bool Process(Player player, TickData time, string args)
			{
				var index = args.IndexOf(' ');
				var name = args.Substring(0, index);
				var id = player.CoreServerManager.Database.ResolveId(name);
				var acc = player.CoreServerManager.Database.GetAccount(id);
				var dt = Utils.FromUnixTimestamp(acc.LastSeen);
				if (player.Rank < 100)
				{
					if (player.GuildRank < 30)
					{
						player.SendInfo("Must be a Guild Leader or Founder");
						return false;
					}
					if (id == 0 || acc == null)
					{
						player.SendError("Account not found!");
						return false;
					}
					if (acc.GuildId != player.Client.Account.GuildId)
					{
						player.SendError("You are only able to search your own guild's players");
						return false;
					}
					player.SendInfo($"Guild Info of Player {name} :\n " +
						$"--------------\n " +
						$"Banned: {acc.Banned}\n " +
						$"Guild Rank: {acc.GuildRank}\n " +
						$"Last Seen: {Utils.TimeAgo(dt)}");
					return true;
				}
				else
				{
					if (id == 0 || acc == null)
					{
						player.SendError("Account not found!");
						return false;
					}
					player.SendInfo($"Full Info of Player {name} :\n " +
						$"------------------------\n " +
						$"Banned: {acc.Banned}\n " +
						$"Guild Rank: {acc.GuildRank}\n " +
						$"Total Fame: {acc.TotalFame}\n " +
						$"Total Credits: {acc.TotalCredits}\n " +
					    $"Rank: {acc.Rank}\n " +
						$"Last Seen: {Utils.TimeAgo(dt)}\n " +
						$"Account ID: {acc.AccountId}\n " +
						$"Guild ID: {acc.GuildId}\n " +
						$"Vault Count: {acc.VaultCount}\n " +
						$"Enemies Killed: {acc.EnemiesKilled}\n " +
						$"--------------\n " +
						$"Email: {acc.UUID.ToString()}");


					return true;
				}
			}
		}
	}
}
