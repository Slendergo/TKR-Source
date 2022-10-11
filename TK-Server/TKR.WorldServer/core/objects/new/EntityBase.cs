using System;
using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.stats;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.objects.@new
{
    public abstract class EntityBase
    {
        public int Id { get; private set; }
        public int ObjectType {get; private set; }
        public float Size
        {
            get => StatManager.GetFloatStat(StatDataType.Size);
            set => StatManager.SetFloatValue(StatDataType.Size, value);
        }
        public string Name
        {
            get => StatManager.GetStringStat(StatDataType.Name);
            set => StatManager.SetStringStat(StatDataType.Name, value);
        }
        public ObjectDesc ObjectDesc { get; private set; }
        public World World { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }
        public float PrevX { get; private set; }
        public float PrevY { get; private set; }
        public bool Dead { get; private set; }
        public StatManager StatManager { get; private set; }

        public EntityBase(World world, ObjectDesc objectDesc)
        {
            World = world;
            ObjectDesc = objectDesc;

            StatManager = new StatManager(this);

            ObjectType = objectDesc.ObjectType;
            if (objectDesc.MinSize != objectDesc.MaxSize)
                Size = objectDesc.MinSize + (int)(Random.Shared.NextDouble() * ((objectDesc.MaxSize - objectDesc.MinSize) / objectDesc.SizeStep)) * objectDesc.SizeStep;
            else
                Size = objectDesc.Size;
            Name = objectDesc.DisplayId;
        }

        public void SetObjectId(int id) => Id = id;

        public void SetPosition(float x, float y)
        {
            X = x;
            Y = y;
            PrevX = x;
            PrevY = y;
        }

        public void Move(float newX, float newY)
        {
            var prevX = X;
            var prevY = Y;
            X = newX;
            Y = newY;
            PrevX = prevX;
            PrevY = prevY;

            World.Census.Move(this);
        }

        public float DistTo(float x, float y) => MathF.Sqrt((x - X) * (x - X) + (y - Y) * (y - Y));
        public float SqDistTo(float x, float y) => (x - X) * (x - X) + (y - Y) * (y - Y);

        public abstract void OnAddedToWorld();
        public abstract void Update(ref TickTime tickTime);
        public abstract void OnRemovedFromWorld();

        public void Expunge() => Dead = true;
    }
}
