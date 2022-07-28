using common.resources;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;
using wServer.core.objects;
using wServer.core.setpieces;
using wServer.networking;

namespace wServer.core.worlds.logic
{
    public class Realm : World
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static string[] _realmNames =
                {
            "Lich", "Goblin", "Ghost",
            "Giant", "Gorgon","Blob",
            "Leviathan", "Unicorn", "Minotaur",
            "Cube", "Pirate", "Spider",
            "Snake", "Deathmage", "Gargoyle",
            "Scorpion", "Djinn", "Phoenix",
            "Satyr", "Drake", "Orc",
            "Flayer", "Cyclops", "Sprite",
            "Chimera", "Kraken", "Hydra",
            "Slime", "Ogre", "Hobbit",
            "Titan", "Medusa", "Golem",
            "Demon", "Skeleton", "Mummy",
            "Imp", "Bat", "Wyrm",
            "Spectre", "Reaper", "Beholder",
            "Dragon", "Harpy"
        };

        private readonly int _mapId;
        private readonly bool _oryxPresent;

        private Oryx _overseer;
        private Task _overseerTask;

        public Realm(ProtoWorld proto, Client client = null) : base(proto)
        {
            _oryxPresent = true;
            _mapId = 1;

            IsRealm = true;
            IsDungeon = false;

            MaxPlayers = 85;
        }

        // since map gets reset, admins not allowed to join when closed. Possible to crash server otherwise.
        public override bool AllowedAccess(Client client) => !Closed && base.AllowedAccess(client);

        public bool CloseRealm()
        {
            if (_overseer == null)
                return false;

            _overseer.InitCloseRealm();

            return true;
        }

        public void EnemyKilled(Enemy enemy, Player killer)
        {
            if (_overseer != null && !enemy.Spawned)
                _overseer.OnEnemyKilled(enemy, killer);
        }

        public override int EnterWorld(Entity entity)
        {
            var ret = base.EnterWorld(entity);

            if (entity is Player player)
                _overseer?.OnPlayerEntered(player);

            return ret;
        }

        public bool IsClosing()
        {
            if (_overseer == null)
                return false;

            return _overseer.Closing;
        }

        public override bool Tick(TickData time)
        {
            try
            {
                base.Tick(time);

                if (Closed || IsPlayersMax())
                    Manager.WorldManager.PortalMonitor.ClosePortal(Id);
                else if (!Manager.WorldManager.PortalMonitor.PortalIsOpen(Id))
                    Manager.WorldManager.PortalMonitor.OpenPortal(Id);

                if (IsLimbo || Deleted)
                    return true;

                if (_overseerTask == null || _overseerTask.IsCompleted)
                {
                    _overseerTask = Task.Factory.StartNew(() =>
                    {
                        if (Closed && Players.Count == 0 && _overseer != null)
                        {
                            Init(); // will reset everything back to the way it was when made
                            Closed = false;
                        }

                        _overseer?.Tick(time);
                    }).ContinueWith(e => Log.Error(e.Exception.InnerException.ToString()), TaskContinuationOptions.OnlyOnFaulted);
                }
            }
            catch (Exception e) { Log.Error($"Unknown Error with Realm Tick {e}"); }

            return false;
        }

        protected override void Init()
        {
            var random = new Random();
            SBName = _realmNames[random.Next(0, _realmNames.Length)];
            FromWorldMap(new MemoryStream(Manager.Resources.Worlds["Realm"].wmap[_mapId - 1]));

            SetPieces.ApplySetPieces(this);

            if (_oryxPresent)
            {
                _overseer = new Oryx(this);
                _overseer.Init();
            }
        }
    }
}
