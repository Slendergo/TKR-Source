using System.Linq;
using System.Text;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class CheckRevengeItems : Command
        {
            public CheckRevengeItems() : base("checkRevengeItems", 110, alias: "cri")
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                var revenge = player.Owner.Manager.Resources.GameData.Items.Where(item => item.Value.Revenge).ToArray();

                var sb = new StringBuilder($"Revenge Items ({revenge.Length}): ");
                for (var i = 0; i < revenge.Length; i++)
                {
                    if (i != 0)
                        sb.Append(", ");

                    sb.Append(revenge[i].Value.ObjectId);
                }

                player.SendInfo(sb.ToString());

                return true;
            }
        }
    }
}
