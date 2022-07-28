using common;

namespace wServer.core
{
    internal class BaseStatManager
    {
        private int[] _base;
        private StatsManager _parent;

        public BaseStatManager(StatsManager parent)
        {
            _parent = parent;
            _base = Utils.ResizeArray(
                parent.Owner.Client.Character.Stats,
                StatsManager.NumStatTypes);

            ReCalculateValues();
        }

        public int this[int index]
        {
            get => _base[index];
            set
            {
                _base[index] = value;
                _parent.StatChanged(index);
            }
        }

        public int[] GetStats() => (int[])_base.Clone();

        protected internal void ReCalculateValues()
        { }
    }
}
