namespace GameEngine
{
    /// <summary>
    /// Simple config class that centralizes core settings for easy tweaking.
    /// </summary>
    public class CoreConfig
    {
        public static int MaximumConnections = 4;
        public static string NetworkName = "PlatformPOC";
        public static string DefaultIp = "127.0.0.1";
        public static int Port = 6666;
        public static float SimulatedMinimumLatency = 0.2f;
        public static float SimulatedLoss = 0.1f;
        public static bool DebugModeEnabled = false;
        public static bool SoundEnabled = false;
        public static float SoundVolume = 1f;
    }
}