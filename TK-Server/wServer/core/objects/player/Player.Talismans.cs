using System.Collections.Generic;
using wServer.networking.packets.outgoing.talisman;

namespace wServer.core.objects
{
    public partial class Player
    {
        private bool SendTalismanData = true;

        private Dictionary<int, TalismanData> TalismanDatas = new Dictionary<int, TalismanData>();

        // todo

        public void HandleTalismans(ref TickTime time)
        {
            if (SendTalismanData && PlayerUpdate.TickId == 1)
            {
                LoadTalismanData();
                UpdateTalismanData();
                SendTalismanData = false;
            }
        }

        private void LoadTalismanData()
        {
            var talismans = GameServer.Database.GetTalismansFromCharacter(Client.Character.CharId);

            foreach (var talisman in talismans)
                TalismanDatas.Add(talisman.Type, new TalismanData(talisman));
        }

        private void UpdateTalismanData()
        {
            var data = new TalismanEssenceData();
            data.Essence = Client.Character.Essence;
            data.EssenceCap = Client.Character.EssenceCap;
            foreach (var talisman in TalismanDatas.Values)
                data.Talismans.Add(talisman);
            Client.SendPacket(data);
        }
    }
}
