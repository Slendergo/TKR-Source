using System;
using wServer.core.objects;

namespace wServer.core
{
    public class SV<T>
    {
        private readonly StatDataType _type;
        private readonly bool _updateSelfOnly;

        private Entity _owner;
        private Func<T, T> _transform;
        private T _tValue;
        private T _value;

        public SV(Entity e, StatDataType type, T value, bool updateSelfOnly = false, Func<T, T> transform = null)
        {
            _owner = e;
            _type = type;
            _updateSelfOnly = updateSelfOnly;
            _transform = transform;
            _value = value;
            _tValue = Transform(value);
        }

        public T GetValue() => _value;

        public void SetValue(T value)
        {
            if (_value != null && _value.Equals(value))
                return;

            _value = value;

            var tVal = Transform(value);

            if (_tValue != null && _tValue.Equals(tVal))
                return;

            _tValue = tVal;

            // hacky fix to xp
            if (_owner is Player && _type == StatDataType.Experience)
                _owner.InvokeStatChange(_type, (int)(object)tVal - Player.GetLevelExp((_owner as Player).Level), _updateSelfOnly);
            else
                _owner.InvokeStatChange(_type, tVal, _updateSelfOnly);
        }

        public override string ToString() => _value.ToString();

        private T Transform(T value) => (_transform == null) ? value : _transform(value);
    }
}
