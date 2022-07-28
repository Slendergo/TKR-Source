using CA.Extensions.Concurrent;
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
        private CoreServerManager CoreServerManager;

        public ChatManager(CoreServerManager coreServerManager) => CoreServerManager = coreServerManager;

        public void Announce(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var clients = CoreServerManager.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].Player.AnnouncementReceived(text);
        }

        public void AnnounceEternalLoot(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var clients = CoreServerManager.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].Player.SendEternalNotif(text);
        }

        public void AnnounceForger(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var clients = CoreServerManager.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].Player.ForgerNotif(text);
        }

        public void AnnounceLoot(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var clients = CoreServerManager.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].Player.SendLootNotif(text);
        }

        public void AnnounceRealm(string text, string name)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var clients = CoreServerManager.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].Player.AnnouncementRealm(text, name);
        }

        public void AnnounceMythicalLoot(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var clients = CoreServerManager.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Player != null);
            for (var i = 0; i < clients.Length; i++)
                clients[i].Player.SendMythicalNotif(text);
        }

        public bool Guild(Player src, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            CoreServerManager.InterServerManager.Publish(Channel.Chat, new ChatMsg()
            {
                Type = ChatType.Guild,
                Instance = CoreServerManager.InstanceId,
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
            CoreServerManager.InterServerManager.AddHandler<ChatMsg>(Channel.Chat, HandleChat);
            CoreServerManager.InterServerManager.AddHandler<RestartMsg>(Channel.Restart, HandleRestart);
            CoreServerManager.InterServerManager.AddHandler<AnnounceMsg>(Channel.Announce, HandleAnnounce);
            CoreServerManager.InterServerManager.NewServer += AnnounceNewServer;
            CoreServerManager.InterServerManager.ServerQuit += AnnounceServerQuit;
        }

        public void Mob(Entity entity, string text)
        {
            if (string.IsNullOrWhiteSpace(text) || entity.Owner == null)
                return;

            var world = entity.Owner;
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
            }, PacketPriority.Low);

            SLogger.Instance.Info($"[{world.Name}({world.Id})] <{name}> {text}");
        }

        public void Oryx(World world, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            world.Broadcast(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "#Oryx the Mad God",
                Txt = text
            }, PacketPriority.Low);
        }

        public bool Party(Player src, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            CoreServerManager.InterServerManager.Publish(Channel.Chat, new ChatMsg()
            {
                Type = ChatType.Party,
                Instance = CoreServerManager.InstanceId,
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
            else
            {
                var tp = new Text()
                {
                    Name = (src.Client.Account.Rank == 10 ? "(D-1) " : src.Client.Account.Rank == 20 ? "(D-2) " : src.Client.Account.Rank == 30 ? "(D-3) " : src.Client.Account.Rank == 40 ? "(D-4) " : src.Client.Account.Rank == 50 ? "(D-5) " : src.Client.Account.Rank == 60 ? "(VIP) " : src.Client.Account.Rank == 80 ? "(Tester) " : src.Client.Account.Rank >= 100 ? "(Admin) " : "") + src.Name,
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
        }

        public bool SendInfo(int target, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            CoreServerManager.InterServerManager.Publish(Channel.Chat, new ChatMsg()
            {
                Type = ChatType.Info,
                Instance = CoreServerManager.InstanceId,
                To = target,
                Text = text
            });

            return true;
        }

        public bool SendInfoMarket(int accId, string itemId, int realPrice, int resultPrice, int Tax)
        {
            CoreServerManager.InterServerManager.Publish(Channel.Chat, new ChatMsg()
            {
                Type = ChatType.Info,
                Instance = CoreServerManager.InstanceId,
                To = accId,
                Text = $"<Marketplace> {itemId} has been sold for {realPrice} (You have obtained: {resultPrice}) Fame, included {Tax}% Tax."
            });

            return true;
        }

        public void Shutdown()
        {
            CoreServerManager.InterServerManager.NewServer -= AnnounceNewServer;
            CoreServerManager.InterServerManager.ServerQuit -= AnnounceServerQuit;
        }

        public bool Tell(Player src, string target, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            var id = CoreServerManager.Database.ResolveId(target);

            if (id == 0)
                return false;

            if (!CoreServerManager.Database.AccountLockExists(id))
                return false;

            var acc = CoreServerManager.Database.GetAccount(id);
            if (acc == null)
                return false;

            if (acc.Hidden)
                return false;

            CoreServerManager.InterServerManager.Publish(Channel.Chat, new ChatMsg()
            {
                Type = ChatType.Tell,
                Instance = CoreServerManager.InstanceId,
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

            Announce($"A new server has come online: {networkMsg.Content.Info.name}");
        }

        private void AnnounceServerQuit(object sender, EventArgs e)
        {
            var networkMsg = (InterServerEventArgs<NetworkMsg>)e;

            Announce($"Server, {networkMsg.Content.Info.name}, is no longer online.");
        }

        private void HandleAnnounce(object sender, InterServerEventArgs<AnnounceMsg> e)
        {
            var user = e.Content.User;
            var message = e.Content.Message;
            var messageTemplate = $"({DateTime.UtcNow:hh:mm:ss}) Staff {user} says: {message}";
            var botName = Program.CoreServerManager.ServerConfig.discordIntegration.botName;
            var clients = CoreServerManager.ConnectionManager.Clients
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
                        var from = CoreServerManager.Database.ResolveIgn(e.Content.From);
                        var to = CoreServerManager.Database.ResolveIgn(e.Content.To);
                        dummies = CoreServerManager.ConnectionManager.Clients
                            .KeyWhereAsParallel(_ => _.Player != null
                                && !_.Account.IgnoreList.Contains(e.Content.From)
                                && (_.Account.AccountId == e.Content.From || _.Account.AccountId == e.Content.To || _.Account.IP == e.Content.SrcIP));
                        for (var i = 0; i < dummies.Length; i++)
                            dummies[i].Player.TellReceived(
                                e.Content.Instance == CoreServerManager.InstanceId
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
                        var to = CoreServerManager.Database.GetAccount(e.Content.To);
                        dummies = CoreServerManager.ConnectionManager.Clients
                            .KeyWhereAsParallel(_ => _.Player != null && _.Account.AccountId == to.AccountId);
                        for (var i = 0; i < dummies.Length; i++)
                            dummies[i].Player.SendInfo(e.Content.Text);
                    }
                    break;

                case ChatType.Guild:
                    {
                        var from = CoreServerManager.Database.ResolveIgn(e.Content.From);
                        dummies = CoreServerManager.ConnectionManager.Clients
                            .KeyWhereAsParallel(_ => _.Player != null
                                && !_.Account.IgnoreList.Contains(e.Content.From)
                                && _.Account.GuildId > 0
                                && _.Account.GuildId == e.Content.To);
                        for (var i = 0; i < dummies.Length; i++)
                            dummies[i].Player.GuildReceived(
                                e.Content.Instance == CoreServerManager.InstanceId
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
                        var from = CoreServerManager.Database.ResolveIgn(e.Content.From);
                        dummies = CoreServerManager.ConnectionManager.Clients
                            .KeyWhereAsParallel(_ => _.Player != null
                                && !_.Account.IgnoreList.Contains(e.Content.From)
                                && _.Account.PartyId > 0
                                && _.Account.PartyId == e.Content.To);
                        for (var i = 0; i < dummies.Length; i++)
                            dummies[i].Player.PartyReceived(
                                e.Content.Instance == CoreServerManager.InstanceId
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

        private void HandleRestart(object sender, InterServerEventArgs<RestartMsg> e)
        {
            var user = e.Content.User;
            var listeners = new KeyValuePair<TimeSpan, Action>[]
            {
                new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(1), () => Program.RestartAnnouncement(1)),
                new KeyValuePair<TimeSpan, Action>(TimeSpan.FromSeconds(30), () => Program.RestartAnnouncement(-1))
            };

            Program.SetupRestarter(TimeSpan.FromMinutes(1.05), listeners);

            var messageTemplate = $"({DateTime.UtcNow:hh:mm:ss}) This server is preparing to restart, requested by Staff {user} via Discord.";
            var botName = Program.CoreServerManager.ServerConfig.discordIntegration.botName;
            var clients = CoreServerManager.ConnectionManager.Clients
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

        private void SendTextPacket(Player src, Text tp, Predicate<Player> conditional)
        {
            src.Owner.PlayersBroadcastAsParallel(_ =>
            {
                if (conditional(_))
                    _.Client.SendPacket(tp, PacketPriority.Normal);
            });
            SLogger.Instance.Info($"[{src.Owner.Name}({src.Owner.Id})] <{src.Name}> {tp.Txt}");
        }
    }
}
