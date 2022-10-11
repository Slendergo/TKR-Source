using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.miscfile.world;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.networking.packets.outgoing;

namespace TKR.WorldServer.logic.behaviors
{
    class ChangeMusicOnDeath : Behavior
    {
        private readonly string _music;

        public ChangeMusicOnDeath(string file)
        {
            _music = file;
        }

        public override void OnDeath(Entity host, ref TickTime time)
        {
            if (host.World.Music != _music)
            {
                var owner = host.World;

                owner.Music = _music;

                var i = 0;
                foreach (var plr in owner.Players.Values)
                {
                    owner.StartNewTimer(100 * i, (w, t) =>
                    {
                        if (plr == null)
                            return;

                        plr.Client.SendMessage(new SwitchMusic()
                        {
                            Music = _music
                        });
                    });
                    i++;
                }
            }
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
        }
    }
}
