namespace PlatformerPOC.Helpers
{
    public static class CoreCommands
    {
        public static void ToggleDebugCommand(IDebugCommandHost host, string command, params string[] arguments)
        {
            Config.DebugModeEnabled = !Config.DebugModeEnabled;
        }

        public static void ToggleSoundCommand(IDebugCommandHost host, string command, params string[] arguments)
        {
            Config.SoundEnabled = !Config.SoundEnabled;
        }
    }
}