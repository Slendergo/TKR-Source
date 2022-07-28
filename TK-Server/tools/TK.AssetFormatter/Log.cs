using System;

namespace tk.assetformatter
{
    public static class Log
    {
        public static void Breakline() => Console.WriteLine("\n");

        public static void Error(string message, Exception exception = null) => Logger(message, ConsoleColor.Red, exception);

        public static void Info(string message) => Logger(message, ConsoleColor.DarkGreen);

        public static void Warn(string message) => Logger(message, ConsoleColor.Yellow);

        private static void Logger(string message, ConsoleColor color, Exception exception = null)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);

            if (exception != null)
                Console.WriteLine($"\n{exception}");

            Console.ResetColor();
        }
    }
}
