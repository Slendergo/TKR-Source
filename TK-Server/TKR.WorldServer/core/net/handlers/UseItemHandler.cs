using NLog.Targets;
using TKR.Shared;
using TKR.Shared.resources;
using TKR.WorldServer.core.activates;
using TKR.WorldServer.core.net.datas;
using TKR.WorldServer.core.objects.containers;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.structures;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.networking;
using TKR.WorldServer.networking.packets.outgoing;
using System.Security.Cryptography;
using TKR.WorldServer.core.worlds.impl;
using TKR.WorldServer.utils;
using System.Linq;
using NLog;
using System.Xml.Linq;
using TKR.WorldServer.logic;
using StackExchange.Redis;
using TKR.WorldServer.core.objects.inventory;

namespace TKR.WorldServer.core.net.handlers
{
    public class UseItemHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.USEITEM;

        public override void Handle(Client client, NetworkReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();
            var slotObject = ObjectSlot.Read(rdr);
            var itemUsePos = Position.Read(rdr);
            var useType = rdr.ReadByte();
            var sellMaxed = rdr.ReadByte();

            var player = client.Player;
            if (player?.World == null)
                return;

            //var entity = player.World.GetEntity(slotObject.ObjectId);
            //if (entity == null)
            //{
            //    client.SendPacket(new InvResult() { Result = 1 });
            //    return;
            //}

            //if (entity is Player && slotObject.ObjectId != player.Id)
            //{
            //    client.SendPacket(new InvResult() { Result = 1 });
            //    return;
            //}

            //if (player.World.DisableAbilities && slotObject.SlotId == 1 && entity is Player) // ability slot
            //{
            //    client.Disconnect("Attempting to activate ability in a disabled world");
            //    return;
            //}

            //if (player.IsInMarket && (player.World is NexusWorld))
            //{
            //    entity.ForceUpdate(slotObject.SlotId);
            //    player.SendInfo("You cannot use items inside the marketplace");
            //    client.SendPacket(new InvResult() { Result = 1 });
            //    return;
            //}

            //var container = entity as IContainer;
            //if(container == null)
            //{
            //    entity.ForceUpdate(slotObject.SlotId);
            //    client.SendPacket(new InvResult() { Result = 1 });
            //    return;
            //}

            //if (player.DistTo(entity) > 3)
            //{
            //    entity.ForceUpdate(slotObject.SlotId);
            //    client.SendPacket(new InvResult() { Result = 1 });
            //    return;
            //}

            //// get item
            //Item item = null;
            //foreach (var stack in player.Stacks.Where(stack => stack.Slot == slotObject.SlotId))
            //{
            //    item = stack.Pop();
            //    if (item == null)
            //        return;
            //    break;
            //}

            //if (item == null)
            //    item = container.Inventory[slotObject.SlotId];
            //if (item == null)
            //    return;

            //if (container is GiftChest)
            //{
            //    entity.ForceUpdate(slotObject.SlotId);
            //    client.SendPacket(new InvResult() { Result = 1 });
            //    player.SendError("Can't use items if they are in a Gift Chest.");
            //    return;
            //}

            //// make sure not trading and trying to cunsume item
            //if (player.tradeTarget != null && item.Consumable)
            //    return;

            //if (player.MP < item.MpCost)
            //{
            //    client.SendPacket(new InvResult() { Result = 1 });
            //    return;
            //}

            //if (slotObject.SlotId == 1)
            //{
            //    player.FameCounter.UseAbility();
            //    player.MP -= item.MpCost;
            //}

            //if (item.ActivateEffects.Any(eff => eff.Effect == ActivateEffects.Heal || eff.Effect == ActivateEffects.HealNova || eff.Effect == ActivateEffects.Magic || eff.Effect == ActivateEffects.MagicNova))
            //    player.FameCounter.DrinkPot();


            //var consume = slotObject.SlotId < 254;
            //foreach (var eff in item.Activates)
            //{
            //    var activate = Activate.New(eff, player);
            //    if (activate == null)
            //        continue;

            //    if (activate.ReturnsBool)
            //        consume = activate.ExecuteBool(item, ref itemUsePos);
            //    else
            //        activate.Execute(item, ref itemUsePos);
            //}

            //// inv use ? might be only used for vault

            //if (item.Consumable)
            //{
            //    if (item.Backpack)
            //    {
            //        if (!player.HasBackpack)
            //            player.HasBackpack = true;
            //        else
            //        {
            //            player.SendError("You already have a Backpack!");
            //            consume = false;
            //        }
            //    }

            //    if (consume)
            //        container.Inventory[slotObject.SlotId] = null;
            //    entity.ForceUpdate(slotObject.SlotId);
            //}

            player.UseItem(time, tickTime, slotObject.ObjectId, slotObject.SlotId, itemUsePos, sellMaxed, useType);
        }
    }
}
