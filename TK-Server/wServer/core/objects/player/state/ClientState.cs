using common;
using common.database;
using common.logger;
using common.resources;
using Mono.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.core.objects.containers;
using wServer.core.worlds;
using wServer.core.worlds.logic;
using wServer.networking.packets.outgoing;
using wServer.utils;

namespace wServer.core.objects.player.state
{
    public sealed class MoveRecord
    {
        public int Time;
        public float X;
        public float Y;

        public MoveRecord(int time, float x, float y)
        {
            Time = time;
            X = x;
            Y = y;
        }
    }

    public sealed class PendingShootAcknowledgement
    {
        private EnemyShoot EnemyShootMessage { get; }
        private ServerPlayerShoot ServerPlayerShootMessage { get; }

        public bool IsServerPlayerShoot => ServerPlayerShootMessage != null;

        public PendingShootAcknowledgement(EnemyShoot enemyShootMessage) => EnemyShootMessage = enemyShootMessage;
        public PendingShootAcknowledgement(ServerPlayerShoot serverPlayerShootMessage) => ServerPlayerShootMessage = serverPlayerShootMessage;

        public KeyValuePair<int, List<Projectile>> Acknowledge(GameServer gameServer, int time)
        {
            if (time == -1)
            {
                // todo check if the entity is visible for this ack if so then we disconnect cus its not valid
                
                return new KeyValuePair<int, List<Projectile>>();
            }

            var ret = new List<Projectile>();

            ObjectDesc containerXMLObject;
            ProjectileDesc xmlProjectile;

            if (EnemyShootMessage != null)
            {
                containerXMLObject = gameServer.Resources.GameData.ObjectDescs[(ushort)EnemyShootMessage.ObjectType];
                xmlProjectile = containerXMLObject.Projectiles[EnemyShootMessage.BulletType];

                for (var i = 0; i < EnemyShootMessage.NumShots; i++)
                {
                    var angle = EnemyShootMessage.Angle + EnemyShootMessage.AngleInc * i;
                    var bulletId = (byte)((EnemyShootMessage.BulletId + i) % 256);
                    ret.Add(new Projectile(time, bulletId, (ushort)EnemyShootMessage.ObjectType, angle, EnemyShootMessage.StartingPos.X, EnemyShootMessage.StartingPos.Y, EnemyShootMessage.Damage, xmlProjectile));
                }

                return new KeyValuePair<int, List<Projectile>>(EnemyShootMessage.ObjectId, ret);
            }

            containerXMLObject  = gameServer.Resources.GameData.ObjectDescs[(ushort)ServerPlayerShootMessage.ObjectType];
            xmlProjectile = containerXMLObject.Projectiles[0];

            ret.Add(new Projectile(time, ServerPlayerShootMessage.BulletId, (ushort)ServerPlayerShootMessage.ObjectType, ServerPlayerShootMessage.Angle, ServerPlayerShootMessage.StartingPos.X, ServerPlayerShootMessage.StartingPos.Y, ServerPlayerShootMessage.Damage, xmlProjectile));
            return new KeyValuePair<int, List<Projectile>>(ServerPlayerShootMessage.ObjectId, ret);
        }
    }

    public sealed class ClientState
    {
        private readonly Player Player;

        private ConcurrentQueue<int> PendingMoves = new ConcurrentQueue<int>();
       
        public ClientState(Player player)
        {
            Player = player;
        }

        private const ushort soulBag = 0x0503;
        private static readonly string[] StackableItems = new string[] { "Magic Dust", "Glowing Shard" }; //stackable items

        public void OnInvSwap(NReader rdr)
        {
            var time = rdr.ReadInt32();
            var position = Position.Read(rdr);
            var slotObj1 = ObjectSlot.Read(rdr);
            var slotObj2 = ObjectSlot.Read(rdr);

            var player = Player;
            var from = Player.World.GetEntity(slotObj1.ObjectId);
            var to = Player.World.GetEntity(slotObj2.ObjectId);
            var slotFrom = slotObj1.SlotId;
            var slotTo = slotObj2.SlotId;

            if (player.IsInMarket && (player.World is NexusWorld))
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

            if (!Inventory.DatExecute(conDataFromTrans, conDataToTrans))
            {
                if (!Inventory.DatRevert(conDataFromTrans, conDataToTrans))
                    Log.Warn($"Failed to Revert Data Changes. {player.Name} has an extra data [ {dataFrom.GetData()} or {dataTo.GetData()} ]");
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0534:
                            if (CheckNoSoulboundBag(itemFrom) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0535:
                            if (CheckNoSoulboundBag(itemFrom) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0536:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0537:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0538:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0539:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x053b:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x53a:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x5077:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0xa003:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0534:
                            if (CheckNoSoulboundBag(itemTo) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0535:
                            if (CheckNoSoulboundBag(itemTo) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0536:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0537:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0538:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x0539:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x053b:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x53a:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0x5077:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
                                from.ForceUpdate(slotFrom);
                                to.ForceUpdate(slotTo);

                                player.Client.SendPacket(new InvResult() { Result = 1 });
                                return;
                            }
                            break;

                        case 0xa003:
                            if (CheckSoulboundBag(container, player) == false)
                            {
                                HandleUnavailableInventoryAction(player, soulBag, container.World.Random, conTo, slotTo);
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


            // swap items
            if (Inventory.Execute(conFromTrans, conToTrans))
            {
                var db = player.GameServer.Database;
                var trans = db.Conn.CreateTransaction();

                if (from is GiftChest && itemFrom != null) db.RemoveGift(player.Client.Account, itemFrom.ObjectType, trans);
                if (to is GiftChest && itemTo != null) db.RemoveGift(player.Client.Account, itemTo.ObjectType, trans);

                if (trans.Execute())
                {
                    while (queue.Count > 0)
                        queue.Dequeue()();

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
            if (itemA == null || itemB == null)
                return false;

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

            if (from is GiftChest && to != player || to is GiftChest && from != player)
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

        public static void HandleUnavailableInventoryAction(Player player, ushort objectId, Random random, IContainer container, int slotId)
        {
            var bag = new Container(player.GameServer, objectId, 60000, true)
            {
                BagOwners = new[] { player.AccountId }
            };
            bag.Inventory[0] = container.Inventory[slotId];
            bag.Inventory.Data[0] = container.Inventory.Data[slotId];
            bag.Move(player.X + (float)((random.NextDouble() * 2 - 1) * 0.5), player.Y + (float)((random.NextDouble() * 2 - 1) * 0.5));
            bag.SetDefaultSize(75);

            container.Inventory[slotId] = null;
            container.Inventory.Data[slotId] = null;

            player.World.EnterWorld(bag);
        }

        public void OnPlayerShoot(NReader rdr, MoveRecord[] moveRecords)
        {
            var time = rdr.ReadInt32();
            var bulletId = rdr.ReadByte();
            var containerType = rdr.ReadInt32();
            var startingPositionX = rdr.ReadSingle();
            var startingPositionY = rdr.ReadSingle();
            var angle = rdr.ReadSingle();

            // get item desc
            if (!Player.GameServer.Resources.GameData.Items.TryGetValue((ushort)containerType, out var item))
            {
                Disconnect("Attempting to shoot a invalid item");
                return;
            }

            var validItem = false;
            for (var i = 0; i < Player.Inventory.Length; i++)
                if(Player.Inventory[i] != null && Player.Inventory[i].ObjectType == containerType)
                {
                    validItem = true;
                    break;
                }

            if (!validItem)
            {
                for (var j = 0; j < item.NumProjectiles; j++)
                    _ = Player.GetNextBulletId();
                return;
            }

            // check ability
            if (item.SlotType == Player.Inventory[1].SlotType)
            {
                if (Player.World.DisableAbilities)
                {
                    Disconnect("Attempting to fire ability in a disabled world");
                    return;
                }
                Console.WriteLine("Ability");
                return;
            }
            
            // check moverecords for valid starting position

            // check weapon
            if (Player.World.DisableShooting)
            {
                Disconnect("Attempting to shoot in a disabled world");
                return;
            }

            if (!Player.IsValidShoot(time, item.RateOfFire))
            {
                Disconnect("Invalid shoot time");
                return;
            }

            var desc = item.Projectiles[0];

            var min = desc.MinDamage;
            var max = desc.MaxDamage;
            if (Player.TalismanDamageIsAverage)
            {
                var avg = (int)((min + max) * 0.5);
                min = avg;
                max = avg;
            }

            var isFullHp = Player.HP == Player.Stats[0];
            var isFullMp = Player.MP == Player.Stats[1];

            var arcGap = item.ArcGap;
            for (var j = 0; j < item.NumProjectiles; j++)
            {
                var nextBulletId = Player.GetNextBulletId();
                var clientBulletId = (bulletId + j) % 128;
                if (nextBulletId != clientBulletId)
                {
                    Disconnect("BulletId is not in sync");
                    return;
                }

                var dmg = Player.Stats.GetAttackDamage(min, max);

                //if (player.TalismanExtraAbilityDamage > 0.0) ability only
                //    dmg = (int)(dmg + (dmg * player.TalismanExtraAbilityDamage));

                if (Player.TalismanExtraDamageOnHitHealth != 0.0)
                    dmg += (int)(dmg * (isFullHp ? Player.TalismanExtraDamageOnHitHealth : -Player.TalismanExtraDamageOnHitHealth));

                if (Player.TalismanExtraDamageOnHitMana != 0.0)
                    dmg += (int)(dmg * (isFullMp ? Player.TalismanExtraDamageOnHitMana : -Player.TalismanExtraDamageOnHitMana));

                // validate shoot position

                Player.ClientState.ActivePlayerProjectiles[nextBulletId] = new Projectile(time, nextBulletId, item.ObjectType, angle + arcGap * j, startingPositionX, startingPositionY, dmg, desc);

                Player.World.BroadcastIfVisibleExclude(new AllyShoot()
                {
                    OwnerId = Player.Id,
                    Angle = angle,
                    ContainerType = item.ObjectType,
                    BulletId = nextBulletId
                }, Player, Player);

                Player.FameCounter.Shoot();
            }
        }

        public void Update(ref TickTime time)
        {
            //check the sent aoes have all been received
            Player.PlayerUpdate.SendUpdate();
            Player.PlayerUpdate.SendNewTick(time.ElapsedMsDelta, Aoes);

            AoeCounts.Add(Player.PlayerUpdate.TickId, Aoes.Count);
            Aoes.Clear();
        }

        public void OnUpdateAck()
        {
            // update adds a list of pending changes
            // this includes tiles,
            // add
            // remove

            // upon updateack received we push them to active client state
        }

        public Queue<PendingShootAcknowledgement> PendingShootAcks { get; } = new Queue<PendingShootAcknowledgement>();
        public Dictionary<int, Projectile> ActivePlayerProjectiles { get; } = new Dictionary<int, Projectile>();
        public Dictionary<int, Projectile> ActivePlayerServerShootProjectiles { get; } = new Dictionary<int, Projectile>();
        public Dictionary<int, Dictionary<byte, Projectile>> ActiveOtherProjectiles { get; } = new Dictionary<int, Dictionary<byte, Projectile>>();

        public void AddEnemyShoot(EnemyShoot enemyShootMessage)
        {
            var shootAck = new PendingShootAcknowledgement(enemyShootMessage);
            PendingShootAcks.Enqueue(shootAck);
        }

        public void AddServerPlayerShoot(ServerPlayerShoot serverPlayerShootMessage)
        {
            var shootAck = new PendingShootAcknowledgement(serverPlayerShootMessage);
            PendingShootAcks.Enqueue(shootAck);
        }

        public void OnShootAck(int time)
        {
            // time is used to set projectile start time
            var pendingServerShoot = PendingShootAcks.Dequeue();

            var projectiles = pendingServerShoot.Acknowledge(Player.GameServer, time);
            if (projectiles.Value.Count == 0)
                return;

            if (pendingServerShoot.IsServerPlayerShoot)
            {
                var projectile = projectiles.Value[0];
                ActivePlayerServerShootProjectiles[projectile.BulletId] = projectile;
            }
            else
            {
                if (!ActiveOtherProjectiles.ContainsKey(projectiles.Key))
                    ActiveOtherProjectiles[projectiles.Key] = new Dictionary<byte, Projectile>();
                foreach (var p in projectiles.Value)
                    ActiveOtherProjectiles[projectiles.Key][p.BulletId] = p;
            }
        }

        private Dictionary<int, int> AoeCounts = new Dictionary<int, int>();
        private Queue<AoeData> PendingAoe = new Queue<AoeData>();
        private List<AoeData> Aoes = new List<AoeData>();

        public void AwaitAoe(AoeData aoe)
        {
            PendingAoe.Enqueue(aoe);
            Aoes.Add(aoe);
        }

        public void OnAoeAck(int tickId, float x, float y)
        {
            if (!AoeCounts.ContainsKey(tickId))
            {
                Disconnect("Invalid AoeAck");
                return;
            }

            AoeCounts[tickId]--;
            if(AoeCounts[tickId] == 0)
                AoeCounts.Remove(tickId);

            if (PendingAoe.Count == 0)
            {
                Disconnect("Received AoeAck without sending Aoe");
                return;
            }

            var aoe = PendingAoe.Dequeue();

            var hit = aoe.Pos.DistTo(x, y) < aoe.Radius; // * 0.2 tollerance
            if (hit)
            {
                if (Player.HasConditionEffect(ConditionEffectIndex.Invulnerable) || Player.HasConditionEffect(ConditionEffectIndex.Invincible) || Player.HasConditionEffect(ConditionEffectIndex.Hidden))
                    return;

                Player.ApplyConditionEffect(aoe.Effect, (int)(aoe.Duration * 1000.0));
                
                var dmg = (int)Player.Stats.GetDefenseDamage(aoe.Damage, false);

                Player.HP -= dmg;

                var damage = new Damage()
                {
                    TargetId = Player.Id,
                    Effects = aoe.Effect,
                    DamageAmount = dmg,
                    Kill = Player.HP <= dmg,
                };
                Player.World.BroadcastIfVisibleExclude(damage, Player, Player);

                if (Player.HP <= 0)
                {
                    _ = Player.World.GameServer.Resources.GameData.ObjectDescs.TryGetValue(aoe.OrigType, out var desc);
                    Player.Death(desc?.ObjectId ?? "Unknown");
                }
            }
        }

        public void AwaitMove(int tickId) => PendingMoves.Enqueue(tickId);

        public void OnMove(int tickId, int time, float newX, float newY, MoveRecord[] moveRecords)
        {
            if (!PendingMoves.TryDequeue(out var serverTickId))
            {
                Disconnect("One too many MovePackets");
                return;
            }

            if (serverTickId != tickId)
            {
                Disconnect("[NewTick -> Move] TickIds don't match");
                return;
            }

            if (tickId > Player.PlayerUpdate.TickId)
            {
                Disconnect("[NewTick -> Move] Invalid tickId");
                return;
            }

            if (AoeCounts.ContainsKey(tickId))
                if(AoeCounts[tickId] != 0)
                {
                    Disconnect($"AoeCounts: {AoeCounts[tickId]} != 0");
                    return;
                }

            Player.LastClientTime = time;

            if (!Player.World.Map.Contains(newX, newY))
            {
                Player.Client.Disconnect("Out of map bounds");
                return;
            }

            if (!Player.World.IsPassable(newX, newY))
            {
                StaticLogger.Instance.Info($"{Player.Name} no-clipped. {newX}, {newY}");
                Player.Client.Disconnect("No clipping");
                return;
            }

            for (var i = 0; i < moveRecords.Length; i++)
            {
                var record = moveRecords[i];
                if (!Player.World.IsPassable(record.X, record.Y))
                {
                    StaticLogger.Instance.Info($"{Player.Name} move record no-clipped. {newX}, {newY}");
                    Player.Client.Disconnect("No clipping");
                    break;
                }
            }

            // trying with just the move records isntead of not

            // check each projectile against the 110 maximum move records sent from move?
            foreach(var activePlayerProjectiles in ActivePlayerProjectiles.Values)
            {
                // do hit detection here
            }

            if (newX != -1 && newX != Player.X || newY != -1 && newY != Player.Y)
            {
                if (Player.Stars <= 2 && Player.Quest != null && Player.DistTo(newX, newY) > 50 && Player.Quest.DistTo(newX, newY) < 0.25)
                {
                    StaticLogger.Instance.Warn($"{Player.Name} was caught teleporting directly to a quest, uh oh");
                    Player.Client.Disconnect("Unexpected Error Occured");
                    return;
                }

                Player.Move(newX, newY);
                Player.PlayerUpdate.UpdateTiles = true;
            }
        }

        private void Disconnect(string reason) => Player.Client.Disconnect(reason);
    }
}
