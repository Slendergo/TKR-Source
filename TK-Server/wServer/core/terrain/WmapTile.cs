using common;
using common.resources;
using System;
using System.Collections.Generic;
using wServer.core.objects;

namespace wServer.core.terrain
{
    public class WmapTile
    {
        public byte Elevation;
        public string ObjCfg;
        public ObjectDesc ObjDesc;
        public int ObjId;
        public ushort ObjType;
        public TileRegion Region;
        public long SightRegion = 1;
        public bool Spawned;
        public TerrainType Terrain;
        public TileDesc TileDesc;
        public ushort TileId;
        public ObjectDef? ToDefine;
        public byte UpdateCount;
        public short X;
        public short Y;

        private WmapDesc _originalDesc;

        public WmapTile(WmapDesc desc, short x, short y)
        {
            _originalDesc = desc;

            Reset();
        }

        public WmapTile Clone() => new WmapTile(_originalDesc, X, Y)
        {
            UpdateCount = (byte)(UpdateCount + 1),
            TileId = _originalDesc.TileId,
            TileDesc = _originalDesc.TileDesc,
            ObjType = _originalDesc.ObjType,
            ObjDesc = ObjDesc,
            ObjCfg = _originalDesc.ObjCfg,
            Terrain = _originalDesc.Terrain,
            Region = _originalDesc.Region,
        };

        public void CopyTo(WmapTile tile)
        {
            tile.TileId = TileId;
            tile.TileDesc = TileDesc;
            tile.ObjType = ObjType;
            tile.ObjDesc = ObjDesc;
            tile.ObjCfg = ObjCfg;
            tile.Terrain = Terrain;
            tile.Region = Region;
            tile.Elevation = Elevation;
        }

        public void InitConnection(Wmap map, int x, int y)
        {
            if (ObjDesc == null || !ObjDesc.Connects || ObjCfg.Contains("conn:"))
                return;

            var connStr = ConnectionComputer.GetConnString((dx, dy) => map.Contains(x + dx, y + dy) && map[x + dx, y + dy].ObjType == ObjDesc.ObjectType);

            ObjCfg = $"{ObjCfg};{connStr};";
        }

        public void Reset(Wmap map = null, int x = 0, int y = 0)
        {
            TileId = _originalDesc.TileId;
            TileDesc = _originalDesc.TileDesc;
            ObjType = _originalDesc.ObjType;
            ObjDesc = _originalDesc.ObjDesc;
            ObjCfg = _originalDesc.ObjCfg;
            Terrain = _originalDesc.Terrain;
            Region = _originalDesc.Region;
            Elevation = _originalDesc.Elevation;

            if (map != null)
                InitConnection(map, x, y);

            UpdateCount++;
        }

        public void SetTile(WmapTile tile)
        {
            TileId = tile.TileId;
            TileDesc = tile.TileDesc;
            ObjType = tile.ObjType;
            ObjDesc = tile.ObjDesc;
            ObjCfg = tile.ObjCfg;
            Terrain = tile.Terrain;
            Region = tile.Region;
            Elevation = tile.Elevation;

            UpdateCount++;
        }

        public ObjectDef ToObjectDef(int x, int y)
        {
            if (ToDefine != null)
                return ToDefine.Value;

            var stats = new List<KeyValuePair<StatDataType, object>>();

            if (!string.IsNullOrEmpty(ObjCfg))
                foreach (var item in ObjCfg.Split(';'))
                {
                    var kv = item.Split(':');

                    switch (kv[0])
                    {
                        case "hp":
                            var hp = Utils.GetInt(kv[1]);
                            stats.Add(new KeyValuePair<StatDataType, object>(StatDataType.HP, hp));
                            stats.Add(new KeyValuePair<StatDataType, object>(StatDataType.MaximumHP, hp));
                            break;

                        case "name":
                            stats.Add(new KeyValuePair<StatDataType, object>(StatDataType.Name, kv[1]));
                            break;

                        case "size":
                            stats.Add(new KeyValuePair<StatDataType, object>(StatDataType.Size, Math.Min(500, Utils.GetInt(kv[1]))));
                            break;

                        case "eff":
                            stats.Add(new KeyValuePair<StatDataType, object>(StatDataType.Effects, Utils.GetInt(kv[1])));
                            break;

                        case "conn":
                            stats.Add(new KeyValuePair<StatDataType, object>(StatDataType.ObjectConnection, Utils.GetInt(kv[1])));
                            break;

                        case "mtype":
                            stats.Add(new KeyValuePair<StatDataType, object>(StatDataType.MerchantMerchandiseType, Utils.GetInt(kv[1])));
                            break;

                        case "mcost":
                            stats.Add(new KeyValuePair<StatDataType, object>(StatDataType.SellablePrice, Math.Max(0, Utils.GetInt(kv[1]))));
                            break;

                        case "mcur":
                            stats.Add(new KeyValuePair<StatDataType, object>(StatDataType.SellablePriceCurrency, Utils.GetInt(kv[1])));
                            break;

                        case "mamnt":
                            stats.Add(new KeyValuePair<StatDataType, object>(StatDataType.MerchantRemainingCount, Utils.GetInt(kv[1])));
                            break;

                        case "mtime":
                            stats.Add(new KeyValuePair<StatDataType, object>(StatDataType.MerchantRemainingMinute, Utils.GetInt(kv[1])));
                            break;

                        case "mdisc":
                            stats.Add(new KeyValuePair<StatDataType, object>(StatDataType.MerchantDiscount, Utils.GetInt(kv[1])));
                            break;

                        case "mrank":
                        case "stars":
                            stats.Add(new KeyValuePair<StatDataType, object>(StatDataType.SellableRankRequirement, Utils.GetInt(kv[1])));
                            break;
                    }
                }

            ToDefine = new ObjectDef()
            {
                ObjectType = ObjType,
                Stats = new ObjectStats()
                {
                    Id = ObjId,
                    Position = new Position()
                    {
                        X = x + 0.5f,
                        Y = y + 0.5f
                    },
                    Stats = stats.ToArray()
                }
            };

            return ToDefine.Value;
        }
    }
}
