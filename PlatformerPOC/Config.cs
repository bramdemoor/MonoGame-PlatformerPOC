namespace PlatformerPOC
{
    /// <summary>
    /// Simple config class that centralizes settings
    /// </summary>
    public static class Config
    {
        public const int MaximumConnections = 4;
        public const string NetworkName = "PlatformPOC";
        public const string DefaultIp = "127.0.0.1";
        public const int Port = 6666;
        public const float SimulatedMinimumLatency = 0.2f;
        public const float SimulatedLoss = 0.1f;
    }
}