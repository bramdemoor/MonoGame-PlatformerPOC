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
    }
}