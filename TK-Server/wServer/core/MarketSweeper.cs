using common.database;
using System.Timers;

namespace wServer
{
    public sealed class MarketSweeper
    {
        public MarketSweeper(Database database)
        {
            Database = database;
            Timer = new Timer(60000);
            Timer.Elapsed += Timer_Elapsed;
        }

        private Database Database { get; set; }
        private Timer Timer { get; set; }

        public void Start() => Timer.Start();

        public void Stop() => Timer.Stop();

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) => DbMarketData.CleanMarket(Database);
    }
}
