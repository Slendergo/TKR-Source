using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using tk.bot.ca.threading.tasks;

namespace tk.bot.handler
{
    public sealed class RestartHandler : CoreHandler
    {
        private const int RESTART_TTL = 1;

        private readonly AutomatedRestarter _restarter;

        public RestartHandler(IConfigurationRoot config, DiscordSocketClient discord)
            : base(config, discord)
        {
            _restarter = new AutomatedRestarter(TimeSpan.FromHours(RESTART_TTL), 1000, (message) => Log.Error(message));
            _restarter.OnFinished += async (s, e) =>
            {
                await _discord.SetStatusAsync(UserStatus.DoNotDisturb);

                var bot = _config["bot-name"];

                Log.Warn($"{Name}: Preparing to restart {bot} systems...");

                var mre = new ManualResetEvent(false);
                var time = 3;
                var source = new CancellationTokenSource();
                var routine = new InternalRoutine(1000, () =>
                {
                    if (source.IsCancellationRequested)
                        return;

                    if (time == 0)
                    {
                        Log.Warn($"{Name}: Restarting {bot} systems now!");

                        source.Cancel();
                        return;
                    }

                    Log.Warn($"{Name}: Restarting {bot} systems within {time} second{(time > 1 ? "s" : "")}");

                    time--;
                });
                routine.AttachToParent(source.Token);
                routine.OnFinished += (s, e) => mre.Set();
                routine.Start();

                mre.WaitOne();

                await _discord.SetStatusAsync(UserStatus.AFK);

                Thread.Sleep(3000);

                var process = new Process();
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = Assembly.GetExecutingAssembly().Location;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                Environment.Exit(0);
            };
        }

        public override void Execute()
        {
            Log.Info($"{Name}: Ready");

            _restarter.Start();
        }
    }
}
