using common.database;
using common.isc.data;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace common.isc
{
    public class ISManager : InterServerChannel, IDisposable
    {
        public EventHandler NewServer;
        public EventHandler ServerPing;
        public EventHandler ServerQuit;

        private const int PingPeriod = 2000;
        private const int ServerTimeout = 30000;

        private static readonly string AppEngineTitleFormat = string.Format("[AppEngine] Servers: {0} | Connections: {1} of {2}",
            ISTextKeys.SERVER_AMOUNT,
            ISTextKeys.CONNECTIONS,
            ISTextKeys.TOTAL_CONNECTIONS
        );

        private static readonly string GameServerTitleFormat = string.Format("[GameServer] Name: {0} | Connections: {1} of {2} | Access: {3}",
            ISTextKeys.SERVER_NAME,
            ISTextKeys.CONNECTIONS,
            ISTextKeys.TOTAL_CONNECTIONS,
            ISTextKeys.GAME_ACCESS
        );

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly bool _isAppEngine;

        private string _accessType;
        private string _consoleTitlePattern;
        private object _dicLock = new object();
        private long _lastPing;
        private Dictionary<string, int> _lastUpdateTime = new Dictionary<string, int>();
        private Dictionary<string, ServerInfo> _servers = new Dictionary<string, ServerInfo>();
        private ServerConfig _settings;
        private Timer _tmr = new Timer(PingPeriod);

        public ISManager(Database db, ServerConfig settings, bool isAppEngine = false) : base(db, settings.serverInfo.instanceId)
        {
            _settings = settings;
            _isAppEngine = isAppEngine;

            if (isAppEngine)
            {
                _consoleTitlePattern = AppEngineTitleFormat;
                _accessType = "any";
            }
            else
            {
                _consoleTitlePattern = GameServerTitleFormat;
                _accessType = _settings.serverInfo.adminOnly ? "Admin" : (_settings.serverSettings.donorOnly ? "Donor" : "Public");
            }

            // kind of fucked up to do this, but can't really think of another way
            db.SetISManager(this);

            // listen to "network" communications
            AddHandler<NetworkMsg>(Channel.Network, HandleNetwork);

            // tell other servers listening that we've join the network
            Publish(Channel.Network, new NetworkMsg()
            {
                Code = NetworkCode.Join,
                Info = _settings.serverInfo
            });
        }

        public string AnnounceInstance(string user, string message)
        {
            using (TimedLock.Lock(_dicLock))
            {
                var serverInfos = _servers.Values.Where(server => server.type == ServerType.World).ToArray();

                if (serverInfos.Length == 0)
                    return "There is no connected server to AppEngine to publish announcement.";

                for (var i = 0; i < serverInfos.Length; i++)
                    Publish(Channel.Announce, new AnnounceMsg() { User = user, Message = message }, serverInfos[i].instanceId);

                return $"Announcement published to **{serverInfos.Length}** connected server{(serverInfos.Length > 1 ? "s" : "")}.";
            }
        }

        public void Dispose() => Shutdown();

        public string GetAppEngineInstance()
        {
            using (TimedLock.Lock(_dicLock))
                return _servers.Values.SingleOrDefault(_ => _.type == ServerType.Account)?.instanceId;
        }

        public string[] GetServerGuids()
        {
            using (TimedLock.Lock(_dicLock))
                return _servers.Keys.ToArray();
        }

        public ServerInfo GetServerInfo(string instanceId)
        {
            using (TimedLock.Lock(_dicLock))
                return _servers.ContainsKey(instanceId) ? _servers[instanceId] : null;
        }

        public ServerInfo[] GetServerList()
        {
            using (TimedLock.Lock(_dicLock))
                return _servers.Values.OrderBy(_ => _.port).ToArray();
        }

        public void Initialize()
        {
            _tmr.Elapsed += (sender, e) => Tick(PingPeriod);
            _tmr.Start();
        }

        public string RestartInstance(string serverName, string user)
        {
            using (TimedLock.Lock(_dicLock))
            {
                var serverInfo = _servers.Values.SingleOrDefault(server => server.name.ToLower().Equals(serverName));

                if (serverInfo != default && serverInfo.type == ServerType.World)
                {
                    Publish(Channel.Restart, new RestartMsg() { User = user }, serverInfo.instanceId);
                    return "Restarting server in 1 minute.";
                }
                else
                    return "Unable to contact server!";
            }
        }

        public void Shutdown()
        {
            _tmr.Stop();

            Publish(Channel.Network, new NetworkMsg()
            {
                Code = NetworkCode.Quit,
                Info = _settings.serverInfo
            });
        }

        public void Tick(int elapsedMs)
        {
            try
            {
                using (TimedLock.Lock(_dicLock))
                {
                    Console.Title = GetFormattedTitle();

                    // update running time
                    _lastPing += elapsedMs;

                    foreach (var s in _lastUpdateTime.Keys.ToArray())
                        _lastUpdateTime[s] += elapsedMs;

                    if (_lastPing < PingPeriod)
                        return;

                    _lastPing = 0;

                    // notify other servers we're still alive. Update info in the process.
                    Publish(Channel.Network, new NetworkMsg()
                    {
                        Code = NetworkCode.Ping,
                        Info = _settings.serverInfo
                    });

                    // check for server timeouts
                    foreach (var s in _lastUpdateTime.Where(s => s.Value > ServerTimeout).ToArray())
                    {
                        var sInfo = _servers[s.Key];

                        RemoveServer(s.Key);

                        // invoke server quit event
                        var networkMsg = new NetworkMsg()
                        {
                            Code = NetworkCode.Timeout,
                            Info = sInfo
                        };

                        ServerQuit?.Invoke(this, new InterServerEventArgs<NetworkMsg>(s.Key, networkMsg));
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        private bool AddServer(string instanceId, ServerInfo info)
        {
            if (_servers.ContainsKey(instanceId))
                return false;

            UpdateServer(instanceId, info);

            return true;
        }

        private string GetFormattedTitle() => _isAppEngine
            ? _consoleTitlePattern
                .Replace(ISTextKeys.SERVER_AMOUNT, (_servers.Count - 1).ToString())
                .Replace(ISTextKeys.CONNECTIONS, _settings.serverInfo.players.ToString())
                .Replace(ISTextKeys.TOTAL_CONNECTIONS, _settings.serverInfo.maxPlayers.ToString())
            : _consoleTitlePattern
                .Replace(ISTextKeys.SERVER_NAME, _settings.serverInfo.name)
                .Replace(ISTextKeys.CONNECTIONS, _settings.serverInfo.players.ToString())
                .Replace(ISTextKeys.TOTAL_CONNECTIONS, _settings.serverInfo.maxPlayers.ToString())
                .Replace(ISTextKeys.GAME_ACCESS, _accessType);

        private void HandleNetwork(object sender, InterServerEventArgs<NetworkMsg> e)
        {
            using (TimedLock.Lock(_dicLock))
            {
                switch (e.Content.Code)
                {
                    case NetworkCode.Join:
                        if (AddServer(e.InstanceId, e.Content.Info))
                        {
                            // make new server aware of this server
                            Publish(Channel.Network, new NetworkMsg()
                            {
                                Code = NetworkCode.Join,
                                Info = _settings.serverInfo
                            });

                            NewServer?.Invoke(this, e);
                        }
                        else
                            UpdateServer(e.InstanceId, e.Content.Info);

                        break;

                    case NetworkCode.Ping:
                        if (!_servers.ContainsKey(e.InstanceId))
                            Log.Info("{0} ({1}) re-joined the network.", e.Content.Info.name, e.InstanceId);
                        UpdateServer(e.InstanceId, e.Content.Info);
                        ServerPing?.Invoke(this, e);
                        break;

                    case NetworkCode.Quit:
                        Log.Info("{0} ({1}) left the network.", e.Content.Info.name, e.InstanceId);
                        RemoveServer(e.InstanceId);
                        ServerQuit?.Invoke(this, e);
                        break;
                }
            }
        }

        private void RemoveServer(string instanceId)
        {
            _servers.Remove(instanceId);
            _lastUpdateTime.Remove(instanceId);
        }

        private void UpdateServer(string instanceId, ServerInfo info)
        {
            _servers[instanceId] = info;
            _lastUpdateTime[instanceId] = 0;
        }

        private struct ISTextKeys
        {
            public const string CONNECTIONS = "{CONNECTIONS}";
            public const string GAME_ACCESS = "{ACCESS_LEVEL}";
            public const string SERVER_AMOUNT = "{SERVER}";
            public const string SERVER_NAME = "{NAME}";
            public const string TOTAL_CONNECTIONS = "{TOTAL_CONNECTIONS}";
        }
    }
}
