using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PlatformerPOC.Screens
{
    public class LobbyScreen : SimpleScreenBase
    {
        private readonly PlatformGame game;

        public LobbyScreen(PlatformGame game)
        {
            this.game = game;
        }

        public override void Draw(GameTime gameTime)
        {
            DrawText("This is the lobby screen", 40);

            if(!game.IsConnected)
            {
                DrawText("Press H to host and J to join (localhost test only)", 60);                
            }
            else
            {
                DrawText("Connected. Press D to disconnect", 60);                

                if (game.IsHost)
                {
                    DrawText("Press S to start the game", 80);                    
                }
                else
                {
                    DrawText("Waiting for the host to start the game", 80);                    
                }
            }
        }

        private void DrawText(string text, int y)
        {
            game.SpriteBatch.DrawString(game.ResourcesHelper.DefaultFont, text, new Vector2(50, y), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.ShutDown();
            }


            if (!game.IsConnected)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.H))
                {
                    game.InitializeAsHost();
                }

                if (Keyboard.GetState().IsKeyDown(Keys.J))
                {
                    game.InitializeAsClient();
                }                
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    game.Disconnect();
                }

                if (game.IsHost)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        game.HostStartGame();
                    }  
                }
            }

        }
         
    }
}