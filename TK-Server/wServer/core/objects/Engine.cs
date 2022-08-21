using wServer.core.worlds;
using common.database;

namespace wServer.core.objects
{
    public class Engine : StaticObject
    {
        public int CurrentAmount { get => _currentAmount.GetValue(); set => _currentAmount.SetValue(value); }
        public int EngineTime { get => _engineTime.GetValue(); set => _engineTime.SetValue(value); }

        private SV<int> _currentAmount;
        private SV<int> _engineTime;

        public Engine(GameServer manager, ushort objType) : base(manager, objType, null, true, false, false)
        {
            _currentAmount = new SV<int>(this, StatDataType.EngineValue, GameServer.Database.GetEngineFuel());
            _engineTime = new SV<int>(this, StatDataType.EngineValue, GameServer.Database.GetEngineTime());
        }


        public override void Tick(ref TickTime time)
        {
            CurrentAmount = GameServer.Database.GetEngineFuel();
            EngineTime = GameServer.Database.GetEngineTime();
            base.Tick(ref time);
        }
    }
}
