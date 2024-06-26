﻿using System;
using TKR.WorldServer.core.structures;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.setpieces
{
    public abstract class ISetPiece
    {
        public abstract int Size { get; }
        public virtual string Map { get; }

        protected Random rand => Random.Shared;

        public virtual void RenderSetPiece(World world, IntPoint pos)
        {
            if (string.IsNullOrEmpty(Map))
                return;

            var data = world.GameServer.Resources.GameData.GetWorldData(Map);
            if (data == null)
            {
                Console.WriteLine($"[{GetType().Name}] Invalid RenderSetPiece {Map}");
                return;
            }
            SetPieces.RenderFromData(world, pos, data);
        }
    }
}
