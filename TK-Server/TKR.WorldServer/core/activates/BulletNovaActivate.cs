using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TKR.Shared.resources;
using TKR.WorldServer.core.net.datas;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.structures;
using TKR.WorldServer.networking.packets.outgoing;

namespace TKR.WorldServer.core.activates
{

    public sealed class BulletNovaActivate : Activate
    {
        private readonly int NumShots;
        private readonly int Color;
        private readonly double PosOffset;

        // todo rewrite the ctor so it doesnt need a retuns bool parameter
        public BulletNovaActivate(Player host, XElement options, bool returnsBool)
            : base(host, options, returnsBool)
        {
            NumShots = GetIntArgument(options, "numShots", 20);
            Color = GetIntArgument(options, "color", 0xFF00AA);
            PosOffset = GetFloatArgument(options, "posOffset", 0);
        }
        
        public override void Execute(Item item, ref Position usePosition)
        {
            var projectileDesc = item.Projectiles[0];

            var shoots = new List<OutgoingMessage>(NumShots);
            for (var i = 0; i < NumShots; i++)
            {
                var nextBulletId = Host.GetNextBulletId(1, true);

                var angle = (float)(i * (Math.PI * 2) / NumShots);

                var newPos = usePosition;
                if (PosOffset != 0.0)
                {
                    newPos = usePosition.PointAt(angle, PosOffset);
                    angle = (float)newPos.AngleTo(usePosition.X, usePosition.Y);
                }

                shoots.Add(new ServerPlayerShoot()
                {
                    BulletType = projectileDesc.BulletType,
                    ObjectType = item.ObjectType,
                    BulletId = nextBulletId,
                    OwnerId = Host.Id,
                    ContainerType = item.ObjectType,
                    StartingPos = newPos,
                    Angle = angle,
                    Damage = Host.Stats.GetAttackDamage(projectileDesc.MinDamage, projectileDesc.MaxDamage, true)
                });
            }

            Host.World.BroadcastIfVisible(new ShowEffect()
            {
                EffectType = EffectType.Trail,
                Pos1 = usePosition,
                TargetObjectId = Host.Id,
                Color = new ARGB((uint)Color)
            }, Host);
            Host.World.BroadcastIfVisible(shoots, ref usePosition);
        }
    }
}