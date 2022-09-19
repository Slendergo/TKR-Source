using TKR.Shared;
using TKR.Shared.database;
using System;
using System.Collections.Generic;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.networking;
using TKR.WorldServer.networking.packets.outgoing;

namespace TKR.WorldServer.core.net.handlers
{
    public class FuelEngineHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.ENGINE_FUEL_ACTION;

        public override void Handle(Client client, NReader rdr, ref TickTime time)
        {
            var myInventory = new FuelEngine[rdr.ReadInt16()];
            for (int i = 0; i < myInventory.Length; i++)
            {
                myInventory[i].ObjectType = rdr.ReadUInt16();
                myInventory[i].slotID = rdr.ReadInt32();
                myInventory[i].Included = rdr.ReadBoolean();
                myInventory[i].ItemData = rdr.ReadInt32();
            }

            if (myInventory.Length < 1)
            {
                client.Player.SendError("You throw nothing into the engine - sad!");
                return;
            }

            var fuel = 0;
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
                    case "Glowing Shard":
                        fuel += myInventory[i].ItemData;
                        break;
                    case "Glowing Talisman":
                        fuel += 50;
                        break;
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

            var acc = client.Player.GameServer.Database.GetAccount(client.Player.AccountId);
            acc.FuelContributed += fuel;
            acc.FlushAsync();

            client.Player.SendInfo($"You manage to power up the engine by {fuel}");
        }
    }
}
