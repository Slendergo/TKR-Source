using common.database;
using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.core;
using wServer.core.objects;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers
{
    internal class AcceptTradeHandler : PacketHandlerBase<AcceptTrade>
    {
        public override PacketId ID => PacketId.ACCEPTTRADE;

        protected override void HandlePacket(Client client, AcceptTrade packet)
        {
            //client.Manager.Logic.AddPendingAction(t => Handle(client, packet));
            Handle(client, packet);
        }

        private void Handle(Client client, AcceptTrade packet)
        {
            var player = client.Player;
            if (player == null || IsTest(client))
                return;

            if (player.tradeAccepted) return;

            player.trade = packet.MyOffer;
            if (player.tradeTarget.trade.SequenceEqual(packet.YourOffer))
            {
                player.tradeAccepted = true;
                player.tradeTarget.Client.SendPacket(new TradeAccepted()
                {
                    MyOffer = player.tradeTarget.trade,
                    YourOffer = player.trade
                });

                if (player.tradeAccepted && player.tradeTarget.tradeAccepted)
                {
                    if (player.Client.Account.Admin != player.tradeTarget.Client.Account.Admin)
                    {
                        player.tradeTarget.CancelTrade();
                        player.CancelTrade();
                        return;
                    }

                    DoTrade(player);
                }
            }
        }

        private void DoTrade(Player player)
        {
            var failedMsg = "Error while trading. Trade unsuccessful.";
            var msg = "Trade Successful!";
            var thisItems = new List<(Item, ItemData)>();
            var targetItems = new List<(Item, ItemData)>();

            var tradeTarget = player.tradeTarget;

            // make sure trade targets are valid
            if (tradeTarget == null || player.Owner == null || tradeTarget.Owner == null || player.Owner != tradeTarget.Owner)
            {
                TradeDone(player, tradeTarget, failedMsg);
                return;
            }

            if (!player.tradeAccepted || !tradeTarget.tradeAccepted)
                return;

            var pInvTrans = player.Inventory.CreateTransaction();
            var tInvTrans = tradeTarget.Inventory.CreateTransaction();

            var pInvDataTrans = player.Inventory.CreateDataTransaction();
            var tInvDataTrans = player.Inventory.CreateDataTransaction();

            for (int i = 4; i < player.trade.Length; i++)
                if (player.trade[i])
                {
                    thisItems.Add((player.Inventory[i], player.Inventory.Data[i]));
                    pInvTrans[i] = null;
                }

            for (int i = 4; i < tradeTarget.trade.Length; i++)
                if (tradeTarget.trade[i])
                {
                    targetItems.Add((tradeTarget.Inventory[i], tradeTarget.Inventory.Data[i]));
                    tInvTrans[i] = null;
                }

            // move thisItems -> tradeTarget
            for (var i = 0; i < 12; i++)
                for (var j = 0; j < thisItems.Count; j++)
                {
                    if ((tradeTarget.SlotTypes[i] == 0 && tInvTrans[i] == null) || (thisItems[j].Item1 != null && tradeTarget.SlotTypes[i] == thisItems[j].Item1.SlotType && tInvTrans[i] == null))
                    {
                        tInvTrans[i] = thisItems[j].Item1;
                        tInvDataTrans[i] = thisItems[j].Item2;
                        thisItems.Remove(thisItems[j]);
                        break;
                    }
                }

            // move tradeItems -> this
            for (var i = 0; i < 12; i++)
                for (var j = 0; j < targetItems.Count; j++)
                {
                    if ((player.SlotTypes[i] == 0 && pInvTrans[i] == null) || (targetItems[j].Item1 != null && player.SlotTypes[i] == targetItems[j].Item1.SlotType && pInvTrans[i] == null))
                    {
                        pInvTrans[i] = targetItems[j].Item1;
                        pInvDataTrans[i] = targetItems[j].Item2;
                        targetItems.Remove(targetItems[j]);
                        break;
                    }
                }

            // save
            if(!Inventory.DatExecute(pInvDataTrans, tInvDataTrans))
            {
                TradeDone(player, tradeTarget, failedMsg);
                return;
            }
            if (!Inventory.Execute(pInvTrans, tInvTrans))
            {
                TradeDone(player, tradeTarget, failedMsg);
                return;
            }

            // check for lingering items
            if (thisItems.Count > 0 || targetItems.Count > 0)
            {
                msg = "An error occured while trading! Some items were lost!";
            }

            // trade successful, notify and save
            TradeDone(player, tradeTarget, msg);
        }

        private void TradeDone(Player player, Player tradeTarget, string msg)
        {
            player.Client.SendPacket(new TradeDone
            {
                Code = 1,
                Description = msg
            });

            if (tradeTarget != null)
            {
                tradeTarget.Client.SendPacket(new TradeDone
                {
                    Code = 1,
                    Description = msg
                });
            }
            player.ResetTrade();
        }
    }
}
