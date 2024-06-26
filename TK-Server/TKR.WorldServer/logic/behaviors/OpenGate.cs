﻿using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.logic;

namespace TKR.WorldServer.logic.behaviors
{
    public class OpenGate : Behavior
    {
        private readonly int area;
        private readonly string target;
        private readonly bool usearea;
        private readonly int xMax;
        private readonly int xMin;
        private readonly int yMax;
        private readonly int yMin;

        public OpenGate(int xMin, int xMax, int yMin, int yMax)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = yMin;
            this.yMax = yMax;
        }

        public OpenGate(string target, int area = 10)
        {
            this.target = target;
            this.area = area;

            usearea = true;
        }

        protected override void OnStateEntry(Entity host, TickTime time, ref object state)
        {
            var map = host.World.Map;

            if (usearea)
                for (var x = (int)host.X - area; x <= (int)host.X + area; x++)
                    for (var y = (int)host.Y - area; y <= (int)host.Y + area; y++)
                    {
                        var tile = host.World.Map[x, y];

                        if (tile.ObjType == host.GameServer.Resources.GameData.DisplayIdToObjectType[target])
                        {
                            tile.ObjType = 0;
                            tile.UpdateCount++;
                        }
                    }
            else
                for (int x = xMax; x <= xMax; x++)
                    for (int y = yMin; y <= yMax; y++)
                    {
                        var tile = host.World.Map[x, y];
                        tile.ObjType = 0;
                        tile.UpdateCount++;
                    }
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        { }
    }
}
