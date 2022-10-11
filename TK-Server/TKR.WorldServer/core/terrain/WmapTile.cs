using System;
using System.Collections.Generic;
using TKR.Shared;
using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.stats;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.objects.connection;
using TKR.WorldServer.core.miscfile.structures;

namespace TKR.WorldServer.core.terrain
{
    public class WmapTile
    {
        public short X;
        public short Y;

        public ushort TileId;

        // static entity stuff
        public int ObjId;
        public ushort ObjType;
        public string ObjCfg;
        public ObjectDesc ObjDesc;

        public TileRegion Region;
        public TerrainType Terrain;
        public ObjectDef? ToDefine;
        public byte Elevation;
        public byte UpdateCount = 1;
        public bool Spawned;

        public WmapTile(WmapDesc _originalDesc)
        {
            TileId = _originalDesc.TileId;
            ObjType = _originalDesc.ObjType;
            ObjDesc = _originalDesc.ObjDesc;
            ObjCfg = _originalDesc.ObjCfg;
            Terrain = _originalDesc.Terrain;
            Region = _originalDesc.Region;
            Elevation = _originalDesc.Elevation;
        }

        public void InitConnection(Wmap map, int x, int y)
        {
            if (ObjDesc == null || !ObjDesc.Connects || ObjCfg.Contains("conn:"))
                return;

            var connStr = ConnectionComputer.GetConnString((dx, dy) => map.Contains(x + dx, y + dy) && map[x + dx, y + dy].ObjType == ObjType);
            ObjCfg = $"{ObjCfg};{connStr};";
        }

        public void CopyTo(WmapTile tile)
        {
            tile.TileId = TileId;
            tile.ObjType = ObjType;
            tile.ObjDesc = ObjDesc;
            tile.ObjCfg = ObjCfg;
            tile.Terrain = Terrain;
            tile.Region = Region;
            tile.Elevation = Elevation;
        }

        public ObjectDef ToObjectDef(int x, int y)
        {
            if (ToDefine != null)
                return ToDefine.Value;

            var stats = new List<ValueTuple<StatDataType, object>>();

            if (!string.IsNullOrEmpty(ObjCfg))
                foreach (var item in ObjCfg.Split(';'))
                {
                    var kv = item.Split(':');

                    switch (kv[0])
                    {
                        case "hp":
                            var hp = Utils.GetInt(kv[1]);
                            stats.Add(ValueTuple.Create(StatDataType.Health, hp));
                            stats.Add(ValueTuple.Create(StatDataType.MaximumHeath, hp));
                            break;

                        case "name":
                            stats.Add(ValueTuple.Create(StatDataType.Name, kv[1]));
                            break;

                        case "size":
                            stats.Add(ValueTuple.Create(StatDataType.Size, Math.Min(500, Utils.GetInt(kv[1]))));
                            break;

                        case "eff":
                            stats.Add(ValueTuple.Create(StatDataType.ConditionBatch1, Utils.GetInt(kv[1])));
                            break;

                        case "conn":
                            stats.Add(ValueTuple.Create(StatDataType.ObjectConnection, Utils.GetInt(kv[1])));
                            break;

                        case "mtype":
                            stats.Add(ValueTuple.Create(StatDataType.MerchandiseType, Utils.GetInt(kv[1])));
                            break;

                        case "mcost":
                            stats.Add(ValueTuple.Create(StatDataType.MerchandisePrice, Math.Max(0, Utils.GetInt(kv[1]))));
                            break;

                        case "mcur":
                            stats.Add(ValueTuple.Create(StatDataType.MerchandiseCurrency, Utils.GetInt(kv[1])));
                            break;

                        case "mamnt":
                            stats.Add(ValueTuple.Create(StatDataType.MerchandiseCount, Utils.GetInt(kv[1])));
                            break;

                        case "mtime":
                            stats.Add(ValueTuple.Create(StatDataType.MerchandiseMinsLeft, Utils.GetInt(kv[1])));
                            break;

                        case "mdisc":
                            stats.Add(ValueTuple.Create(StatDataType.MerchandiseDiscount, Utils.GetInt(kv[1])));
                            break;

                        case "mrank":
                        case "stars":
                            stats.Add(ValueTuple.Create(StatDataType.MerchandiseRankReq, Utils.GetInt(kv[1])));
                            break;
                    }
                }

            ToDefine = new ObjectDef()
            {
                ObjectType = ObjType,
                Stats = new ObjectStats()
                {
                    Id = ObjId,
                    X = x + 0.5f,
                    Y = y + 0.5f,
                    Stats = stats
                }
            };

            return ToDefine.Value;
        }
    }
}
