using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Linq;
using Discord;
using System.Text;

namespace tk.bot.service
{
    public class CommandHandler
    {
        private const string ERROR_BAD_ARGS_COUNT_FEW = "BadArgCount: The input text has too few parameters.";
        private const string ERROR_BAD_ARGS_COUNT_MANY = "BadArgCount: The input text has too many parameters.";
        private const string ERROR_UNKNOWN_COMMAND = "UnknownCommand: Unknown command.";

        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _provider;

        public CommandHandler(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config, IServiceProvider provider)
        {
            _discord = discord;
            _commands = commands;
            _config = config;
            _provider = provider;

            _discord.MessageReceived += OnMessageReceivedAsync;
        }

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg))
                return;

            if (msg.Author.Id == _discord.CurrentUser.Id)
                return;

            var context = new SocketCommandContext(_discord, msg);
            var argPos = 0;

            if (msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
#if DEBUG

                var user = context.User as SocketGuildUser;

                if (!user.Roles.Any(role => _config["admin-roles"].Contains(role.Name)))
                {
                    await context.Channel.SendMessageAsync("Sorry, but I'm running on debug mode. Only admin roles can use my commands.");
                    return;
                }

#endif
                var result = await _commands.ExecuteAsync(context, argPos, _provider);
                var sb = new StringBuilder();

                if (!result.IsSuccess)
                {
                    switch (result.ToString())
                    {
                        default: sb.Append(result.ToString()); break;
                        case ERROR_UNKNOWN_COMMAND: sb.Append($"Unknown command, to begin try: ```py\n@{_config.BotName()} help\n```"); break;
                        case ERROR_BAD_ARGS_COUNT_FEW:
                        case ERROR_BAD_ARGS_COUNT_MANY:
                            {
                                var cmd = _commands.Search(context, argPos).Commands.First().Command;
                                var builder = new EmbedBuilder() { Color = new Color(114, 137, 218) };
                                builder.AddField(field =>
                                {
                                    field.Name = string.Join(", ", cmd.Aliases);
                                    field.Value = $"**Usage:** `{cmd.Name}`{string.Join(" ", cmd.Parameters.Select(p => $" `<{p.Name}>`"))}\n**Description:** {cmd.Summary}";
                                    field.IsInline = false;
                                });

                                await context.Channel.SendMessageAsync("", false, builder.Build());
                            }
                            return;
                    }

                    await context.Channel.SendMessageAsync(sb.ToString());
                }
            }
        }
    }
}
