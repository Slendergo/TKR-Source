using TKR.Shared.resources;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.utils;
using TKR.WorldServer.core.structures;

namespace TKR.WorldServer.core.worlds
{
    public class KingdomPortalMonitor
    {
        public const int MAX_PER_REALM = 85;

        private static readonly List<string> _allowedNames = new List<string>()
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

        private static readonly List<string> _activeNames = [];

        private readonly GameServer _gameServer;
        private readonly World _world;
        private readonly Dictionary<int, Portal> _activePortals = [];

        private readonly object _lock = new object();

        public KingdomPortalMonitor(GameServer manager, World world)
        {
            _gameServer = manager;
            _world = world;
        }

        public void Update(ref TickTime time)
        {
            CreateRealmIfNeeded();
            UpdateRealmNames();
        }

        private void CreateRealmIfNeeded()
        {
            var activeCount = 0;
            var maxRealms = 0;

            lock (_lock)
            {
                activeCount = _activeNames.Count;
                maxRealms = _gameServer.Configuration.serverSettings.maxRealms;

                if (_allowedNames.Count == 0 || activeCount >= maxRealms)
                    return;
            }

            var totalPlayers = _world.GameServer.ConnectionManager.GetPlayerCount();
            var realmsNeeded = 3 + totalPlayers / (MAX_PER_REALM + 100);

            if (activeCount < realmsNeeded)
                CreateNewRealm();
        }

        public void CreateNewRealm()
        {
            var name = GetNewName();
            if (name == null)
                return;
            _gameServer.WorldManager.CreateNewRealmAsync(name);
        }

        private string GetNewName()
        {
            lock (_lock)
            {
                if (_allowedNames.Count == 0)
                    return null;

                var index = Random.Shared.Next(_allowedNames.Count);
                var name = _allowedNames[index];
                _allowedNames.RemoveAt(index);
                _activeNames.Add(name);
                return name;
            }
        }

        private void UpdateRealmNames()
        {
            Portal[] portals;
            lock (_lock)
                portals = [.. _activePortals.Values];

            for (var i = 0; i < portals.Length; i++)
            {
                var portal = portals[i];

                var count = 0;
                portal.WorldInstance.GetPlayerCount(ref count);

                var maxPlayers = portal.WorldInstance.MaxPlayers;
                count = Math.Min(count, maxPlayers);

                portal.Name = $"{portal.WorldInstance.GetDisplayName()} ({count}/{maxPlayers})";
            }
        }

        public void AddPortal(World world)
        {
            var pos = GetRandPosition();

            Portal portal = null;

            lock (_lock)
            {
                if (_activePortals.ContainsKey(world.Id))
                    return;

                portal = (Portal)_world.CreateNewEntity("Nexus Portal", pos.X + 0.5f, pos.Y + 0.5f);
                portal.WorldInstance = world;
                portal.Name = $"{world.GetDisplayName()} (0/{MAX_PER_REALM})";
                portal.SetDefaultSize(MAX_PER_REALM);
                _activePortals.Add(world.Id, portal);
            }

            if (portal != null)
                _world.EnterWorld(portal);
        }

        public bool PortalIsOpen(int worldId)
        {
            if (!TryGetPortal(worldId, out var portal))
                return false;
            return portal.Usable && !portal.Locked;
        }

        public void OpenPortal(int worldId)
        {
            if (TryGetPortal(worldId, out var portal))
                portal.Usable = true;
        }

        public void ClosePortal(int worldId)
        {
            if (TryGetPortal(worldId, out var portal))
                portal.Usable = false;
        }

        private bool TryGetPortal(int worldId, out Portal portal)
        {
            lock (_lock)
                return _activePortals.TryGetValue(worldId, out portal);
        }

        public void RemovePortal(int worldId)
        {
            Portal portal = null;

            lock (_lock)
            {
                if (!_activePortals.TryGetValue(worldId, out portal))
                    return;

                var name = portal.WorldInstance.DisplayName;
                _activeNames.Remove(name);
                _allowedNames.Add(name);
                _activePortals.Remove(worldId);
            }

            if (portal != null)
                _world.LeaveWorld(portal);
        }

        private Position GetRandPosition()
        {
            var x = 0;
            var y = 0;

            var realmPortalRegions = _world.Map.Regions.Where(t => t.Value == TileRegion.Realm_Portals).ToArray();
            if (realmPortalRegions.Length > 0)
            {
                var availablePositions = new List<IntPoint>();

                foreach (var region in realmPortalRegions)
                {
                    var posX = region.Key.X + 0.5f;
                    var posY = region.Key.Y + 0.5f;

                    var occupied = _activePortals.Values.Any(p =>
                        Math.Abs(p.X - posX) < 0.1f && Math.Abs(p.Y - posY) < 0.1f);

                    if (!occupied)
                        availablePositions.Add(region.Key);
                }

                if (availablePositions.Count > 0)
                {
                    var selectedPos = availablePositions[Random.Shared.Next(availablePositions.Count)];
                    x = selectedPos.X;
                    y = selectedPos.Y;
                }
                else
                {
                    StaticLogger.Instance.Warn("All realm portal positions are occupied, portals may overlap");
                    var fallbackRegion = realmPortalRegions[Random.Shared.Next(realmPortalRegions.Length)];
                    x = fallbackRegion.Key.X;
                    y = fallbackRegion.Key.Y;
                }
            }
            else
            {
                StaticLogger.Instance.Error("No Realm_Portals regions found in Nexus map!");
            }
            return new Position(x, y);
        }
    }
}