using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace tk.bot.handler
{
    public sealed class ReconnectHandler : CoreHandler
    {
        private const int RECONNECT_TTL = 15;

        private readonly ulong _guildId;
        private readonly Func<Task> _reconnect;

        private bool _reconnecting = false;

        public ReconnectHandler(IConfigurationRoot config, DiscordSocketClient discord)
            : base(config, discord, TimeSpan.FromSeconds(RECONNECT_TTL))
        {
            _guildId = _config["guild-id"].ToUnsignedLong();
            _reconnect += OnReconnectAsync;
        }

        private void HandleReconnect(bool guildCheck = false)
        {
            var guildConnected = true;

            if (guildCheck)
            {
                var guild = _discord.GetGuild(_guildId);

                guildConnected = guild.IsConnected;
            }

            if (_discord.ConnectionState == ConnectionState.Disconnected || !guildConnected)
            {
                _reconnecting = true;
                _reconnect.Invoke();
            }
        }

        protected override void CoreRoutine(int delta)
        {
            if (_reconnecting)
                return;

            switch (_discord.ConnectionState)
            {
                case ConnectionState.Connected: HandleReconnect(true); break;
                case ConnectionState.Disconnected: HandleReconnect(); break;
            }
        }

        private async Task OnReconnectAsync()
        {
            if (_discord.ConnectionState != ConnectionState.Connected)
                Log.Warn("Reconnecting to Discord API");
            else
                Log.Warn($"Reconnecting to Guild {_guildId}");

            await _discord.LogoutAsync();
            await _discord.StopAsync();
            await _discord.LoginAsync(TokenType.Bot, _config["token"]);
            await _discord.StartAsync();
            await _discord.SetGameAsync(_config["game"], null, ActivityType.Watching);
            await _discord.SetStatusAsync(UserStatus.DoNotDisturb);

            _reconnecting = false;
        }
    }
}
