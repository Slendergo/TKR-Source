using common;
using common.database;
using System;
using System.Collections.Generic;
using wServer.core;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.core.net.handlers
{
    public class FuelEngineHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.ENGINE_FUEL_ACTION;

        public override void Handle(Client client, NReader rdr, ref TickTime time)
        {
            var myInventory = new FuelEngine[rdr.ReadInt16()];
            var rnd = new Random();
            for (int i = 0; i < myInventory.Length; i++)
            {
                myInventory[i].ObjectType = rdr.ReadUInt16();
                myInventory[i].slotID = rdr.ReadInt32();
                myInventory[i].Included = rdr.ReadBoolean();
                myInventory[i].ItemData = rdr.ReadInt32();
            }


            //var list = new List<Item>();
            var list = new List<int>();
            var gameData = client.GameServer.Resources.GameData;
            var forgeItems = new FuelEngine[myInventory.Length];
            int fuel = 0;

            if (forgeItems.Length < 1)
            {
                client.Player.SendError("You throw nothing into the engine - sad!");
                return;
            }

            for (var i = 0; i < myInventory.Length; i++)
            {
                Console.WriteLine("i=" + i +" | SlotID="+ myInventory[i].slotID +" | ObjectType="+ myInventory[i].ObjectType);
            }

            for (var i = 0; i < myInventory.Length; i++)
            {
                var slot = myInventory[i].slotID;
                if (client.Player.Inventory[slot] == null || myInventory[i].ObjectType != client.Player.Inventory[slot].ObjectType)
                {
                    client.Player.SendError("Something wrong happened. Try again. [0x02]");
                    return;
                }

                if (client.Player.Inventory[slot].DisplayName == "Glowing Shard" && client.Player.Inventory.Data[slot] == null)
                {
                    client.Player.SendError("Something wrong happened. Try again. [0x03]");
                    return;
                }

                switch (client.Player.Inventory[slot].DisplayName)
                {
                    case "Glowing Shard": fuel += myInventory[i].ItemData; break;
                    case "Glowing Talisman": fuel += 50; break;
                }
            }

            if (!client.Player.GameServer.WorldManager.Nexus.TryAddFuelToEngine(client.Player, fuel))
            {
                client.Player.SendError($"Engine is max capacity");
                return;
            }
            for (var i = 0; i < myInventory.Length; i++)
            {
                client.Player.Inventory[myInventory[i].slotID] = null;
                client.Player.Inventory.Data[myInventory[i].slotID] = null;
            }
            client.Player.SendInfo("You manage to power up the engine by "+fuel);
        }

        //private void AnnounceFuel(int fuel, Client client)
        //{
        //    var miscItem = client.GameServer.Resources.GameData.Items[itemValue];

        //    //  if (client.Player.Rank >= 60)
        //    //    return;

        //    if (itemValue == 0x497e) //Supreme
        //        client.Player.GameServer.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged a [{miscItem.DisplayName}]!");

        //    if (itemValue == 0x5012 || itemValue == 0x5013) //Fire Items
        //        client.Player.GameServer.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged a Fire Elemental Item [{miscItem.DisplayName}]!");

        //    if (itemValue == 0x5010) //Water Item
        //        client.Player.GameServer.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged a Water Elemental Item [{miscItem.DisplayName}]!");

        //    if (itemValue == 0x5011 || itemValue == 0x500e) //Wind Items
        //        client.Player.GameServer.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged a Wind Elemental Item [{miscItem.DisplayName}]!");

        //    if (itemValue == 0x500f) //Earth Item
        //        client.Player.GameServer.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged an Earth Elemental Item [{miscItem.DisplayName}]!");
        //    if (itemValue == 0xa420)
        //    {
        //        client.Player.GameServer.ChatManager.AnnounceForger($"[{client.Player.Name}] has forged both Claws of Cerberus into [{miscItem.DisplayName}]!");
        //        client.Player.Client.SendPacket(new GlobalNotification() { Text = "eternalloot" });
        //    }
        //}
    }
}
