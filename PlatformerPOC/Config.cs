using Microsoft.Xna.Framework.Input;

namespace PlatformerPOC
{
    /// <summary>
    /// Simple config class that centralizes settings for easy tweaking.
    /// </summary>
    public static class Config
    {
        // Keys
        public const Keys moveLeftKey = Keys.Left;
        public const Keys moveRightKey = Keys.Right;
        public const Keys moveDownKey = Keys.Down;
        public const Keys moveUpKey = Keys.Up;
        public const Keys actionKey = Keys.Space;

        // Network
        public static int MaximumConnections = 4;
        public static string NetworkName = "PlatformPOC";
        public static string DefaultIp = "127.0.0.1";
        public static int Port = 6666;
        public const int MaxPlayers = 16;

        // Editor
        public static bool EditMode = false;

        // Debug mode
        public static bool DebugModeEnabled = true;
        public static bool VerboseDebugOutput = true;

        // Graphics
        public static int ScreenResolutionWidth = 1280;
        public static int ScreenResolutionHeight = 720;

        // Sound & effects:
        public static bool SoundEnabled = true;
        public static float SoundVolume = 1f;
        public static bool ScreenShakeEnabled = true;
    }
}