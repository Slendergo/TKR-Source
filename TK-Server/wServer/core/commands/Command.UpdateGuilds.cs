using System;
using wServer.core.objects;
using wServer.utils;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class UpdateGuilds : Command
        {
            public UpdateGuilds() : base("ug", permLevel: 100)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                try
                {
                    var db = player.CoreServerManager.Database;
                    var id = 1;
                    do
                    {
                        var guild = db.GetGuild(id);
                        if (guild != null)
                        {
                            if (guild.Level == 0)
                            {
                                guild.GuildLootBoost = 0.0f;
                                player.SendInfo("Guild Updated Successfully!");
                                guild.FlushAsync();
                            }
                            else if (guild.Level == 1)
                            {
                                guild.GuildLootBoost = .15f;
                                player.SendInfo("Guild Updated Successfully!");
                                guild.FlushAsync();
                            }
                            else if (guild.Level == 2)
                            {
                                guild.GuildLootBoost = .30f;
                                player.SendInfo("Guild Updated Successfully!");
                                guild.FlushAsync();
                            }
                            else if (guild.Level == 3)
                            {
                                guild.GuildLootBoost = .45f;
                                player.SendInfo("Guild Updated Successfully!");
                                guild.FlushAsync();
                            }

                            SLogger.Instance.Warn($"Guild Name: {guild.Name}, LBoost: {guild.GuildLootBoost}");
                        }
                        id++;
                    }
                    while (db.GetGuild(id) != null);
                }
                catch (Exception ex)
                {
                    SLogger.Instance.Warn(ex.ToString());
                    return false;
                }
                return true;
            }
        }
    }
}
