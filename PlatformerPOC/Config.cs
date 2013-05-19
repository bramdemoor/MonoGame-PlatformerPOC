using Microsoft.Xna.Framework.Input;

namespace PlatformerPOC
{
    /// <summary>
    /// Simple config class that centralizes settings for easy tweaking.
    /// </summary>
    public static class Config
    {
        public const Keys moveLeftKey = Keys.Left;
        public const Keys moveRightKey = Keys.Right;
        public const Keys moveDownKey = Keys.Down;
        public const Keys moveUpKey = Keys.Up;
        public const Keys actionKey = Keys.Space;

        public static bool EditMode = false;

        public const int MaxPlayers = 16;

        public static int MaximumConnections = 4;
        public static string NetworkName = "PlatformPOC";
        public static string DefaultIp = "127.0.0.1";
        public static int Port = 6666;
        public static float SimulatedMinimumLatency = 0.2f;
        public static float SimulatedLoss = 0.1f;
        public static bool DebugModeEnabled = false;
        public static bool VerboseDebugOutput = true;
        public static bool SoundEnabled = false;
        public static float SoundVolume = 1f;
        public static int ScreenResolutionWidth = 1280;
        public static int ScreenResolutionHeight = 720;
    }
}