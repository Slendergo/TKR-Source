using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.core.worlds;

namespace wServer.core._core.entities
{
    public sealed class GameObject
    {
        public int Id;
        public int ObjectType;
        public float X;
        public float Y;
        public float PrevX;
        public float PrevY;

        public IntPoint IPosition => new IntPoint((int)X, (int)Y);
        public Position Position => new Position(X, Y);

        public GameObject(World world, int objectId, int objectType)
        {
        }

        public void OnEnterWorld()
        {
        }

        public void Update(ref TickTime tickTime)
        {
        }

        public void OnLeaveWorld()
        {
        }
    }
}
