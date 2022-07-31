using common.resources;
using System.IO;
using terrain;

namespace wServer.core.worlds.logic
{
    public class TestWorld : World
    {
        public TestWorld(int id, WorldResource resource) : base(id, resource)
        { 
        }

        public override void Init()
        {
        }

        public void LoadJson(string json)
        {
            FromWorldMap(new MemoryStream(Json2Wmap.Convert(Manager.Resources.GameData, json)));
        }
    }
}
