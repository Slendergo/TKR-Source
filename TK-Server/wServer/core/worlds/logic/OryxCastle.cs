using common.resources;
using System.Collections.Generic;
using System.Linq;
using wServer.networking;

namespace wServer.core.worlds.logic
{
    internal class OryxCastle : World
    {
        private readonly int PlayersEntering;

        public OryxCastle(ProtoWorld proto, int playersEntering) : base(proto) => PlayersEntering = playersEntering;

        // this is to get everyone in one spawn when the Castle comes from quaking
        public OryxCastle(ProtoWorld proto, Client client = null) : base(proto) => PlayersEntering = 0;

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
