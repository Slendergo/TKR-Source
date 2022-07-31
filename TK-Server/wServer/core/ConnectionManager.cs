using CA.Extensions.Concurrent;
using common.isc.data;
using common.resources;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using wServer.core.worlds;
using wServer.core.worlds.logic;
using wServer.networking;
using wServer.networking.connection;
using wServer.networking.packets.outgoing;

namespace wServer.core
{
    public sealed class ConnectionManager
    {
        public CoreServerManager CoreServerManager;

        private const int CONNECTING_TTL = 15;
        private const int RECON_TTL = 15;

        private int NextClientId;

        public ConnectionManager(CoreServerManager coreServerManager) => CoreServerManager = coreServerManager;

        public ConcurrentDictionary<Client, PlayerInfo> Clients { get; set; }
        private ConcurrentDictionary<Client, DateTime> Connecting { get; set; }
        private ConnectionListener ConnectionListener { get; set; }
        private long LastTickTime { get; set; }
        private int MaxPlayerCount { get; set; }
        private ConcurrentDictionary<int, ReconInfo> Reconnecting { get; set; }

        public void AddConnection(ConnectionInfo connectionInfo)
        {
            if (connectionInfo == null)
                return;

            if (connectionInfo.Account == null)
            {
                connectionInfo.Client.Disconnect("Account is null in Add()");
                return;
            }

            if (!connectionInfo.Account.NameChosen)
            {
                connectionInfo.Client.SendFailure("Choose a name first!.");
                return;
            }

            if (connectionInfo.Reconnecting || connectionInfo.Account.Admin || PlayerCount() < MaxPlayerCount)
            {
                HandleConnect(connectionInfo);
                return;
            }

            connectionInfo.Client.SendFailure("Server at max capacity.");
        }

        public void AddReconnect(int accountId, Reconnect rcp)
        {
            if (rcp == null)
                return;

            var rInfo = new ReconInfo(rcp.GameId, rcp.Key, DateTime.Now.AddSeconds(RECON_TTL));
            Reconnecting.TryAdd(accountId, rInfo);
        }

        public void ClientConnected(Client client)
        {
            Connecting.TryRemove(client, out _); // _ is a discard
            // update PlayerInfo with world data
            var plrInfo = Clients[client];
            plrInfo.WorldInstance = client.Player.World.Id;
            plrInfo.WorldName = client.Player.World.IdName;
        }

        public void Disconnect(Client client)
        {
            var player = client.Player;
            player?.World?.LeaveWorld(player);

            Clients.TryRemove(client, out var playerInfo);

            // recalculate usage statistics
            CoreServerManager.ServerConfig.serverInfo.players = PlayerCount();
            CoreServerManager.ServerConfig.serverInfo.maxPlayers = MaxPlayerCount;
            CoreServerManager.ServerConfig.serverInfo.playerList.Remove(playerInfo);
        }

        public void HandleConnect(ConnectionInfo connectionInfo)
        {
            var client = connectionInfo.Client;
            var acc = connectionInfo.Account;
            var gameId = connectionInfo.GameId;

            if (connectionInfo.Reconnecting)
            {
                if (acc == null || connectionInfo == null || client == null)
                    return;

                if (!Reconnecting.TryRemove(acc.AccountId, out var rInfo))
                {
                    client.SendFailure("Invalid reconnect.", Failure.MessageWithDisconnect);
                    return;
                }

                if (!gameId.Equals(rInfo.Destination))
                {
                    client.SendFailure("Invalid reconnect destination.", Failure.MessageWithDisconnect);
                    return;
                }

                if (!connectionInfo.Key.SequenceEqual(rInfo.Key))
                {
                    client.SendFailure("Invalid reconnect key.", Failure.MessageWithDisconnect);
                    return;
                }
            }
            else
            {
                if (gameId != World.Test)
                    gameId = World.Nexus;
            }

            if (!client.CoreServerManager.Database.AcquireLock(acc))
            {
                // disconnect current connected client (if any)
                var otherClients = Clients.KeyWhereAsParallel(_ => _ == client || _.Account != null && (_.Account.AccountId == acc.AccountId));
                foreach (var otherClient in otherClients)
                    otherClient.Disconnect("!client.Manager.Database.AcquireLock(acc)");

                // try again...
                if (!client.CoreServerManager.Database.AcquireLock(acc))
                {
                    client.SendFailure("Account in Use (" + client.CoreServerManager.Database.GetLockTime(acc)?.ToString("%s") + " seconds until timeout)");
                    return;
                }
            }

            acc.Reload(); // make sure we have the latest data
            client.Account = acc;

            // connect client to realm manager
            if (!client.CoreServerManager.ConnectionManager.TryConnect(client))
            {
                client.SendFailure("Failed to connect");
                return;
            }

            var world = client.CoreServerManager.WorldManager.GetWorld(gameId);

            if (world == null || world.Deleted)
            {
                client.SendPacket(new Text()
                {
                    BubbleTime = 0,
                    NumStars = -1,
                    Name = "*Error*",
                    Txt = "World does not exist."
                });
                world = client.CoreServerManager.WorldManager.GetWorld(World.Nexus);
            }

            if (world is TestWorld && !acc.Admin)
            {
                client.SendFailure("Only players with admin permissions can make test maps.", Failure.MessageWithDisconnect);
                return;
            }

            if (!world.AllowedAccess(client) && world.InstanceType != WorldResourceInstanceType.Guild)
            {
                if (!world.Persist && world.TotalConnects <= 0)
                    client.CoreServerManager.WorldManager.RemoveWorld(world);

                client.SendPacket(new Text()
                {
                    BubbleTime = 0,
                    NumStars = -1,
                    Name = "*Error*",
                    Txt = "Access denied"
                });

                if (!(world is NexusWorld))
                    world = client.CoreServerManager.WorldManager.GetWorld(World.Nexus);
                else
                {
                    client.Disconnect("world is Nexus");
                    return;
                }
            }

            if (world is TestWorld)
            {
                var mapFolder = $"{CoreServerManager.ServerConfig.serverSettings.logFolder}/maps";

                if (!Directory.Exists(mapFolder))
                    Directory.CreateDirectory(mapFolder);

                System.IO.File.WriteAllText($"{mapFolder}/{acc.Name}_{DateTime.Now.Ticks}.jm", connectionInfo.MapInfo);

                try
                {
                    (world as TestWorld).LoadJson(connectionInfo.MapInfo);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            var seed = (uint)((long)Environment.TickCount * connectionInfo.GUID.GetHashCode()) % uint.MaxValue;

            client.Random = new wRandom(seed);
            client.TargetWorld = world.Id;

            if (!acc.Hidden)
            {
                acc.RefreshLastSeen();
                acc.FlushAsync();
            }

            // send out map info
            var mapSize = Math.Max(world.Map.Width, world.Map.Height);

            client.SendPackets(new OutgoingMessage[]
            {
                new MapInfo()
                {
                    Music = world.Music,
                    Width = mapSize,
                    Height = mapSize,
                    Name = world.IdName,
                    DisplayName = world.DisplayName,
                    Seed = seed,
                    Background = world.Background,
                    Difficulty = world.Difficulty,
                    AllowPlayerTeleport = world.AllowTeleport,
                    ShowDisplays = world.ShowDisplays,
                },
                new AccountList() // send out account lock/ignore list
                {
                    AccountListId = 0, // locked list
                    AccountIds = client.Account.LockList.Select(i => i.ToString()).ToArray()
                },
                new AccountList()
                {
                    AccountListId = 1, // ignore list
                    AccountIds = client.Account.IgnoreList.Select(i => i.ToString()).ToArray()
                }
            });
            client.State = ProtocolState.Handshaked;

            Connecting.TryAdd(client, DateTime.Now.AddSeconds(CONNECTING_TTL));
        }

        public void Initialize()
        {
            MaxPlayerCount = CoreServerManager.ServerConfig.serverSettings.maxPlayers;
            Reconnecting = new ConcurrentDictionary<int, ReconInfo>();
            Connecting = new ConcurrentDictionary<Client, DateTime>();
            Clients = new ConcurrentDictionary<Client, PlayerInfo>();
            ConnectionListener = new ConnectionListener(this);
            ConnectionListener.Initialize();
        }

        public int PlayerCount() => Clients.Count + Reconnecting.Count;

        public void Shutdown() => ConnectionListener.Shutdown();

        public void StartListening() => ConnectionListener.StartListening();

        public void Tick(long time)
        {
            if (time - LastTickTime > 5000)
            {
                LastTickTime = time;

                var dateTime = DateTime.Now;

                // process reconnect timeouts
                foreach (var r in Reconnecting.Where(r => DateTime.Compare(r.Value.Timeout, dateTime) < 0))
                    Reconnecting.TryRemove(r.Key, out var ignored);

                // process connecting timeouts
                // for those that go through the connection process but never send a Create or Load packet
                foreach (var c in Connecting.Where(c => DateTime.Compare(c.Value, dateTime) < 0))
                    Connecting.TryRemove(c.Key, out var ignored);
            }
        }

        public bool TryConnect(Client client)
        {
            if (client?.Account == null)
                return false;

            client.Id = Interlocked.Increment(ref NextClientId);

            var playerInfo = new PlayerInfo()
            {
                AccountId = client.Account.AccountId,
                GuildId = client.Account.GuildId,
                Name = client.Account.Name,
                WorldInstance = -1
            };

            Clients[client] = playerInfo;

            // recalculate usage statistics
            CoreServerManager.ServerConfig.serverInfo.players = PlayerCount();
            CoreServerManager.ServerConfig.serverInfo.maxPlayers = MaxPlayerCount;
            CoreServerManager.ServerConfig.serverInfo.playerList.Add(playerInfo);

            return true;
        }
    }
}
