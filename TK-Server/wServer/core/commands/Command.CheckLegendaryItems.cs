using System.Linq;
using System.Text;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class CheckLegendaryItems : Command
        {
            public CheckLegendaryItems() : base("checkLegendaryItems", 110, alias: "cli")
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                var legendary = player.Owner.Manager.Resources.GameData.Items.Where(item => item.Value.Legendary).ToArray();

                var sb = new StringBuilder($"Legendary Items ({legendary.Length}): ");
                for (var i = 0; i < legendary.Length; i++)
                {
                    if (i != 0)
                        sb.Append(", ");

                    sb.Append(legendary[i].Value.ObjectId);
                }

                player.SendInfo(sb.ToString());

                return true;
            }
        }
    }
}
