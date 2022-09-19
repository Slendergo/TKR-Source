using TKR.Shared.resources;

namespace TKR.WorldServer.core.terrain
{
    public class WmapDesc
    {
        public byte Elevation;
        public string ObjCfg;
        public ObjectDesc ObjDesc;
        public ushort ObjType;
        public TileRegion Region;
        public TerrainType Terrain;
        public TileDesc TileDesc;
        public ushort TileId;
    }
}
