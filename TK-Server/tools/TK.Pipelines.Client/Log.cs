using System;

namespace tk.pipelines.client
{
    public static class Log
    {
        public static void Error(string message, Exception exception = null) => Logger("Error", message, ConsoleColor.Red, exception);

        public static void Info(string message) => Logger("Info", message, ConsoleColor.Green);

        public static void Warn(string message) => Logger("Warning", message, ConsoleColor.Yellow);

        private static void Logger(string severity, string message, ConsoleColor color, Exception exception = null)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"{DateTime.UtcNow:hh:mm:ss} [{severity}] {message}");

            if (exception != null)
                Console.WriteLine($"\n{exception}");

            Console.ResetColor();
        }
    }
}
