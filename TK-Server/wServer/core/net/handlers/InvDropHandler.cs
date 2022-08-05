using common;
using System;
using System.Linq;
using wServer.core;
using wServer.core.objects;
using wServer.core.worlds.logic;
using wServer.networking;
using wServer.networking.packets.outgoing;
using static wServer.core.objects.Player;

namespace wServer.core.net.handlers
{
    public class InvDropHandler : IMessageHandler
    {
        private static readonly Random Rand = new Random();
        private const ushort normBag = 0x0500;
        private const ushort soulBag = 0x0503;

        public override PacketId MessageId => PacketId.INVDROP;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var slot = ObjectSlot.Read(rdr);

            var player = client.Player;
            if (player == null || player.World == null || player.tradeTarget != null || player.Client == null)
                return;

            //if (player.Stars < 2 && player.Rank < 10)
            //{
            //    if (!(player.World is VaultWorld))
            //    {
            //        player.SendHelp("To use this feature you need 2 stars or D-1 rank.");
            //        player.Client.SendPacket(new InvResult() { Result = 1 });
            //        return;
            //    }
            //}

            if (player.World is MarketplaceWorld)
            {
                player.Client.SendPacket(new InvResult() { Result = 1 });
                player.SendError("<Marketplace> Drop an Item is restricted in the Marketplace!");
                return;
            }

            IContainer con;

            // container isn't always the player's inventory, it's given by the SlotObject's ObjectId
            if (slot.ObjectId != player.Id)
            {
                if (player.World.GetEntity(slot.ObjectId) is Player)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    return;
                }

                con = player.World.GetEntity(slot.ObjectId) as IContainer;
            }
            else 
                con = player;

            if (slot.ObjectId == player.Id && player.Stacks.Any(stack => stack.Slot == slot.SlotId))
            {
                player.Client.SendPacket(new InvResult() { Result = 1 });
                return; // don't allow dropping of stacked items
            }

            if (con?.Inventory[slot.SlotId] == null)
            {
                //give proper error
                player.Client.SendPacket(new InvResult() { Result = 1 });
                return;
            }

            var item = con.Inventory[slot.SlotId];
            var data = con.Inventory.Data[slot.SlotId];

            if (!player.CanUseThisFeature(GenericRank.VIP))
            {
                player.HandleUnavailableInventoryAction(soulBag, Rand, con, slot.SlotId);
                player.Client.SendPacket(new InvResult() { Result = 0 });
                return;
            }

            if (con is GiftChest)
            {
                var trans = player.GameServer.Database.Conn.CreateTransaction();

                player.GameServer.Database.RemoveGift(player.Client.Account, item.ObjectType, trans);

                if (!trans.Execute())
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    return;
                }
            }

            var isSoulbound = item.Soulbound || item.Revenge || item.Legendary || item.BagType >= 3 || item.SPlus || item.SNormal || item.Tier >= 7;
            var container = new Container(player.GameServer, isSoulbound ? soulBag : normBag, 60000, true);

            if (isSoulbound) //TODOS LOS ITEMS SE DROPEAN EN SOULBOUND BAG
                container.BagOwners = new int[] { player.AccountId };

            // init container
            container.Inventory[0] = item;
            container.Inventory.Data[0] = data;
            container.Move(player.X + (float)((Rand.NextDouble() * 2 - 1) * 0.5), player.Y + (float)((Rand.NextDouble() * 2 - 1) * 0.5));
            container.SetDefaultSize(75);

            player.World.EnterWorld(container);

            // send success
            con.Inventory[slot.SlotId] = null;
            con.Inventory.Data[slot.SlotId] = null;

            player.Client.SendPacket(new InvResult() { Result = 0 });
        }
    }
}
