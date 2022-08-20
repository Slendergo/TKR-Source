using common;
using common.database;
using common.logger;
using common.resources;
using Mono.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.core.objects;
using wServer.core.objects.containers;
using wServer.core.worlds.logic;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.core.net.handlers
{
    public class InvSwapHandler : IMessageHandler
    {
        private const ushort soulBag = 0x0503;
        private static readonly string[] StackableItems = new string[] { "Magic Dust" }; //stackable items

        public override MessageId MessageId => MessageId.INVSWAP;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();
            var position = Position.Read(rdr);
            var slotObj1 = ObjectSlot.Read(rdr);
            var slotObj2 = ObjectSlot.Read(rdr);

            Handle(client.Player, client.Player.World.GetEntity(slotObj1.ObjectId), client.Player.World.GetEntity(slotObj2.ObjectId), slotObj1.SlotId, slotObj2.SlotId);
        }

        private void Handle(Player player, Entity from, Entity to, int slotFrom, int slotTo)
        {
            if (player.IsInMarket)
            {
                from.ForceUpdate(slotFrom);
                to.ForceUpdate(slotTo);
                player.SendInfo("You cannot swap inventory items inside the marketplace");
                player.Client.SendPacket(new InvResult() { Result = 1 });
                return;
            }

            if (player == null || player.Client == null || player.World == null)
                return;

            if (!ValidateEntities(player, from, to) || player.tradeTarget != null)
            {
                from.ForceUpdate(slotFrom);
                to.ForceUpdate(slotTo);
                player.Client.SendPacket(new InvResult() { Result = 1 });
                return;
            }

            var conFrom = (IContainer)from;
            var conTo = (IContainer)to;

            // check if stacking operation
            if (to == player)
            {
                foreach (var stack in player.Stacks)
                    if (stack.Slot == slotTo)
                    {
                        var stackTrans = conFrom.Inventory.CreateTransaction();
                        var item = stack.Push(stackTrans[slotFrom]);

                        if (item == null) // success
                        {
                            if (from is GiftChest && stackTrans[slotFrom] != null)
                            {
                                var trans = player.GameServer.Database.Conn.CreateTransaction();
                                player.GameServer.Database.RemoveGift(player.Client.Account, stackTrans[slotFrom].ObjectType, trans);
                                trans.Execute();
                            }
                            stackTrans[slotFrom] = null;
                            Inventory.Execute(stackTrans);
                            player.Client.SendPacket(new InvResult() { Result = 0 });
                            return;
                        }
                    }
            }
            
            // not stacking operation, continue on with normal swap

            // validate slot types
            if (!ValidateSlotSwap(player, conFrom, conTo, slotFrom, slotTo))
            {
                from.ForceUpdate(slotFrom);
                to.ForceUpdate(slotTo);
                player.Client.SendPacket(new InvResult() { Result = 1 });
                return;
            }

            // setup swap
            var queue = new Queue<Action>();
            var conFromTrans = conFrom.Inventory.CreateTransaction();
            var conToTrans = conTo.Inventory.CreateTransaction();
            var itemFrom = conFromTrans[slotFrom];
            var itemTo = conToTrans[slotTo];

            conToTrans[slotTo] = itemFrom;
            conFromTrans[slotFrom] = itemTo;

            var conDataFromTrans = conFrom.Inventory.CreateDataTransaction();
            var conDataToTrans = conTo.Inventory.CreateDataTransaction();
            var dataFrom = conDataFromTrans[slotFrom];
            var dataTo = conDataToTrans[slotTo];

            conDataToTrans[slotTo] = dataFrom;
            conDataFromTrans[slotFrom] = dataTo;

            if (ValidateStackable(dataTo, dataFrom, slotTo, slotFrom))
            {
                if (IsSameKindOfItem(itemTo, itemFrom, StackableItems))
                {
                    var rest = dataTo.Stack + dataFrom.Stack;
                    if (rest > dataTo.MaxStack)
                    {
                        dataTo.Stack = dataTo.MaxStack;

                        conDataToTrans[slotTo] = dataTo;
                        conToTrans[slotTo] = itemTo;

                        dataFrom.Stack = rest - dataFrom.MaxStack;
                        conDataFromTrans[slotFrom] = dataFrom;
                        conFromTrans[slotFrom] = itemFrom;
                    }
                    else
                    {
                        dataTo.Stack += dataFrom.Stack;
                        conDataToTrans[slotTo] = dataTo;
                        conToTrans[slotTo] = itemTo;

                        dataFrom = null;
                        itemFrom = null;
                        conDataFromTrans[slotFrom] = null;
                        conFromTrans[slotFrom] = null;
                    }
                    to?.InvokeStatChange((StatDataType)((int)StatDataType.InventoryData0 + slotTo), dataTo?.GetData() ?? "{}");
                    from?.InvokeStatChange((StatDataType)((int)StatDataType.InventoryData0 + slotFrom), dataFrom?.GetData() ?? "{}");
                }
            }

            #region Check

            if (!(to is Player) || !(from is Player))
            {
                if (!(to is Player))
                {
                    var container = to as Container;

                    switch (container.ObjectDesc.ObjectType) /* Case UP => No Boosted, Case Below => Boosted */
                    {
                        #region No Soulbound Bags

                        /* Realm Chest */
                        case 0x0501:
                            if (CheckNoSoulboundBag(itemFrom) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;
                        /* Brown Bag */
                        case 0x0500:
                            if (CheckNoSoulboundBag(itemFrom) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0534:
                            if (CheckNoSoulboundBag(itemFrom) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        /* Pink Bag */
                        case 0x0506:
                            if (CheckNoSoulboundBag(itemFrom) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0535:
                            if (CheckNoSoulboundBag(itemFrom) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        #endregion No Soulbound Bags

                        /* Purple Bag */
                        case 0x0503:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0536:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        /* Gold Bag */
                        case 0x0532:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0537:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        /* Cyan Bag */
                        case 0x0509:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0538:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        /* Blue Bag */
                        case 0x050b:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0539:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        /* Orange Bag */
                        case 0x0533:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x053b:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        /* White Bag */
                        case 0x50c:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x53a:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        /* Revenge Bag */
                        case 0x5076:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x5077:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;
                        /* Eternal Bag */
                        case 0xa002:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0xa003:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;
                    }
                }
                else
                {
                    var container = from as Container;

                    switch (container.ObjectDesc.ObjectType) /* Case UP => No Boosted, Case Below => Boosted */
                    {
                        #region No Soulbound Bags

                        /* Realm Chest */
                        case 0x0501:
                            if (CheckNoSoulboundBag(itemTo) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;
                        /* Brown Bag */
                        case 0x0500:
                            if (CheckNoSoulboundBag(itemTo) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0534:
                            if (CheckNoSoulboundBag(itemTo) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        /* Pink Bag */
                        case 0x0506:
                            if (CheckNoSoulboundBag(itemTo) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0535:
                            if (CheckNoSoulboundBag(itemTo) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        #endregion No Soulbound Bags

                        /* Purple Bag */
                        case 0x0503:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0536:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        /* Gold Bag */
                        case 0x0532:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0537:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        /* Cyan Bag */
                        case 0x0509:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0538:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        /* Blue Bag */
                        case 0x050b:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0539:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        /* Orange Bag */
                        case 0x0533:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x053b:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        /* White Bag */
                        case 0x50c:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x53a:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        /* Revenge Bag */
                        case 0x5076:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x5077:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;
                        /* Eternal Bag */
                        case 0xa002:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0xa003:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                player.HandleUnavailableInventoryAction(soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;
                    }
                }
            }

            #endregion Check

            if (!Inventory.DatExecute(conDataFromTrans, conDataToTrans))
            {
                if (!Inventory.DatRevert(conDataFromTrans, conDataToTrans))
                    Log.Warn($"Failed to Revert Data Changes. {player.Name} has an extra data [ {dataFrom.GetData()} or {dataTo.GetData()} ]");
            }

            // swap items
            if (Inventory.Execute(conFromTrans, conToTrans))
            {
                var db = player.GameServer.Database;
                var trans = db.Conn.CreateTransaction();

                if (from is GiftChest && itemFrom != null) db.RemoveGift(player.Client.Account, itemFrom.ObjectType, trans);
                if (to is GiftChest && itemTo != null) db.RemoveGift(player.Client.Account, itemTo.ObjectType, trans);

                if (trans.Execute())
                {
                    while (queue.Count > 0) queue.Dequeue()();

                    player.Client.SendPacket(new InvResult() { Result = 0 });
                    return;
                }

                // if execute failed, undo inventory changes
                if (!Inventory.Revert(conFromTrans, conToTrans))
                    Log.Warn($"Failed to revert changes. {player.Name} has an extra {itemFrom?.ObjectId} or {itemTo?.ObjectId}");
            }

            from.ForceUpdate(slotFrom);
            to.ForceUpdate(slotTo);

            player.Client.SendPacket(new InvResult() { Result = 1 });
        }

        private bool CheckNoSoulboundBag(Item item)
        {
            if (item != null && (item.Legendary || item.Revenge || item.Eternal || item.SPlus || item.SNormal || item.Tier != null && item.Tier >= 7 || item.BagType >= 3))
                return false;
            return true;
        }

        private bool CheckSoulboundBag(Container container, Player player)
        {
            foreach (var owner in container.BagOwners)
                if (owner != player.AccountId)
                    return false;
            return true;
        }

        private bool ValidateStackable(ItemData itemA, ItemData itemB, int slotA, int slotB) =>
            itemA != null && itemB != null
            && slotA != slotB
            && itemA.Stack > 0 && itemA.MaxStack > 0
            && itemA.Stack < itemA.MaxStack
            && itemB.Stack > 0 && itemB.MaxStack > 0
            && itemB.Stack < itemB.MaxStack;

        private bool IsSameKindOfItem(Item itemA, Item itemB, string[] objsId)
        {
            var itemAObjId = itemA.ObjectId;
            var itemBObjId = itemB.ObjectId;
            foreach (var obj in objsId)
            {
                if (itemAObjId.Contains(obj) && itemBObjId.Contains(obj))
                    return true;
            }
            return false;
        }

        private bool ValidateEntities(Player player, Entity from, Entity to)
        {
            // returns false if bad input

            if (from == null || to == null) return false;
            if ((from as IContainer) == null || (to as IContainer) == null) return false;
            if (from is Player && from != player || to is Player && to != player) return false;
            if (from is Container &&
                (from as Container).BagOwners.Length > 0 &&
                !(from as Container).BagOwners.Contains(player.AccountId))
                return false;
            if (to is Container &&
                (to as Container).BagOwners.Length > 0 &&
                !(to as Container).BagOwners.Contains(player.AccountId))
                return false;

            if (from is GiftChest && to != player ||
                to is GiftChest && from != player)
                return false;

            if (from is SpecialChest && to != player ||
                to is SpecialChest && from != player)
                return false;

            var aPos = new Vector2(from.X, from.Y);
            var bPos = new Vector2(to.X, to.Y);

            if (Vector2.DistanceSquared(aPos, bPos) > 1) return false;

            return true;
        }

        private bool ValidateSlotSwap(Player player, IContainer conA, IContainer conB, int slotA, int slotB)
            => (slotA < 12 && slotB < 12 || player.HasBackpack) &&
                conB.AuditItem(conA.Inventory[slotA], slotB) && conA.AuditItem(conB.Inventory[slotB], slotA);

    }
}
