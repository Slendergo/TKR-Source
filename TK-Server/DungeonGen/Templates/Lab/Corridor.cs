using dungeonGen.definitions;
using RotMG.Common.Rasterizer;

namespace dungeonGen.templates.Lab
{
    public class Corridor : MapCorridor
    {
        public override void Rasterize(Room src, Room dst, Point srcPos, Point dstPos) => Default(srcPos, dstPos, new DungeonTile { TileType = LabTemplate.LabFloor });
    }
}
