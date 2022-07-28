using common.database;
using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using wServer.core;
using wServer.core.objects;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers
{
    internal class PotionStorageInteractionHandler : PacketHandlerBase<PotionStorageInteraction>
    {
        public const string POTION_OF_LIFE = "Potion of Life";
        public const string POTION_OF_MANA = "Potion of Mana";
        public const string POTION_OF_ATTACK = "Potion of Attack";
        public const string POTION_OF_DEFENSE = "Potion of Defense";
        public const string POTION_OF_DEXTERITY = "Potion of Dexterity";
        public const string POTION_OF_SPEED = "Potion of Speed";
        public const string POTION_OF_VITALITY = "Potion of Vitality";
        public const string POTION_OF_WISDOM = "Potion of Wisdom";


        public override PacketId ID => PacketId.POTION_STORAGE_INTERACTION;

        private Player Player { get; set; }

        protected override void HandlePacket(Client client, PotionStorageInteraction packet)
        {
            Player = client.Player;

            var type = packet.Type;

            var typeName = type == 0 ? POTION_OF_LIFE : type == 1 ? POTION_OF_MANA : type == 2 ? POTION_OF_ATTACK : type == 3 ? POTION_OF_DEFENSE : type == 4 ? POTION_OF_SPEED : type == 5 ? POTION_OF_DEXTERITY : type == 6 ? POTION_OF_VITALITY : type == 7 ? POTION_OF_WISDOM : "Unknown";
            if (typeName == "Unknown")
            {
                Player.SendInfo("Unknown Error");
                return;
            }

            switch (packet.Action)
            {
                case 0:
                    ModifyAdd(type, typeName);
                    break;
                case 1:
                    ModifyRemove(type, typeName);
                    break;
                case 2:
                    ModifyRemove(type, typeName, false, true);
                    break;
                case 3:
                    ModifyRemove(type, typeName, true);
                    break;
                case 4:
                    ModifyRemove(type, typeName, false, false, true);
                    break;
            }
        }

        private void ModifyAdd(byte type, string typeName)
        {
            if (!CanModifyStat(type, false))
            {
                Player.SendInfo($"You can store no more {typeName}");
                return;
            }

            var isGreater = false;
            var potIndex = ScanInventory(typeName);
            if (potIndex == -1)
            {
                potIndex = ScanInventory($"Greater {typeName}");
                isGreater = true;
            }

            if (potIndex == -1)
            {
                Player.SendInfo($"You dont have any {typeName} in your inventory");
                return;
            }

            var transaction = Player.Inventory.CreateTransaction();
            transaction[potIndex] = null;
            transaction.Execute(); // might not need this as a transaction

            ModifyStat(type, true);
            if (isGreater)
                ModifyStat(type, true);

            Player.SendInfo($"You deposited a {(isGreater ? $"Greater {typeName}" : typeName)}!");
        }

        private void ModifyRemove(byte type, string typeName, bool isSell = false, bool isConsume = false, bool isMax = false)
        {
            if (CanModifyStat(type, true))
            {
                Player.SendInfo($"You have no more {typeName}");
                return;
            }

            if (isConsume)
            {
                var statInfo = Player.CoreServerManager.Resources.GameData.Classes[Player.ObjectType].Stats;
                if (Player.Stats.Base[type] >= statInfo[type].MaxValue)
                {
                    Player.SendInfo($"You are already maxed");
                    return;
                }

                Player.Stats.Base[type] += type < 2 ? 5 : 1;
                if (Player.Stats.Base[type] >= statInfo[type].MaxValue)
                    Player.Stats.Base[type] = statInfo[type].MaxValue;

                ModifyStat(type, false);
                Player.SendInfo($"You consumed a {typeName}!");
                return;
            }
            else if (isSell)
            {
                var fameToAdd = type < 2 ? 5 : 2;
                Player.CurrentFame = Player.Client.Account.Fame += fameToAdd;
                Player.Client.Account.TotalFame += fameToAdd;
                Player.CoreServerManager.Database.ReloadAccount(Player.Client.Account);
                Player.SendInfo($"You sold a {typeName} for {fameToAdd} fame!");
            }
            else if (isMax)
            {
                var statInfo = Player.CoreServerManager.Resources.GameData.Classes[Player.ObjectType].Stats;
                if (Player.Stats.Base[type] >= statInfo[type].MaxValue)
                {
                    Player.SendInfo($"You are already maxed");
                    return;
                }

                var toMax = type < 2 ? (statInfo[type].MaxValue - Player.Stats.Base[type]) / 5 : (statInfo[type].MaxValue - Player.Stats.Base[type]);
                var newToMax = 0;

                if (CheckMax(type, toMax))
                {
                    newToMax = toMax - ToMaxCalc(type, toMax);
                    toMax = newToMax;
                    Player.SendInfo($"Not enough {typeName} to max, using [{newToMax}]");
                }


                Player.Stats.Base[type] += type < 2 ? 5 * toMax : 1 * toMax;

                if (Player.Stats.Base[type] >= statInfo[type].MaxValue)
                    Player.Stats.Base[type] = statInfo[type].MaxValue;

                ModifyStat(type, false, toMax);
                if (newToMax > 0)
                    return;
                else
                    Player.SendInfo($"You Maxed {typeName.Remove(0, 9)}!");
                return;
            }
            else
            {
                var potion = Player.CoreServerManager.Resources.GameData.Items[Player.CoreServerManager.Resources.GameData.IdToObjectType[typeName]];
                var index = Player.Inventory.GetAvailableInventorySlot(potion);
                if (index == -1)
                {
                    Player.SendInfo("Your inventory is full!");
                    return;
                }

                var transaction = Player.Inventory.CreateTransaction();
                transaction[index] = potion;
                transaction.Execute(); // might not need this as a transaction

                Player.SendInfo($"You withdrew a {typeName}!");
            }

            ModifyStat(type, false);
        }


        private int ScanInventory(string item)
        {
            for (var i = 0; i < Player.Inventory.Length; i++)
                if (Player.Inventory[i]?.ObjectId == item)
                    return i;
            return -1;
        }

        private void ModifyStat(byte type, bool isAdd, int amount = 1)
        {
            var newAmount = isAdd ? amount : -amount;

            if (type == 0)
            {
                Player.SPSLifeCount += newAmount;
                Player.Client.Account.SPSLifeCount += newAmount;
            }
            else if (type == 1)
            {
                Player.SPSManaCount += newAmount;
                Player.Client.Account.SPSManaCount += newAmount;
            }
            else if (type == 2)
            {
                Player.SPSAttackCount += newAmount;
                Player.Client.Account.SPSAttackCount += newAmount;
            }
            else if (type == 3)
            {
                Player.SPSDefenseCount += newAmount;
                Player.Client.Account.SPSDefenseCount += newAmount;
            }
            else if (type == 4)
            {
                Player.SPSSpeedCount += newAmount;
                Player.Client.Account.SPSSpeedCount += newAmount;
            }
            else if (type == 5)
            {
                Player.SPSDexterityCount += newAmount;
                Player.Client.Account.SPSDexterityCount += newAmount;
            }
            else if (type == 6)
            {
                Player.SPSVitalityCount += newAmount;
                Player.Client.Account.SPSVitalityCount += newAmount;
            }
            else if (type == 7)
            {
                Player.SPSWisdomCount += newAmount;
                Player.Client.Account.SPSWisdomCount += newAmount;
            }
        }

        private int ToMaxCalc(byte type, int toMax)
        {
            switch (type)
            {
                case 0: return toMax - Player.SPSLifeCount;
                case 1: return toMax - Player.SPSManaCount;
                case 2: return toMax - Player.SPSAttackCount;
                case 3: return toMax - Player.SPSDefenseCount;
                case 4: return toMax - Player.SPSSpeedCount;
                case 5: return toMax - Player.SPSDexterityCount;
                case 6: return toMax - Player.SPSVitalityCount;
                case 7: return toMax - Player.SPSWisdomCount;
                default: return 0;
            }
        }

        private bool CheckMax(byte type, int toMax)
        {
            switch (type)
            {
                case 0: return Player.SPSLifeCount < toMax;
                case 1: return Player.SPSManaCount < toMax;
                case 2: return Player.SPSAttackCount < toMax;
                case 3: return Player.SPSDefenseCount < toMax;
                case 4: return Player.SPSSpeedCount < toMax;
                case 5: return Player.SPSDexterityCount < toMax;
                case 6: return Player.SPSVitalityCount < toMax;
                case 7: return Player.SPSWisdomCount < toMax;
                default: return false;
            }
        }

        private bool CanModifyStat(byte type, bool checkZero)
        {
            switch (type)
            {
                case 0: return checkZero ? Player.SPSLifeCount <= 0 : Player.SPSLifeCount < Player.SPSLifeCountMax;
                case 1: return checkZero ? Player.SPSManaCount <= 0 : Player.SPSManaCount < Player.SPSManaCountMax;
                case 2: return checkZero ? Player.SPSAttackCount <= 0 : Player.SPSAttackCount < Player.SPSAttackCountMax;
                case 3: return checkZero ? Player.SPSDefenseCount <= 0 : Player.SPSDefenseCount < Player.SPSDefenseCountMax;
                case 4: return checkZero ? Player.SPSSpeedCount <= 0 : Player.SPSSpeedCount < Player.SPSSpeedCountMax;
                case 5: return checkZero ? Player.SPSDexterityCount <= 0 : Player.SPSDexterityCount < Player.SPSDexterityCountMax;
                case 6: return checkZero ? Player.SPSVitalityCount <= 0 : Player.SPSVitalityCount < Player.SPSVitalityCountMax;
                case 7: return checkZero ? Player.SPSWisdomCount <= 0 : Player.SPSWisdomCount < Player.SPSWisdomCountMax;
                default: return false;
            }
        }
    }
}
