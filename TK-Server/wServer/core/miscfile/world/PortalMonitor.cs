using common.resources;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.core.objects;
using wServer.core.worlds;
using wServer.utils;

namespace wServer.core
{
    public class KingdomPortalMonitor
    {
        public const int MAX_PER_REALM = 85;

        private readonly GameServer GameServer;
        private readonly Dictionary<int, Portal> Portals = new Dictionary<int, Portal>();
        private readonly World World;

        private readonly object Access = new object();
        private Random Random = new Random();

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

        private static readonly List<string> Actives = new List<string>();

        public KingdomPortalMonitor(GameServer manager, World world)
        {
            GameServer = manager;
            World = world;
        }

        public void CreateNewRealm()
        {
            lock (Access)
            {
                var name = Names[Random.Next(Names.Count)];
                Actives.Add(name);
                GameServer.WorldManager.CreateNewRealmAsync(name);
            }
        }

        public void AddPortal(World world)
        {
            lock (Access)
            {
                if (Portals.ContainsKey(world.Id))
                    return;

                var pos = GetRandPosition();

                var portal = new Portal(GameServer, 0x0712, null)
                {
                    WorldInstance = world,
                    Name = world.GetDisplayName() + $" (0/{MAX_PER_REALM})"
                };

                portal.SetDefaultSize(80);

                portal.Move(pos.X + 0.5f, pos.Y + 0.5f);

                _ = World.EnterWorld(portal);
                Portals.Add(world.Id, portal);
            }
        }

        public bool PortalIsOpen(int worldId)
        {
            lock (Access)
            {
                if (!Portals.ContainsKey(worldId))
                    return false;
                return Portals[worldId].Usable && !Portals[worldId].Locked;
            }
        }

        public void OpenPortal(int worldId)
        {
            lock (Access)
            {
                if (!Portals.ContainsKey(worldId))
                    return;

                var portal = Portals[worldId];
                if (!portal.Usable)
                    Portals[worldId].Usable = true;
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

        public void Update(ref TickTime time)
        {
            lock (Access)
            {
                CreateRealmIfExists();

                foreach (var p in Portals.Values)
                {
                    //if (p.WorldInstance == null || p.WorldInstance.Deleted)
                    //    continue;

                    var count = p.WorldInstance.Players.Count;
                    p.WorldInstance.GetPlayerCount(ref count);

                    var updatedCount = $"{p.WorldInstance.GetDisplayName()} ({Math.Min(count, p.WorldInstance.MaxPlayers)}/{p.WorldInstance.MaxPlayers})";

                    if (p.Name.Equals(updatedCount))
                        continue;
                    p.Name = updatedCount;
                }
            }
        }

        static int i = -1;
        private void CreateRealmIfExists()
        {
            if (Names.Count == 0 || Actives.Count >= 14)
                return;

            var totalPlayers = World.GameServer.ConnectionManager.GetPlayerCount();
            var realmsNeeded = 1 + totalPlayers / (MAX_PER_REALM + 15);
            if (Actives.Count < realmsNeeded)
            {
                CreateNewRealm();
                Console.WriteLine($"There are: {totalPlayers} player online right now: this requires: {realmsNeeded} realms and we have {Actives.Count}");
            }
        }

        public void RemovePortal(int worldId)
        {
            lock (Access)
            {
                if (!Portals.TryGetValue(worldId, out var portal))
                    return;

                var name = portal.WorldInstance.DisplayName;
                Actives.Remove(name);
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
                    sRegion = realmPortalRegions.ElementAt(Random.Next(0, realmPortalRegions.Length));
                } 
                while (Portals.Values.Any(p => p.X == sRegion.Key.X + 0.5f && p.Y == sRegion.Key.Y + 0.5f));

                x = sRegion.Key.X;
                y = sRegion.Key.Y;
            }

            return new Position() { X = x, Y = y };
        }
    }
}
