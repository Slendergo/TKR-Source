using common.resources;
using System;
using System.Collections.Generic;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers
{
    internal class ForgeFusionHandler : PacketHandlerBase<ForgeFusion>
    {
        private readonly ushort[] _EarthWeaponsList = { 0x500f };
        private readonly ushort[] _fireWeaponsList = { 0x5012, 0x5013 };
        private readonly ushort[] _WaterWeaponsList = { 0x5010 };
        private readonly ushort[] _WindWeaponsList = { 0x5011, 0x500e };
        public override PacketId ID => PacketId.FORGEFUSION; //TODO : potion to great potion to souls

        protected override void HandlePacket(Client client, ForgeFusion packet) => Handle(client, packet);

        private void AnnounceForger(ushort itemValue, Client client)
        {
            var miscItem = client.CoreServerManager.Resources.GameData.Items[itemValue];

          //  if (client.Player.Rank >= 60)
            //    return;

            if (itemValue == 0x5012 || itemValue == 0x5013) //Fire Items
                client.Player.CoreServerManager.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged a Fire Elemental Item [{miscItem.DisplayName}]!");

            if (itemValue == 0x5010) //Water Item
                client.Player.CoreServerManager.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged a Water Elemental Item [{miscItem.DisplayName}]!");

            if (itemValue == 0x5011 || itemValue == 0x500e) //Wind Items
                client.Player.CoreServerManager.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged a Wind Elemental Item [{miscItem.DisplayName}]!");

            if (itemValue == 0x500f) //Earth Item
                client.Player.CoreServerManager.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged an Earth Elemental Item [{miscItem.DisplayName}]!");
            if (itemValue == 0xa420)
            {
                client.Player.CoreServerManager.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged both Claws of Cerberus into [{miscItem.DisplayName}]!");
                client.Player.Client.SendPacket(new GlobalNotification() { Text = "eternalloot" });
            }
        }
        private void Handle(Client client, ForgeFusion packet)
        {
            var rnd = new Random();
            var list = new List<Item>();
            var gameData = client.CoreServerManager.Resources.GameData;
            var forgeItems = new ForgeItem[packet.myInventory.Length];

            if (forgeItems.Length < 2) { client.Player.SendError("Select more than one item!"); return; }
            if (forgeItems.Length > 2) { client.Player.SendError("For now, select only 2 items to forge."); return; }

            for (var i = 0; i < packet.myInventory.Length; i++)
            {
                var slot = packet.myInventory[i].slotID;

                if (client.Player.Inventory[slot] == null || packet.myInventory[i].ObjectType != client.Player.Inventory[slot].ObjectType)
                {
                    client.Player.SendError("Something wrong happened. Try again.");
                    return;
                }
            }

            for (var i = 0; i < packet.myInventory.Length; i++)
            {
                forgeItems[i].ObjectType = packet.myInventory[i].ObjectType;
                forgeItems[i].slotID = packet.myInventory[i].slotID;
                forgeItems[i].Included = packet.myInventory[i].Included;
                list.Add(gameData.Items[packet.myInventory[i].ObjectType]);
            }

            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                    break;
                for (var j = 0; j < i; j++)
                {
                    if (list[j] == null || list[i] == null)
                        break;

                    switch (list[i].ObjectType * list[j].ObjectType)
                    {
                        #region Fuse Pots

                        case 0xae9 * 0xaea: //Life/Mana
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4978];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0xa1f * 0xa20: //Attack/Defense
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4979];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0xa21 * 0xa4c: //Speed/Dexterity
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497a];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0xa34 * 0xa35: //Vit/Wis
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497b];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x4978 * 0x4979: //Life/Mana/Att/Def
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497c];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x497a * 0x497b: //Spd/Dex/Vit/ws
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497d];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x497c * 0x497d: //Sup Potion
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x497e];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        #endregion Fuse Pots

                        #region Greater Potions

                        case 0xae9 * 0xae9: //Life
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x236E];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0xaea * 0xaea: //Mana
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x236F];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0xa1f * 0xa1f: //Attack
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x2368];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0xa20 * 0xa20: //Defense
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x2369];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0xa21 * 0xa21: //Speed
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x236A];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0xa4c * 0xa4c: //Dexterity
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x236D];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0xa34 * 0xa34:  //Vitality
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x236B];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0xa35 * 0xa35:  //Wisdom
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x236C];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        #endregion Greater Potions

                        #region Soul Potions

                        case 0x236E * 0x236E: //Life
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4970];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x236F * 0x236F: //Mana
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4971];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x2368 * 0x2368: //Attack
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4972];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x2369 * 0x2369: //Defense
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4973];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x236A * 0x236A: //Speed
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4974];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x236D * 0x236D: //Dexterity
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4975];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x236B * 0x236B:  //Vitality
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4976];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x236C * 0x236C:  //Wisdom
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x4977];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        #endregion Soul Potions

                        #region Fragments

                        case 0x5019 * 0x501a:  //Fire/Water Fragment
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x501e];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x501b * 0x501c:  //Wind/Earth Fragment
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x501d];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x501d * 0x501e:  //Elemental Fragment
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x501f];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x501f * 0x5019:  //Fire Items
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            ushort itemValue1 = _fireWeaponsList[rnd.Next(_fireWeaponsList.Length)];
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[itemValue1];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            AnnounceForger(itemValue1, client);
                            return;

                        case 0x501f * 0x501a:  //Water Items
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            ushort itemValue2 = _WaterWeaponsList[rnd.Next(_WaterWeaponsList.Length)];
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[itemValue2];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            AnnounceForger(itemValue2, client);
                            return;

                        case 0x501f * 0x501b:  //Wind Items
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            ushort itemValue3 = _WindWeaponsList[rnd.Next(_WindWeaponsList.Length)];
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[itemValue3];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            AnnounceForger(itemValue3, client);
                            return;

                        case 0x501f * 0x501c:  //Earth Items
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            ushort itemValue4 = _EarthWeaponsList[rnd.Next(_EarthWeaponsList.Length)];
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[itemValue4];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            AnnounceForger(itemValue4, client);
                            return;

                        case 0x5011 * 0x501b: // Reforge Wind Carrier into Velocity
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x500e];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x500e * 0x501b: // Reforge Velocity into Wind Carrier
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x5011];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x5012 * 0x5019: // Reforge Rod of Fire into Ancient Wand
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x5013];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        case 0x5013 * 0x5019: // Reforge Ancient Wand into Rod of Fire
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0x5012];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            return;

                        #endregion Fragments
                        #region Talisman
                        case 0xa41e * 0xa41f:  //Cerberus's Claws
                            list.Remove(list[i]);
                            list.Remove(list[j]);
                            client.Player.Inventory[forgeItems[0].slotID] = gameData.Items[0xa420];
                            client.Player.Inventory[forgeItems[1].slotID] = null;
                            AnnounceForger(0xa420, client);
                            return;
                            #endregion
                    }
				}
            }
        }
    }
}
