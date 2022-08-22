using common.database.info;
using System;
using System.Collections.Generic;
using wServer.networking.packets.outgoing.talisman;

namespace wServer.core.objects
{
    public partial class Player
    {
        private bool SendTalismanData = true;

        private Dictionary<int, TalismanData> TalismanDatas = new Dictionary<int, TalismanData>();
        public List<int> ActiveTalismans = new List<int>();

        public float TalismanLootBoost { get; set; }

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

        public void GiveEssence(int amount)
        {
            Console.WriteLine("Essence Gained: " + amount);
            var essenceCap = Client.Character.EssenceCap;
            var essence = Math.Min(essenceCap, Client.Character.Essence + amount);
            Client.Character.Essence = essence;

            if(essence == essenceCap)
                SendInfo($"You have hit the limit of Talisman Essence");
            else
                SendInfo($"You have gained: {amount} Talisman Essence");
        }

        public void HandleTalismans(ref TickTime time)
        {
            try
            {
                if (SendTalismanData && (PlayerUpdate.TickId + 1) % 2 == 0)
                {
                    UpdateTalsimans();
                    SendTalismanData = false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
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
            GameServer.Database.SaveTalismansToCharacter(Client.Account.AccountId, Client.Character.CharId, talismans);
        }

        private void LoadTalismanData()
        {
            UpdateEssenceCap();
            var talismans = GameServer.Database.GetTalismansFromCharacter(Client.Account.AccountId, Client.Character.CharId);
            foreach (var talisman in talismans)
            {
                TalismanDatas.Add(talisman.Type, new TalismanData(talisman));
                if (talisman.Active)
                    ActiveTalismans.Add(talisman.Type);
            }
            ApplyTalismanEffects();
        }

        public void UpdateEssenceCap()
        {
            Client.Character.EssenceCap = GetTalismanEssenceCap(Fame);
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
