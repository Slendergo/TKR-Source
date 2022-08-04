using common.database;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using wServer.core;
using wServer.core.objects;
using wServer.core.worlds.logic;
using wServer.networking.connection;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking
{
    public partial class Client
    {
        internal object DcLock = new object();

        //Temporary connection state
        internal int TargetWorld = -1;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private NetworkHandler _handler;
        private ConnectionListener _server;
        private volatile ProtocolState _state;

        public Client(ConnectionListener server, CoreServerManager coreServerManager, SocketAsyncEventArgs send, SocketAsyncEventArgs receive)
        {
            _server = server;

            CoreServerManager = coreServerManager;

            _handler = new NetworkHandler(this, send, receive);
        }

        public DbAccount Account { get; internal set; }
        public DbChar Character { get; internal set; }
        public CoreServerManager CoreServerManager { get; private set; }
        public int Id { get; internal set; }
        public string IpAddress { get; private set; }
        public bool IsLagging { get; private set; }
        public Player Player { get; internal set; }
        public wRandom Random { get; internal set; }
        public Socket Socket { get; private set; }
        public ProtocolState State { get => _state; internal set => _state = value; }

        public void BeginHandling(Socket skt)
        {
            Socket = skt;

            try
            {
                IpAddress = ((IPEndPoint)skt.RemoteEndPoint).Address.ToString();
            }
            catch
            {
                IpAddress = "";
            }

            _handler.BeginHandling(Socket);
        }

        public void Disconnect(string reason = "")
        {
            if (State == ProtocolState.Disconnected)
                return;

            using (TimedLock.Lock(DcLock))
            {
                State = ProtocolState.Disconnected;

                if (!string.IsNullOrEmpty(reason) && CoreServerManager.ServerConfig.serverInfo.debug)
                    Log.Warn("Disconnecting client ({0}) @ {1}... {2}", Account?.Name ?? " ", IpAddress, reason);

                if (Account != null)
                    try
                    {
                        Save();
                    }
                    catch (Exception e)
                    {
                        var msg = $"{e.Message}\n{e.StackTrace}";
                        Log.Error(msg);
                    }
                //StopTask_ = true;
                Player?.CleanupPlayerUpdate();
                CoreServerManager.ConnectionManager.Disconnect(this);

                _server.Disconnect(this);
            }
        }

        public bool IsReady()
        {
            if (State == ProtocolState.Disconnected)
                return false;

            if (State == ProtocolState.Ready && Player?.World == null)
                return false;

            return true;
        }

        public void Reconnect(Reconnect pkt)
        {
            if (Account == null)
            {
                Disconnect("Tried to reconnect an client with a null account...");
                return;
            }

            //Log.Trace("Reconnecting client ({0}) @ {1} to {2}...", Account.Name, IP, pkt.Name);
            CoreServerManager.ConnectionManager.AddReconnect(Account.AccountId, pkt);
            SendPacket(pkt, PacketPriority.High);
        }

        public void Reset()
        {
            Id = 0; // needed so that inbound packets that are currently queued are discarded.

            Account = null;
            Character = null;
            IpAddress = null;
            Player = null;

            // reset client ping/pong values
            //_pingTime = -1;
            //_pongTime = -1;

            _handler.Reset();
        }

        public async void SendFailure(string text, int errorId = Failure.MessageWithDisconnect)
        {
            SendPacket(new Failure()
            {
                ErrorId = errorId,
                ErrorDescription = text
            });

            if (errorId == Failure.MessageWithDisconnect || errorId == Failure.ForceCloseGame)
            {
                var t = Task.Delay(1000);
                await t;

                Disconnect($"SendFailure: {text}");
            }
        }

        public void SendPacket(OutgoingMessage pkt, PacketPriority priority = PacketPriority.Normal)
        {
            if (State != ProtocolState.Disconnected)
                _handler.SendPacket(pkt, priority);
        }

        public void SendPackets(IEnumerable<OutgoingMessage> pkts, PacketPriority priority = PacketPriority.Normal)
        {
            if (State != ProtocolState.Disconnected)
                _handler.SendPackets(pkts, priority);
        }

        internal void ProcessPacket(Packet pkt, ref TickTime time)
        {
            using (TimedLock.Lock(DcLock))
            {
                if (State == ProtocolState.Disconnected)
                    return;

                try
                {
                    if (!PacketHandlers.Handlers.TryGetValue(pkt.MessageID, out var handler))
                        return;

                    handler.Handle(this, (IncomingMessage)pkt, ref time);
                }
                catch (Exception e)
                {
                    Log.Error($"Error when handling packet '{pkt}, {e}'...");
                    Disconnect("Packet handling error.");
                }
            }
        }

        private void Save() // only when disconnect
        {
            var acc = Account;

            if (Character == null || Player == null || Player.World is TestWorld)
            {
                CoreServerManager.Database.ReleaseLock(acc);
                return;
            }

            Player.SaveToCharacter();
            acc?.RefreshLastSeen();
            acc?.FlushAsync();

            if (CoreServerManager != null && CoreServerManager.Database != null && Player.FameCounter != null && Player.FameCounter.ClassStats != null)
                if (CoreServerManager.Database.SaveCharacter(acc, Character, Player.FameCounter.ClassStats, true))
                    CoreServerManager.Database.ReleaseLock(acc);
        }
    }
}
