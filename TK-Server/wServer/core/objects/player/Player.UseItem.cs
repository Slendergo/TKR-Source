using CA.Extensions.Concurrent;
using common;
using common.resources;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.core.worlds;
using wServer.core.worlds.logic;
using wServer.networking;
using wServer.networking.packets;
using wServer.networking.packets.outgoing;
using wServer.networking.packets.outgoing.talisman;
using wServer.utils;

namespace wServer.core.objects
{
    partial class Player
    {
        public const int MaxAbilityDist = 14;

        public static readonly ConditionEffect[] NegativeEffs = new ConditionEffect[]
        {
            new ConditionEffect(ConditionEffectIndex.Slowed, 0),
            new ConditionEffect(ConditionEffectIndex.Paralyzed, 0),
            new ConditionEffect(ConditionEffectIndex.Weak, 0),
            new ConditionEffect(ConditionEffectIndex.Stunned, 0),
            new ConditionEffect(ConditionEffectIndex.Confused, 0),
            new ConditionEffect(ConditionEffectIndex.Blind, 0),
            new ConditionEffect(ConditionEffectIndex.Quiet, 0),
            new ConditionEffect(ConditionEffectIndex.ArmorBroken, 0),
            new ConditionEffect(ConditionEffectIndex.Bleeding, 0),
            new ConditionEffect(ConditionEffectIndex.Dazed,0),
            new ConditionEffect(ConditionEffectIndex.Sick, 0),
            new ConditionEffect(ConditionEffectIndex.Drunk, 0),
            new ConditionEffect(ConditionEffectIndex.Hallucinating, 0),
            new ConditionEffect(ConditionEffectIndex.Hexed, 0),
            new ConditionEffect(ConditionEffectIndex.Curse, 0),
            new ConditionEffect(ConditionEffectIndex.Unstable, 0)
        };

        public bool PoisonWis = false;

        public void AEItemDust(TickTime time, Item item, Position target, int slot, int objId, ActivateEffect eff)
        {
            var entity = World.GetEntity(objId);

            var dustItem = World.GameServer.ItemDustWeights.ItemDusts.GetRandom(World.Random);
            if (entity is Container container)
                container.Inventory[slot] = dustItem;
            else
                Inventory[slot] = dustItem;

            SendInfo($"Used a Item Dust and obtained a {item.DisplayName}");
        }
        public void AESpecialDust(TickTime time, Item item, Position target, int slot, int objId, ActivateEffect eff)
        {
            var entity = World.GetEntity(objId);

            var dustItem = World.GameServer.ItemDustWeights.SpecialDust.GetRandom(World.Random);
            if (entity is Container container)
                container.Inventory[slot] = dustItem;
            else
                Inventory[slot] = dustItem;

            GameServer.ChatManager.AnnounceLoot($"[{Name}] used a Special Dust and Obtained an [{dustItem.DisplayId ?? dustItem.ObjectId}]!");
        }

        public void AEUnlockChest(TickTime time, Item item, Position target, int slot, int objId, ActivateEffect eff)
        {
            Player player = this;
            var db = GameServer.Database;
            var acc = player.Client.Account;
            var trans = db.Conn.CreateTransaction();
            GameServer.Database.CreateChest(acc, trans);
            var t2 = trans.ExecuteAsync();
            acc.Reload("vaultCount");
            acc.Reload("fame");
            acc.Reload("totalFame");
            player.CurrentFame = acc.Fame;
            //(Owner as Vault).AddChest(this);
            player.SendInfo("Your Vault has been unlocked! If u are in your Vault, go out and enter again.");
        }

        public void AEUnlockSlotChar(TickTime time, Item item, Position target, int slot, int objId, ActivateEffect eff)
        {
            Player player1 = this;
            var account = player1.Client.Account;
            var transi = GameServer.Database.Conn.CreateTransaction();
            transi.AddCondition(Condition.HashEqual(account.Key, "maxCharSlot", account.MaxCharSlot));
            transi.HashIncrementAsync(account.Key, "maxCharSlot");
            var tr2 = transi.ExecuteAsync();
            account.MaxCharSlot++;
            player1.SendInfo("New Character Slot Unlocked!, go to Character selector to use them!");
        }

        public void UseItem(TickTime time, int objId, int slot, Position pos, int sellMaxed)
        {
            if (World.DisableAbilities && slot == 1) // ability slot
            {
                Client.Disconnect("Attempting to activate ability in a disabled world");
                return;
            }

            //Log.Debug(objId + ":" + slot);
            var entity = World.GetEntity(objId);
            if (entity == null)
            {
                Client.SendPacket(new InvResult() { Result = 1 });
                return;
            }

            if (entity is Player && objId != Id)
            {
                Client.SendPacket(new InvResult() { Result = 1 });
                return;
            }

            if (IsInMarket)
            {
                entity.ForceUpdate(slot);
                SendInfo("You cannot use items inside the marketplace");
                Client.SendPacket(new InvResult() { Result = 1 });
                return;
            }

            var container = entity as IContainer;

            // eheh no more clearing BBQ loot bags
            if (this.Dist(entity) > 3)
            {
                entity.ForceUpdate(slot);
                Client.SendPacket(new InvResult() { Result = 1 });
                return;
            }

            var cInv = container?.Inventory.CreateTransaction();

            // get item
            Item item = null;
            foreach (var stack in Stacks.Where(stack => stack.Slot == slot))
            {
                item = stack.Pop();

                if (item == null)
                    return;

                break;
            }
            if (item == null)
            {
                if (container == null)
                    return;

                item = cInv[slot];
            }

            if (item == null)
                return;

            // make sure not trading and trying to cunsume item
            if (tradeTarget != null && item.Consumable)
                return;
            Player player = this as Player;
            if (MP < item.MpCost)
            {
                Client.SendPacket(new InvResult() { Result = 1 });
                return;
            }

            // use item
            var slotType = 10;
            if (slot < cInv.Length)
            {
                slotType = container.SlotTypes[slot];

                if (item.TypeOfConsumable)
                {
                    var gameData = GameServer.Resources.GameData;
                    var db = GameServer.Database;

                    if (item.Consumable)
                    {
                        Item successor = null;
                        if (item.SuccessorId != null)
                            successor = gameData.Items[gameData.IdToObjectType[item.SuccessorId]];
                        cInv[slot] = successor;

                        var trans = db.Conn.CreateTransaction();
                        if (container is GiftChest)
                            if (successor != null)
                                db.SwapGift(Client.Account, item.ObjectType, successor.ObjectType, trans);
                            else
                            {
                                player.SendError("Can't use items if they are in a Gift Chest.");
                                return;
                            }
                    }

                    if (!Inventory.Execute(cInv)) // can result in the loss of an item if inv trans fails..
                    {
                        entity.ForceUpdate(slot);
                        return;
                    }

                    if (slotType > 0)
                    {
                        FameCounter.UseAbility();
                    }
                    else
                    {
                        if (item.ActivateEffects.Any(eff => eff.Effect == ActivateEffects.Heal ||
                                                            eff.Effect == ActivateEffects.HealNova ||
                                                            eff.Effect == ActivateEffects.Magic ||
                                                            eff.Effect == ActivateEffects.MagicNova))
                        {
                            FameCounter.DrinkPot();
                        }
                    }

                    Activate(time, item, slot, pos, objId, sellMaxed);
                    return;
                }

                if (slotType > 0)
                {
                    FameCounter.UseAbility();
                }
            }
            else
            {
                FameCounter.DrinkPot();
            }

            //Log.Debug(item.SlotType + ":" + slotType);
            if (item.InvUse)
                Activate(time, item, slot, pos, objId, sellMaxed);

            if (item.Consumable || item.SlotType == slotType)
                Activate(time, item, slot, pos, objId, sellMaxed);
            else
                Client.SendPacket(new InvResult() { Result = 1 });
        }

        public static void ActivateHealHp(Player player, int amount, bool broadcastSelf = false)
        {
            if (amount <= 0)
                return;

            var maxHp = player.Stats[0];
            var newHp = Math.Min(maxHp, player.HP + amount);
            if (newHp == player.HP)
                return;

            var effect = new ShowEffect()
            {
                EffectType = EffectType.Potion,
                TargetObjectId = player.Id,
                Color = new ARGB(0xffffffff)
            };

            var notif = new Notification()
            {
                Color = new ARGB(0xff00ff00),
                ObjectId = player.Id,
                Message = "+" + (newHp - player.HP)
            };

            if (broadcastSelf)
            {
                player.Client.SendPacket(effect);
                player.Client.SendPacket(notif);

            }
            else
            {
                player.World.BroadcastIfVisible(effect, player);
                player.World.BroadcastIfVisible(notif, player);
            }

            player.HP = newHp;
        }

        public static void ActivateHealMp(Player player, int amount, bool broadcastSelf = false)
        {
            var maxMp = player.Stats[1];
            var newMp = Math.Min(maxMp, player.MP + amount);
            if (newMp == player.MP)
                return;

            var effect = new ShowEffect()
            {
                EffectType = EffectType.Potion,
                TargetObjectId = player.Id,
                Color = new ARGB(0xffffffff)
            };

            var notif = new Notification()
            {
                Color = new ARGB(0xff6084E0),
                ObjectId = player.Id,
                Message = "+" + (newMp - player.MP)
            };

            if (broadcastSelf)
            {
                player.Client.SendPacket(effect);
                player.Client.SendPacket(notif);

            }
            else
            {
                player.World.BroadcastIfVisible(effect, player);
                player.World.BroadcastIfVisible(notif, player);
            }

            player.MP = newMp;
        }

        private void Activate(TickTime time, Item item, int slot, Position target, int objId, int sellmaxed)
        {
            var playeren = this as Player;
            MP -= item.MpCost;

            var entity1 = World.GetEntity(objId);

            if (entity1 is GiftChest)
            {
                playeren.SendError("You can't use items in Gift Chests");
                return;
            }

            foreach (var eff in item.ActivateEffects)
            {
                switch (eff.Effect)
                {
                    case ActivateEffects.UpgradeStat:
                        AEUpgradeStat(time, item, target, objId, slot, eff);
                        break;

                    case ActivateEffects.UpgradeActivate:
                        AEUpgradeActivate(time, item, target, objId, slot, eff);
                        break;

                    case ActivateEffects.Fame:
                        AEAddFame(time, item, target, eff);
                        break;

                    case ActivateEffects.XPBoost:
                        AEXPBoost(time, item, target, slot, objId, eff);
                        break;

                    case ActivateEffects.MagicDust:
                        AEMagicDust(time, item, target, slot, objId, eff);
                        break;

                    case ActivateEffects.SpecialDust:
                        AESpecialDust(time, item, target, slot, objId, eff);
                        break;

                    case ActivateEffects.MiscellaneousDust:
                        AEMiscellaneousDust(time, item, target, slot, objId, eff);
                        break;

                    case ActivateEffects.ItemDust:
                        AEItemDust(time, item, target, slot, objId, eff);
                        break;

                    case ActivateEffects.PotionDust:
                        AEPotionDust(time, item, target, slot, objId, eff);
                        break;

                    case ActivateEffects.UnlockChest:
                        AEUnlockChest(time, item, target, slot, objId, eff);
                        break;

                    case ActivateEffects.UnlockSlotChar:
                        AEUnlockSlotChar(time, item, target, slot, objId, eff);
                        break;

                    case ActivateEffects.LDBoost:
                        AELDBoost(time, item, target, eff);
                        break;

                    case ActivateEffects.GenericActivate:
                        AEGenericActivate(time, item, target, eff);
                        break;

                    case ActivateEffects.Create:
                        AECreate(time, item, target, slot, eff);
                        break;

                    case ActivateEffects.Dye:
                        AEDye(time, item, target, eff);
                        break;

                    case ActivateEffects.Shoot:
                        AEShoot(time, item, target, eff);
                        break;

                    case ActivateEffects.IncrementStat:
                        AEIncrementStat(time, item, target, eff, objId, slot, sellmaxed);
                        break;

                    case ActivateEffects.Heal:
                        AEHeal(time, item, target, eff);
                        break;

                    case ActivateEffects.Magic:
                        AEMagic(time, item, target, eff);
                        break;

                    case ActivateEffects.HealNova:
                        AEHealNova(time, item, target, eff);
                        break;

                    case ActivateEffects.StatBoostSelf:
                        AEStatBoostSelf(time, item, target, eff);
                        break;

                    case ActivateEffects.StatBoostAura:
                        AEStatBoostAura(time, item, target, eff);
                        break;

                    case ActivateEffects.BulletNova:
                        AEBulletNova(time, item, target, eff);
                        break;

                    case ActivateEffects.ConditionEffectSelf:
                        AEConditionEffectSelf(time, item, target, eff);
                        break;

                    case ActivateEffects.ConditionEffectAura:
                        AEConditionEffectAura(time, item, target, eff);
                        break;

                    case ActivateEffects.Teleport:
                        AETeleport(time, item, target, eff);
                        break;

                    case ActivateEffects.PoisonGrenade:
                        AEPoisonGrenade(time, item, target, eff);
                        break;

                    case ActivateEffects.VampireBlast:
                        AEVampireBlast(time, item, target, eff);
                        break;

                    case ActivateEffects.Trap:
                        AETrap(time, item, target, eff);
                        break;

                    case ActivateEffects.StasisBlast:
                        StasisBlast(time, item, target, eff);
                        break;

                    case ActivateEffects.Pet:

                        #region Pet

                        Entity en = Resolve(GameServer, eff.ObjectId);
                        en.Move(X, Y);
                        en.SetPlayerOwner(this);
                        World.EnterWorld(en);
                        World.Timers.Add(new WorldTimer(30 * 1000, (w, t) =>
                        {
                            w.LeaveWorld(en);
                        }));

                        #endregion Pet

                        break;

                    case ActivateEffects.Decoy:
                        AEDecoy(time, item, target, eff);
                        break;

                    case ActivateEffects.Lightning:
                        AELightning(time, item, target, eff);
                        break;

                    case ActivateEffects.UnlockPortal:
                        AEUnlockPortal(time, item, target, eff);
                        break;

                    case ActivateEffects.MagicNova:
                        AEMagicNova(time, item, target, eff);
                        break;

                    case ActivateEffects.ClearConditionEffectAura:
                        AEClearConditionEffectAura(time, item, target, eff);
                        break;

                    case ActivateEffects.RemoveNegativeConditions:
                        AERemoveNegativeConditions(time, item, target, eff);
                        break;

                    case ActivateEffects.ClearConditionEffectSelf:
                        AEClearConditionEffectSelf(time, item, target, eff);
                        break;

                    case ActivateEffects.RemoveNegativeConditionsSelf:
                        AERemoveNegativeConditionSelf(time, item, target, eff);
                        break;

                    case ActivateEffects.ShurikenAbility:
                        AEShurikenAbility(time, item, target, eff);
                        break;

                    case ActivateEffects.ShurikenAbilityBerserk:
                        AEShurikenAbilityBerserk(time, item, target, eff);
                        break;

                    case ActivateEffects.DazeBlast:
                        break;

                    case ActivateEffects.PermaPet:
                        AEPermaPet(time, item, target, eff);
                        break;

                    case ActivateEffects.Backpack:
                        AEBackpack(time, item, target, slot, objId, eff);
                        break;

                    case ActivateEffects.UnlockTalisman:
                        AEUnlockTalisman(time, item, target, slot, objId, eff);
                        break;

                    default:
                        StaticLogger.Instance.Warn("Activate effect {0} not implemented.", eff.Effect);
                        break;
                }
            }
        }

        private void AEUnlockTalisman(TickTime time, Item item, Position target, int slot, int objId, ActivateEffect eff)
        {
            var desc = GameServer.Resources.GameData.GetTalisman(eff.Type);
            if (desc == null)
            {
                SendError("Error unable to consume!");
                Inventory[slot] = item;
                return;
            }

            var characterId = Client.Character.CharId;
            var accountId = AccountId;

            if (GameServer.Database.HasTalismanOnCharacter(accountId, characterId, eff.Type)) {
                SendInfo("You have already unlocked this talisman!");
                Inventory[slot] = item;
                return;
            }

            var cost = (int)(desc.BaseUpgradeCost * desc.CostMultiplier);
            if (desc.MaxLevels == 1)
                cost = (int)(desc.BaseUpgradeCost + desc.TierUpgradeCost * desc.CostMultiplier);
            
            GameServer.Database.AddTalismanToCharacter(accountId, characterId, eff.Type, 1, 0, cost, 0);

            var newTalismanInfo = new TalismanData(eff.Type, 1, 0, cost, 0, false);

            Client.Player.UnlockTalisman(newTalismanInfo);
        }

        private void AEAddFame(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            if (World is TestWorld || Client.Account == null)
                return;

            var acc = Client.Account;
            acc.Reload("fame");
            acc.Reload("totalFame");
            acc.Fame += eff.Amount;
            acc.TotalFame += eff.Amount;
            acc.FlushAsync();
            acc.Reload("fame");
            acc.Reload("totalFame");
            CurrentFame = acc.Fame;
            //Manager.Database.UpdateFame(acc, eff.Amount, null);
            /*Manager.Database.UpdateCurrency(acc, eff.Amount, CurrencyType.Fame, trans)
                .ContinueWith(t =>
                {
                    CurrentFame = acc.Fame;
                });
            trans.Execute();*/
        }

        private void AEBackpack(TickTime time, Item item, Position target, int slot, int objId, ActivateEffect eff)
        {
            var entity = World.GetEntity(objId);
            var containerItem = entity as Container;
            if (HasBackpack)
            {
                SendInfo("You already have a Backpack!");
                if (containerItem != null)
                    containerItem.Inventory[slot] = item;
                else
                    Inventory[slot] = item;
                return;
            }
            HasBackpack = true;
        }

        private void AEBulletNova(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            var shoots = item.SpellProjectiles == 0 ? 20 : item.SpellProjectiles;
            var prjs = new Projectile[shoots];
            var prjDesc = item.Projectiles[0]; //Assume only one
            var batch = new OutgoingMessage[shoots + 1];
            for (var i = 0; i < shoots; i++)
            {
                var proj = CreateProjectile(prjDesc, item.ObjectType,
                    World.Random.Next(prjDesc.MinDamage, prjDesc.MaxDamage),
                    time.TotalElapsedMs, target, (float)(i * (Math.PI * 2) / shoots));
                World.AddProjectile(proj);
                FameCounter.Shoot(proj);
                batch[i] = new ServerPlayerShoot()
                {
                    BulletId = proj.ProjectileId,
                    OwnerId = Id,
                    ContainerType = item.ObjectType,
                    StartingPos = target,
                    Angle = proj.Angle,
                    Damage = (short)proj.Damage
                };
                prjs[i] = proj;
            }
            batch[shoots] = new ShowEffect()
            {
                EffectType = EffectType.Trail,
                Pos1 = target,
                TargetObjectId = Id,
                Color = new ARGB(eff.Color != 0 ? eff.Color : 0xFFFF00AA)
            };

            foreach (var player in World.Players.Values)
                if(player.DistSqr(this) < PlayerUpdate.VISIBILITY_RADIUS_SQR)
                   player.Client.SendPackets(batch);
        }

        private void AEClearConditionEffectAura(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            this.AOE(eff.Range, true, player =>
            {
                var condition = eff.CheckExistingEffect;
                ConditionEffects conditions = 0;
                conditions |= (ConditionEffects)(1 << (Byte)condition.Value);
                if (!condition.HasValue || player.HasConditionEffect(conditions))
                {
                    player.ApplyConditionEffect(new ConditionEffect(eff.ConditionEffect.Value, 0));
                }
            });
        }

        private void AEClearConditionEffectSelf(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            var condition = eff.CheckExistingEffect;
            ConditionEffects conditions = 0;

            if (condition.HasValue)
                conditions |= (ConditionEffects)(1 << (Byte)condition.Value);

            if (!condition.HasValue || HasConditionEffect(conditions))
            {
                ApplyConditionEffect(new ConditionEffect(eff.ConditionEffect.Value, 0));
            }
        }

        private void AEConditionEffectAura(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            var duration = eff.DurationMS;
            var range = eff.Range;
            if (eff.UseWisMod)
            {
                duration = (int)(UseWisMod(eff.DurationSec) * 1000);
                range = UseWisMod(eff.Range);
            }

            this.AOE(range, true, player =>
            {
                player.ApplyConditionEffect(new ConditionEffect(eff.ConditionEffect.Value, duration));
            });

            var color = 0xffffffff;
            if (eff.ConditionEffect.Value == ConditionEffectIndex.Damaging)
                color = 0xffff0000;
            World.BroadcastIfVisible(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                TargetObjectId = Id,
                Color = new ARGB(color),
                Pos1 = new Position() { X = range }
            }, this);
        }

        private void AEConditionEffectSelf(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            var duration = eff.DurationMS;
            if (eff.UseWisMod)
                duration = (int)(UseWisMod(eff.DurationSec) * 1000);

            ApplyConditionEffect(new ConditionEffect(eff.ConditionEffect.Value, duration));
            World.BroadcastIfVisible(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                TargetObjectId = Id,
                Color = new ARGB(0xffffffff),
                Pos1 = new Position() { X = 1 }
            }, this);
        }

        private void AECreate(TickTime time, Item item, Position target, int slot, ActivateEffect eff)
        {
            var gameData = GameServer.Resources.GameData;

            if (!gameData.IdToObjectType.TryGetValue(eff.Id, out ushort objType) ||
                !gameData.Portals.ContainsKey(objType))
                return; // object not found, ignore

            var entity = Resolve(GameServer, objType);
            var timeoutTime = gameData.Portals[objType].Timeout;

            entity.Move(X, Y);
            World.EnterWorld(entity);

            World.Timers.Add(new WorldTimer(timeoutTime * 1000, (world, t) => world.LeaveWorld(entity)));

            var openedByMsg = gameData.Portals[objType].DungeonName + " opened by " + Name + "!";
            World.Broadcast(new Notification
            {
                Color = new ARGB(0xFF00FF00),
                ObjectId = Id,
                Message = openedByMsg
            });
            World.ForeachPlayer(_ => _.SendInfo(openedByMsg));
        }

        private void AEDecoy(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            var decoy = new Decoy(this, eff.DurationMS);
            decoy.Move(X, Y);
            World.EnterWorld(decoy);
        }

        private void AEDye(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            if (item.Texture1 != 0)
                Texture1 = item.Texture1;
            if (item.Texture2 != 0)
                Texture2 = item.Texture2;
        }

        private void AEGenericActivate(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            var targetPlayer = eff.Target.Equals("player");
            var centerPlayer = eff.Center.Equals("player");
            var duration = eff.UseWisMod ? (int)(UseWisMod(eff.DurationSec) * 1000) : eff.DurationMS;
            var range = eff.UseWisMod
                ? UseWisMod(eff.Range)
                : eff.Range;

            if (eff.ConditionEffect != null)
                World.AOE(eff.Center.Equals("mouse") ? target : new Position { X = X, Y = Y }, range, targetPlayer, entity =>
                {
                    if (!entity.HasConditionEffect(ConditionEffects.Stasis) && !entity.HasConditionEffect(ConditionEffects.Invincible))
                    {
                        entity.ApplyConditionEffect(new ConditionEffect(eff.ConditionEffect.Value, duration));
                    }
                });

            World.BroadcastIfVisible(new ShowEffect()
            {
                EffectType = (EffectType)eff.VisualEffect,
                TargetObjectId = Id,
                Color = new ARGB(eff.Color),
                Pos1 = centerPlayer ? new Position() { X = range } : target,
                Pos2 = new Position() { X = target.X - range, Y = target.Y }
            }, this);
        }

        private void AEHeal(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            if (!HasConditionEffect(ConditionEffects.Sick))
            {
                var healthAmount = eff.Amount;
                ActivateHealHp(this, healthAmount);
            }
        }

        private void AEHealNova(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            var amount = eff.Amount;
            var range = eff.Range;
            if (eff.UseWisMod)
            {
                amount = (int)UseWisMod(eff.Amount, 0);
                range = UseWisMod(eff.Range);
            }

            this.AOE(range, true, player =>
            {
                if (!player.HasConditionEffect(ConditionEffects.Sick))
                    ActivateHealHp(player as Player, amount);
            });

            World.BroadcastIfVisible(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                TargetObjectId = Id,
                Color = new ARGB(0xffffffff),
                Pos1 = new Position() { X = range }
            }, this);
        }

        private void AEIncrementStat(TickTime time, Item item, Position target, ActivateEffect eff, int objId, int slot, int sellMaxed)
        {
            var addition = 0;
            switch (Rank)
            {
                case common.RankingType.Supporter1:
                    addition = 10;
                    break;
                case common.RankingType.Supporter2:
                    addition = 20;
                    break;
                case common.RankingType.Supporter3:
                    addition = 30;
                    break;
                case common.RankingType.Supporter4:
                    addition = 40;
                    break;
                case common.RankingType.Supporter5:
                    addition = 50;
                    break;
            }
            var totalAllowed = 50 + addition;

            var idx = StatsManager.GetStatIndex((StatDataType)eff.Stats);
            var statInfo = GameServer.Resources.GameData.Classes[ObjectType].Stats;
            var statname = StatsManager.StatIndexToName(idx);
            var ent = World.GetEntity(objId);
            var container = ent as Container;
            var storeAmount = eff.Amount == 5 ? 1 : eff.Amount == 10 ? 2 : eff.Amount == 2 ? 2 : 1;
            if (Stats.Base[idx] < statInfo[idx].MaxValue)
            {
                Stats.Base[idx] += eff.Amount;
                if (Stats.Base[idx] >= statInfo[idx].MaxValue)
                {
                    Stats.Base[idx] = statInfo[idx].MaxValue;
                    return;
                }
            }

            if (!UpgradeEnabled && item.Maxy && Stats.Base[idx] >= statInfo[idx].MaxValue)
            {
                if (container != null)
                {
                    Stats.Base[idx] = statInfo[idx].MaxValue;
                    container.Inventory[slot] = item;
                }
                else
                {
                    Stats.Base[idx] = statInfo[idx].MaxValue;
                    Inventory[slot] = item;
                }
                SendError("You're maxed!");
                return;
            }
            else if (UpgradeEnabled && item.Maxy)
            {
                if (container != null)
                {
                    container.Inventory[slot] = item;
                }
                else
                {
                    Inventory[slot] = item;
                }
                SendError("You're maxed!");
                return;
            }

            if (statname == "MpRegen")
                statname = "Wisdom";
            else if (statname == "HpRegen")
                statname = "Vitality";
            else if (statname == "MaxHitPoints")
                statname = "Life";
            else if (statname == "MaxMagicPoints")
                statname = "Mana";


            if ((sellMaxed == 2) && UpgradeEnabled) //"Store" Selected where Supreme IS active
            {
                //Stats.Base[idx] = statInfo[idx].MaxValue + (idx == 0 ? 50 : idx == 1 ? 50 : 10); ??
                if (container != null)
                    container.Inventory[slot] = null;
                else
                    Inventory[slot] = null;
                var storedAmount = HandleTX(statname, storeAmount);
                GameServer.Database.ReloadAccount(Client.Account);
                if (storedAmount == 998)
                {
                    int fameValue = idx < 2 ? 5 : 2;
                    fameValue += eff.Amount == 10 ? 5 : eff.Amount == 2 ? 2 : 0;
                    Client.Account.Reload("fame");
                    Client.Account.Reload("totalFame");
                    Client.Account.Fame += fameValue;
                    Client.Account.TotalFame += fameValue;

                    CurrentFame = Client.Account.Fame;
                    GameServer.Database.ReloadAccount(Client.Account);
                    SendError($"Your {statname} is currently Full. Sold for {fameValue} fame.");
                }
                else if (storedAmount == 999)
                {
                    SendError($"An error has occured, try again later.");
                }
                else
                {
                    SendInfo($"Added {storeAmount} {statname} to your Potion Storage! [{storedAmount} / {totalAllowed}]");
                }

                return;
            }
            else if (Stats.Base[idx] >= statInfo[idx].MaxValue && (sellMaxed == 2) && !UpgradeEnabled) //"Store" Selected where Supreme NOT active
            {
                Stats.Base[idx] = statInfo[idx].MaxValue;
                if (container != null)
                    container.Inventory[slot] = null;
                else
                    Inventory[slot] = null;
                var storedAmount = HandleTX(statname, storeAmount);
                GameServer.Database.ReloadAccount(Client.Account);
                if (storedAmount == 998)
                {
                    int fameValue = idx < 2 ? 5 : 2;
                    fameValue += eff.Amount == 10 ? 5 : eff.Amount == 2 ? 2 : 0;
                    Client.Account.Reload("fame");
                    Client.Account.Reload("totalFame");
                    Client.Account.Fame += fameValue;
                    Client.Account.TotalFame += fameValue;

                    CurrentFame = Client.Account.Fame;
                    GameServer.Database.ReloadAccount(Client.Account);
                    SendError($"Your {statname} is currently Full. Sold for {fameValue} fame.");
                }
                else if (storedAmount == 999)
                {
                    SendError($"An error has occured, try again later.");
                }
                else
                {
                    SendInfo($"Added {storeAmount} {statname} to your Potion Storage! [{storedAmount} / {totalAllowed}]");
                }

                return;
            }

            if ((Stats.Base[idx] >= statInfo[idx].MaxValue + (idx == 0 ? 50 : idx == 1 ? 50 : 10)) && (sellMaxed == 1) && UpgradeEnabled) //"Sell" Selected where Supreme is active
            {
                Stats.Base[idx] = statInfo[idx].MaxValue + (idx == 0 ? 50 : idx == 1 ? 50 : 10);
                if (container != null)
                    container.Inventory[slot] = null;
                else
                    Inventory[slot] = null;
                int fameValue = idx < 2 ? 5 : 2;
                fameValue += eff.Amount == 10 ? 5 : eff.Amount == 2 ? 2 : 0;
                Client.Account.Reload("fame");
                Client.Account.Reload("totalFame");
                Client.Account.Fame += fameValue;
                Client.Account.TotalFame += fameValue;

                CurrentFame = Client.Account.Fame;
                GameServer.Database.ReloadAccount(Client.Account);
                SendInfo($"Your {statname} got sold for {fameValue} Fame!");
                return;
            }
            else if ((Stats.Base[idx] >= statInfo[idx].MaxValue + (idx == 0 ? 50 : idx == 1 ? 50 : 10)) && (sellMaxed == 0) && UpgradeEnabled)//"Off" Selected where Supreme IS active
            {
                if (container != null)
                    container.Inventory[slot] = item;
                else
                    Inventory[slot] = item;
                Stats.Base[idx] = statInfo[idx].MaxValue + (idx == 0 ? 50 : idx == 1 ? 50 : 10);
                SendInfo("You're Maxed in this Stat!");
                return;
            }

            if (Stats.Base[idx] >= statInfo[idx].MaxValue && (sellMaxed == 1) && !UpgradeEnabled)//"Sell" Selected where Supreme NOT active
            {
                Stats.Base[idx] = statInfo[idx].MaxValue;
                if (container != null)
                    container.Inventory[slot] = null;
                else
                    Inventory[slot] = null;
                int fameValue = idx < 2 ? 5 : 2;
                fameValue += eff.Amount == 10 ? 5 : eff.Amount == 2 ? 2 : 0;

                Client.Account.Reload("fame");
                Client.Account.Reload("totalFame");
                Client.Account.Fame += fameValue;
                Client.Account.TotalFame += fameValue;
                CurrentFame = Client.Account.Fame;

                GameServer.Database.ReloadAccount(Client.Account);
                SendInfo($"Your {statname} got sold for {fameValue} Fame!");
                return;
            }
            else if (Stats.Base[idx] >= statInfo[idx].MaxValue && (sellMaxed == 0) && !UpgradeEnabled)//"Off" Selected where Supreme NOT active
            {
                if (container != null)
                    container.Inventory[slot] = item;
                else
                    Inventory[slot] = item;
                Stats.Base[idx] = statInfo[idx].MaxValue;
                SendInfo("You're Maxed in this Stat!");
                return;
            }
            else
            {
                SendInfo($"Consumed Potion of {statname}.");
            }
        }

        private int HandleTX(string statname, int amount)
        {
            var addition = 0;
            switch (Rank)
            {
                case common.RankingType.Supporter1:
                    addition = 10;
                    break;
                case common.RankingType.Supporter2:
                    addition = 20;
                    break;
                case common.RankingType.Supporter3:
                    addition = 30;
                    break;
                case common.RankingType.Supporter4:
                    addition = 40;
                    break;
                case common.RankingType.Supporter5:
                    addition = 50;
                    break;
            }
            var maxAllowed = 50 + addition;

            switch (statname)
            {
                case "Wisdom":
                    if (Client.Account.SPSWisdomCount < maxAllowed)
                    {
                        Client.Account.SPSWisdomCount += amount;
                        return Client.Account.SPSWisdomCount;
                    }
                    return 998;
                case "Vitality":
                    if (Client.Account.SPSVitalityCount < maxAllowed)
                    {
                        Client.Account.SPSVitalityCount += amount;
                        return Client.Account.SPSVitalityCount;
                    }
                    return 998;
                case "Life":
                    if (Client.Account.SPSLifeCount < maxAllowed)
                    {
                        Client.Account.SPSLifeCount += amount;
                        return Client.Account.SPSLifeCount;
                    }
                    return 998;
                case "Mana":
                    if (Client.Account.SPSManaCount < maxAllowed)
                    {
                        Client.Account.SPSManaCount += amount;
                        return Client.Account.SPSManaCount;
                    }
                    return 998;
                case "Speed":
                    if (Client.Account.SPSSpeedCount < maxAllowed)
                    {
                        Client.Account.SPSSpeedCount += amount;
                        return Client.Account.SPSSpeedCount;
                    }
                    return 998;
                case "Attack":
                    if (Client.Account.SPSAttackCount < maxAllowed)
                    {
                        Client.Account.SPSAttackCount += amount;
                        return Client.Account.SPSAttackCount;
                    }
                    return 998;
                case "Defense":
                    if (Client.Account.SPSDefenseCount < maxAllowed)
                    {
                        Client.Account.SPSDefenseCount += amount;
                        return Client.Account.SPSDefenseCount;
                    }
                    return 998;
                case "Dexterity":
                    if (Client.Account.SPSDexterityCount < maxAllowed)
                    {
                        Client.Account.SPSDexterityCount += amount;
                        return Client.Account.SPSDexterityCount;
                    }
                    return 998;
            }
            return 999;
        }

        private void AELDBoost(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            //  if (LDBoostTime < 0 || (LDBoostTime > eff.DurationMS && eff.DurationMS >= 0))
            //      return;

            if (LDBoostTime < 0) //|| (LDBoostTime > eff.DurationMS && eff.DurationMS >= 0))
                return;

            if (LDBoostTime <= 0)
            {
                SendInfo2("Your Loot Drop Potion has been activated!");
            }
            if (LDBoostTime > 0)
            {
                SendInfo2("Your Loot Drop Potion has been stacked up!");
            }
            LDBoostTime = eff.DurationMS + LDBoostTime;
            InvokeStatChange(StatDataType.LDBoostTime, LDBoostTime / 1000, true);
        }

        private void AELightning(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            const double coneRange = Math.PI / 4;
            var mouseAngle = Math.Atan2(target.Y - Y, target.X - X);

            // get starting target
            var startTarget = this.GetNearestEntity(MaxAbilityDist, false, e => e is Enemy &&
                Math.Abs(mouseAngle - Math.Atan2(e.Y - Y, e.X - X)) <= coneRange);

            // no targets? bolt air animation
            if (startTarget == null)
            {
                var angles = new double[] { mouseAngle, mouseAngle - coneRange, mouseAngle + coneRange };
                for (var i = 0; i < 3; i++)
                {
                    var x = (int)(MaxAbilityDist * Math.Cos(angles[i])) + X;
                    var y = (int)(MaxAbilityDist * Math.Sin(angles[i])) + Y;
                    World.BroadcastIfVisible(new ShowEffect()
                    {
                        EffectType = EffectType.Trail,
                        TargetObjectId = Id,
                        Color = new ARGB(0xffff0088),
                        Pos1 = new Position()
                        {
                            X = x,
                            Y = y
                        },
                        Pos2 = new Position() { X = 350 }
                    }, ref target);
                }
                return;
            }

            var current = startTarget;
            var targets = new Entity[eff.MaxTargets];
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i] = current;
                var next = current.GetNearestEntity(10, false, e =>
                {
                    if (!(e is Enemy) ||
                        e.HasConditionEffect(ConditionEffects.Invincible) ||
                        e.HasConditionEffect(ConditionEffects.Stasis) ||
                        Array.IndexOf(targets, e) != -1)
                        return false;

                    return true;
                });

                if (next == null)
                    break;

                current = next;
            }

            for (var i = 0; i < targets.Length; i++)
            {
                if (targets[i] == null)
                    break;

                var prev = i == 0 ? this : targets[i - 1];

                var damage = eff.UseWisMod ? UseWisMod(eff.TotalDamage) : eff.TotalDamage;

                (targets[i] as Enemy).Damage(this, time, (int)damage, false);

                if (eff.ConditionEffect != null)
                    targets[i].ApplyConditionEffect(new ConditionEffect(eff.ConditionEffect.Value, (int)(eff.EffectDuration * 1000)));

                World.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.Lightning,
                    TargetObjectId = prev.Id,
                    Color = new ARGB(0xffff0088),
                    Pos1 = new Position()
                    {
                        X = targets[i].X,
                        Y = targets[i].Y
                    },
                    Pos2 = new Position() { X = 350 }
                }, this);
            }
        }

        private void AEMagic(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            for (var i = 0; i < 4; i++)
            {
                var item1 = Inventory[i];

                if (item1 == null || !item1.Legendary && !item1.Revenge)
                    continue;

                if (item1.SonicBlaster)
                {
                    SonicBlaster(i);
                }
            }
            
            var pkts = new List<OutgoingMessage>();
            var healthAmount = eff.Amount;
            //if (BigSkill10)
            //    healthAmount = 75 * healthAmount / 100;
            ActivateHealMp(this, healthAmount);
        }

        private void AEMagicDust(TickTime time, Item item, Position target, int slot, int objId, ActivateEffect eff)
        {
            if (Inventory.Data[slot] == null)
            {
                SendError("Something wrong happens with your Magic Dust!");
                return;
            }
         
            if (Inventory.Data[slot].Stack < Inventory.Data[slot].MaxStack)
                return;

            var entity = World.GetEntity(objId);

            var dustItem = World.GameServer.ItemDustWeights.MagicDust.GetRandom(World.Random);
            if (entity is Container container)
                container.Inventory[slot] = dustItem;
            else
                Inventory[slot] = dustItem;
            
            SendInfo($"Used a Magic Dust and obtained a {dustItem.DisplayName ?? dustItem.ObjectId}");

            Inventory.Data[slot] = null;
            InvokeStatChange((StatDataType)((int)StatDataType.InventoryData0 + slot), Inventory.Data[slot]?.GetData() ?? "{}");
        }

        private void AEMagicNova(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            this.AOE(eff.Range, true, player =>
                ActivateHealMp(player as Player, eff.Amount));

            World.BroadcastIfVisible(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                TargetObjectId = Id,
                Color = new ARGB(0xffffffff),
                Pos1 = new Position() { X = eff.Range }
            }, this);
        }

        private void AEMiscellaneousDust(TickTime time, Item item, Position target, int slot, int objId, ActivateEffect eff)
        {
            var entity = World.GetEntity(objId);

            var dustItem = World.GameServer.ItemDustWeights.MiscDust.GetRandom(World.Random);
            if (entity is Container container)
                container.Inventory[slot] = dustItem;
            else
                Inventory[slot] = dustItem;

            SendInfo($"You used a Miscellaneous Dust and obtained a {dustItem.DisplayName ?? dustItem.ObjectId}");
        }

        private void AEPermaPet(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            var type = GameServer.Resources.GameData.IdToObjectType[eff.ObjectId];
            var desc = GameServer.Resources.GameData.ObjectDescs[type];

            PetId = desc.ObjectType;
            SpawnPetIfAttached(World);
        }

        private void AEPoisonGrenade(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            if (MathsUtils.DistSqr(target.X, target.Y, X, Y) > MaxAbilityDist * MaxAbilityDist) return;
            var impDamage = eff.ImpactDamage;
            if (eff.UseWisMod)
            {
                impDamage = (int)UseWisMod(eff.ImpactDamage);
                PoisonWis = true;
            }

            World.BroadcastIfVisible(new ShowEffect()
            {
                EffectType = EffectType.Throw,
                Color = new ARGB(eff.Color != 0 ? eff.Color : 0xffffffff),
                TargetObjectId = Id,
                Pos1 = target,
                Duration = eff.ThrowTime / 1000
            }, this);

            var x = new Placeholder(GameServer, eff.ThrowTime * 1000);
            x.Move(target.X, target.Y);
            World.EnterWorld(x);
            World.Timers.Add(new WorldTimer(eff.ThrowTime, (world, t) =>
            {
                world.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    Color = new ARGB(eff.Color != 0 ? eff.Color : 0xffffffff),
                    TargetObjectId = x.Id,
                    Pos1 = new Position() { X = eff.Radius },
                    Pos2 = new Position() { X = Id, Y = 255 }
                }, x);

                world.AOE(target, eff.Radius, false, entity =>
                {
                    PoisonEnemy(world, (Enemy)entity, eff);
                    ((Enemy)entity).Damage(this, time, impDamage, true);
                });
            }));
        }

        private void AEPotionDust(TickTime time, Item item, Position target, int slot, int objId, ActivateEffect eff)
        {
            var entity = World.GetEntity(objId);

            var dustItem = World.GameServer.ItemDustWeights.PotionDust.GetRandom(World.Random);
            if (entity is Container container)
                container.Inventory[slot] = dustItem;
            else
                Inventory[slot] = dustItem;

            SendInfo($"You used a Potion Dust and obtained a {dustItem.DisplayName ?? dustItem.ObjectId}");
        }

        private void AERemoveNegativeConditions(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            this.AOE(eff.Range, true, player => player.ApplyConditionEffect(NegativeEffs));
            World.BroadcastIfVisible(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                TargetObjectId = Id,
                Color = new ARGB(0xffffffff),
                Pos1 = new Position() { X = eff.Range }
            }, this);
        }

        private void AERemoveNegativeConditionSelf(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            ApplyConditionEffect(NegativeEffs);
            World.BroadcastIfVisible(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                TargetObjectId = Id,
                Color = new ARGB(0xffffffff),
                Pos1 = new Position() { X = 1 }
            }, this);
        }

        private void AEShoot(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            var arcGap = item.ArcGap * Math.PI / 180;
            var startAngle = Math.Atan2(target.Y - Y, target.X - X) - (item.NumProjectiles - 1) / 2 * arcGap;
            var prjDesc = item.Projectiles[0]; //Assume only one

            var sPkts = new OutgoingMessage[item.NumProjectiles];
            for (var i = 0; i < item.NumProjectiles; i++)
            {
                var proj = CreateProjectile(prjDesc, item.ObjectType, Stats.GetAttackDamage(prjDesc.MinDamage, prjDesc.MaxDamage, true), time.TotalElapsedMs, new Position() { X = X, Y = Y }, (float)(startAngle + arcGap * i));
                World.AddProjectile(proj);
                sPkts[i] = new AllyShoot()
                {
                    OwnerId = Id,
                    Angle = proj.Angle,
                    ContainerType = item.ObjectType,
                    BulletId = proj.ProjectileId
                };
                FameCounter.Shoot(proj);
            }

            for (var i = 0; i < item.NumProjectiles; i++)
                World.BroadcastIfVisibleExclude(sPkts[i], this, this);
        }

        private void AEShurikenAbility(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            if (!HasConditionEffect(ConditionEffects.NinjaSpeedy))
            {
                ApplyConditionEffect(ConditionEffectIndex.NinjaSpeedy);
                return;
            }

            if (MP >= item.MpEndCost)
            {
                MP -= item.MpEndCost;
                AEShoot(time, item, target, eff);
            }

            ApplyConditionEffect(ConditionEffectIndex.NinjaSpeedy, 0);
        }

        private void AEShurikenAbilityBerserk(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            if (!HasConditionEffect(ConditionEffects.Berserk))
            {
                ApplyConditionEffect(ConditionEffectIndex.Berserk);
                return;
            }

            if (MP >= item.MpEndCost)
            {
                MP -= item.MpEndCost;
                AEShoot(time, item, target, eff);
            }

            ApplyConditionEffect(ConditionEffectIndex.Berserk, 0);
        }

        private void AEShurikenAbilityDamaging(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            if (!HasConditionEffect(ConditionEffects.NinjaDamaging))
            {
                ApplyConditionEffect(ConditionEffectIndex.NinjaDamaging);
                return;
            }

            if (MP >= item.MpEndCost)
            {
                MP -= item.MpEndCost;
                AEShoot(time, item, target, eff);
            }

            ApplyConditionEffect(ConditionEffectIndex.NinjaDamaging, 0);
        }

        private void AEStatBoostAura(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            var idx = StatsManager.GetStatIndex((StatDataType)eff.Stats);
            var amount = eff.Amount;
            var duration = eff.DurationMS;
            var range = eff.Range;
            if (eff.UseWisMod)
            {
                amount = (int)UseWisMod(eff.Amount, 0);
                duration = (int)(UseWisMod(eff.DurationSec) * 1000);
                range = UseWisMod(eff.Range);
            }

            this.AOE(range, true, player =>
            {
                if (player.HasConditionEffect(ConditionEffects.HPBoost))
                {
                    return;
                }

                if (!player.HasConditionEffect(ConditionEffects.HPBoost))
                {
                    World.Timers.Add(new WorldTimer(0, (world, t) => player.ApplyConditionEffect(ConditionEffectIndex.HPBoost, duration)));
                }

                ((Player)player).Stats.Boost.ActivateBoost[idx].Push(amount, false);
                ((Player)player).Stats.ReCalculateValues();

                // hack job to allow instant heal of nostack boosts
                //if (eff.NoStack && amount > 0 && idx == 0)
                //{
                //    ((Player)player).HP = Math.Min(((Player)player).Stats[0], ((Player)player).HP + amount);
                //}

                World.Timers.Add(new WorldTimer(duration, (world, t) =>
                {
                    ((Player)player).Stats.Boost.ActivateBoost[idx].Pop(amount, false);
                    ((Player)player).Stats.ReCalculateValues();
                }));
            });

            World.BroadcastIfVisible(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                TargetObjectId = Id,
                Color = new ARGB(0xffffffff),
                Pos1 = new Position() { X = range }
            }, this);
        }

        private void AEStatBoostSelf(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            var idx = StatsManager.GetStatIndex((StatDataType)eff.Stats);
            var s = eff.Amount;
            Stats.Boost.ActivateBoost[idx].Push(s, false);
            Stats.ReCalculateValues();
            World.Timers.Add(new WorldTimer(eff.DurationMS, (world, t) =>
            {
                Stats.Boost.ActivateBoost[idx].Pop(s, false);
                Stats.ReCalculateValues();
            }));

            World.BroadcastIfVisible(new ShowEffect()
            {
                EffectType = EffectType.Potion,
                TargetObjectId = Id,
                Color = new ARGB(0xffffffff)
            }, this);
        }

        private void AETeleport(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            TeleportPosition(time, target, true);
        }

        private void AETrap(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            World.BroadcastIfVisible(new ShowEffect()
            {
                EffectType = EffectType.Throw,
                Color = new ARGB(0xff9000ff),
                TargetObjectId = Id,
                Pos1 = target
            }, ref target);

            World.Timers.Add(new WorldTimer(1500, (world, t) =>
            {
                var trap = new Trap(this, eff.Radius, eff.TotalDamage, eff.ConditionEffect ?? ConditionEffectIndex.Slowed, eff.EffectDuration);
                trap.Move(target.X, target.Y);
                world.EnterWorld(trap);
            }));
        }

        private void AEUnlockPortal(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            //var gameData = CoreServerManager.Resources.GameData;

            //// find locked portal
            //var portals = World.StaticObjects
            //    .Values.Where(_ => _ is Portal
            //    && _.ObjectDesc.ObjectId.Equals(eff.LockedName)
            //    && _.DistSqr(this) <= 9d)
            //    .Select(_ => _ as Portal);
            //if (!portals.Any())
            //    return;
            //var portal = portals.Aggregate(
            //    (curmin, x) => curmin == null || x.DistSqr(this) < curmin.DistSqr(this) ? x : curmin);
            //if (portal == null)
            //    return;

            //// get proto of world
            //if (!CoreServerManager.Resources.Worlds.Data.TryGetValue(eff.DungeonName, out ProtoWorld proto))
            //{
            //    SLogger.Instance.Error("Unable to unlock portal. \"" + eff.DungeonName + "\" does not exist.");
            //    return;
            //}

            //if (proto.portals == null || proto.portals.Length < 1)
            //{
            //    SLogger.Instance.Error("World is not associated with any portals.");
            //    return;
            //}

            //// create portal of unlocked world
            //var portalType = (ushort)proto.portals[0];
            //if (!(Resolve(CoreServerManager, portalType) is Portal uPortal))
            //{
            //    SLogger.Instance.Error("Error creating portal: {0}", portalType);
            //    return;
            //}

            //var portalDesc = gameData.Portals[portal.ObjectType];
            //var uPortalDesc = gameData.Portals[portalType];

            //// create world
            //World world;
            //if (proto.id < 0)
            //    world = CoreServerManager.WorldManager.GetWorld(proto.id);
            //else
            //{
            //    DynamicWorld.TryGetWorld(proto, Client, out world);
            //    world = CoreServerManager.WorldManager.CreateNewWorld(world ?? new World(proto));
            //}
            //uPortal.WorldInstance = world;

            //// swap portals
            //if (!portalDesc.NexusPortal || !CoreServerManager.WorldManager.PortalMonitor.RemovePortal(portal))
            //    World.LeaveWorld(portal);
            //uPortal.Move(portal.X, portal.Y);
            //uPortal.Name = uPortalDesc.DisplayId;
            //var uPortalPos = new Position() { X = portal.X - .5f, Y = portal.Y - .5f };
            //if (!uPortalDesc.NexusPortal || !CoreServerManager.WorldManager.PortalMonitor.AddPortal(world.Id, uPortal, uPortalPos))
            //    World.EnterWorld(uPortal);

            //// setup timeout
            //if (!uPortalDesc.NexusPortal)
            //{
            //    var timeoutTime = gameData.Portals[portalType].Timeout;
            //    World.Timers.Add(new WorldTimer(timeoutTime * 1000, (w, t) => w.LeaveWorld(uPortal)));
            //}

            //// announce
            //World.Broadcast(new Notification
            //{
            //    Color = new ARGB(0xFF00FF00),
            //    ObjectId = Id,
            //    Message = "Unlocked by " + Name
            //});
            //World.PlayersBroadcastAsParallel(_ => _.SendInfo($"{world.SBName} unlocked by {Name}!"));
        }

        private void AEUpgradeActivate(TickTime time, Item item, Position target, int objId, int slot, ActivateEffect eff)
        {
            var playerDesc = GameServer.Resources.GameData.Classes[ObjectType];
            var maxed = playerDesc.Stats.Where((t, i) => Stats.Base[i] >= t.MaxValue).Count();
            var entity = World.GetEntity(objId);
            var container = entity as Container;
            if (maxed < 8)
            {
                SendError("You must be 8/8 to Upgrade.");
                if (container != null)
                    container.Inventory[slot] = item;
                else
                    Inventory[slot] = item;
                return;
            }
            if (UpgradeEnabled)
            {
                SendInfo("You already have your Character Upgraded.");
                if (container != null)
                    container.Inventory[slot] = item;
                else
                    Inventory[slot] = item;
                return;
            }
            SendInfo("Your Character has been Upgraded successfully.");
            UpgradeEnabled = true;
            var chr = Client.Character;
            chr.UpgradeEnabled = UpgradeEnabled;
        }

        private void AEUpgradeStat(TickTime time, Item item, Position target, int objId, int slot, ActivateEffect eff)
        {
            var entity = World.GetEntity(objId);
            var container = entity as Container;
            if (UpgradeEnabled)
            {
                var idx = StatsManager.GetStatIndex((StatDataType)eff.Stats);
                var statname = StatsManager.StatIndexToName(idx);
                if (statname == "MpRegen")
                    statname = "Wisdom";
                else if (statname == "HpRegen")
                    statname = "Vitality";
                else if (statname == "MaxHitPoints")
                    statname = "Life";
                else if (statname == "MaxMagicPoints")
                    statname = "Mana";

                var statInfo = GameServer.Resources.GameData.Classes[ObjectType].Stats;

                Stats.Base[idx] += eff.Amount;
                if (Stats.Base[idx] > statInfo[idx].MaxValue + (idx < 2 ? 50 : 10))
                {
                    Stats.Base[idx] = statInfo[idx].MaxValue + (idx < 2 ? 50 : 10);
                    SendInfo("You're maxed.");
                    if (container != null)
                        container.Inventory[slot] = item;
                    else
                        Inventory[slot] = item;
                    return;
                }

                SendInfo($"Soul of {statname} consumed. {statInfo[idx].MaxValue + (idx < 2 ? 50 : 10) - Stats.Base[idx]} left to Max.");
            }
            else
            {
                SendInfo("A character that isn't Upgraded can't use Soul Potions.");
                if (container != null)
                    container.Inventory[slot] = item;
                else
                    Inventory[slot] = item;
                return;
            }
        }

        private void AEVampireBlast(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            var pkts = new List<OutgoingMessage>()
            {
                new ShowEffect()
                {
                    EffectType = EffectType.Trail,
                    TargetObjectId = Id,
                    Pos1 = target,
                    Color = new ARGB(0xFFFF0000)
                },
                new ShowEffect
                {
                    EffectType = EffectType.Diffuse,
                    Color = new ARGB(0xFFFF0000),
                    TargetObjectId = Id,
                    Pos1 = target,
                    Pos2 = new Position { X = target.X + eff.Radius, Y = target.Y }
                }
            };

            World.BroadcastIfVisible(pkts[0], ref target);
            World.BroadcastIfVisible(pkts[1], ref target);

            var totalDmg = 0;
            var effDamage = eff.UseWisMod ? UseWisMod(eff.TotalDamage) : eff.TotalDamage;
            var enemies = new List<Enemy>();

            World.AOE(target, eff.Radius, false, enemy =>
            {
                enemies.Add(enemy as Enemy);
                totalDmg += (enemy as Enemy).Damage(this, time, (int)effDamage, false);
            });

            var players = new List<Player>();
            this.AOE(eff.Radius, true, player =>
            {
                if (!player.HasConditionEffect(ConditionEffects.Sick))
                {
                    players.Add(player as Player);
                    ActivateHealHp(player as Player, totalDmg);
                }
            });

            if (enemies.Count > 0)
            {
                for (var i = 0; i < 5; i++)
                {
                    var a = World.Random.NextLength(enemies);
                    var b = World.Random.NextLength(players);

                    World.BroadcastIfVisible(new ShowEffect()
                    {
                        EffectType = EffectType.Flow,
                        TargetObjectId = b.Id,
                        Pos1 = new Position() { X = a.X, Y = a.Y },
                        Color = new ARGB(0xffffffff)
                    }, ref target);
                }
            }
        }

        private void AEXPBoost(TickTime time, Item item, Position target, int slot, int objId, ActivateEffect eff)
        {
            if (XPBoostTime < 0 || (XPBoostTime > eff.DurationMS && eff.DurationMS >= 0))
                return;

            var entity = World.GetEntity(objId);
            var containerItem = entity as Container;

            if (XPBoostTime > 0 && XPBoosted)
            {
                SendInfo("You already have a XPBooster activated!");
                if (containerItem != null)
                    containerItem.Inventory[slot] = item;
                else
                    Inventory[slot] = item;
                return;
            }
            if (Level >= 20)
            {
                SendInfo("You're level 20!");
                if (containerItem != null)
                    containerItem.Inventory[slot] = item;
                else
                    Inventory[slot] = item;
                return;
            }
            XPBoostTime = eff.DurationMS;
            XPBoosted = true;
            InvokeStatChange(StatDataType.XPBoostTime, XPBoostTime / 1000, true);
        }

        private void HealingPlayersPoison(World world, Player player, ActivateEffect eff)
        {
            var remainingHeal = eff.TotalDamage;
            var perHeal = eff.TotalDamage * 1000 / eff.DurationMS;

            WorldTimer tmr = null;
            var x = 0;

            bool healTick(World w, TickTime t)
            {
                if (player.World == null || w == null)
                    return true;

                if (x % 4 == 0) // make sure to change this if timer delay is changed
                {
                    var thisHeal = perHeal;
                    if (remainingHeal < thisHeal)
                        thisHeal = remainingHeal;

                    ActivateHealHp(player, thisHeal);

                    remainingHeal -= thisHeal;
                    if (remainingHeal <= 0)
                        return true;
                }
                x++;

                tmr.Reset();
                return false;
            }

            tmr = new WorldTimer(250, healTick);
            world.Timers.Add(tmr);
        }

        private void PoisonEnemy(World world, Enemy enemy, ActivateEffect eff)
        {
            var remainingDmg = (int)StatsManager.GetDefenseDamage(enemy, eff.TotalDamage, enemy.Defense);
            var perDmg = remainingDmg * 1000 / eff.DurationMS;

            if (PoisonWis)
            {
                remainingDmg = (int)UseWisMod(remainingDmg);
                perDmg = (int)UseWisMod(perDmg);
            }

            WorldTimer tmr = null;
            var x = 0;

            bool poisonTick(World w, TickTime t)
            {
                if (enemy.World == null || w == null)
                    return true;

                if (x % 4 == 0)
                {
                    var thisDmg = perDmg;
                    if (remainingDmg < thisDmg)
                        thisDmg = remainingDmg;

                    if (enemy == null)
                        return false;
                    enemy?.Damage(this, t, thisDmg, true);
                    remainingDmg -= thisDmg;
                    if (remainingDmg <= 0)
                        return true;
                }
                x++;

                tmr.Reset();
                return false;
            }

            tmr = new WorldTimer(250, poisonTick);
            world.Timers.Add(tmr);
        }

        private void StasisBlast(TickTime time, Item item, Position target, ActivateEffect eff)
        {
            World.BroadcastIfVisible(new ShowEffect()
            {
                EffectType = EffectType.Concentrate,
                TargetObjectId = Id,
                Pos1 = target,
                Pos2 = new Position() { X = target.X + 3, Y = target.Y },
                Color = new ARGB(0xffffffff)
            }, ref target);

            World.AOE(target, 3, false, enemy =>
            {
                if (enemy.HasConditionEffect(ConditionEffects.StasisImmune))
                {
                    World.BroadcastIfVisible(new Notification()
                    {
                        ObjectId = enemy.Id,
                        Color = new ARGB(0xff00ff00),
                        Message = "Immune",
                        PlayerId = Id
                    }, enemy);
                }
                else if (!enemy.HasConditionEffect(ConditionEffects.Stasis) && !enemy.HasConditionEffect(ConditionEffects.StasisImmune))
                {
                    enemy.ApplyConditionEffect(ConditionEffectIndex.Stasis, eff.DurationMS);

                    World.Timers.Add(new WorldTimer(eff.DurationMS - 200, (world, t) => enemy.ApplyConditionEffect(ConditionEffectIndex.StasisImmune, 3200)));

                    World.BroadcastIfVisible(new Notification()
                    {
                        ObjectId = enemy.Id,
                        Color = new ARGB(0xffff0000),
                        Message = "Stasis",
                        PlayerId = Id
                    }, enemy);
                }
            });
        }

        private float UseWisMod(float value, int offset = 1)
        {
            double totalWisdom = Stats.Base[7] + Stats.Boost[7];

            if (totalWisdom < 50)
                return value;

            double m = (value < 0) ? -1 : 1;
            double n = (value * totalWisdom / 150) + (value * m);
            n = Math.Floor(n * Math.Pow(100, offset)) / Math.Pow(100, offset);
            if (n - (int)n * m >= 1 / Math.Pow(100, offset) * m)
            {
                return ((int)(n * 10)) / 10.0f;
            }

            return (int)n;
        }
    }
}
