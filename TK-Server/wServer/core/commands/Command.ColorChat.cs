using common;
using System;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class ColorChat : Command
        {
            public ColorChat() : base("colorchat", permLevel: 30, alias: "cc")
            {
            } //Donor-3

            protected override bool Process(Player player, TickData time, string color)
            {
                if (String.IsNullOrWhiteSpace(color))
                {
                    player.SendInfo("Usage: /colorchat <color> \n Number of the color needs to be a HexCode (0xFFFFFF = White, use 0x instahead #), search in google HexCode + Color. PS: /cc its the alias of this command. \nBe careful with the color you choose, you may not be able to read!");
                    return true;
                }

                player.ColorChat = Utils.FromString(color);

                var acc = player.Client.Account;
                acc.ColorChat = player.ColorChat;
                acc.FlushAsync();

                return true;
            }
        }
    }
}
