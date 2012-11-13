using System.Collections.Generic;

namespace GameEngine.DebugHelpers
{
    public static class CoreCommands
    {
        public static void ToggleDebugCommand(IDebugCommandHost host, string command, IList<string> arguments)
        {
            CoreConfig.DebugModeEnabled = !CoreConfig.DebugModeEnabled;
        } 
    }
}