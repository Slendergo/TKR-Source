using common.resources;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.core.objects;
using wServer.core.worlds;

namespace wServer.core
{
    public class KingdomPortalMonitor
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly GameServer GameServer;
        private readonly Dictionary<int, Portal> Portals = new Dictionary<int, Portal>();
        private readonly World World;

        private readonly object Access = new object();
        private Random _rand = new Random();

        private static readonly List<string> Names = new List<string>()
        {
            "Meanem Empire",
            "Aidisha Empire",
            "Chasal Empire",
            "Upiria Kingdom",
            "Weaston Kingdom",
            "Shikecaea Dynasty",
            "Kreakimore Empire",
            "Ecechourean Empire",
            "Vrostudel Empire",
            "Raesrerin Kingdom",
            "Asoborg Kingdom",
            "Ochaitia Dynasty",
            "Purian Kingdom",
            "Yuiria Empire",
            "Exunao Dynasty",
            "Yullaicia Dynasty",
            "Poggisha Empire",
            "Grukhanid Empire",
            "Gaethibet Kingdom",
            "Daphethen Dynasty"
        };

        public KingdomPortalMonitor(GameServer manager, World world)
        {
            GameServer = manager;
            World = world;
        }

        public async void CreateNewRealm()
        {
            var world = await GameServer.WorldManager.CreateNewRealmAsync();
            AddPortal(world.Id);
        }

        public bool AddPortal(int worldId)
        {
            lock (Access)
            {
                if (Portals.ContainsKey(worldId))
                    return false;

                var currWorld = GameServer.WorldManager.GetWorld(worldId);
                if (currWorld == null)
                    return false;

                currWorld.DisplayName = Names[_rand.Next(0, Names.Count)];
                Names.Remove(currWorld.DisplayName);

                var pos = GetRandPosition();

                var portal = new Portal(GameServer, 0x0712, null)
                {
                    WorldInstance = currWorld,
                    Name = currWorld.GetDisplayName() + " (0)"
                };
                portal.SetDefaultSize(80);

                portal.Move(pos.X + 0.5f, pos.Y + 0.5f);

                World.EnterWorld(portal);
                Portals.Add(worldId, portal);
            }
            return true;
        }

        public bool PortalIsOpen(int worldId)
        {
            if (World == null)
                return false;

            lock (Access)
            {
                if (!Portals.ContainsKey(worldId))
                    return false;
                return Portals[worldId].Usable && !Portals[worldId].Locked;
            }
        }

        public void ClosePortal(int worldId)
        {
            lock (Access)
            {
                if (!Portals.ContainsKey(worldId))
                    return;

                var portal = Portals[worldId];
                if (portal.Usable)
                    portal.Usable = false;
            }
        }

        public void OpenPortal(int worldId)
        {
            lock (Access)
            {
                try
                {
                    if (!Portals.ContainsKey(worldId))
                        return;

                    var portal = Portals[worldId];
                    if (!portal.Usable)
                        Portals[worldId].Usable = true;
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    return;
                }
            }
        }

        public void Update(ref TickTime time)
        {
            lock (Access)
            {
                foreach (var p in Portals.Values)
                {
                    if (p.WorldInstance == null || p.WorldInstance.Deleted)
                        continue;

                    var count = p.WorldInstance.Players.Count;
                    var updatedCount = $"{p.WorldInstance.GetDisplayName()} ({Math.Min(count, p.WorldInstance.MaxPlayers)}/{p.WorldInstance.MaxPlayers})";

                    if (p.Name.Equals(updatedCount))
                        continue;
                    p.Name = updatedCount;
                }
            }
        }

        public void RemovePortal(int worldId)
        {
            lock (Access)
            {
                if (!Portals.TryGetValue(worldId, out var portal))
                    return;

                var name = portal.WorldInstance.DisplayName;
                Names.Add(name);

                World.LeaveWorld(portal);
                Portals.Remove(worldId);
            }
        }

        private Position GetRandPosition()
        {
            var x = 0;
            var y = 0;
            var realmPortalRegions = World.Map.Regions.Where(t => t.Value == TileRegion.Realm_Portals).ToArray();

            if (realmPortalRegions.Length > Portals.Count)
            {
                KeyValuePair<IntPoint, TileRegion> sRegion;

                do
                {
                    sRegion = realmPortalRegions.ElementAt(_rand.Next(0, realmPortalRegions.Length));
                } 
                while (Portals.Values.Any(p => p.X == sRegion.Key.X + 0.5f && p.Y == sRegion.Key.Y + 0.5f));

                x = sRegion.Key.X;
                y = sRegion.Key.Y;
            }

            return new Position() { X = x, Y = y };
        }
    }
}
