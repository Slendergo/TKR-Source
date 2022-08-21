using wServer.core.worlds;

namespace wServer.core.objects
{
    public class Engine : StaticObject
    {
        public int CurrentAmount { get => _currentAmount.GetValue(); set => _currentAmount.SetValue(value); }

        private SV<int> _currentAmount;

        public Engine(GameServer manager, ushort objType) : base(manager, objType, null, true, false, false)
        {
            _currentAmount = new SV<int>(this, StatDataType.EngineValue, 0);
        }


        public override void Tick(ref TickTime time)
        {
            base.Tick(ref time);
        }
    }
}
