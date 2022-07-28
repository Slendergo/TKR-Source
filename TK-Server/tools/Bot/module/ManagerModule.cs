using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace tk.bot.module
{
    [Name("Manager")]
    public class ManagerModule : ModuleBase<SocketCommandContext>
    {
        private static readonly Dictionary<string, (string, Func<SocketCommandContext, IConfigurationRoot, string[], Task>)> SERVERS_ARGS
            = new Dictionary<string, (string, Func<SocketCommandContext, IConfigurationRoot, string[], Task>)>()
            {
                { "-list", ("Display servers list to regular format", ServersToListAsync) },
                { "-restart", ("Request a restart action to a specific server connected to AppEngine", ServersRestartAsync) },
                { "-announce", ("Announce to all servers connected to AppEngine", ServersAnnounceAsync) }
            };

        private readonly IConfigurationRoot _config;
#pragma warning disable
        private readonly CommandService _service;
#pragma warning enable

        public ManagerModule(CommandService service, IConfigurationRoot config)
        {
            _service = service;
            _config = config;
        }

        [Command("servers"), Alias("sv")]
        [Summary("Display a list of servers commands")]
        public async Task Servers(params string[] args)
        {
            var user = Context.User as SocketGuildUser;

            if (!user.IsAdmin(_config))
            {
                await ReplyAsync(StaticTexts.INSUFFICIENT_LEVEL_ERROR);
                return;
            }

            if (args.Length == 0)
            {
                var builder = new EmbedBuilder() { Color = new Color(114, 137, 218) };

                foreach (var serverArg in SERVERS_ARGS)
                    builder.AddField(field =>
                    {
                        field.Name = $"servers {serverArg.Key}";
                        field.Value = $"**Description:** {serverArg.Value.Item1}";
                        field.IsInline = false;
                    });

                await ReplyAsync("", false, builder.Build());
                return;
            }

            if (string.IsNullOrWhiteSpace(args[0]) || !SERVERS_ARGS.ContainsKey(args[0]))
            {
                await ReplyAsync("Argument doesn't exist for this command.");
                return;
            }

            await SERVERS_ARGS[args[0]].Item2(Context, _config, args);
        }

        private static async Task ServersAnnounceAsync(SocketCommandContext context, IConfigurationRoot config, params string[] args)
        {
            if (args.Length == 1)
            {
                await context.Channel.SendMessageAsync("To announce in all servers is required to parse its `message`.");
                return;
            }

            args = args.Skip(1).ToArray();

            using var client = new WebClient();

            var user = context.User as SocketGuildUser;
            var data = new Dictionary<string, string>
            {
                ["token"] = config["token-hash"],
                ["cmd"] = "announce",
                ["user"] = user.Username,
                ["message"] = string.Join(" ", args)
            };

            try
            {
                var result = client.DownloadString(data.ToQuery(config["api"]));

                await context.Channel.SendMessageAsync(result);
            }
            catch (Exception e)
            {
                var response = e is WebException ? $"AppEngine is offline." : e.Message;

                await context.Channel.SendMessageAsync(response);
            }
        }

        private static async Task ServersRestartAsync(SocketCommandContext context, IConfigurationRoot config, params string[] args)
        {
            if (args.Length == 1)
            {
                await context.Channel.SendMessageAsync("To restart a server is required to parse its `name`.");
                return;
            }

            args = args.Skip(1).ToArray();

            using var client = new WebClient();

            var user = context.User as SocketGuildUser;
            var data = new Dictionary<string, string>
            {
                ["token"] = config["token-hash"],
                ["cmd"] = "restart",
                ["user"] = user.Username,
                ["server"] = string.Join(" ", args)
            };

            try
            {
                var result = client.DownloadString(data.ToQuery(config["api"]));

                await context.Channel.SendMessageAsync(result);
            }
            catch (Exception e)
            {
                var response = e is WebException ? $"AppEngine is offline." : e.Message;

                await context.Channel.SendMessageAsync(response);
            }
        }

        private static async Task ServersToListAsync(SocketCommandContext context, IConfigurationRoot config, params string[] args)
        {
            using var client = new WebClient();

            var data = new Dictionary<string, string>
            {
                ["token"] = config["token-hash"],
                ["cmd"] = "list"
            };

            try
            {
                var xml = XElement.Parse(client.DownloadString(data.ToQuery(config["api"])));

                if (xml.HasElement("Error"))
                {
                    await context.Channel.SendMessageAsync(xml.GetValue<string>("Error"));
                    return;
                }

                if (!xml.HasElement("Server"))
                {
                    await context.Channel.SendMessageAsync("There is no server online at this moment.");
                    return;
                }

                var builder = new EmbedBuilder() { Color = new Color(114, 137, 218) };
                var amountServers = 0;
                var amountPlayers = 0;
                var amountTotalPlayers = 0;

                foreach (var server in xml.Elements("Server"))
                {
                    builder.AddField(field =>
                    {
                        field.Name = server.GetValue<string>("Name");

                        var usageText = server.GetValue<string>("UsageText");
                        var sb = new StringBuilder();

                        if (usageText.Equals("0"))
                            sb.Append("Recently started, cannot obtain data yet.");
                        else
                        {
                            var usage = usageText.Split('/');
                            var current = int.Parse(usage[0]);
                            var max = int.Parse(usage[1]);

                            if (current == 0)
                                sb.Append("There is no player online.");
                            else
                            {
                                sb.Append($"There {(current > 1 ? "are" : "is")} {current}/{max} player{(current > 1 ? "s" : "")} online.");

                                amountPlayers += current;
                                amountTotalPlayers += max;
                            }
                        }

                        field.Value = $"{sb}\n";
                        field.IsInline = false;
                    });
                    amountServers++;
                }

                var sb = new StringBuilder();
                sb.Append($"There {(amountServers > 1 ? "are" : "is")}");
                sb.Append($" {amountServers} server{(amountServers > 1 ? "s" : "")}");
                sb.Append($" with {amountPlayers}/{amountTotalPlayers}");
                sb.Append($" player{(amountPlayers > 1 ? "s" : "")} online:");

                await context.Channel.SendMessageAsync(sb.ToString(), false, builder.Build());
            }
            catch (Exception e)
            {
                var response = e is WebException ? $"AppEngine is offline." : e.Message;

                await context.Channel.SendMessageAsync(response);
            }
        }
    }
}
