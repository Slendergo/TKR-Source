using wServer.core;
using wServer.core.objects;

namespace wServer.logic.transitions
{
    internal class GroundTransition : Transition
    {
        //State storage: none

        private readonly string _ground;
        private ushort? _groundType;

        public GroundTransition(string ground, string targetState)
            : base(targetState)
        {
            _ground = ground;
        }

        protected override bool TickCore(Entity host, TickData time, ref object state)
        {
            if (_groundType == null)
                _groundType = host.CoreServerManager.Resources.GameData.IdToTileType[_ground];

            var tile = host.Owner.Map[(int)host.X, (int)host.Y];

            return tile.TileId == _groundType;
        }
    }
}
