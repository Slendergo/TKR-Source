using wServer.core.objects;

namespace wServer.core.commands
{
	public abstract partial class Command
	{
		internal class CheckForGodMode : Command
		{
			public CheckForGodMode() : base("cgmode", permLevel: 100, alias: "cgm")
			{
			}

			protected override bool Process(Player player, TickData time, string color)
			{
				for (int i = 0; i < 5; i++)
				{
					var e = Entity.Resolve(player.CoreServerManager, "shtrs Ice Shield");
					e.Move(player.X, player.Y);
					player.Owner.EnterWorld(e);
					e.Spawned = true;
				}
				return true;
			}
		}
	}
}
