namespace PlatformerPOC.Helpers
{
    public static class CoreCommands
    {
        public static void ToggleDebugCommand(IDebugCommandHost host, string command, params string[] arguments)
        {
            CoreConfig.DebugModeEnabled = !CoreConfig.DebugModeEnabled;
        }

        public static void ToggleSoundCommand(IDebugCommandHost host, string command, params string[] arguments)
        {
            CoreConfig.SoundEnabled = !CoreConfig.SoundEnabled;
        }
    }
}