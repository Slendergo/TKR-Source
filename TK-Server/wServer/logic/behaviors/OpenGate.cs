using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
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

        protected override void OnStateEntry(Entity host, TickData time, ref object state)
        {
            var map = host.Owner.Map;
            var w = map.Width;
            var h = map.Height;

            if (usearea)
                for (var x = (int)host.X - area; x <= (int)host.X + area; x++)
                    for (var y = (int)host.Y - area; y <= (int)host.Y + area; y++)
                    {
                        var tile = host.Owner.Map[x, y];

                        if (tile.ObjType == host.CoreServerManager.Resources.GameData.DisplayIdToObjectType[target])
                        {
                            tile.ObjType = 0;
                            tile.UpdateCount++;

                            host.Owner.Map[x, y] = tile;
                        }
                    }
            else
                for (int x = xMax; x <= xMax; x++)
                    for (int y = yMin; y <= yMax; y++)
                    {
                        var tile = host.Owner.Map[x, y];
                        tile.ObjType = 0;
                        tile.UpdateCount++;

                        host.Owner.Map[x, y] = tile;
                    }
        }

        protected override void TickCore(Entity host, TickData time, ref object state)
        { }
    }
}
