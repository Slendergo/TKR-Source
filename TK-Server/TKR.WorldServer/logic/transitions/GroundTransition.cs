using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.logic;

namespace TKR.WorldServer.logic.transitions
{
    internal class OnGroundTransition : Transition
    {
        //State storage: none

        private readonly string _ground;
        private ushort? _groundType;

        public OnGroundTransition(string ground, string targetState)
            : base(targetState)
        {
            _ground = ground;
        }

        protected override bool TickCore(Entity host, TickTime time, ref object state)
        {
            if (_groundType == null)
                _groundType = host.GameServer.Resources.GameData.IdToTileType[_ground];

            var tile = host.World.Map[(int)host.X, (int)host.Y];
            return tile.TileId == _groundType;
        }
    }
}
