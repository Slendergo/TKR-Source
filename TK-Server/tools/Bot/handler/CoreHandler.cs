using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using tk.bot.ca.threading.tasks;

namespace tk.bot.handler
{
    public abstract class CoreHandler : ICoreHandler
    {
        protected readonly IConfigurationRoot _config;
        protected readonly DiscordSocketClient _discord;

        private readonly int _timeout;

        private InternalRoutine _routine;
#pragma warning disable
        private CancellationToken _token;
#pragma warning enable

        protected CoreHandler(IConfigurationRoot config, DiscordSocketClient discord)
        {
            _config = config;
            _discord = discord;
            _timeout = -1;
        }

        protected CoreHandler(IConfigurationRoot config, DiscordSocketClient discord, TimeSpan timeout)
        {
            _config = config;
            _discord = discord;
            _timeout = (int)timeout.TotalMilliseconds;
        }

        public CancellationToken GetToken => _token;

        protected string Name => GetType().Name;

        public void AttachToParent(CancellationToken token) => _token = token;

        public virtual void Execute()
        {
            if (_timeout == -1)
            {
                Log.Error("Invalid callback!", new ArgumentException("Method 'Execute' shouldn't be invoked when 'timeout' isn't declared.", "timeout"));
                return;
            }

            Log.Info($"{Name}: Ready");

            _routine = new InternalRoutine(_timeout, CoreRoutine, (message) => Log.Info(message));
            _routine.AttachToParent(GetToken);
            _routine.OnDeltaVariation += (s, e) => Log.Info($"{Name} -> delta: {e.Delta}ms, timeout: {e.Timeout}, tps: {e.TicksPerSecond}");
            _routine.OnFinished += (s, e) => Log.Info($"{Name}... finished!");
            _routine.Start();
        }

        protected virtual void CoreRoutine(int delta)
        { }
    }
}
