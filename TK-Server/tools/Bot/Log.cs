using System;

namespace tk.bot
{
    public static class Log
    {
        public static void Error(string message, Exception exception = null) => Logger("Error", message, exception);

        public static void Info(string message) => Logger("Info", message, null);

        public static void Warn(string message) => Logger("Warning", message, null);

        private static void Logger(string severity, string message, Exception exception)
        {
            Console.WriteLine($"{DateTime.UtcNow:hh:mm:ss} [{severity}] {message}");

            if (exception != null)
                Console.WriteLine($"\n{exception}");
        }
    }
}
