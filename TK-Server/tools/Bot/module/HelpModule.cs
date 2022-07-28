using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace tk.bot.module
{
    [Name("Help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
#pragma warning disable
        private readonly IConfigurationRoot _config;
#pragma warning enable
        private readonly CommandService _service;

        public HelpModule(CommandService service, IConfigurationRoot config)
        {
            _service = service;
            _config = config;
        }

        [Command("help")]
        [Summary("Show a list of all commands")]
        public async Task HelpAsync()
        {
            var builder = new EmbedBuilder() { Color = new Color(114, 137, 218) };

            foreach (var module in _service.Modules)
            {
                string description = null;

                foreach (var cmd in module.Commands)
                {
                    var result = await cmd.CheckPreconditionsAsync(Context);

                    if (result.IsSuccess)
                    {
                        var aliases = cmd.Aliases.ToArray();

                        if (aliases.Length == 1)
                            description += $"`{aliases[0]}`\n";
                        else
                        {
                            var aliasesRemain = aliases.Skip(1).ToArray();

                            description += $"`{aliases[0]}` (alias{(aliasesRemain.Length > 1 ? "es" : "")}: `{string.Join("`, `", aliasesRemain)}`)\n";
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(description))
                    builder.AddField(field =>
                    {
                        var value = string.IsNullOrWhiteSpace(module.Summary) ? description : $"__Description:__ {module.Summary}\n\n{description}\n";

                        field.Name = module.Name;
                        field.Value = value;
                        field.IsInline = false;
                    });
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("link")]
        [Summary("Get all links used to connect to TK Realms (browser or Flash Projector)")]
        public async Task LinkAsync()
        {
            var builder = new EmbedBuilder() { Color = new Color(114, 137, 218) };
            var domains = _config["domains"].Split("§");
            var path = _config["client-path"];

            builder.AddField(field =>
            {
                field.Name = "Browser";
                field.Value = $":link: **Option 1:** {domains[0]}\n:link: **Option 2:** {domains[1]}";
                field.IsInline = false;
            });
            builder.AddField(field =>
            {
                field.Name = "Flash Projector";
                field.Value = $":link: **Option 1:** {domains[0] + path}\n:link: **Option 2:** {domains[1] + path}";
                field.IsInline = false;
            });

            await ReplyAsync("", false, builder.Build());
        }
    }
}
