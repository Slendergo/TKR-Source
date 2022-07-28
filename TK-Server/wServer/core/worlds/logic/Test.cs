using common.resources;
using System.IO;
using terrain;

namespace wServer.core.worlds.logic
{
    public class Test : World
    {
        private static ProtoWorld _testProto = new ProtoWorld
        {
            name = "Test World",
            sbName = "Test World",
            id = 0,
            setpiece = false,
            showDisplays = false,
            background = 0,
            blocking = 0,
            difficulty = 0,
            isLimbo = false,
            maps = Empty<string>.Array,
            persist = false,
            portals = Empty<int>.Array,
            restrictTp = false,
            wmap = Empty<byte[]>.Array
        };

        public Test() : base(_testProto)
        { }

        public bool JsonLoaded { get; private set; }

        public void LoadJson(string json)
        {
            if (!JsonLoaded)
            {
                FromWorldMap(new MemoryStream(Json2Wmap.Convert(Manager.Resources.GameData, json)));
                JsonLoaded = true;
            }

            InitShops();
        }

        protected override void Init()
        { }
    }
}
