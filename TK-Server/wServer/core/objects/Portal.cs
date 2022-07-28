using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wServer.core.worlds;
using wServer.utils;

namespace wServer.core.objects
{
    public class Portal : StaticObject
    {
        public object CreateWorldLock = new object();

        private SV<bool> _usable;

        public Portal(CoreServerManager manager, ushort objType, int? life) : base(manager, ValidatePortal(manager, objType), life, false, true, false)
        {
            _usable = new SV<bool>(this, StatDataType.PortalUsable, true);
            Locked = manager.Resources.GameData.Portals[ObjectType].Locked;
        }

        public event EventHandler<World> WorldInstanceSet;

        public Task CreateWorldTask { get; set; }
        public bool Locked { get; private set; }
        public bool Usable { get => _usable.GetValue(); set => _usable.SetValue(value); }
        public World WorldInstance { get; set; }

        public void CreateWorld(Player player)
        {
            World world = null;

            foreach (var p in player.CoreServerManager.Resources.Worlds.Data.Values.Where(p => p.portals != null && p.portals.Contains(ObjectType)))
            {
                if (p.id < 0)
                    world = player.CoreServerManager.WorldManager.GetWorld(p.id);
                else
                {
                    DynamicWorld.TryGetWorld(p, player.Client, out world);

                    world = player.CoreServerManager.WorldManager.AddWorld(world ?? new World(p));
                }

                break;
            }

            WorldInstance = world;
            WorldInstanceSet?.Invoke(this, world);
        }

        public override bool HitByProjectile(Projectile projectile, TickData time) => false;

        protected override void ExportStats(IDictionary<StatDataType, object> stats)
        {
            stats[StatDataType.PortalUsable] = Usable ? 1 : 0;

            base.ExportStats(stats);
        }

        private static ushort ValidatePortal(CoreServerManager manager, ushort objType)
        {
            var portals = manager.Resources.GameData.Portals;

            if (!portals.ContainsKey(objType))
            {
                SLogger.Instance.Warn($"Portal {objType.To4Hex()} does not exist. Using Portal of Cowardice.");

                objType = 0x0703; // default to Portal of Cowardice
            }

            return objType;
        }
    }
}
