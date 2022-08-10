using common;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using wServer.core;
using wServer.core.net.handlers;
using wServer.core.worlds;
using wServer.networking.packets.outgoing;
using wServer.utils;


namespace wServer.networking.connection
{
    public sealed class NetworkHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly int BufferSize;
        private readonly int PrefixLength;
        private Client Client;
        private ConcurrentQueue<OutgoingMessage> Pending;
        private SocketAsyncEventArgs Receive;
        private SocketAsyncEventArgs Send;

        public NetworkHandler(Client client, SocketAsyncEventArgs send, SocketAsyncEventArgs receive)
        {
            PrefixLength = ReceiveToken.PrefixLength;
            BufferSize = ConnectionListener.BufferSize;
            Client = client;

            Receive = receive;
            Receive.Completed += ProcessReceive;

            Send = send;
            Send.Completed += ProcessSend;

            Pending = new ConcurrentQueue<OutgoingMessage>();
        }

        public void BeginHandling(Socket socket)
        {
            Send.AcceptSocket = socket;
            Receive.AcceptSocket = socket;

            Client.State = ProtocolState.Connected;

            StartReceive(Receive);
            StartSendAsync(Send);
        }

        public void Reset()
        {
            ((SendToken)Send.UserToken).Reset();
            ((ReceiveToken)Receive.UserToken).Reset();

            Pending = new ConcurrentQueue<OutgoingMessage>();
        }

        public void SendPacket(OutgoingMessage pkt) => Pending.Enqueue(pkt);

        public void SendPackets(IEnumerable<OutgoingMessage> pkts)
        {
            foreach (var pkt in pkts)
                SendPacket(pkt);
        }

        private static int ReadPacketBytes(SocketAsyncEventArgs e, ReceiveToken r, int bytesNotRead)
        {
            var offset = r.BufferOffset + e.BytesTransferred - bytesNotRead;
            var remainingBytes = r.PacketLength - r.BytesRead;

            if (bytesNotRead < remainingBytes)
            {
                Buffer.BlockCopy(e.Buffer, offset, r.PacketBytes, r.BytesRead, bytesNotRead);
                r.BytesRead += bytesNotRead;
                return 0;
            }

            Buffer.BlockCopy(e.Buffer, offset, r.PacketBytes, r.BytesRead, remainingBytes);

            r.BytesRead = r.PacketLength;
            return bytesNotRead - remainingBytes;
        }

        private bool? FlushPending(SendToken s)
        {
            try
            {
                while (Pending.TryDequeue(out var packet))
                {
                    var bytesWritten = packet.Write(s.Data, s.BytesAvailable);
                    if (!bytesWritten.HasValue)
                        continue;

                    if (bytesWritten == 0)
                    {
                        Pending.Enqueue(packet);
                        return true;
                    }

                    s.BytesAvailable += bytesWritten.Value;
                }

                if (s.BytesAvailable <= 0)
                    return false;
            }
            catch (Exception e)
            {
                Log.Error(e);
                Client.Disconnect("Error when handling pending packets");
            }

            return true;
        }

        private void ProcessReceive(object sender, SocketAsyncEventArgs e)
        {
            var r = (ReceiveToken)e.UserToken;

            if (Client.State == ProtocolState.Disconnected)
            {
                r.Reset();
                return;
            }

            if (e.SocketError != SocketError.Success)
            {
                var msg = "";
                if (e.SocketError != SocketError.ConnectionReset)
                    msg = "Receive SocketError = " + e.SocketError;

                Client.Disconnect(msg);
                return;
            }

            var bytesNotRead = e.BytesTransferred;
            if (bytesNotRead == 0)
            {
                Client.Disconnect("bytesNotRead == 0");
                return;
            }

            while (bytesNotRead > 0)
            {
                bytesNotRead = ReadPacketBytes(e, r, bytesNotRead);

                if (r.BytesRead == PrefixLength)
                {
                    r.PacketLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(r.PacketBytes, 0));

                    if (r.PacketLength == 1014001516)
                    {
                        SendPolicyFile();
                        r.Reset();
                        break;
                    }

                    if (r.PacketLength < PrefixLength || r.PacketLength > BufferSize)
                    {
                        r.Reset();
                        break;
                    }
                }

                if (r.BytesRead == r.PacketLength)
                {
                    if (Client.IsReady())
                    {
                        var id = r.GetPacketId();
                        var payload = r.GetPacketBody();
                        if (Client.Player == null) // read it instantly if there is no player otherwise we will append it to the world instance
                        {
                            var handler = MessageHandlers.GetHandler(id);
                            if (handler == null)
                            {
                                Client.PacketSpamAmount++;
                                if (Client.PacketSpamAmount > 32)
                                    Client.Disconnect($"Packet Spam: {Client.IpAddress}");
                                SLogger.Instance.Error($"Unknown MessageId: {id} - {Client.IpAddress}");
                                continue;
                            }

                            // todoo redo
                            try
                            {
                                NReader rdr = null;
                                if (payload.Length != 0)
                                    rdr = new NReader(new MemoryStream(payload));
                                var time = new TickTime();
                                handler.Handle(Client, rdr, ref time);
                                rdr?.Dispose();
                            }
                            catch (Exception exx)
                            {
                                if (!(exx is EndOfStreamException))
                                    SLogger.Instance.Error("Error processing packet ({0}, {1}, {2})\n{3}", (Client.Account != null) ? Client.Account.Name : "", Client.IpAddress, id, exx);
                                Client.Disconnect($"Read Error for packet: {id}");
                            }
                        }
                        else
                            Client.Player.IncomingMessages.Enqueue(new InboundBuffer(Client, id, payload));
                    }
                    r.Reset();
                }
            }

            StartReceive(e);
        }

        private void ProcessSend(object sender, SocketAsyncEventArgs e)
        {
            var s = (SendToken)e.UserToken;

            if (Client.State == ProtocolState.Disconnected)
            {
                s.Reset();
                return;
            }

            if (e.SocketError != SocketError.Success)
            {
                Client.Disconnect("Send SocketError = " + e.SocketError);
                return;
            }

            s.BytesSent += e.BytesTransferred;
            s.BytesAvailable -= s.BytesSent;

            var delay = 0;
            if (s.BytesAvailable <= 0)
                delay = 1;

            StartSendAsync(e, delay);
        }

        private void SendPolicyFile()
        {
            if (Client.Socket == null)
                return;

            try
            {
                var s = new NetworkStream(Client.Socket);
                var wtr = new NWriter(s);
                wtr.WriteNullTerminatedString(
                    @"<cross-domain-policy>" +
                    @"<allow-access-from domain=""*"" to-ports=""*"" />" +
                    @"</cross-domain-policy>");
                wtr.Write((byte)'\r');
                wtr.Write((byte)'\n');
            }
            catch (Exception e)
            {
                SLogger.Instance.Error(e.ToString());
            }
        }

        private void StartReceive(SocketAsyncEventArgs e)
        {
            if (Client.State == ProtocolState.Disconnected)
                return;

            e.SetBuffer(e.Offset, BufferSize);

            bool willRaiseEvent;
            try
            {
                willRaiseEvent = e.AcceptSocket.ReceiveAsync(e);
            }
            catch (Exception exception)
            {
                Client.Disconnect($"[{Client.Account?.Name}:{Client.Account?.AccountId} {Client.IpAddress}] {exception}");
                return;
            }

            if (!willRaiseEvent)
                ProcessReceive(null, e);
        }

        private async void StartSendAsync(SocketAsyncEventArgs e, int delay = 0)
        {
            if (Client?.State == ProtocolState.Disconnected)
                return;

            try
            {
                var s = (SendToken)e.UserToken;

                if (delay > 0)
                    await Task.Delay(delay);

                if (s.BytesAvailable <= 0)
                {
                    s.Reset();
                    if (!FlushPending(s).HasValue)
                        return;
                }

                var willRaiseEvent = false;

                try
                {
                    var bytesToSend = s.BytesAvailable > BufferSize ? BufferSize : s.BytesAvailable;

                    e.SetBuffer(s.BufferOffset, bytesToSend);

                    Buffer.BlockCopy(s.Data, s.BytesSent, e.Buffer, s.BufferOffset, bytesToSend);

                    willRaiseEvent = e.AcceptSocket.SendAsync(e);
                }
                catch
                {
                }
                finally
                {
                    if (!willRaiseEvent)
                        ProcessSend(null, e);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StartSend: {ex}");

                Client.Player.SendError("Unknown error sent to nexus");
                Client.Reconnect(new Reconnect()
                {
                    Host = "",
                    Port = Client.GameServer.Configuration.serverInfo.port,
                    GameId = World.NEXUS_ID,
                    Name = "Nexus"
                });
            }
        }
    }
}
