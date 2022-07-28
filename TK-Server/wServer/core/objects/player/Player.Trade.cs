using common.database;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.core.worlds.logic;
using wServer.networking.packets.outgoing;

namespace wServer.core.objects
{
    partial class Player
    {
        internal Dictionary<Player, int> potentialTrader = new Dictionary<Player, int>();
        internal bool[] trade;
        internal bool tradeAccepted;
        internal Player tradeTarget;

        public void CancelTrade()
        {
            Client.SendPacket(new TradeDone()
            {
                Code = 1,
                Description = "Trade canceled!"
            });

            if (tradeTarget != null && tradeTarget.Client != null)
                tradeTarget.Client.SendPacket(new TradeDone()
                {
                    Code = 1,
                    Description = "Trade canceled!"
                });

            ResetTrade();
        }

        public void RequestTrade(string name)
        {
            if (Owner is Test) return;
            if (Owner is Marketplace)
            {
                SendError("<Marketplace> Trade is restricted in the Marketplace!");
                return;
            }

            CoreServerManager.Database.ReloadAccount(Client.Account);

            var acc = Client.Account;

            if (!acc.NameChosen)
            {
                SendError("A unique name is required before trading with others!");
                return;
            }

            /*if (!this.CanUseThisFeature(GenericRank.VIP))
            {
                SendError("You cannot peform this action due higher rank.");
                return;
            }*/

            if (tradeTarget != null)
            {
                SendError("Already trading!");
                return;
            }

            if (acc.Rank >= 60)
            {
                SendError("You cannot trade.");
                return;
            }

            if (Database.GuestNames.Contains(name))
            {
                SendError(name + " needs to choose a unique name first!");
                return;
            }

            var target = Owner.GetUniqueNamedPlayer(name);

            if (target == null || !target.CanBeSeenBy(this))
            {
                SendError(name + " not found!");
                return;
            }
            if (target == this)
            {
                SendError("You can't trade with yourself!");
                return;
            }

            /*if (!target.CanUseThisFeature(GenericRank.VIP))
            {
                SendError("Cannot trade with a higher rank.");
                return;
            }*/

            if (target.Client.Account.IgnoreList.Contains(AccountId))
                return; // account is ignored

            if (target.tradeTarget != null)
            {
                SendError(target.Name + " is already trading!");
                return;
            }

            if (potentialTrader.ContainsKey(target))
            {
                tradeTarget = target;
                trade = new bool[12];
                tradeAccepted = false;
                target.tradeTarget = this;
                target.trade = new bool[12];
                target.tradeAccepted = false;
                potentialTrader.Clear();
                target.potentialTrader.Clear();

                // shouldn't be needed since there is checks on
                // invswap, invdrop, and useitem packets for trading
                //MonitorTrade();
                //target.MonitorTrade();

                var my = new TradeItem[12];

                for (int i = 0; i < 12; i++)
                    my[i] = new TradeItem()
                    {
                        Item = Inventory[i] == null ? -1 : Inventory[i].ObjectType,
                        SlotType = SlotTypes[i],
                        Included = false,
                        Tradeable = Inventory[i] != null && i >= 4 && !Inventory[i].Soulbound,
                        ItemData = Inventory.Data[i]?.GetData() ?? "{}"
                    };

                var your = new TradeItem[12];

                for (int i = 0; i < 12; i++)
                    your[i] = new TradeItem()
                    {
                        Item = target.Inventory[i] == null ? -1 : target.Inventory[i].ObjectType,
                        SlotType = target.SlotTypes[i],
                        Included = false,
                        Tradeable = target.Inventory[i] != null && i >= 4 && !target.Inventory[i].Soulbound,
                        ItemData = target.Inventory.Data[i]?.GetData() ?? "{}"
                    };

                Client.SendPacket(new TradeStart()
                {
                    MyItems = my,
                    YourName = target.Name,
                    YourItems = your
                });
                target.Client.SendPacket(new TradeStart()
                {
                    MyItems = your,
                    YourName = Name,
                    YourItems = my
                });
            }
            else
            {
                target.potentialTrader[this] = 1000 * 20;
                target.Client.SendPacket(new TradeRequested()
                {
                    Name = Name
                });
                SendInfo("You have sent a trade request to " + target.Name + "!");
                return;
            }
        }

        public void ResetTrade()
        {
            if (tradeTarget != null)
            {
                tradeTarget.tradeTarget = null;
                tradeTarget.trade = null;
                tradeTarget.tradeAccepted = false;
            }

            tradeTarget = null;
            trade = null;
            tradeAccepted = false;
        }

        private void CheckTradeTimeout(TickData time)
        {
            var newState = new List<Tuple<Player, int>>();

            foreach (var i in potentialTrader)
                newState.Add(new Tuple<Player, int>(i.Key, i.Value - time.ElaspedMsDelta));

            foreach (var i in newState)
            {
                if (i.Item2 < 0)
                {
                    i.Item1.SendInfo("Trade to " + Name + " has timed out!");
                    potentialTrader.Remove(i.Item1);
                }
                else
                    potentialTrader[i.Item1] = i.Item2;
            }
        }
    }
}
