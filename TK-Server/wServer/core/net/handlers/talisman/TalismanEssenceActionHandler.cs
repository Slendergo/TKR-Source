using common;
using common.database;
using common.resources;
using System.Collections.Generic;
using System.Linq;
using wServer.core;
using wServer.core.objects;
using wServer.core.objects.vendors;
using wServer.core.worlds.logic;
using wServer.networking;
using wServer.networking.packets;
using wServer.core.net.handlers;
using wServer.networking.packets.outgoing;
using System;
using wServer.networking.packets.outgoing.talisman;

namespace wServer.core.net.handlers
{
    public sealed class TalismanEssenceActionHandler : IMessageHandler
    {
        enum TalismanActionType : byte
        {
            ADD_ESSENCE = 0,
            TIER_UP = 1,
            ENABLE = 2,
            DISABLE = 3 
        }

        public override MessageId MessageId => MessageId.TALISMAN_ESSENCE_ACTION;

        public override void Handle(Client client, NReader rdr, ref TickTime time)
        {
            var action = rdr.ReadByte();
            var type = rdr.ReadInt32();
            var amount = rdr.ReadInt32();

            if(action > (byte)TalismanActionType.DISABLE)
                return;

            var player = client.Player;
            
            var talisman = player.GetTalisman(type);
            if (talisman == null)
            {
                player.SendError("You dont have that talisman unlocked");
                return;
            }

            var desc = player.GameServer.Resources.GameData.GetTalisman(talisman.Type);
            if (desc == null)
            {
                player.SendError("ERROR: Unable to find talisman.");
                return;
            }

            switch ((TalismanActionType)action)
            {
                case TalismanActionType.ADD_ESSENCE:
                    AddEssence(player, talisman, desc, amount);
                    break;
                case TalismanActionType.TIER_UP:
                    TierUpgrade(player, talisman, desc, amount);
                    break;
                case TalismanActionType.ENABLE:
                    EnableTalisman(player, talisman, desc);
                    break;
                case TalismanActionType.DISABLE:
                    DisableTalisman(player, talisman, desc);
                    break;
            }
        }

        private void AddEssence(Player player, TalismanData talisman, TalismanDesc desc, int amount)
        {
            var remainingEssence = GetEssenceRemaining(player, amount);
            if(remainingEssence < 0)
            {
                player.SendError("You dont have enough essence to do that");
                return;
            }

            if(talisman.Level > desc.MaxLevels)
            {
                talisman.CurrentXP = talisman.ExpGoal;
                talisman.Level = (byte)desc.MaxLevels;
                player.UpdateTalsimans();
                player.SendError("You area alraedy at max level");
                return;
            }

            if (talisman.CurrentXP >= talisman.ExpGoal && talisman.Level >= desc.MaxLevels)
            {
                talisman.CurrentXP = talisman.ExpGoal;
                talisman.Level = (byte)desc.MaxLevels;
                player.UpdateTalsimans();
                player.SendError("You area alraedy at max level");
                return;
            }

            talisman.CurrentXP += amount;
            if(talisman.CurrentXP >= talisman.ExpGoal)
            {
                talisman.Level += 1;
                talisman.CurrentXP = 0;
                talisman.ExpGoal = (int)(talisman.Level * desc.BaseUpgradeCost * desc.CostMultiplier);
                if(talisman.Level == desc.MaxLevels)
                    talisman.ExpGoal = (int)(talisman.Level * desc.BaseUpgradeCost + desc.TierUpgradeCost * desc.CostMultiplier);
            }

            player.GameServer.Database.UpdateTalisman(player.AccountId, talisman.Type, talisman.Level, talisman.CurrentXP, talisman.ExpGoal, talisman.Tier);

            player.Client.Account.Essence -= amount;
            player.UpdateTalsimans();
        }

        private void TierUpgrade(Player player, TalismanData talisman, TalismanDesc desc, int amount)
        {
            var remainingEssence = GetEssenceRemaining(player, amount);
            if (remainingEssence < 0)
            {
                player.SendError("You dont have enough essence to do that");
                return;
            }

            if (talisman.Level != desc.MaxLevels)
            {
                player.SendError("You must max all levels to upgrade tier");
                return;
            }

            if (talisman.Tier > 2 || talisman.Tier >= desc.Tiers.Count - 1) // we start at tier 0 as first tier
            {
                player.SendError("You are already at max tier");
                return;
            }

            talisman.Tier += 1;
            talisman.CurrentXP = 0;
            talisman.Level = 1;
            if (talisman.Level == desc.MaxLevels)
                talisman.ExpGoal = (int)((talisman.Level * desc.BaseUpgradeCost + desc.TierUpgradeCost) * desc.CostMultiplier);
            else
                talisman.ExpGoal = (int)(talisman.Level * desc.BaseUpgradeCost * desc.CostMultiplier);

            player.GameServer.Database.UpdateTalisman(player.AccountId, talisman.Type, talisman.Level, talisman.CurrentXP, talisman.ExpGoal, talisman.Tier);

            player.Client.Account.Essence -= amount;
            player.UpdateTalsimans();
        }

        private void EnableTalisman(Player player, TalismanData talisman, TalismanDesc desc)
        {
            if (talisman.Active)
            {
                player.SendError("Talisman is already enabled");
                return;
            }

            if(desc.Requires16 && player.GetMaxedStats() != 16)
            {
                player.UpdateTalsimans();
                player.SendError("You must be 16/16");
                return;
            }

            if (player.ActiveTalismans.Count >= 4)
            {
                player.UpdateTalsimans();
                player.SendError("You can only equip 4 talismans for now");
                return;
            }

            if (talisman.Level > desc.MaxLevels)
            {
                talisman.CurrentXP = talisman.ExpGoal;
                talisman.Level = (byte)desc.MaxLevels;
                player.UpdateTalsimans();
                player.SendError("You area alraedy at max level");
                return;
            }

            talisman.Active = true;
            player.GameServer.Database.SetCharacterActiveTalisman(player.AccountId, player.Client.Character.CharId, talisman.Type, talisman.Active);
            player.ActivateTalisman(talisman.Type);
        }

        private void DisableTalisman(Player player, TalismanData talisman, TalismanDesc desc)
        {
            if (!talisman.Active)
            {
                player.SendError("Talisman is already disabled");
                return;
            }

            if (talisman.Level > desc.MaxLevels)
            {
                talisman.CurrentXP = talisman.ExpGoal;
                talisman.Level = (byte)desc.MaxLevels;
                player.UpdateTalsimans();
                player.SendError("You area alraedy at max level");
                return;
            }

            talisman.Active = false;
            player.GameServer.Database.SetCharacterActiveTalisman(player.AccountId, player.Client.Character.CharId, talisman.Type, talisman.Active);
            player.DeactivateTalisman(talisman.Type);
        }

        private int GetEssenceRemaining(Player player, int amount)
        {
            if(amount < 0)
                return -1;
            return player.Client.Account.Essence - amount;
        }
    }
}
