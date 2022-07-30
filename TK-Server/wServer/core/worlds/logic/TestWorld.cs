using common.resources;
using System.IO;
using terrain;

namespace wServer.core.worlds.logic
{
    public class TestWorld : World
    {
        public TestWorld(int id, WorldResource resource) : base(id, resource)
        { }

        public bool JsonLoaded { get; private set; }

        public void LoadJson(string json)
        {
            if (!JsonLoaded)
            {
                FromWorldMap(new MemoryStream(Json2Wmap.Convert(Manager.Resources.GameData, json)));
                JsonLoaded = true;
            }
        }

        public override void Init()
        {
        }
    }
}
