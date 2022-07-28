using CA.Extensions.Concurrent;
using System.Linq;
using System.Text;
using wServer.core;
using wServer.core.objects;
using wServer.core.worlds.logic;

namespace wServer.logic.behaviors
{
    internal class AnnounceOnDeath : Behavior
    {
        public static readonly string PLAYER_COUNT = "{COUNT}";
        public static readonly string PLAYER_LIST = "{PL_LIST}";
        private readonly string _message;

        public AnnounceOnDeath(string msg) => _message = msg;

        protected internal override void Resolve(State parent) => parent.Death += (sender, e) =>
        {
            if (e.Host.Spawned || e.Host.Owner is Test)
                return;

            var owner = e.Host.Owner;
            var sb = new StringBuilder();
            var players = owner.Players.ValueWhereAsParallel(_ => _.Client != null);
            for (var i = 0; i < players.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                sb.Append(players[i].Name);
            }

            var playerList = sb.ToString();
            var playerCount = owner.Players.Count(p => p.Value.Client != null).ToString();
            var announcement = _message.Replace(PLAYER_COUNT, playerCount).Replace(PLAYER_LIST, playerList);
            e.Host.CoreServerManager.ChatManager.Announce(announcement);
        };

        protected override void TickCore(Entity host, TickData time, ref object state)
        { }
    }
}
