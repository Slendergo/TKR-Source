using System.Collections.Concurrent;
using wServer.core.worlds.logic;
using wServer.core.net.handlers;
using wServer.networking.packets.outgoing;

namespace wServer.core.objects
{
    public partial class Player
    {
        public const int DcThresold = 12000;

        public int LastClientTime = -1;
        public long LastServerTime = -1;

        private const int PingPeriod = 1000;

        private int _cnt;
        private ConcurrentQueue<long> _gotoAckTimeout = new ConcurrentQueue<long>();
        private long _latSum;
        private long _pingTime = -1;
        private long _pongTime = -1;
        private ConcurrentQueue<long> _shootAckTimeout = new ConcurrentQueue<long>();
        private long _sum;
        public int _tps;
        private ConcurrentQueue<long> _updateAckTimeout = new ConcurrentQueue<long>();

        public int Latency { get; private set; }
        public long TimeMap { get; private set; }

        public void AwaitGotoAck(long serverTime) => _gotoAckTimeout.Enqueue(serverTime + DcThresold);

        public long C2STime(int clientTime) => clientTime + TimeMap;

        public int GotoAckCount() => _gotoAckTimeout.Count;

        public void GotoAckReceived()
        {
            if (!_gotoAckTimeout.TryDequeue(out var ignored))
                Client.Disconnect("One too many GotoAcks");
        }

        public void Pong(TickTime tickTime, int time, int serial)
        {
            _cnt++;

            _sum += tickTime.TotalElapsedMs - time;
            TimeMap = _sum / _cnt;

            _latSum += (tickTime.TotalElapsedMs - serial) / 2;
            Latency = (int)_latSum / _cnt;

            _pongTime = tickTime.TotalElapsedMs;
        }

        public void UpdateAckReceived()
        {
            if (!_updateAckTimeout.TryDequeue(out var ignored))
                Client.Disconnect("One too many UpdateAcks");
        }

        private bool KeepAlive(TickTime time)
        {
            if (Client.State == networking.ProtocolState.Disconnected)
                return false;

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
                Serial = (int)time.TotalElapsedMs,
                RTT = Latency//(int)(_pingTime - _pongTime) - PingPeriod
            });
            return UpdateOnPing();
        }

        private bool UpdateOnPing()
        {
            // renew account lock
            try
            {
                if (!GameServer.Database.RenewLock(Client.Account))
                    Client.Disconnect("RenewLock failed. (Pong)");
            }
            catch
            {
                Client.Disconnect("RenewLock failed. (Timeout)");
                return false;
            }

            // save character
            if (!(World is TestWorld))
            {
                SaveToCharacter();
                Client.Character?.FlushAsync();
            }
            return true;
        }
    }
}
