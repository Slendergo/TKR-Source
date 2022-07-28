using System;
using System.Text;

namespace wServer.core
{
    public class DbEvents
    {
        public DbEvents(CoreServerManager manager)
        {
            var db = manager.Database;

            // setup event for expiring keys
            db.Sub.Subscribe($"__keyevent@{db.DatabaseIndex}__:expired", (s, buff) =>
            {
                Expired?.Invoke(this, new DbEventArgs(Encoding.UTF8.GetString(buff)));
            });
        }

        public event EventHandler<DbEventArgs> Expired;
    }
}
