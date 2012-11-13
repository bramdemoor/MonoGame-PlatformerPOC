using GameEngine.DebugHelpers;
using Microsoft.Xna.Framework.Input;

namespace PlatformerPOC.Editor
{
    public class Editor
    {
        private PlatformGame game;

        public Editor(PlatformGame platformGame)
        {
            game = platformGame;
        }

        public void ToggleEditModeCommand(IDebugCommandHost host, string command, string[] arguments)
        {
            if(Config.EditMode == false)
            {
                game.IsMouseVisible = true;

                Config.EditMode = true;
            }
            else
            {
                game.IsMouseVisible = false;

                Config.EditMode = false;
            }
        }

        public void Update()
        {
            // Get current mouseState
            var mouseStateCurrent = Mouse.GetState();

            // Left MouseClick
            if (mouseStateCurrent.LeftButton == ButtonState.Pressed)
            {
                game.DebugCommandUI.Echo("FYI: Left mouse pressed");
            }            
        }


    }
}