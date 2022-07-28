using StackExchange.Redis;
using System.Threading.Tasks;

namespace common.database
{
    public interface IRedisModel
    {
        Task InitAsync(IDatabase db, params string[] args);

        string KeyPattern(params string[] args);
    }
}
