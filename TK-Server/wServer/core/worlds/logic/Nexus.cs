using common.resources;
using wServer.networking;

namespace wServer.core.worlds.logic
{
    internal class Nexus : World
    {
        public Nexus(ProtoWorld proto, Client client = null) : base(proto) => IsDungeon = false;

        protected override void Init()
        {
            base.Init();

            var monitor = Manager.WorldManager.PortalMonitor;
            var realms = Manager.WorldManager.GetRealms();
            foreach (var realm in realms)
                monitor.AddPortal(realm.Id);
        }
    }
}
