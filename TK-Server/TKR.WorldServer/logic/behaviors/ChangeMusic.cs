using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.miscfile.world;
using TKR.WorldServer.core.objects;
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

                var i = 0;
                foreach (var plr in owner.Players.Values)
                {
                    owner.Timers.Add(new WorldTimer(100 * i, (w, t) =>
                    {
                        if (plr == null)
                            return;

                        plr.Client.SendPacket(new SwitchMusic()
                        {
                            Music = _music
                        });
                    }));
                    i++;
                }
            }
        }
    }
}