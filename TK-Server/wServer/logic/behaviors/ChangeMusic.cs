using wServer.core;
using wServer.core.objects;
using wServer.networking.packets.outgoing;

namespace wServer.logic.behaviors
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
            if (host.Owner.Music != _music)
            {
                var owner = host.Owner;

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