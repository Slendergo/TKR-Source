using CA.Extensions.Concurrent;
using common;
using common.isc;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.core.objects;
using wServer.core.worlds;
using wServer.networking;
using wServer.networking.packets.outgoing;
using wServer.utils;

namespace wServer.core
{
    public class ChatManager
    {
        private GameServer GameServer;

        public ChatManager(GameServer gameServer) => GameServer = gameServer;

        public void ServerAnnounce(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var clients = GameServer.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].Player.AnnouncementReceived(text);
        }

        public void Announce(Player player, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var clients = GameServer.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].Player.AnnouncementReceived(text, player.Name);
        }

        public void AnnounceEternalLoot(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var clients = GameServer.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].Player.SendEternalNotif(text);
        }

        public void AnnounceForger(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var clients = GameServer.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].Player.ForgerNotif(text);
        }

        public void AnnounceEngine(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var clients = GameServer.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].Player.EngineNotif(text);
        }

        public void AnnounceLoot(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var clients = GameServer.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].Player.SendLootNotif(text);
        }

        public void AnnounceRealm(string text, string name)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var clients = GameServer.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].Player.AnnouncementRealm(text, name);
        }

        public void AnnounceMythicalLoot(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var clients = GameServer.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].Player.SendMythicalNotif(text);
        }

        public bool Guild(Player src, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            GameServer.InterServerManager.Publish(Channel.Chat, new ChatMsg()
            {
                Type = ChatType.Guild,
                Instance = GameServer.InstanceId,
                ObjectId = src.Id,
                Stars = src.Stars,
                From = src.Client.Account.AccountId,
                To = src.Client.Account.GuildId,
                Text = text
            });

            return true;
        }

        public void Initialize()
        {
            GameServer.InterServerManager.AddHandler<ChatMsg>(Channel.Chat, HandleChat);
            GameServer.InterServerManager.AddHandler<AnnounceMsg>(Channel.Announce, HandleAnnounce);
            GameServer.InterServerManager.NewServer += AnnounceNewServer;
            GameServer.InterServerManager.ServerQuit += AnnounceServerQuit;
        }

        public void Mob(Entity entity, string text)
        {
            if (string.IsNullOrWhiteSpace(text) || entity.World == null)
                return;

            var world = entity.World;
            var name = entity.ObjectDesc.DisplayId;

            Enemy enemy = null;

            if (entity is Enemy)
                enemy = entity as Enemy;

            var displayenemy =
                  enemy.Legendary ? $"Legendary {entity.ObjectDesc.DisplayId ?? entity.ObjectDesc.ObjectId}" :
                  enemy.Epic ? $"Epic {entity.ObjectDesc.DisplayId ?? entity.ObjectDesc.ObjectId}" :
                  enemy.Rare ? $"Rare {entity.ObjectDesc.DisplayId ?? entity.ObjectDesc.ObjectId}" :
                  entity.ObjectDesc.DisplayId ?? entity.ObjectDesc.ObjectId;

            name = displayenemy;

            world.Broadcast(new Text()
            {
                ObjectId = entity.Id,
                BubbleTime = 5,
                NumStars = -1,
                Name = $"#{name}",
                Txt = text
            });

            StaticLogger.Instance.Info($"[{world.IdName}({world.Id})] <{name}> {text}");
        }

        public void TalismanKing(World world, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            world.Broadcast(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "#The Talisman King",
                Txt = text
            });
        }

        public bool Party(Player src, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            GameServer.InterServerManager.Publish(Channel.Chat, new ChatMsg()
            {
                Type = ChatType.Party,
                Instance = GameServer.InstanceId,
                ObjectId = src.Id,
                Stars = src.Stars,
                From = src.Client.Account.AccountId,
                To = src.Client.Account.PartyId,
                Text = text,
                SrcIP = src.Client.IpAddress
            });

            return true;
        }

        public void Say(Player src, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var supporter = 0;
            if (src.IsSupporter1)
                supporter++;
            if (src.IsSupporter2)
                supporter++;
            if (src.IsSupporter3)
                supporter++;
            if (src.IsSupporter4)
                supporter++;
            if (src.IsSupporter5)
                supporter++;

            var nameTag = "";
             if (src.IsCommunityManager)
                nameTag = "[CM] ";
            if(src.IsCommunityManager && supporter > 0)
                nameTag = $"[CM | S-{supporter}] ";
            else if (supporter > 0)
                nameTag = $"[S-{supporter}] ";

            if (src.Client.Account.Name == "Slendergo" || src.Client.Account.Name == "ModBBQ" || src.Client.Account.Name == "Orb")
                nameTag = "[Owner] ";

            var tp = new Text()
            {
                Name = $"{nameTag}{src.Name}",
                ObjectId = src.Id,
                NumStars = src.Stars,
                BubbleTime = 5,
                Recipient = "",
                Txt = text,
                NameColor = (src.Client.Account.ColorNameChat != 0) ? src.Client.Account.ColorNameChat : 0x123456,
                TextColor = (src.Client.Account.ColorChat != 0) ? src.Client.Account.ColorChat : 0xFFFFFF
            };

            SendTextPacket(src, tp, p => !p.Client.Account.IgnoreList.Contains(src.AccountId));
        }

        public bool SendInfo(int target, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            GameServer.InterServerManager.Publish(Channel.Chat, new ChatMsg()
            {
                Type = ChatType.Info,
                Instance = GameServer.InstanceId,
                To = target,
                Text = text
            });

            return true;
        }

        public bool SendInfoMarket(int accId, string itemId, int realPrice, int resultPrice, int Tax)
        {
            GameServer.InterServerManager.Publish(Channel.Chat, new ChatMsg()
            {
                Type = ChatType.Info,
                Instance = GameServer.InstanceId,
                To = accId,
                Text = $"<Marketplace> {itemId} has been sold for {realPrice} (You have obtained: {resultPrice}) Fame, included {Tax}% Tax."
            });

            return true;
        }

        public void Dispose()
        {
            GameServer.InterServerManager.NewServer -= AnnounceNewServer;
            GameServer.InterServerManager.ServerQuit -= AnnounceServerQuit;
        }

        public bool Tell(Player src, string target, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            var id = GameServer.Database.ResolveId(target);

            if (id == 0)
                return false;

            if (!GameServer.Database.AccountLockExists(id))
                return false;

            var acc = GameServer.Database.GetAccount(id);
            if (acc == null)
                return false;

            if (acc.Hidden)
                return false;

            GameServer.InterServerManager.Publish(Channel.Chat, new ChatMsg()
            {
                Type = ChatType.Tell,
                Instance = GameServer.InstanceId,
                ObjectId = src.Id,
                Stars = src.Stars,
                From = src.Client.Account.AccountId,
                To = id,
                Text = text,
                SrcIP = src.Client.IpAddress
            });

            return true;
        }

        private void AnnounceNewServer(object sender, EventArgs e)
        {
            var networkMsg = (InterServerEventArgs<NetworkMsg>)e;

            if (networkMsg.Content.Info.type == ServerType.Account)
                return;

            ServerAnnounce($"A new server has come online: {networkMsg.Content.Info.name}");
        }

        private void AnnounceServerQuit(object sender, EventArgs e)
        {
            var networkMsg = (InterServerEventArgs<NetworkMsg>)e;

            ServerAnnounce($"Server, {networkMsg.Content.Info.name}, is no longer online.");
        }

        private void HandleAnnounce(object sender, InterServerEventArgs<AnnounceMsg> e)
        {
            var user = e.Content.User;
            var message = e.Content.Message;
            var messageTemplate = $"({DateTime.UtcNow:hh:mm:ss}) Staff {user} says: {message}";
            var botName = GameServer.Configuration.discordIntegration.botName;
            var clients = GameServer.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].SendPacket(new Text()
                {
                    ObjectId = -1,
                    BubbleTime = 10,
                    NumStars = 70,
                    Name = botName,
                    Recipient = clients[i].Player.Name,
                    Txt = messageTemplate
                });
        }

        private void HandleChat(object sender, InterServerEventArgs<ChatMsg> e)
        {
            Client[] dummies;
            switch (e.Content.Type)
            {
                case ChatType.Tell:
                    {
                        var from = GameServer.Database.ResolveIgn(e.Content.From);
                        var to = GameServer.Database.ResolveIgn(e.Content.To);
                        dummies = GameServer.ConnectionManager.Clients
                            .KeyWhereAsParallel(_ => _.Player != null
                                && !_.Account.IgnoreList.Contains(e.Content.From)
                                && (_.Account.AccountId == e.Content.From || _.Account.AccountId == e.Content.To || _.Account.IP == e.Content.SrcIP));
                        for (var i = 0; i < dummies.Length; i++)
                            dummies[i].Player.TellReceived(
                                e.Content.Instance == GameServer.InstanceId
                                    ? e.Content.ObjectId
                                    : -1,
                                e.Content.Stars,
                                e.Content.Admin,
                                from,
                                to,
                                e.Content.Text
                            );
                    }
                    break;

                case ChatType.Info:
                    {
                        var to = GameServer.Database.GetAccount(e.Content.To);
                        dummies = GameServer.ConnectionManager.Clients
                            .KeyWhereAsParallel(_ => _.Player != null && _.Account.AccountId == to.AccountId);
                        for (var i = 0; i < dummies.Length; i++)
                            dummies[i].Player.SendInfo(e.Content.Text);
                    }
                    break;

                case ChatType.Guild:
                    {
                        var from = GameServer.Database.ResolveIgn(e.Content.From);
                        dummies = GameServer.ConnectionManager.Clients
                            .KeyWhereAsParallel(_ => _.Player != null
                                && !_.Account.IgnoreList.Contains(e.Content.From)
                                && _.Account.GuildId > 0
                                && _.Account.GuildId == e.Content.To);
                        for (var i = 0; i < dummies.Length; i++)
                            dummies[i].Player.GuildReceived(
                                e.Content.Instance == GameServer.InstanceId
                                    ? e.Content.ObjectId
                                    : -1,
                                e.Content.Stars,
                                from,
                                e.Content.Text
                            );
                    }
                    break;

                case ChatType.Party:
                    {
                        var from = GameServer.Database.ResolveIgn(e.Content.From);
                        dummies = GameServer.ConnectionManager.Clients
                            .KeyWhereAsParallel(_ => _.Player != null
                                && !_.Account.IgnoreList.Contains(e.Content.From)
                                && _.Account.PartyId > 0
                                && _.Account.PartyId == e.Content.To);
                        for (var i = 0; i < dummies.Length; i++)
                            dummies[i].Player.PartyReceived(
                                e.Content.Instance == GameServer.InstanceId
                                    ? e.Content.ObjectId
                                    : -1,
                                e.Content.Stars,
                                from,
                                e.Content.Text
                            );
                    }
                    break;
            }
        }

        private void SendTextPacket(Player src, Text tp, Predicate<Player> predicate)
        {
            src.World.ForeachPlayer(_ =>
            {
                if (predicate(_))
                    _.Client.SendPacket(tp);
            });
            StaticLogger.Instance.Info($"[{src.World.IdName}({src.World.Id})] <{src.Name}> {tp.Txt}");
        }
    }
}
