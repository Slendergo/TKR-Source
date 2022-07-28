using common;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Size : Command
        {
            public Size() : base("size", permLevel: 90)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                if (string.IsNullOrEmpty(args))
                {
                    player.SendError("Usage: /size <positive integer>. Using 0 will restore the default size for the sprite.");
                    return false;
                }

                var size = Utils.FromString(args);
                var min = player.Rank < 80 ? 75 : 0;
                var max = player.Rank < 80 ? 125 : 5000;
                if (size < min && size != 0 || size > max)
                {
                    player.SendError($"Invalid size. Size needs to be within the range: {min}-{max}. Use 0 to reset size to default.");
                    return false;
                }

                var acc = player.Client.Account;
                acc.Size = size;
                acc.FlushAsync();

                var target = player;
                if (size == 0)
                    target.RestoreDefaultSize();
                else
                    target.Size = size;

                return true;
            }
        }
    }
}
