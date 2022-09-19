using NLog;

namespace TKR.WorldServer.utils
{
    public static class StaticLogger
    {
        public static readonly Logger Instance = LogManager.GetCurrentClassLogger();
    }
}
