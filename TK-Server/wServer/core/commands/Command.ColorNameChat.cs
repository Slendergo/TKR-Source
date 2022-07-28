using common;
using System;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class ColorNameChat : Command
        {
            public ColorNameChat() : base("colornamechat", permLevel: 20, alias: "cnc")
            {
            } //Donor-2

            protected override bool Process(Player player, TickData time, string color)
            {
                if (String.IsNullOrWhiteSpace(color))
                {
                    player.SendInfo("Usage: /colorchat <color> \n Number of the color needs to be a HexCode (0xFFFFFF = White, use 0x instahead #), search in google HexCode + Color. PS: /cnc its the alias of this command. \nBe careful with the color you choose, you may not be able to read!");
                    return true;
                }

                player.ColorNameChat = Utils.FromString(color);

                var acc = player.Client.Account;
                acc.ColorNameChat = player.ColorNameChat;
                acc.FlushAsync();

                return true;
            }
        }
    }
}
