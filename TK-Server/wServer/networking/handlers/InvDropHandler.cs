using System;
using System.Linq;
using wServer.core;
using wServer.core.objects;
using wServer.core.worlds.logic;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;
using static wServer.core.objects.Player;

namespace wServer.networking.handlers
{
    internal class InvDropHandler : PacketHandlerBase<InvDrop>
    {
        private static readonly Random Rand = new Random();
        private const ushort normBag = 0x0500;
        private const ushort soulBag = 0x0503;

        public override PacketId ID => PacketId.INVDROP;

        protected override void HandlePacket(Client client, InvDrop packet) => Handle(client.Player, packet.SlotObject);

        private void Handle(Player player, ObjectSlot slot)
        {
            if (player == null || player.Owner == null || player.tradeTarget != null || player.Client == null)
                return;

            if (player.Stars < 2 && player.Rank < 10)
            {
                if (!(player.Owner is Vault))
                {
                    player.SendHelp("To use this feature you need 2 stars or D-1 rank.");
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    return;
                }
            }

            if (player.Owner is Marketplace)
            {
                player.Client.SendPacket(new InvResult() { Result = 1 });
                player.SendError("<Marketplace> Drop an Item is restricted in the Marketplace!");
                return;
            }

            IContainer con;

            // container isn't always the player's inventory, it's given by the SlotObject's ObjectId
            if (slot.ObjectId != player.Id)
            {
                if (player.Owner.GetEntity(slot.ObjectId) is Player)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    return;
                }

                con = player.Owner.GetEntity(slot.ObjectId) as IContainer;
            }
            else con = player as IContainer;

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
                var trans = player.CoreServerManager.Database.Conn.CreateTransaction();

                player.CoreServerManager.Database.RemoveGift(player.Client.Account, item.ObjectType, trans);

                if (!trans.Execute())
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    return;
                }
            }

            var isSoulbound = item.Soulbound || item.Revenge || item.Legendary || item.BagType >= 3 || item.SPlus || item.SNormal || item.Tier >= 7;
            var container = new Container(player.CoreServerManager, isSoulbound ? soulBag : normBag, 60000, true);

            if (isSoulbound) //TODOS LOS ITEMS SE DROPEAN EN SOULBOUND BAG
                container.BagOwners = new int[] { player.AccountId };

            // init container
            container.Inventory[0] = item;
            container.Inventory.Data[0] = data;
            container.Move(player.X + (float)((Rand.NextDouble() * 2 - 1) * 0.5), player.Y + (float)((Rand.NextDouble() * 2 - 1) * 0.5));
            container.SetDefaultSize(75);

            player.Owner.EnterWorld(container);

            // send success
            con.Inventory[slot.SlotId] = null;
            con.Inventory.Data[slot.SlotId] = null;

            player.Client.SendPacket(new InvResult() { Result = 0 });
        }
    }
}
