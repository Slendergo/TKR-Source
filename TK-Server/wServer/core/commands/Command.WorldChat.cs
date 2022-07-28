using wServer.core.objects;
using wServer.networking.packets.outgoing;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class WorldChat : Command
        {
            public WorldChat() : base("wc", permLevel: 30)
            { }

            protected override bool Process(Player player, TickData time, string args)
            {
                if (args.Length == 0)
                {
                    player.SendHelp("Usage: /wc <saytext>");
                    return false;
                }

                var saytext = string.Join(" ", args);
                player.CoreServerManager.WorldManager
                    .WorldsBroadcastAsParallel(_ =>
                        _.PlayersBroadcastAsParallel(__ =>
                            __.Client.SendPacket(new Text
                            {
                                BubbleTime = 10,
                                NumStars = player.Stars,
                                Name = player.Name,
                                Txt = $" {saytext}",
                                NameColor = player.ColorNameChat,
                                TextColor = player.ColorChat
                            })
                        )
                    );
                return true;
            }
        }
    }
}
