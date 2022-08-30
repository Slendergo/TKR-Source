using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.core.worlds;

namespace wServer.core._core.entities
{
    public class Momentum
    {
        public float Speed;
        public float Facing;
        public GameObject GameObject;

        public Momentum(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public bool Move(float dt)
        {

            return true;
        }
    }

    public abstract class GameObject
    {
        public int Id;
        public int ObjectType;
        public World World;
        public float X;
        public float Y;
        public float PrevX;
        public float PrevY;
        public Momentum Momentum;

        public IntPoint IPosition => new IntPoint((int)X, (int)Y);
        public Position Position => new Position(X, Y);

        public GameObject(int objectId, int objectType, World world)
        {
            Id = objectId;
            ObjectType = objectType;
            World = world;

            Momentum = new Momentum(this);
        }

        public void SetPosition(float x, float y)
        {
            var prevX = X;
            var prevY = Y;
            X = x;
            Y = y;
            PrevX = prevX;
            PrevY = prevY;
        }

        public abstract void OnEnterWorld();
        public abstract bool Update(ref TickTime tickTime);
        public abstract void OnLeaveWorld();
    }
}
