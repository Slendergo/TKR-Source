using System.Collections.Generic;
using TKR.WorldServer.core.net.stats;

namespace TKR.WorldServer.core.objects
{
    public class Engine : StaticObject
    {
        private StatTypeValue<int> _currentAmount;
        public int CurrentAmount
        {
            get => _currentAmount.GetValue();
            set => _currentAmount.SetValue(value);
        }

        private StatTypeValue<int> _engineTime;
        public int EngineTime
        {
            get => _engineTime.GetValue();
            set => _engineTime.SetValue(value);
        }

        public Engine(GameServer manager, ushort objType) 
            : base(manager, objType, null, true, false, false)
        {
            var nexus = GameServer.WorldManager.Nexus;
            _currentAmount = new StatTypeValue<int>(this, StatDataType.EngineValue, nexus.EngineFuel);
            _engineTime = new StatTypeValue<int>(this, StatDataType.EngineTime, nexus.EngineStageTime);
        }

        protected override void ExportStats(IDictionary<StatDataType, object> stats, bool isOtherPlayer)
        {
            stats[StatDataType.EngineValue] = CurrentAmount;
            stats[StatDataType.EngineTime] = EngineTime;
            base.ExportStats(stats, isOtherPlayer);
        }
    }
}
