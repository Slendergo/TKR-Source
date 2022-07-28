using common;
using common.resources;
using dungeonGen.definitions;
using Ionic.Zlib;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using wServer.core.objects;
using wServer.core.objects.vendors;
using wServer.core.worlds;

namespace wServer.core.terrain
{
    public class Wmap
    {
        public Dictionary<IntPoint, TileRegion> Regions;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private XmlData _dat;
        private Tuple<IntPoint, ushort, string>[] _entities;
        private WmapTile[,] _tiles;

        public Wmap(XmlData dat)
        {
            _dat = dat;

            Regions = new Dictionary<IntPoint, TileRegion>();
        }

        public int Height { get; private set; }
        public int Width { get; private set; }
        public WmapTile this[int x, int y] { get => !Contains(x, y) ? null : _tiles[x, y]; set => _tiles[x, y] = value; }

        public bool Contains(IntPoint point)
        {
            var x = point.X;
            var y = point.Y;

            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return false;

            return true;
        }

        public bool Contains(float x, float y) => !(x < 0 || x >= Width || y < 0 || y >= Height);

        public bool Contains(int x, int y) => !(x < 0 || x >= Width || y < 0 || y >= Height);

        public IEnumerable<Entity> InstantiateEntities(CoreServerManager manager, IntPoint offset = new IntPoint())
        {
            foreach (var i in _entities)
            {
                var entity = Entity.Resolve(manager, i.Item2);
                entity.Move(i.Item1.X + 0.5f + offset.X, i.Item1.Y + 0.5f + offset.Y);

                if (i.Item3 != null)
                    foreach (var item in i.Item3.Split(';'))
                    {
                        var kv = item.Split(':');

                        switch (kv[0])
                        {
                            case "hp":
                                (entity as Enemy).HP = Utils.GetInt(kv[1]);
                                (entity as Enemy).MaximumHP = (entity as Enemy).HP;
                                break;

                            case "name":
                                entity.Name = kv[1]; break;
                            case "size":
                                entity.SetDefaultSize(Math.Min(500, Utils.GetInt(kv[1])));
                                break;

                            case "eff":
                                entity.ConditionEffects = (ConditionEffects)ulong.Parse(kv[1]);
                                break;

                            case "conn":
                                (entity as ConnectedObject).Connection = objects.ConnectionInfo.Infos[(uint)Utils.GetInt(kv[1])];
                                break;

                            case "mtype":
                                (entity as Merchant).Item = (ushort)Utils.GetInt(kv[1]);
                                break;

                            case "mcost":
                                (entity as SellableObject).Price = Math.Max(0, Utils.GetInt(kv[1]));
                                break;

                            case "mcur":
                                (entity as SellableObject).Currency = (CurrencyType)Utils.GetInt(kv[1]);
                                break;

                            case "mamnt":
                                (entity as Merchant).Count = Utils.GetInt(kv[1]);
                                break;

                            case "mtime":
                                (entity as Merchant).TimeLeft = Utils.GetInt(kv[1]);
                                break;

                            case "mdisc": // not implemented
                                break;

                            case "mrank":
                            case "stars": // provided for backwards compatibility with older maps
                                (entity as SellableObject).RankReq = Utils.GetInt(kv[1]);
                                break;

                            case "xOffset":
                                var xo = float.Parse(kv[1]);
                                entity.Move(entity.X + xo, entity.Y);
                                break;

                            case "yOffset":
                                var yo = float.Parse(kv[1]);
                                entity.Move(entity.X, entity.Y + yo);
                                break;

                            case "mtax":
                                (entity as SellableObject).Tax = Utils.FromString(kv[1]);
                                break;
                        }
                    }

                yield return entity;
            }
        }

        public int Load(Stream stream, int idBase)
        {
            var ver = stream.ReadByte();

            if (ver < 0 || ver > 2)
                throw new NotSupportedException("WMap version " + ver);

            using (var rdr = new BinaryReader(new ZlibStream(stream, CompressionMode.Decompress)))
            {
                var dict = new List<WmapDesc>();
                var c = rdr.ReadInt16();

                for (var i = 0; i < c; i++)
                {
                    var desc = new WmapDesc
                    {
                        TileId = rdr.ReadUInt16()
                    };

                    desc.TileDesc = _dat.Tiles[desc.TileId];

                    var obj = rdr.ReadString();

                    desc.ObjType = 0;

                    if (_dat.IdToObjectType.ContainsKey(obj))
                        desc.ObjType = _dat.IdToObjectType[obj];
                    else if (!string.IsNullOrEmpty(obj))
                        Log.Warn($"Object: {obj} not found.");

                    desc.ObjCfg = rdr.ReadString();
                    desc.Terrain = (TerrainType)rdr.ReadByte();
                    desc.Region = (TileRegion)rdr.ReadByte();

                    if (ver == 1)
                        desc.Elevation = rdr.ReadByte();

                    _dat.ObjectDescs.TryGetValue(desc.ObjType, out desc.ObjDesc);
                    dict.Add(desc);
                }

                Width = rdr.ReadInt32();
                Height = rdr.ReadInt32();

                _tiles = new WmapTile[Width, Height];

                var enCount = 0;
                var entities = new List<Tuple<IntPoint, ushort, string>>();

                for (short y = 0; y < Height; y++)
                    for (short x = 0; x < Width; x++)
                    {
                        var tile = new WmapTile(dict[rdr.ReadInt16()], x, y);

                        if (ver == 2)
                            tile.Elevation = rdr.ReadByte();

                        if (tile.Region != 0)
                            Regions.Add(new IntPoint(x, y), tile.Region);

                        var desc = tile.ObjDesc;

                        if (tile.ObjType != 0 && (desc == null || !desc.Static || desc.Enemy))
                        {
                            entities.Add(new Tuple<IntPoint, ushort, string>(new IntPoint(x, y), tile.ObjType, tile.ObjCfg));

                            if (desc == null || !(desc.Enemy && desc.Static))
                                tile.ObjType = 0;
                        }

                        if (tile.ObjType != 0 && (desc == null || !(desc.Enemy && desc.Static)))
                        {
                            enCount++;
                            tile.ObjId = idBase + enCount;
                        }

                        _tiles[x, y] = tile;
                    }

                for (var x = 0; x < Width; x++)
                    for (var y = 0; y < Height; y++)
                        _tiles[x, y].InitConnection(this, x, y);

                _entities = entities.ToArray();

                return enCount;
            }
        }

        public int Load(DungeonTile[,] tiles, int idBase)
        {
            Width = tiles.GetLength(0);
            Height = tiles.GetLength(1);

            var wTiles = new WmapDesc[Width, Height];

            for (var i = 0; i < Width; i++)
                for (var j = 0; j < Height; j++)
                {
                    var dTile = tiles[i, j];
                    var wTile = new WmapDesc
                    {
                        TileId = _dat.IdToTileType[dTile.TileType.Name]
                    };
                    wTile.TileDesc = _dat.Tiles[wTile.TileId];
                    wTile.Terrain = TerrainType.None;
                    wTile.Region = (dTile.Region == null) ? TileRegion.None : (TileRegion)Enum.Parse(typeof(TileRegion), dTile.Region);

                    if (dTile.Object != null)
                    {
                        wTile.ObjType = _dat.IdToObjectType[dTile.Object.ObjectType.Name];
                        wTile.ObjCfg = dTile.Object.ToString();
                        _dat.ObjectDescs.TryGetValue(wTile.ObjType, out wTile.ObjDesc);
                    }

                    wTiles[i, j] = wTile;
                }

            _tiles = new WmapTile[Width, Height];

            var enCount = 0;
            var entities = new List<Tuple<IntPoint, ushort, string>>();

            for (short y = 0; y < Height; y++)
                for (short x = 0; x < Width; x++)
                {
                    var tile = new WmapTile(wTiles[x, y], x, y);

                    if (tile.Region != 0)
                        Regions.Add(new IntPoint(x, y), tile.Region);

                    var desc = tile.ObjDesc;

                    if (tile.ObjType != 0 && (desc == null || !desc.Static || desc.Enemy))
                    {
                        entities.Add(new Tuple<IntPoint, ushort, string>(new IntPoint(x, y), tile.ObjType, tile.ObjCfg));

                        if (desc == null || !(desc.Enemy && desc.Static))
                            tile.ObjType = 0;
                    }

                    if (tile.ObjType != 0 && (desc == null || !(desc.Enemy && desc.Static)))
                    {
                        enCount++;
                        tile.ObjId = idBase + enCount;
                    }

                    _tiles[x, y] = tile;
                }

            for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                    _tiles[x, y].InitConnection(this, x, y);

            _entities = entities.ToArray();

            return enCount;
        }

        // typically this method is used with setpieces. It's data is
        // copied to the supplied world at the said position
        public void ProjectOntoWorld(World world, IntPoint pos)
        {
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                {
                    var projX = pos.X + x;
                    var projY = pos.Y + y;

                    if (!world.Map.Contains(projX, projY))
                        continue;

                    var tile = world.Map[projX, projY];
                    var spTile = _tiles[x, y];

                    if (spTile.TileId == 255)
                        continue;

                    spTile.CopyTo(tile);

                    if (spTile.ObjId != 0)
                        tile.ObjId = world.GetNextEntityId();

                    if (tile.Region != 0)
                        world.Map.Regions.Add(new IntPoint(projX, projY), spTile.Region);

                    tile.UpdateCount++;
                }

            foreach (var e in InstantiateEntities(world.Manager, pos))
            {
                if (!world.Map.Contains((int)e.X, (int)e.Y))
                    continue;

                world.EnterWorld(e);
            }
        }

        public void ResetTiles()
        {
            Regions.Clear();

            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                {
                    var t = _tiles[x, y];

                    t.Reset();

                    if (t.Region != 0)
                        Regions.Add(new IntPoint(x, y), t.Region);
                }
        }
    }
}
