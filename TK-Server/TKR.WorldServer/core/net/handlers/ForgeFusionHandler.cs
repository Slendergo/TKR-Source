﻿using TKR.Shared;
using System;
using System.Collections.Generic;
using TKR.WorldServer.networking;
using TKR.WorldServer.networking.packets.outgoing;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.core.net.datas;

namespace TKR.WorldServer.core.net.handlers
{
    public class ForgeFusionHandler : IMessageHandler
    {
        private readonly ushort[] _EarthWeaponsList = { 0x500f };
        private readonly ushort[] _fireWeaponsList = { 0x5012, 0x5013 };
        private readonly ushort[] _WaterWeaponsList = { 0x5010 };
        private readonly ushort[] _WindWeaponsList = { 0x5011, 0x500e };
        public override MessageId MessageId => MessageId.FORGEFUSION;

        public override void Handle(Client client, NetworkReader rdr, ref TickTime time)
        {
            var myInventory = new ForgeItem[rdr.ReadInt16()];
            for (int i = 0; i < myInventory.Length; i++)
            {
                myInventory[i].ObjectType = rdr.ReadUInt16();
                myInventory[i].slotID = rdr.ReadInt32();
                myInventory[i].Included = rdr.ReadBoolean();
            }


            //var list = new List<Item>();
            var list = new List<int>();
            var gameData = client.GameServer.Resources.GameData;
            var forgeItems = new ForgeItem[myInventory.Length];

            if (forgeItems.Length < 2)
            {
                client.Player.SendError("Select more than one item!");
                return;
            }

            //if (forgeItems.Length > 2) { client.Player.SendError("For now, select only 2 items to forge."); return; }

            for (var i = 0; i < myInventory.Length; i++)
            {
                var slot = myInventory[i].slotID;
                if (client.Player.Inventory[slot] == null || myInventory[i].ObjectType != client.Player.Inventory[slot].ObjectType)
                {
                    client.Player.SendError("Something wrong happened. Try again.");
                    return;
                }
            }

            for (var i = 0; i < myInventory.Length; i++)
            {
                forgeItems[i].ObjectType = myInventory[i].ObjectType;
                forgeItems[i].slotID = myInventory[i].slotID;
                forgeItems[i].Included = myInventory[i].Included;
                //list.Add(gameData.Items[packet.myInventory[i].ObjectType]);
                list.Add(myInventory[i].ObjectType);
            }

            list.Sort();
            list.Reverse();
            var forgeList = string.Join(", ", list);
            //client.Player.SendError(forgeList);

            switch (forgeList)
            {
                //case "2794, 2793, 2636, 2613, 2612, 2593, 2592, 2591":
                //    client.Player.SendInfo("Successfully forged " + client.CoreServerManager.Resources.GameData.Items[0x497e].DisplayName); // supreme potion
                //    list.Clear();
                //    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497e];
                //    for (var i = 1; i < forgeItems.Length; i++)
                //        client.Player.Inventory[forgeItems[i].slotID] = null;
                //    return;
                //case "2592, 2591":
                //    client.Player.SendInfo("Successfully forged " + client.CoreServerManager.Resources.GameData.Items[0x4979].DisplayName); // att/def
                //    list.Clear();
                //    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4979];
                //    for (var i = 1; i < forgeItems.Length; i++)
                //        client.Player.Inventory[forgeItems[i].slotID] = null;
                //    return;
                //case "2794, 2793":
                //    client.Player.SendInfo("Successfully forged " + client.CoreServerManager.Resources.GameData.Items[0x4978].DisplayName); //Life/Mana
                //    list.Clear();
                //    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4978];
                //    for (var i = 1; i < forgeItems.Length; i++)
                //        client.Player.Inventory[forgeItems[i].slotID] = null;
                //    return;
                //case "2636, 2593":
                //    client.Player.SendInfo("Successfully forged " + client.CoreServerManager.Resources.GameData.Items[0x497a].DisplayName); //Speed/Dexterity
                //    list.Clear();
                //    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497a];
                //    for (var i = 1; i < forgeItems.Length; i++)
                //        client.Player.Inventory[forgeItems[i].slotID] = null;
                //    return;
                //case "2613, 2612":
                //    client.Player.SendInfo("Successfully forged " + client.CoreServerManager.Resources.GameData.Items[0x497b].DisplayName); //Vit/Wis -- wrong
                //    list.Clear();
                //    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497b];
                //    for (var i = 1; i < forgeItems.Length; i++)
                //        client.Player.Inventory[forgeItems[i].slotID] = null;
                //    return;

                case "18801, 18800":
                    client.Player.SendInfo("Successfully forged " + client.GameServer.Resources.GameData.Items[0x4978].DisplayName); // life/mana
                    list.Clear();
                    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4978];
                    for (var i = 1; i < forgeItems.Length; i++)
                        client.Player.Inventory[forgeItems[i].slotID] = null;
                    return;
                case "18803, 18802":
                    client.Player.SendInfo("Successfully forged " + client.GameServer.Resources.GameData.Items[0x4979].DisplayName); // att/def
                    list.Clear();
                    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4979];
                    for (var i = 1; i < forgeItems.Length; i++)
                        client.Player.Inventory[forgeItems[i].slotID] = null;
                    return;
                case "18805, 18804":
                    client.Player.SendInfo("Successfully forged " + client.GameServer.Resources.GameData.Items[0x497A].DisplayName); // spd/dex
                    list.Clear();
                    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497A];
                    for (var i = 1; i < forgeItems.Length; i++)
                        client.Player.Inventory[forgeItems[i].slotID] = null;
                    return;
                case "18807, 18806":
                    client.Player.SendInfo("Successfully forged " + client.GameServer.Resources.GameData.Items[0x497B].DisplayName); // vit/wis
                    list.Clear();
                    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497B];
                    for (var i = 1; i < forgeItems.Length; i++)
                        client.Player.Inventory[forgeItems[i].slotID] = null;
                    return;


                case "39172, 18809, 18808":
                    client.Player.SendInfo("Successfully forged " + client.GameServer.Resources.GameData.Items[0x497c].DisplayName); //Life/Mana/Att/Def
                    list.Clear();
                    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497c];
                    for (var i = 1; i < forgeItems.Length; i++)
                        client.Player.Inventory[forgeItems[i].slotID] = null;
                    return;
                case "39172, 18811, 18810":
                    client.Player.SendInfo("Successfully forged " + client.GameServer.Resources.GameData.Items[0x497d].DisplayName); //Spd/Dex/Vit/ws
                    list.Clear();
                    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497d];
                    for (var i = 1; i < forgeItems.Length; i++)
                        client.Player.Inventory[forgeItems[i].slotID] = null;
                    return;
                case "39171, 18813, 18812":
                    //client.Player.SendInfo("Successfully forged " + client.CoreServerManager.Resources.GameData.Items[0x497e].DisplayName);
                    AnnounceForger(0x497e, client);
                    list.Clear();
                    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497e];
                    for (var i = 1; i < forgeItems.Length; i++)
                        client.Player.Inventory[forgeItems[i].slotID] = null;
                    return;
                case "20506, 20505": //Fire/Water Fragment
                    //client.Player.SendInfo("Successfully forged " + client.CoreServerManager.Resources.GameData.Items[0x497e].DisplayName);
                    AnnounceForger(0x501e, client);
                    list.Clear();
                    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x501e];
                    for (var i = 1; i < forgeItems.Length; i++)
                        client.Player.Inventory[forgeItems[i].slotID] = null;
                    return;
                case "20508, 20507": //Wind/Earth Fragment
                    //client.Player.SendInfo("Successfully forged " + client.CoreServerManager.Resources.GameData.Items[0x497e].DisplayName);
                    AnnounceForger(0x501d, client);
                    list.Clear();
                    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x501d];
                    for (var i = 1; i < forgeItems.Length; i++)
                        client.Player.Inventory[forgeItems[i].slotID] = null;
                    return;
                case "20510, 20509": //Elemental Fragment
                    //client.Player.SendInfo("Successfully forged " + client.CoreServerManager.Resources.GameData.Items[0x497e].DisplayName);
                    AnnounceForger(0x501f, client);
                    list.Clear();
                    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x501f];
                    for (var i = 1; i < forgeItems.Length; i++)
                        client.Player.Inventory[forgeItems[i].slotID] = null;
                    return;
                case "20511, 20505": //Fire Items
                    //client.Player.SendInfo("Successfully forged " + client.CoreServerManager.Resources.GameData.Items[0x497e].DisplayName); 
                    ushort itemValue1 = _fireWeaponsList[Random.Shared.Next(_fireWeaponsList.Length)];
                    AnnounceForger(itemValue1, client);
                    list.Clear();
                    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[itemValue1];
                    for (var i = 1; i < forgeItems.Length; i++)
                        client.Player.Inventory[forgeItems[i].slotID] = null;
                    return;
                case "20511, 20506": //Water Items
                    //client.Player.SendInfo("Successfully forged " + client.CoreServerManager.Resources.GameData.Items[0x497e].DisplayName); 
                    ushort itemValue2 = _WaterWeaponsList[Random.Shared.Next(_WaterWeaponsList.Length)];
                    AnnounceForger(itemValue2, client);
                    list.Clear();
                    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[itemValue2];
                    for (var i = 1; i < forgeItems.Length; i++)
                        client.Player.Inventory[forgeItems[i].slotID] = null;
                    return;
                case "20511, 20507": //Wind Items
                    //client.Player.SendInfo("Successfully forged " + client.CoreServerManager.Resources.GameData.Items[0x497e].DisplayName);
                    ushort itemValue3 = _WindWeaponsList[Random.Shared.Next(_WindWeaponsList.Length)];
                    AnnounceForger(itemValue3, client);
                    list.Clear();
                    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[itemValue3];
                    for (var i = 1; i < forgeItems.Length; i++)
                        client.Player.Inventory[forgeItems[i].slotID] = null;
                    return;
                case "20511, 20508": //Earth Items
                    //client.Player.SendInfo("Successfully forged " + client.CoreServerManager.Resources.GameData.Items[0x497e].DisplayName);
                    ushort itemValue4 = _EarthWeaponsList[Random.Shared.Next(_EarthWeaponsList.Length)];
                    AnnounceForger(itemValue4, client);
                    list.Clear();
                    client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[itemValue4];
                    for (var i = 1; i < forgeItems.Length; i++)
                        client.Player.Inventory[forgeItems[i].slotID] = null;
                    return;
                default:
                    client.Player.SendError("The combination doesn't work");
                    return;

            }




            //            #region Soul Potions

            //            case 0x236E * 0x236E: //Life
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4970];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0x236F * 0x236F: //Mana
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4971];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0x2368 * 0x2368: //Attack
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4972];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0x2369 * 0x2369: //Defense
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4973];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0x236A * 0x236A: //Speed
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4974];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0x236D * 0x236D: //Dexterity
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4975];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0x236B * 0x236B:  //Vitality
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4976];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0x236C * 0x236C:  //Wisdom
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4977];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            #endregion Soul Potions

            //            case 0x497a * 0x497b: //Spd/Dex/Vit/ws
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497d];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0x497c * 0x497d: //Sup Potion sup 0x9903
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497e];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            #endregion Fuse Pots

            //            #region Greater Potions

            //            case 0xae9 * 0xae9: //Life
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x236E];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0xaea * 0xaea: //Mana
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x236F];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0xa1f * 0xa1f: //Attack
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x2368];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0xa20 * 0xa20: //Defense
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x2369];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0xa21 * 0xa21: //Speed
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x236A];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0xa4c * 0xa4c: //Dexterity
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x236D];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0xa34 * 0xa34:  //Vitality
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x236B];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0xa35 * 0xa35:  //Wisdom
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x236C];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            #endregion Greater Potions

            //            #region Fragments

            //            case 0x5019 * 0x501a:  //Fire/Water Fragment
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x501e];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0x501b * 0x501c:  //Wind/Earth Fragment
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x501d];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0x501d * 0x501e:  //Elemental Fragment
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x501f];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0x501f * 0x5019:  //Fire Items
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                ushort itemValue1 = _fireWeaponsList[Random.Shared.Next(_fireWeaponsList.Length)];
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[itemValue1];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                AnnounceForger(itemValue1, client);
            //                return;

            //            case 0x501f * 0x501a:  //Water Items
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                ushort itemValue2 = _WaterWeaponsList[Random.Shared.Next(_WaterWeaponsList.Length)];
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[itemValue2];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                AnnounceForger(itemValue2, client);
            //                return;

            //            case 0x501f * 0x501b:  //Wind Items
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                ushort itemValue3 = _WindWeaponsList[Random.Shared.Next(_WindWeaponsList.Length)];
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[itemValue3];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                AnnounceForger(itemValue3, client);
            //                return;

            //            case 0x501f * 0x501c:  //Earth Items
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                ushort itemValue4 = _EarthWeaponsList[Random.Shared.Next(_EarthWeaponsList.Length)];
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[itemValue4];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                AnnounceForger(itemValue4, client);
            //                return;

            //            case 0x5011 * 0x501b: // Reforge Wind Carrier into Velocity
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x500e];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0x500e * 0x501b: // Reforge Velocity into Wind Carrier
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x5011];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0x5012 * 0x5019: // Reforge Rod of Fire into Ancient Wand
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x5013];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            case 0x5013 * 0x5019: // Reforge Ancient Wand into Rod of Fire
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x5012];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                return;

            //            #endregion Fragments
            //            #region Talisman
            //            case 0xa41e * 0xa41f:  //Cerberus's Claws
            //                list.Remove(list[i]);
            //                list.Remove(list[j]);
            //                client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0xa420];
            //                client.Player.Inventory[forgeItems[1].slotID] = null;
            //                AnnounceForger(0xa420, client);
            //                return;
            //                #endregion
            //        }
            //	}
            // }
        }

        private void AnnounceForger(ushort itemValue, Client client)
        {
            var miscItem = client.GameServer.Resources.GameData.Items[itemValue];

            //  if (client.Player.Rank >= 60)
            //    return;

            if (itemValue == 0x497e) //Supreme
                client.Player.GameServer.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged a [{miscItem.DisplayName}]!");

            if (itemValue == 0x5012 || itemValue == 0x5013) //Fire Items
                client.Player.GameServer.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged a Fire Elemental Item [{miscItem.DisplayName}]!");

            if (itemValue == 0x5010) //Water Item
                client.Player.GameServer.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged a Water Elemental Item [{miscItem.DisplayName}]!");

            if (itemValue == 0x5011 || itemValue == 0x500e) //Wind Items
                client.Player.GameServer.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged a Wind Elemental Item [{miscItem.DisplayName}]!");

            if (itemValue == 0x500f) //Earth Item
                client.Player.GameServer.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged an Earth Elemental Item [{miscItem.DisplayName}]!");
            if (itemValue == 0xa420)
            {
                client.Player.GameServer.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged both Claws of Cerberus into [{miscItem.DisplayName}]!");
                client.Player.Client.SendPacket(new GlobalNotificationMessage(0, "eternalloot"));
            }
        }
    }
}
