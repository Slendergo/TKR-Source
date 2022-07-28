using dungeonGen.definitions;
using Json;
using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dungeonGen
{
    public static class JsonMap
    {
        public static DungeonTile[,] Load(string json)
        {
            var map = (JsonObject)SimpleJson.DeserializeObject(json);
            var w = (uint)(long)map["width"];
            var h = (uint)(long)map["height"];
            var result = new DungeonTile[w, h];
            var tiles = new Dictionary<ushort, DungeonTile>();

            var id = ushort.MinValue;

            foreach (JsonObject tile in (JsonArray)map["dict"])
            {
                var mapTile = new DungeonTile();
                var tileType = (string)tile.GetValueOrDefault("ground", "Space");

                mapTile.TileType = new TileType(tileType == "Space" ? 0xfe : (uint)tileType.GetHashCode(), tileType);
                mapTile.Region = tile.ContainsKey("regions") ? (string)((JsonObject)((JsonArray)tile["regions"])[0])["id"] : null;

                if (tile.ContainsKey("objs"))
                {
                    var obj = (JsonObject)((JsonArray)tile["objs"])[0];
                    var tileObj = new DungeonObject { ObjectType = new ObjectType((uint)((string)obj["id"]).GetHashCode(), (string)obj["id"]) };
                    if (obj.ContainsKey("name"))
                    {
                        var attrs = (string)obj["name"];

                        tileObj.Attributes = attrs.Split(';')
                            .Where(attr => !string.IsNullOrEmpty(attr))
                            .Select(attr => attr.Split(':'))
                            .Select(attr => new KeyValuePair<string, string>(attr[0], attr[1]))
                            .ToArray();
                    }
                    else
                        tileObj.Attributes = Empty<KeyValuePair<string, string>>.Array;

                    mapTile.Object = tileObj;
                }
                else
                    mapTile.Object = null;

                tiles[id++] = mapTile;
            }

            var data = RotMG.Common.IO.Zlib.Decompress(Convert.FromBase64String((string)map["data"]));
            var index = 0;

            for (var y = 0; y < h; y++)
                for (var x = 0; x < w; x++)
                    result[x, y] = tiles[(ushort)((data[index++] << 8) | data[index++])];

            return result;
        }

        public static string Save(DungeonTile[,] map)
        {
            var w = map.GetUpperBound(0) + 1;
            var h = map.GetUpperBound(1) + 1;
            var tiles = new JsonArray();
            var indexLookup = new Dictionary<DungeonTile, short>(new TileComparer());
            var data = new byte[w * h * 2];
            var ptr = 0;

            for (var y = 0; y < h; y++)
                for (var x = 0; x < w; x++)
                {
                    var tile = map[x, y];

                    if (!indexLookup.TryGetValue(tile, out short index))
                    {
                        indexLookup.Add(tile, index = (short)tiles.Count);
                        tiles.Add(tile);
                    }

                    data[ptr++] = (byte)(index >> 8);
                    data[ptr++] = (byte)(index & 0xff);
                }

            for (int i = 0; i < tiles.Count; i++)
            {
                var tile = (DungeonTile)tiles[i];
                var jsonTile = new JsonObject { ["ground"] = tile.TileType.Name };

                if (!string.IsNullOrEmpty(tile.Region))
                {
                    var region = new JsonObject { { "id", tile.Region } };

                    jsonTile["regions"] = new JsonArray { region };
                }

                if (tile.Object != null)
                {
                    var obj = new JsonObject { { "id", tile.Object.ObjectType.Name } };

                    if (tile.Object.Attributes.Length > 0)
                    {
                        var objAttrs = tile.Object.Attributes.Select(kvp => kvp.Key + ":" + kvp.Value).ToArray();

                        obj["name"] = string.Join(",", objAttrs);
                    }

                    jsonTile["objs"] = new JsonArray { obj };
                }

                tiles[i] = jsonTile;
            }

            var mapObj = new JsonObject
            {
                ["width"] = w,
                ["height"] = h,
                ["dict"] = tiles,
                ["data"] = Convert.ToBase64String(Zlib.Compress(data))
            };

            return mapObj.ToString();
        }

        private struct TileComparer : IEqualityComparer<DungeonTile>
        {
            public bool Equals(DungeonTile x, DungeonTile y) => x.TileType == y.TileType && x.Region == y.Region && x.Object == y.Object;

            public int GetHashCode(DungeonTile obj)
            {
                var code = (int)obj.TileType.Id;

                if (obj.Region != null)
                    code = code * 7 + obj.Region.GetHashCode();

                if (obj.Object != null)
                    code = code * 13 + obj.Object.GetHashCode();

                return code;
            }
        }
    }
}
