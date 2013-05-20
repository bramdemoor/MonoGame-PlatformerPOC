using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.Domain;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Editor
{
    public class Editor
    {
        private MouseState mouseStatePrevious;
        private MouseState mouseStateCurrent;

        private readonly PlatformGame game;

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
            mouseStatePrevious = mouseStateCurrent;

            mouseStateCurrent = Mouse.GetState();

            if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
            {
                var pos = new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y);

                var worldCoords = game.renderer.GetWorldCoords(pos);

                var tiles = GameWorld.PixelsToTiles(worldCoords);                

                game.DebugCommandUI.Echo(string.Format("FYI: Tile modify at {0}", tiles));
            }            
        }


    }
}