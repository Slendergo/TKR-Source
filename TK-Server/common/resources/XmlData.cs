using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace common.resources
{
    public class XmlData
    {
        public Dictionary<ushort, PlayerDesc> Classes = new Dictionary<ushort, PlayerDesc>();
        public Dictionary<string, ushort> DisplayIdToObjectType = new Dictionary<string, ushort>();
        public Dictionary<string, ushort> IdToObjectType = new Dictionary<string, ushort>(StringComparer.InvariantCultureIgnoreCase);
        public Dictionary<string, ushort> IdToTileType = new Dictionary<string, ushort>();
        public Dictionary<ushort, Item> Items = new Dictionary<ushort, Item>();
        public Dictionary<ushort, ObjectDesc> ObjectDescs = new Dictionary<ushort, ObjectDesc>();
        public Dictionary<ushort, XElement> ObjectTypeToElement = new Dictionary<ushort, XElement>();
        public Dictionary<ushort, string> ObjectTypeToId = new Dictionary<ushort, string>();
        public Dictionary<ushort, PortalDesc> Portals = new Dictionary<ushort, PortalDesc>();
        public Dictionary<ushort, SkinDesc> Skins = new Dictionary<ushort, SkinDesc>();
        public Dictionary<int, ItemType> SlotTypeToItemType = new Dictionary<int, ItemType>();
        public Dictionary<ushort, TileDesc> Tiles = new Dictionary<ushort, TileDesc>();
        public Dictionary<ushort, XElement> TileTypeToElement = new Dictionary<ushort, XElement>();
        public Dictionary<ushort, string> TileTypeToId = new Dictionary<ushort, string>();

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly List<string> _gameXmls;
        public IList<string> GameXmls { get; }

        public byte[] ZippedXMLS { get; private set; }

        public XmlData(string dir, bool isFromMetis = false, Action<float, float, string, bool> progress = null)
        {
            if (!isFromMetis) Log.Info("Loading XmlData...");

            //Make the list to handle all the xmls
            GameXmls = new ReadOnlyCollection<string>(_gameXmls = new List<string>());

            LoadXmls(dir, "*.xml", isFromMetis, progress);
            LoadXmls(dir, "*.dat", isFromMetis, progress);

            //zip all the xmls in binary/bytes
            ZippedXMLS = ZipGameXmls();
        }

        //convert xmls to byte
        //ZippedXMLS is used in getServerXmls in server/char/getServerXmls.cs
        private byte[] ZipGameXmls()
        {
            using (var ms = new MemoryStream())
            {
                var wtr = new NWriter(ms);
                wtr.Write(GameXmls.Count);
                foreach (var xml in GameXmls)
                    wtr.Write32UTF(xml);

                return ms.ToArray();//Utils.Deflate();
            }
        }

        private void AddGrounds(XElement root) => root.Elements("Ground").Select(e =>
        {
            var id = e.GetAttribute<string>("id");
            var type = e.GetAttribute<ushort>("type");

            if (TileTypeToId.ContainsKey(type))
                Log.Warn("'{0}' and '{1}' have the same type of '0x{2:x4}'", id, TileTypeToId[type], type);

            if (IdToTileType.ContainsKey(id))
                Log.Warn("'0x{0:x4}' and '0x{1:x4}' have the same id of '{2}'", type, IdToTileType[id], id);

            TileTypeToId[type] = id;
            TileTypeToElement[type] = e;
            IdToTileType[id] = type;

            Tiles[type] = new TileDesc(type, e);

            return e;
        }).ToArray();

        private void AddObjects(XElement root, bool isFromMetis) => root.Elements("Object").Select(e =>
         {
             var cls = e.GetValue<string>("Class");

             if (string.IsNullOrWhiteSpace(cls)) return e;

             ushort type = 0;
             try
             {
                 type = e.GetAttribute<ushort>("type");
             }
             catch
             {
                 Log.Error("XML Error: " + e);
             }
             

             if (isFromMetis)
             {
                 switch (cls)
                 {
                     case "Equipment":
                         Items[type] = new Item(type, e);
                         break;

                     default: break;
                 }

                 return e;
             }

             var id = e.GetAttribute<string>("id");
             var displayId = e.GetValue<string>("DisplayId");
             var displayName = string.IsNullOrWhiteSpace(displayId) ? id : displayId;

             if (ObjectTypeToId.ContainsKey(type))
                 Log.Warn("'{0}' and '{1}' have the same type of '0x{2:x4}'", id, ObjectTypeToId[type], type);

             if (IdToObjectType.ContainsKey(id))
                 Log.Warn("'0x{0:x4}' and '0x{1:x4}' have the same id of '{2}'", type, IdToObjectType[id], id);

             ObjectTypeToId[type] = id;
             ObjectTypeToElement[type] = e;
             IdToObjectType[id] = type;
             DisplayIdToObjectType[displayName] = type;

             switch (cls)
             {
                 case "Equipment":
                 case "Dye":
                     Items[type] = new Item(type, e);
                     break;

                 case "Player":
                     var pDesc = Classes[type] = new PlayerDesc(type, e);
                     ObjectDescs[type] = Classes[type];
                     SlotTypeToItemType[pDesc.SlotTypes[0]] = ItemType.Weapon;
                     SlotTypeToItemType[pDesc.SlotTypes[1]] = ItemType.Ability;
                     SlotTypeToItemType[pDesc.SlotTypes[2]] = ItemType.Armor;
                     SlotTypeToItemType[pDesc.SlotTypes[3]] = ItemType.Ring;
                     break;

                 case "GuildHallPortal":
                 case "Portal":
                     Portals[type] = new PortalDesc(type, e);
                     ObjectDescs[type] = Portals[type];
                     break;

                 case "Skin":
                     Skins[type] = new SkinDesc(type, e);
                     break;

                 default:
                     ObjectDescs[type] = new ObjectDesc(type, e);
                     break;
             }

             return e;
         }).ToArray();

        private void LoadXmls(string basePath, string ext, bool isFromMetis, Action<float, float, string, bool> progress)
        {
            var xmls = Directory.EnumerateFiles(basePath, ext, SearchOption.AllDirectories).ToArray();
            var current = 0;
            var total = xmls.Length;
            var xmlRange = Enumerable.Range(0, xmls.Length);
            xmlRange.Select(i =>
            {
                var xml = File.ReadAllText(xmls[i]);

                _gameXmls.Add(xml);

                if (isFromMetis)
                {
                    Interlocked.Increment(ref current);

                    progress.Invoke(current, total, xmls[i], current == total);
                }

                
                try
                {
                    ProcessXml(XElement.Parse(xml), isFromMetis);
                }
                catch(Exception e)
                {
                    Log.Error("Exception: " + e);
                    Log.Error("XML Path Error: " + xmls[i]);
                }

                return i;
            }).ToArray();
        }

        private void ProcessXml(XElement root, bool isFromMetis)
        {
            AddObjects(root, isFromMetis);

            if (!isFromMetis) AddGrounds(root);
        }
    }
}
