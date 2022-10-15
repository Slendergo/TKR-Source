using System;
using System.Collections.Generic;
using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.stats;

namespace TKR.WorldServer.core.objects
{
    public abstract class Character : Entity
    {
        public int HP { get => _hp.GetValue(); set => _hp.SetValue(value); }
        public int MaximumHP { get => _maximumHP.GetValue(); set => _maximumHP.SetValue(value); }

        private SV<int> _hp;
        private SV<int> _maximumHP;

        private static Random R = new Random();//temp fix

        protected Character(GameServer manager, ushort objType) : base(manager, objType)
        {
            _hp = new SV<int>(this, StatDataType.Health, 0);
            _maximumHP = new SV<int>(this, StatDataType.MaximumHealth, 0);

            if (ObjectDesc != null)
            {
                if (ObjectDesc.SizeStep != 0)
                {
                    var step = R.Next(0, (ObjectDesc.MaxSize - ObjectDesc.MinSize) / ObjectDesc.SizeStep + 1) * ObjectDesc.SizeStep;
                    SetDefaultSize(ObjectDesc.MinSize + step);
                }
                else
                    SetDefaultSize(ObjectDesc.MinSize);

                HP = ObjectDesc.MaxHP;
                MaximumHP = HP;
            }
        }

        protected override void ExportStats(IDictionary<StatDataType, object> stats, bool isOtherPlayer)
        {
            base.ExportStats(stats, isOtherPlayer);
            stats[StatDataType.Health] = HP;
            if (!(this is Player))
                stats[StatDataType.MaximumHealth] = MaximumHP;
        }
    }
}
