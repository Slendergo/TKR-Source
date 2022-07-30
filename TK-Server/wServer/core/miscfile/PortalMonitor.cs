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

        private CoreServerManager _manager;
        private Dictionary<int, Portal> _portals;
        private Random _rand;
        private World _world;

        public PortalMonitor(CoreServerManager manager, World world)
        {
            _manager = manager;
            _world = world;
            _portals = new Dictionary<int, Portal>();
            _rand = new Random();
        }

        public bool AddPortal(int worldId)
        {
            if (_world == null)
                return false;

            if (_portals.ContainsKey(worldId))
                return false;

            var currWorld = _manager.WorldManager.GetWorld(worldId);
            if(currWorld == null)
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
                
            return true;
        }

        public void ClosePortal(int worldId)
        {
            if (_world == null)
                return;

                if (!_portals.ContainsKey(worldId))
                    return;

                var portal = _portals[worldId];
                if (portal.Usable)
                    portal.Usable = false;
        }

        public void OpenPortal(int worldId)
        {
            if (_world == null)
                return;

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

        public bool RemovePortal(int worldId)
        {
            if (!_portals.ContainsKey(worldId))
                if (_world == null)
                    return false;

                if (!_portals.ContainsKey(worldId))
                    return false;

                var portal = _portals[worldId];

                _world.LeaveWorld(portal);
                _portals.Remove(worldId);
                return true;
        }

        public bool RemovePortal(Portal portal)
        {
            if (_world == null)
                return false;

                if (!_portals.ContainsValue(portal))
                    return false;

                var worldId = _portals.FirstOrDefault(p => p.Value == portal).Key;
                _world.LeaveWorld(portal);
                _portals.Remove(worldId);

                return true;
        }

        public bool RemovePortal(World world)
        {
            if (_world == null)
                return false;

                var portal = _portals.FirstOrDefault(p => p.Value.WorldInstance == world);

                if (portal.Value == null)
                    return false;

                _world.LeaveWorld(portal.Value);
                _portals.Remove(portal.Key);

                return true;
        }

        public void RenamePortal(int worldId, string name)
        {
            if (_world == null)
                return;

                if (_portals.ContainsKey(worldId) && !_portals[worldId].Name.Equals(name))
                    _portals[worldId].Name = name;
        }

        public void Update(ref TickTime time)
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

        public void UpdateWorldInstance(int worldId, World world)
        {
            if (_world == null)
                return;

                if (!_portals.ContainsKey(worldId))
                    return;

                _portals[worldId].WorldInstance = world;
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
