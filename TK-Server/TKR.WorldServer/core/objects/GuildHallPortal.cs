﻿using TKR.Shared.resources;

namespace TKR.WorldServer.core.objects
{
    internal class GuildHallPortal : StaticObject
    {
        public readonly PortalDesc PortalDescr;

        public GuildHallPortal(GameServer manager, ushort objType, int? life) : base(manager, objType, life, false, true, false)
        {
            PortalDescr = manager.Resources.GameData.Portals[ObjectType];
        }
    }
}