using System.Collections.Concurrent;
using wServer.core.worlds.logic;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.core.objects
{
    public partial class Player
    {
        public const int DcThresold = 12000;

        public int LastClientTime = -1;
        public long LastServerTime = -1;

        private const int PingPeriod = 3000;

        private ConcurrentQueue<int> _clientTimeLog = new ConcurrentQueue<int>();
        private int _cnt;
        private ConcurrentQueue<long> _gotoAckTimeout = new ConcurrentQueue<long>();
        private long _latSum;
        private ConcurrentQueue<int> _move = new ConcurrentQueue<int>();
        private long _pingTime = -1;
        private long _pongTime = -1;
        private ConcurrentQueue<int> _serverTimeLog = new ConcurrentQueue<int>();
        private ConcurrentQueue<long> _shootAckTimeout = new ConcurrentQueue<long>();
        private long _sum;
        public int _tps;
        private ConcurrentQueue<long> _updateAckTimeout = new ConcurrentQueue<long>();

        public int Latency { get; private set; }
        public long TimeMap { get; private set; }

        public void AwaitGotoAck(long serverTime) => _gotoAckTimeout.Enqueue(serverTime + DcThresold);

        public void AwaitMove(int tickId) => _move.Enqueue(tickId);

        public long C2STime(int clientTime) => clientTime + TimeMap;

        public int GotoAckCount() => _gotoAckTimeout.Count;

        public void GotoAckReceived()
        {
            if (!_gotoAckTimeout.TryDequeue(out var ignored))
                Client.Disconnect("One too many GotoAcks");
        }

        public void MoveReceived(TickData time, Move pkt)
        {
            if (!_move.TryDequeue(out var tickId))
            {
                Client.Disconnect("One too many MovePackets");
                return;
            }

            if (tickId != pkt.TickId)
            {
                Client.Disconnect("[NewTick -> Move] TickIds don't match");
                return;
            }

            if (pkt.TickId > PlayerUpdate.TickId)
            {
                Client.Disconnect("[NewTick -> Move] Invalid tickId");
                return;
            }

            var lastClientTime = LastClientTime;
            var lastServerTime = LastServerTime;

            LastClientTime = pkt.Time;
            LastServerTime = time.TotalElapsedMs;

            if (lastClientTime == -1)
                return;

            _clientTimeLog.Enqueue(pkt.Time - lastClientTime);
            _serverTimeLog.Enqueue((int)(time.TotalElapsedMs - lastServerTime));

            if (_clientTimeLog.Count < 30)
                return;

            if (_clientTimeLog.Count > 30)
            {
                _clientTimeLog.TryDequeue(out var ignore);
                _serverTimeLog.TryDequeue(out ignore);
            }
        }

        public void Pong(TickData time, Pong pongPkt)
        {
            _cnt++;

            _sum += time.TotalElapsedMs - pongPkt.Time;
            TimeMap = _sum / _cnt;

            _latSum += (time.TotalElapsedMs - pongPkt.Serial) / 2;
            Latency = (int)_latSum / _cnt;

            _pongTime = time.TotalElapsedMs;
        }

        public void UpdateAckReceived()
        {
            if (!_updateAckTimeout.TryDequeue(out var ignored))
                Client.Disconnect("One too many UpdateAcks");
        }

        private bool KeepAlive(TickData time)
        {
            if (_pingTime == -1)
            {
                _pingTime = time.TotalElapsedMs - PingPeriod;
                _pongTime = time.TotalElapsedMs;
            }

            // check for disconnect timeout
            if (time.TotalElapsedMs - _pongTime > DcThresold)
            {
                Client.Disconnect("Connection timeout. (KeepAlive)");
                return false;
            }


            // check for shootack timeout
            if (_shootAckTimeout.TryPeek(out long timeout))
            {
                if (time.TotalElapsedMs > timeout)
                {
                    Client.Disconnect("Connection timeout. (ShootAck)");
                    return false;
                }
            }

            // check for updateack timeout
            if (_updateAckTimeout.TryPeek(out timeout))
            {
                if (time.TotalElapsedMs > timeout)
                {
                    Client.Disconnect("Connection timeout. (UpdateAck)");
                    return false;
                }
            }

            // check for gotoack timeout
            if (_gotoAckTimeout.TryPeek(out timeout))
            {
                if (time.TotalElapsedMs > timeout)
                {
                    Client.Disconnect("Connection timeout. (GotoAck)");
                    return false;
                }
            }

            if (time.TotalElapsedMs - _pingTime < PingPeriod)
                return true;

            // send ping
            _pingTime = time.TotalElapsedMs;
            Client.SendPacket(new Ping()
            {
                Serial = (int)time.TotalElapsedMs
            });
            return UpdateOnPing();
        }

        private bool UpdateOnPing()
        {
            // renew account lock
            try
            {
                if (!CoreServerManager.Database.RenewLock(Client.Account))
                    Client.Disconnect("RenewLock failed. (Pong)");
            }
            catch
            {
                Client.Disconnect("RenewLock failed. (Timeout)");
                return false;
            }

            // save character
            if (!(Owner is Test))
            {
                SaveToCharacter();
                Client.Character?.FlushAsync();
            }
            return true;
        }
    }
}
