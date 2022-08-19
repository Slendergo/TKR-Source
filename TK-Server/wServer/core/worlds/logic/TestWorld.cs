using common.resources;
using System.IO;
using terrain;

namespace wServer.core.worlds.logic
{
    public class TestWorld : World
    {
        public TestWorld(GameServer gameServer, int id, WorldResource resource) : base(gameServer, id, resource)
        { 
        }

        public override void Init()
        {
        }

        public void LoadJson(string json)
        {
            FromWorldMap(new MemoryStream(Json2Wmap.Convert(GameServer.Resources.GameData, json)));
        }
    }
}
