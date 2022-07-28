using CA.Threading.Tasks;
using StackExchange.Redis;
using System.Threading;

namespace common.database
{
    public interface IRedisService
    {
        CancellationTokenSource Cts { get; set; }
        IDatabase Db { get; set; }
        InternalRoutine Routine { get; set; }

        void Core(int delta);

        void Start();

        void Stop();
    }
}
