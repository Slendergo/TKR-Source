﻿using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.networking.packets.outgoing;

namespace TKR.WorldServer.logic.behaviors
{
    class ChangeMusic : Behavior
    {
        //State storage: none

        private readonly string _music;

        public ChangeMusic(string file)
        {
            _music = file;
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        { }

        protected override void OnStateEntry(Entity host, TickTime time, ref object state)
        {
            if (host.World.Music != _music)
            {
                var owner = host.World;
                owner.Music = _music;
                foreach (var plr in owner.Players.Values)
                    plr.Client.SendPacket(new SwitchMusic()
                    {
                        Music = _music
                    });
            }
        }
    }
}