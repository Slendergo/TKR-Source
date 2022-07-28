using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using tk.bot.handler;

namespace tk.bot.service
{
    public class StartupService
    {
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;
        private readonly DiscordSocketClient _discord;
        private readonly List<ICoreHandler> _handlers;
        private readonly IServiceProvider _provider;
        private readonly CancellationTokenSource _source;

        private bool _initializing = true;

        public StartupService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, IConfigurationRoot config)
        {
            _provider = provider;
            _config = config;
            _discord = discord;
            _commands = commands;

            _handlers = new List<ICoreHandler>();
            _source = new CancellationTokenSource();

            _discord.Connected += OnConnectedAsync;
            _discord.Disconnected += OnDisconnectedAsync;
        }

        public async Task StartAsync()
        {
            var token = _config["token"];

            if (string.IsNullOrWhiteSpace(token))
                throw new FileLoadException("Please enter bot's token into the `settings.yml` file found in the applications root directory.");

            _config["token-hash"] = token.ComputeTokenHash();
            _config.ConcatArray("admin-roles");
            _config.ConcatArray("notification-roles");
            _config.ConcatArray("domains");
            _config["notification-indexes"] = _config["notification-roles"].GenerateIndexes();

            await _discord.LoginAsync(TokenType.Bot, _config["token"]);
            await _discord.StartAsync();
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            await _discord.SetGameAsync(_config["game"], null, ActivityType.Watching);
            await _discord.SetStatusAsync(UserStatus.Online);
        }

        private async Task OnConnectedAsync()
        {
            _config["bot-name"] = $"{_discord.CurrentUser.Username}#{_discord.CurrentUser.DiscriminatorValue}";

            Log.Info($"{_config["bot-name"]} {(_initializing ? "connected" : "reconnected")}!");

            if (_initializing)
                await Task.Run(() =>
                {
                    _initializing = false;

                    _handlers.AddRange(new ICoreHandler[] {
                        new ReconnectHandler(_config, _discord),
                        new RestartHandler(_config, _discord)
                    });

                    Thread.Sleep(10000);

                    _handlers.ForEach(handler =>
                    {
                        handler.AttachToParent(_source.Token);

                        handler.Execute();
                    });
                }, _source.Token);

            await Task.Delay(-1);
        }

        private async Task OnDisconnectedAsync(Exception e)
        {
            Log.Error($"{_config["bot-name"]} disconnected!", e);

            await Task.Delay(-1);
        }
    }
}
