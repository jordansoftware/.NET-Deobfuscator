using System;

namespace UniversalNetRemover.Utils
{
    public static class Logger
    {
        public static event Action<string>? OnLog;

        public static void Info(string message) => Log("INFO", message);
        public static void Success(string message) => Log("SUCCESS", message);
        public static void Warning(string message) => Log("WARNING", message);
        public static void Error(string message) => Log("ERROR", message);

        private static void Log(string level, string message)
        {
            string formatted = $"[{DateTime.Now:HH:mm:ss}] [{level}] {message}";
            OnLog?.Invoke(formatted);
            System.Diagnostics.Debug.WriteLine(formatted);
        }
    }
}