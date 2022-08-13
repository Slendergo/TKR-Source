using System.Collections.Generic;
using wServer.networking.packets.outgoing.talisman;

namespace wServer.core.objects
{
    public partial class Player
    {
        private bool SendTalismanData = true;

        // todo

        public void HandleTalismans(ref TickTime time)
        {
            if (SendTalismanData && PlayerUpdate.TickId == 1)
            {
                UpdateTalismanData();
                SendTalismanData = false;
            }
        }

        private void UpdateTalismanData()
        {
            var talismanDataA = new TalismanData(0, 3, 100, 1000, 0);
            var talismanDataB = new TalismanData(1, 5, 200, 2000, 1);
            var talismanDataC = new TalismanData(2, 8, 300, 3000, 2);

            var data = new TalismanEssenceData();
            data.Talismans.Add(talismanDataA);
            data.Talismans.Add(talismanDataB);
            data.Talismans.Add(talismanDataC);
            Client.SendPacket(data);
        }
    }
}
