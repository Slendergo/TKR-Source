using dungeonGen.definitions;

namespace dungeonGen.templates.PirateCave
{
    public class Background : MapRender
    {
        public override void Rasterize() => Rasterizer.Clear(new DungeonTile { TileType = PirateCaveTemplate.ShallowWater });
    }
}
