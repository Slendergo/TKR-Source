using TKR.Shared;
using TKR.Shared.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKR.WorldServer.core;
using TKR.WorldServer.core.miscfile.stats;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.core.objects
{
    public class Portal : StaticObject
    {
        public World WorldInstance;

        private SV<bool> _usable;

        public Portal(GameServer manager, ushort objType, int? life) : base(manager, ValidatePortal(manager, objType), life, false, true, false)
        {
            _usable = new SV<bool>(this, StatDataType.PortalUsable, true);

            Locked = ObjectDesc.Locked;
        }

        public bool Locked { get; private set; }
        public bool Usable { get => _usable.GetValue(); set => _usable.SetValue(value); }

        protected override void ExportStats(List<ValueTuple<StatDataType, object>> stats, bool isOtherPlayer)
        {
            stats.Add(ValueTuple.Create(StatDataType.PortalUsable, Usable ? 1 : 0));

            base.ExportStats(stats, isOtherPlayer);
        }

        private static ushort ValidatePortal(GameServer manager, ushort objType)
        {
            if (!manager.Resources.GameData.ObjectDescs.ContainsKey(objType))
            {
                StaticLogger.Instance.Warn($"Portal {objType.To4Hex()} does not exist. Using Portal of Cowardice.");
                objType = 0x0703; // default to Portal of Cowardice
            }
            return objType;
        }
    }
}
