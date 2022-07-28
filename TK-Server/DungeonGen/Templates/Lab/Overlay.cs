using dungeonGen.definitions;

namespace dungeonGen.templates.Lab
{
    public class Overlay : MapRender
    {
        public override void Rasterize()
        {
            var wall = new DungeonTile
            {
                TileType = LabTemplate.Space,
                Object = new DungeonObject { ObjectType = LabTemplate.LabWall }
            };

            var w = Rasterizer.Width;
            var h = Rasterizer.Height;
            var buf = Rasterizer.Bitmap;

            for (var x = 0; x < w; x++)
                for (var y = 0; y < h; y++)
                {
                    if (buf[x, y].TileType != LabTemplate.Space || buf[x, y].Object != null)
                        continue;

                    var isWall = false;

                    if (x == 0 || y == 0 || x + 1 == w || y + 1 == h)
                        isWall = false;
                    else
                    {
                        for (var dx = -1; dx <= 1 && !isWall; dx++)
                            for (var dy = -1; dy <= 1 && !isWall; dy++)
                                if (buf[x + dx, y + dy].TileType != LabTemplate.Space)
                                {
                                    isWall = true;
                                    break;
                                }
                    }

                    if (isWall)
                        buf[x, y] = wall;
                }
        }
    }
}
