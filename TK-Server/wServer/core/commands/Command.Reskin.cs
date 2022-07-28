using common;
using System.Linq;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Reskin : Command
        {
            public Reskin() : base("reskin", permLevel: 90)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                var skins = player.CoreServerManager.Resources.GameData.Skins
                    .Where(d => d.Value.PlayerClassType == player.ObjectType)
                    .Select(d => d.Key)
                    .ToArray();

                if (string.IsNullOrEmpty(args))
                {
                    var choices = skins.ToCommaSepString();
                    player.SendError("Usage: /reskin <positive integer>");
                    player.SendError("Choices: " + choices);
                    return false;
                }

                var skin = (ushort)Utils.FromString(args);

                /*if (skin != 0 && !skins.Contains(skin))
                {
                    player.SendError("Error setting skin. Either the skin type doesn't exist or the skin is for another class.");
                    return false;
                }*/

                var skinDesc = player.CoreServerManager.Resources.GameData.Skins[skin];
                var size = skinDesc.Size;
                size = 100;

                player.SetDefaultSkin(skin);
                player.SetDefaultSize(size);
                return true;
            }
        }
    }
}
