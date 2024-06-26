﻿using TKR.Shared.resources;
using System.IO;
using TKR.WorldServer.core.terrain;

namespace TKR.WorldServer.core.worlds.impl
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
