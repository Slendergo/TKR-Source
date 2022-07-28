using System;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class ReplaceTile : Behavior
    {
        private readonly string _objName;
        private readonly int _range;
        private readonly string _replacedObjName;

        public ReplaceTile(string objName, string replacedObjName, int range)
        {
            _objName = objName;
            _range = range;
            _replacedObjName = replacedObjName;
        }

        protected override void OnStateEntry(Entity host, TickData time, ref object state)
        {
            var dat = host.CoreServerManager.Resources.GameData;
            var tileId = dat.IdToTileType[_objName];
            var replacedTileId = dat.IdToTileType[_replacedObjName];
            var map = host.Owner.Map;
            var w = map.Width;
            var h = map.Height;

            for (var y = 0; y < h; y++)
                for (var x = 0; x < w; x++)
                {
                    var tile = map[x, y];

                    if (tile.TileId != tileId || tile.TileId == replacedTileId)
                        continue;

                    var dx = Math.Abs(x - (int)host.X);
                    var dy = Math.Abs(y - (int)host.Y);

                    if (dx > _range || dy > _range)
                        continue;

                    tile.TileId = replacedTileId;

                    var tileDesc = Program.CoreServerManager.Resources.GameData.Tiles[tile.TileId];

                    tile.TileDesc = tileDesc;

                    if (tile.ObjId == 0)
                        tile.ObjId = host.Owner.GetNextEntityId();

                    map[x, y].SetTile(tile);
                }
        }

        protected override void TickCore(Entity host, TickData time, ref object state)
        { }
    }
}
