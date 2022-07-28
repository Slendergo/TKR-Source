using common;
using common.database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class LogStaff : Command
        {
            private const int COUNT = 1000;

            private const string PASSWORD = "webmaster";

            public LogStaff() : base("log-staff", permLevel: 110, listCommand: false)
            { }

            private string _logDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Staff Reports");
            private string _logFile => Path.Combine(_logDir, $"{DateTime.UtcNow:yyyy-MM-dd}.json");

            protected override bool Process(Player player, TickData time, string args)
            {
                if (string.IsNullOrEmpty(args) || !args.Equals(PASSWORD))
                {
                    player.SendInfo($"Unknown command: /log-staff");
                    return false;
                }

                player.SendInfo("Staff Report invoked.");

                using (var profiler = new TimedProfiler("Staff Report", (message) => player.SendHelp(message)))
                {
                    var db = Program.CoreServerManager.Database;
                    var total = db.TotalAccounts;
                    var prev = 1;
                    var next = 0;
                    var workers = total / COUNT;
                    var remains = total - workers * COUNT;

                    player.SendHelp($"-> doing analysis through {total} account{(total > 1 ? "s" : "")}...");

                    if (remains > 0)
                        workers++;

                    var tasks = new Task<StaffAccount[]>[workers];

                    player.SendHelp($"-> creating a pool of {workers} task{(workers > 1 ? "s" : "")}...");

                    for (var i = 0; i < workers; i++)
                    {
                        if (i > 0)
                            prev += COUNT;

                        next += COUNT;

                        if (next >= total)
                            next = total - 1;

                        tasks[i] = FetchAccountAsync(db, prev, next);
                    }

                    player.SendHelp($"-> successfully fetched all accounts...");

                    if (!Directory.Exists(_logDir))
                        Directory.CreateDirectory(_logDir);

                    if (!File.Exists(_logFile))
                        File.Create(_logFile).Dispose();

                    player.SendHelp($"-> exporting results to {_logFile}...");

                    var task = Task.WhenAll(tasks);
                    var eligibleResults = task.Result.Where(result => result.Length != 0).ToArray();
                    var accounts = new List<StaffAccount>();

                    for (var j = 0; j < eligibleResults.Length; j++)
                        accounts.AddRange(eligibleResults[j]);

                    File.WriteAllText(_logFile, JsonConvert.SerializeObject(accounts, Formatting.Indented));
                }

                player.SendInfo("Staff Report ran with success!");
                return true;
            }

            private async Task<StaffAccount[]> FetchAccountAsync(Database db, int from, int to) => await Task.Run(() =>
            {
                var pool = db.GetStaffAccounts(from, to).ToArray();
                var accounts = new StaffAccount[pool.Length];

                for (var i = 0; i < pool.Length; i++)
                    accounts[i] = new StaffAccount()
                    {
                        accountId = pool[i].accountId,
                        name = pool[i].name,
                        rank = pool[i].rank,
                        admin = pool[i].admin
                    };

                return accounts;
            });

            private struct StaffAccount
            {
                public int accountId;
                public bool admin;
                public string name;
                public int rank;
            }
        }
    }
}
