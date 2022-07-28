using common.resources;
using System.IO;
using wServer.networking;

namespace wServer.core.worlds.logic
{
    public class GuildHall : World
    {
        public GuildHall(ProtoWorld proto, Client client = null) : base(proto)
        {
            if (client == null)
                return;

            GuildId = client.Account.GuildId;
            IsDungeon = false;
        }

        public int GuildId { get; private set; }

        public override bool AllowedAccess(Client client) => base.AllowedAccess(client) && client.Account.GuildId == GuildId;

        public override World GetInstance(Client client)
        {
            var manager = client.CoreServerManager;
            var guildId = client.Account.GuildId;
            var worlds = manager.WorldManager.GetWorlds(); // join existing if possible
            foreach (var world in worlds)
            {
                if (!(world is GuildHall) || (world as GuildHall).GuildId != guildId)
                    continue;

                if (world.Players.Count > 0)
                    return world;

                manager.WorldManager.RemoveWorld(world);
                break; // if empty guild hall, reset by making new one
            }

            // create new instance of guild hall
            var gHall = new GuildHall(manager.Resources.Worlds[Name], client)
            { IsLimbo = false };

            return Manager.WorldManager.AddWorld(gHall);
        }

        protected override void Init()
        {
            switch (Level())
            {
                case 0: FromWorldMap(new MemoryStream(Manager.Resources.Worlds[Name].wmap[0])); break;
                case 1: FromWorldMap(new MemoryStream(Manager.Resources.Worlds[Name].wmap[1])); break;
                case 2: FromWorldMap(new MemoryStream(Manager.Resources.Worlds[Name].wmap[2])); break;
                case 3: FromWorldMap(new MemoryStream(Manager.Resources.Worlds[Name].wmap[3])); break;
            }
        }

        private int Level()
        {
            var guild = Manager.Database.GetGuild(GuildId);
            return (guild != null) ? guild.Level : 0;
        }
    }
}
