using common.resources;
using NLog;
using System;
using System.Threading.Tasks;
using wServer.core.objects;
using wServer.core.setpieces;
using wServer.networking;

namespace wServer.core.worlds.logic
{
    public class RealmWorld : World
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

        private readonly bool _oryxPresent;

        private KingdomManager _overseer;
        private Task _overseerTask;

        public RealmWorld(int id, WorldResource resource, Client client = null) : base(id, resource)
        {
            _oryxPresent = true;
            IsRealm = true;
        }

        // since map gets reset, admins not allowed to join when closed. Possible to crash server otherwise.
        public override bool AllowedAccess(Client client) => base.AllowedAccess(client);

        public bool CloseRealm()
        {
            if (_overseer == null)
                return false;
            if(_overseer.CurrentState != KindgomState.Expired)
                _overseer.CurrentState = KindgomState.Closing;
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

        protected override void UpdateLogic(ref TickTime time)
        {
            try
            {
                if (Closed || IsPlayersMax())
                    Manager.WorldManager.Nexus.PortalMonitor.ClosePortal(Id);
                else if (!Manager.WorldManager.Nexus.PortalMonitor.PortalIsOpen(Id))
                    Manager.WorldManager.Nexus.PortalMonitor.OpenPortal(Id);

                var t = time;
                if (_overseerTask == null || _overseerTask.IsCompleted)
                {
                    _overseerTask = Task.Factory.StartNew(() =>
                    {
                        _overseer?.Tick(ref t);
                    }).ContinueWith(e => Log.Error(e.Exception.InnerException.ToString()), TaskContinuationOptions.OnlyOnFaulted);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Unknown Error with Realm Tick {e}");
            }

            base.UpdateLogic(ref time);
        }

        static Random Random = new Random();

        public override void Init()
        {
            DisplayName = _realmNames[Random.Next(0, _realmNames.Length)];

            SetPieces.ApplySetPieces(this);

            if (_oryxPresent)
            {
                _overseer = new KingdomManager(this);
                _overseer.Init();
            }
            base.Init();
        }
    }
}
