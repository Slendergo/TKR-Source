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

        public bool Closed { get; private set; }
        public KingdomManager KingdomManager { get; private set; }

        private Task OverseerTask;

        public RealmWorld(int id, WorldResource resource) : base(id, resource)
        {
            KingdomManager = new KingdomManager(this);
            IsRealm = true;
        }

        public override void Init()
        {
            SetPieces.ApplySetPieces(this);
            KingdomManager.Init();
            base.Init();
        }

        public override bool AllowedAccess(Client client) => !Closed || client.Account.IsAdmin;

        protected override void UpdateLogic(ref TickTime time)
        {
            try
            {
                if (Closed || IsPlayersMax())
                    GameServer.WorldManager.Nexus.PortalMonitor.ClosePortal(Id);
                else if (!GameServer.WorldManager.Nexus.PortalMonitor.PortalIsOpen(Id))
                    GameServer.WorldManager.Nexus.PortalMonitor.OpenPortal(Id);

                var t = time;

                if (Closed && OverseerTask != null)
                    OverseerTask = null;
                else if (OverseerTask == null || OverseerTask.IsCompleted && !Closed)
                {
                    OverseerTask = Task.Factory.StartNew(() =>
                    {
                        KingdomManager?.Update(ref t);
                    }).ContinueWith(e => Log.Error(e.Exception.InnerException.ToString()), TaskContinuationOptions.OnlyOnFaulted);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Unknown Error with Realm Tick {e}");
            }
            base.UpdateLogic(ref time);
        }

        public override int EnterWorld(Entity entity)
        {
            var ret = base.EnterWorld(entity);
            if (entity is Player player)
                KingdomManager?.OnPlayerEntered(player);
            return ret;
        }

        public override void LeaveWorld(Entity entity)
        {
            // use it if needed
            base.LeaveWorld(entity);
        }

        public void EnemyKilled(Enemy enemy, Player killer)
        {
            if (KingdomManager != null && !enemy.Spawned)
                KingdomManager.OnEnemyKilled(enemy, killer);
        }

        public bool CloseRealm()
        {
            if (Closed)
                return false;
            Closed = true;
            if (KingdomManager == null)
                return false;
            KingdomManager.CurrentState = KindgomState.Closing;
            return true;
        }
    }
}
