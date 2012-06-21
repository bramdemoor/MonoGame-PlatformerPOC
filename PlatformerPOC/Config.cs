namespace PlatformerPOC
{
    /// <summary>
    /// Simple config class that centralizes settings
    /// </summary>
    public class Config
    {
        public const int MaximumConnections = 4;
        public const string networkName = "PlatformPOC";
        public const string defaultIp = "127.0.0.1";
        public const int port = 6666;
        public const float SimulatedMinimumLatency = 0.2f;
        public const float SimulatedLoss = 0.1f;
    }
}