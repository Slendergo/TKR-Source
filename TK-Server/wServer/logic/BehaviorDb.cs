using common.resources;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using wServer.core;
using wServer.core.objects;
using wServer.logic.loot;

namespace wServer.logic
{
    public partial class BehaviorDb
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static float _rightAngle = 0;
        private static float _leftAngle = 180;
        private static float _upAngle = -90;
        private static float _downAngle = 90;
        public CoreServerManager Manager { get; private set; }

        private static int _initializing;
        internal static BehaviorDb InitDb;
        internal static XmlData InitGameData => InitDb.Manager.Resources.GameData;

        public BehaviorDb(CoreServerManager manager)
        {
            Manager = manager;
            MobDrops.Init(manager);

            Definitions = new Dictionary<ushort, Tuple<State, Loot>>();

            if (Interlocked.Exchange(ref _initializing, 1) == 1)
            {
                Log.Error("Attempted to initialize multiple BehaviorDb at the same time.");
                throw new InvalidOperationException("Attempted to initialize multiple BehaviorDb at the same time.");
            }
            InitDb = this;

            var fields = GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.FieldType == typeof(_))
                .ToArray();
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                ((_)field.GetValue(this))();
                field.SetValue(this, null);
            }

            InitDb = null;
            _initializing = 0;

            Log.Info("Behavior Database initialized...");
        }

        public void ResolveBehavior(Entity entity)
        {
            if (Definitions.TryGetValue(entity.ObjectType, out Tuple<State, Loot> def))
                entity.SwitchTo(def.Item1);
        }

        private delegate ctor _();

        private struct ctor
        {
            public ctor Init(string id, State rootState, params MobDrops[] defs)
            {
                var d = new Dictionary<string, State>();
                rootState.Resolve(d);
                rootState.ResolveChildren(d);
                var dat = InitDb.Manager.Resources.GameData;

                if (!dat.IdToObjectType.ContainsKey(id))
                {
                    Log.Error($"Failed to add behavior: {id}. Xml data not found.");
                    return this;
                }

                if (defs.Length > 0)
                {
                    var loot = new Loot(defs);
                    rootState.Death += (sender, e) => loot.Handle((Enemy)e.Host, e.Time);
                    InitDb.Definitions.Add(dat.IdToObjectType[id], new Tuple<State, Loot>(rootState, loot));
                }
                else
                    InitDb.Definitions.Add(dat.IdToObjectType[id], new Tuple<State, Loot>(rootState, null));
                return this;
            }

            public ctor InitMany(string objTypeMin, string objTypeMax, Func<string, State> rootState, params MobDrops[] defs)
            {
                XmlData dat = InitDb.Manager.Resources.GameData;
                ushort idMin = dat.IdToObjectType[objTypeMin];
                ushort idMax = dat.IdToObjectType[objTypeMax];
                int count = idMax - idMin;
                for (int i = 0; i <= count; i++)
                    if (!InitDb.Definitions.ContainsKey((ushort)(idMin + i)))
                        Init(dat.ObjectTypeToId[(ushort)(idMin + i)],
                            rootState(dat.ObjectTypeToId[(ushort)(idMin + i)]), defs);
                return this;
            }
        }

        private static ctor Behav()
        {
            return new ctor();
        }

        public Dictionary<ushort, Tuple<State, Loot>> Definitions { get; private set; }
    }
}
