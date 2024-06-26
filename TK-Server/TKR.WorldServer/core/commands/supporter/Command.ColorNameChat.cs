﻿using TKR.Shared;
using System;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.commands
{
    public abstract partial class Command
    {
        internal class ColorNameChat : Command
        {
            public override RankingType RankRequirement => RankingType.Supporter1;
            public override string CommandName => "colornamechat";

            protected override bool Process(Player player, TickTime time, string color)
            {
                if (string.IsNullOrWhiteSpace(color))
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
