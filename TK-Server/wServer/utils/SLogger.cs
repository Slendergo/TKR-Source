using NLog;

namespace wServer.utils
{
    public static class StaticLogger
    {
        public static readonly Logger Instance = LogManager.GetCurrentClassLogger();
    }
}
