using common.resources;
using wServer.networking;

namespace wServer.core.worlds.logic
{
    public class Marketplace : World
    {
        public Marketplace(int id, WorldResource resource, Client client = null) : base(id, resource) => IsDungeon = false;
    }
}
