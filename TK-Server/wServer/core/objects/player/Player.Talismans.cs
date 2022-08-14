using common.database.info;
using System.Collections.Generic;
using wServer.networking.packets.outgoing.talisman;

namespace wServer.core.objects
{
    public partial class Player
    {
        private bool SendTalismanData = true;

        private Dictionary<int, TalismanData> TalismanDatas = new Dictionary<int, TalismanData>();
        public List<int> ActiveTalismans = new List<int>();

        public TalismanData GetTalisman(int type) => TalismanDatas.TryGetValue(type, out var data) ? data : null;

        public void UnlockTalisman(TalismanData talismanData)
        {
            TalismanDatas.Add(talismanData.Type, talismanData);
            UpdateTalsimans();
        }

        public void ActivateTalisman(int type)
        {
            if (ActiveTalismans.Contains(type))
                return;
            ActiveTalismans.Add(type);
            UpdateTalsimans();
        }

        public void DeactivateTalisman(int type)
        {
            if (!ActiveTalismans.Contains(type))
                return;
            _ = ActiveTalismans.Remove(type);
            UpdateTalsimans();
        }

        public void HandleTalismans(ref TickTime time)
        {
            if (SendTalismanData && (PlayerUpdate.TickId + 1) % 2 == 0)
            {
                UpdateTalsimans();
                SendTalismanData = false;
            }
        }

        public void SaveTalismanData()
        {
            var talismans = new List<DbTalismanEntry>(TalismanDatas.Count);
            foreach (var talisman in TalismanDatas.Values)
                talismans.Add(new DbTalismanEntry()
                {
                    Type = talisman.Type,
                    Tier = talisman.Tier,
                    Level = talisman.Level,
                    Exp = talisman.CurrentXP,
                    Goal = talisman.ExpGoal,
                    Active = talisman.Active
                });
            GameServer.Database.SaveTalismansToCharacter(Client.Character.CharId, talismans);
        }

        private void LoadTalismanData()
        {
            var talismans = GameServer.Database.GetTalismansFromCharacter(Client.Character.CharId);
            foreach (var talisman in talismans)
            {
                TalismanDatas.Add(talisman.Type, new TalismanData(talisman));
                if (talisman.Active)
                    ActiveTalismans.Add(talisman.Type);
            }
            ApplyTalismanEffects();
        }

        private void ApplyTalismanEffects()
        {
            Stats.ReCalculateValues();
        }

        public void UpdateTalsimans()
        {
            ApplyTalismanEffects();
            var data = new TalismanEssenceData()
            {
                Essence = Client.Character.Essence,
                EssenceCap = Client.Character.EssenceCap
            };
            foreach (var talisman in TalismanDatas.Values)  
                data.Talismans.Add(talisman);
            Client.SendPacket(data);
        }
    }
}
