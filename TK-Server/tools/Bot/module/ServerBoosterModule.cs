using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tk.bot.module
{
    [Name("Server Booster")]
    public class ServerBoosterModule : ModuleBase<SocketCommandContext>
    {
        private static readonly Dictionary<string, (string, Func<SocketCommandContext, IConfigurationRoot, string[], Task>)> NOTIFICATION_ARGS
            = new Dictionary<string, (string, Func<SocketCommandContext, IConfigurationRoot, string[], Task>)>()
            {
                { "-list", ("Display roles able to get realm event notifications", NotificationList) },
                { "-add", ("Add one or more notification roles", NotificationAdd) },
                { "-remove", ("Remove one or more notification roles", NotificationRemove) }
            };

        private readonly IConfigurationRoot _config;
#pragma warning disable
        private readonly CommandService _service;
#pragma warning enable

        public ServerBoosterModule(CommandService service, IConfigurationRoot config)
        {
            _service = service;
            _config = config;
        }

        [Command("notification"), Alias("not")]
        [Summary("Display a list of notification roles commands")]
        public async Task Notification(params string[] args)
        {
            if (args.Length == 0)
            {
                var builder = new EmbedBuilder() { Color = new Color(114, 137, 218) };

                foreach (var notificationArg in NOTIFICATION_ARGS)
                    builder.AddField(field =>
                    {
                        field.Name = $"notification {notificationArg.Key}";
                        field.Value = $"**Description:** {notificationArg.Value.Item1}";
                        field.IsInline = false;
                    });

                await ReplyAsync("", false, builder.Build());
                return;
            }

            if (string.IsNullOrWhiteSpace(args[0]) || !NOTIFICATION_ARGS.ContainsKey(args[0]))
            {
                await ReplyAsync("Argument doesn't exist for this command.");
                return;
            }

            await NOTIFICATION_ARGS[args[0]].Item2(Context, _config, args);
        }

        private static async Task NotificationAdd(SocketCommandContext context, IConfigurationRoot config, params string[] args)
        {
            var user = context.User as SocketGuildUser;

            if (!user.HasRole("Server Booster") && !user.IsAdmin(config))
            {
                await context.Channel.SendMessageAsync(StaticTexts.INSUFFICIENT_LEVEL_ERROR);
                return;
            }

            if (user.CountRolesFromString(config["notification-roles"]) >= 3)
            {
                await context.Channel.SendMessageAsync("You can only get notification by at most **3** realm events.");
                return;
            }

            if (args.Length == 1)
            {
                var sb = new StringBuilder("To __add__ a notification role is required to parse its `<id>`.");
                sb.Append("\n> **Example**: adding a single notification role");
                sb.Append($"\n```py\n@{config.BotName()} notification -add 1\n```");
                sb.Append("\n> **Example**: adding multiple notification roles");
                sb.Append($"\n```py\n@{config.BotName()} notification -add 1 2 3\n```");

                await context.Channel.SendMessageAsync(sb.ToString());
                return;
            }

            args = args.Skip(1).ToArray();

            if (!args.Any(arg => double.TryParse(arg, out var number)))
            {
                await context.Channel.SendMessageAsync("Only numeric <id> argument is valid for this command to execute.");
                return;
            }

            if (!args.Any(arg => config["notification-indexes"].Contains(arg)))
            {
                await context.Channel.SendMessageAsync($"Argument{(args.Length > 1 ? "s do not" : " does not")} contains an invalid <id> for notification roles.");
                return;
            }

            if (args.Length > 3)
            {
                await context.Channel.SendMessageAsync("You can only add at most **3** notification roles.");
                return;
            }

            var numericArgs = args.Select(arg => int.Parse(arg) - 1).ToList();
            var notificationRoles = config["notification-roles"].Split("§");
            var eligibleRoles = new List<string>();

            for (var i = 0; i < numericArgs.Count; i++)
                eligibleRoles.Add(notificationRoles[numericArgs[i]]);

            var roles = context.Guild.Roles.Where(role => eligibleRoles.Contains(role.Name)).ToArray();

            await user.AddRolesAsync(roles);
            await context.Channel.SendMessageAsync($"You have been successfully added {roles.Length} notification role{(roles.Length > 1 ? "s" : "")}!");
        }

        private static async Task NotificationList(SocketCommandContext context, IConfigurationRoot config, params string[] args)
        {
            var user = context.User as SocketGuildUser;

            if (!user.HasRole("Server Booster") && !user.IsAdmin(config))
            {
                await context.Channel.SendMessageAsync(StaticTexts.INSUFFICIENT_LEVEL_ERROR);
                return;
            }

            var notificationRoles = config["notification-roles"].Split("§");
            var amount = notificationRoles.Length;

            if (amount == 0)
            {
                await context.Channel.SendMessageAsync("There is no notification role available.");
                return;
            }

            var builder = new EmbedBuilder() { Color = new Color(114, 137, 218) };
            builder.Title = $"There {(amount > 1 ? "are" : "is")} {amount} notification role{(amount > 1 ? "s" : "")} available:";
            builder.Description = "";

            for (var i = 0; i < amount; i++)
                builder.Description += $"`ID: {(i + 1)}` - {notificationRoles[i]}\n";

            builder.Footer = new EmbedFooterBuilder() { Text = "Use notification role ID in command `notification -add <ID>` to receive it." };

            await context.Channel.SendMessageAsync("", false, builder.Build());
        }

        private static async Task NotificationRemove(SocketCommandContext context, IConfigurationRoot config, params string[] args)
        {
            var user = context.User as SocketGuildUser;

            if (!user.HasRole("Server Booster") && !user.IsAdmin(config))
            {
                await context.Channel.SendMessageAsync(StaticTexts.INSUFFICIENT_LEVEL_ERROR);
                return;
            }

            if (user.CountRolesFromString(config["notification-roles"]) == 0)
            {
                await context.Channel.SendMessageAsync("You do not have any notification role to remove.");
                return;
            }

            if (args.Length == 1)
            {
                var sb = new StringBuilder("To __remove__ a notification role is required to parse its `<id>`.");
                sb.Append("\n> **Example**: removing a single notification role");
                sb.Append($"\n```py\n@{config.BotName()} notification -remove 1\n```");
                sb.Append("\n> **Example**: removing multiple notification roles");
                sb.Append($"\n```py\n@{config.BotName()} notification -remove 1 2\n```");
                sb.Append("\n> **Example**: removing all notification roles");
                sb.Append($"\n```py\n@{config.BotName()} notification -remove --all\n```");

                await context.Channel.SendMessageAsync(sb.ToString());
                return;
            }

            args = args.Skip(1).ToArray();

            if (args.Length > 3)
            {
                await context.Channel.SendMessageAsync("You can only remove at most **3** notification roles.");
                return;
            }

            IRole[] roles;

            if (args.Contains("--all"))
            {
                roles = user.Roles.Where(role => config["notification-roles"].Contains(role.Name)).ToArray();

                await user.RemoveRolesAsync(roles);
                await context.Channel.SendMessageAsync($"You have been successfully removed {roles.Length} notification role{(roles.Length > 1 ? "s" : "")}!");
                return;
            }

            if (!args.Any(arg => double.TryParse(arg, out var number)))
            {
                await context.Channel.SendMessageAsync("Only numeric <id> argument is valid for this command to execute.");
                return;
            }

            if (!args.Any(arg => config["notification-indexes"].Contains(arg)))
            {
                await context.Channel.SendMessageAsync($"Argument{(args.Length > 1 ? "s do not" : " does not")} contains an invalid <id> for notification roles.");
                return;
            }

            var numericArgs = args.Select(arg => int.Parse(arg) - 1).ToList();
            var notificationRoles = config["notification-roles"].Split("§");
            var eligibleRoles = new List<string>();

            for (var i = 0; i < numericArgs.Count; i++)
                eligibleRoles.Add(notificationRoles[numericArgs[i]]);

            roles = context.Guild.Roles.Where(role => eligibleRoles.Contains(role.Name)).ToArray();

            await user.RemoveRolesAsync(roles);
            await context.Channel.SendMessageAsync($"You have been successfully removed {roles.Length} notification role{(roles.Length > 1 ? "s" : "")}!");
        }
    }
}
