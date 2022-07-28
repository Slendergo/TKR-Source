using common.resources;
using wServer.networking;

namespace wServer.core.worlds.logic
{
    public class Marketplace : World
    {
        public Marketplace(ProtoWorld proto, Client client = null) : base(proto) => IsDungeon = false;
    }
}
