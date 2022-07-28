using NLog;

namespace wServer.utils
{
    public static class SLogger
    {
        public static readonly Logger Instance = LogManager.GetCurrentClassLogger();
    }
}
