using common.resources;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.core.objects;
using wServer.core.worlds;

namespace wServer.core
{
    public class PortalMonitor
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private GameServer _manager;
        private Dictionary<int, Portal> _portals;
        private Random _rand;
        private World _world;

        private object access = new object();

        public PortalMonitor(GameServer manager, World world)
        {
            _manager = manager;
            _world = world;
            _portals = new Dictionary<int, Portal>();
            _rand = new Random();
        }

        public async void CreateNewRealm()
        {
            var world = await _manager.WorldManager.CreateNewRealmAsync();
            AddPortal(world.Id);
        }

        public bool AddPortal(int worldId)
        {
            lock (access)
            {
                if (_portals.ContainsKey(worldId))
                    return false;

                var currWorld = _manager.WorldManager.GetWorld(worldId);
                if (currWorld == null)
                {
                    Console.WriteLine("Unable to locate world");
                    return false;
                }

                var pos = GetRandPosition();

                var portal = new Portal(_manager, 0x0712, null)
                {
                    WorldInstance = currWorld,
                    Name = currWorld.GetDisplayName() + " (0)"
                };
                portal.SetDefaultSize(80);

                portal.Move(pos.X + 0.5f, pos.Y + 0.5f);

                _world.EnterWorld(portal);
                _portals.Add(worldId, portal);
            }
            return true;
        }

        public bool PortalIsOpen(int worldId)
        {
            if (_world == null)
                return false;

            lock (access)
            {
                if (!_portals.ContainsKey(worldId))
                    return false;
                return _portals[worldId].Usable && !_portals[worldId].Locked;
            }
        }

        public void ClosePortal(int worldId)
        {
            lock (access)
            {
                if (!_portals.ContainsKey(worldId))
                    return;

                var portal = _portals[worldId];
                if (portal.Usable)
                    portal.Usable = false;
            }
        }

        public void OpenPortal(int worldId)
        {
            lock (access)
            {
                try
                {
                    if (!_portals.ContainsKey(worldId))
                        return;

                    var portal = _portals[worldId];
                    if (!portal.Usable)
                        _portals[worldId].Usable = true;
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
            lock (access)
            {
                foreach (var p in _portals.Values)
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
            lock (access)
            {
                if (!_portals.TryGetValue(worldId, out var portal))
                    return;
                _world.LeaveWorld(portal);
                _portals.Remove(worldId);
            }
        }

        private Position GetRandPosition()
        {
            var x = 0;
            var y = 0;
            var realmPortalRegions = _world.Map.Regions.Where(t => t.Value == TileRegion.Realm_Portals).ToArray();

            if (realmPortalRegions.Length > _portals.Count)
            {
                KeyValuePair<IntPoint, TileRegion> sRegion;

                do
                {
                    sRegion = realmPortalRegions.ElementAt(_rand.Next(0, realmPortalRegions.Length));
                } 
                while (_portals.Values.Any(p => p.X == sRegion.Key.X + 0.5f && p.Y == sRegion.Key.Y + 0.5f));

                x = sRegion.Key.X;
                y = sRegion.Key.Y;
            }

            return new Position() { X = x, Y = y };
        }
    }
}
