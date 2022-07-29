using common.resources;
using System.Collections.Generic;
using System.Linq;
using wServer.networking;

namespace wServer.core.worlds.logic
{
    internal class OryxCastle : World
    {
        private readonly int PlayersEntering;

        public OryxCastle(int id, WorldResource resource, int playersEntering) : base(id, resource) => PlayersEntering = playersEntering;

        public override KeyValuePair<IntPoint, TileRegion>[] GetSpawnPoints()
        {
            if (PlayersEntering < 20)
                return Map.Regions.Where(t => t.Value == TileRegion.Spawn).Take(1).ToArray();
            else if (PlayersEntering < 40)
                return Map.Regions.Where(t => t.Value == TileRegion.Spawn).Take(2).ToArray();
            else if (PlayersEntering < 60)
                return Map.Regions.Where(t => t.Value == TileRegion.Spawn).Take(3).ToArray();
            else
                return Map.Regions.Where(t => t.Value == TileRegion.Spawn).ToArray();
        }
    }
}
