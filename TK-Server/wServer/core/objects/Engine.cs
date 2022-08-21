using wServer.core.worlds;
using common.database;
using System.Collections.Generic;

namespace wServer.core.objects
{
    public class Engine : StaticObject
    {

        private SV<int> _currentAmount;
        public int CurrentAmount
        {
            get => _currentAmount.GetValue();
            set => _currentAmount.SetValue(value);
        }

        private SV<int> _engineTime;
        public int EngineTime
        {
            get => _engineTime.GetValue();
            set => _engineTime.SetValue(value);
        }

        public Engine(GameServer manager, ushort objType) : base(manager, objType, null, true, false, false)
        {
            var nexus = GameServer.WorldManager.Nexus;
            _currentAmount = new SV<int>(this, StatDataType.EngineValue, nexus.EngineFuel);
            _engineTime = new SV<int>(this, StatDataType.EngineTime, nexus.EngineStageTime);
        }

        protected override void ExportStats(IDictionary<StatDataType, object> stats)
        {
            stats[StatDataType.EngineValue] = CurrentAmount;
            stats[StatDataType.EngineTime] = EngineTime;
            base.ExportStats(stats);
        }
    }
}
