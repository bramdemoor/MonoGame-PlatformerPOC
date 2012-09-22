using GameEngine;
using GameEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PlatformerPOC.Screens
{
    public class LobbyScreen : SimpleScreenBase
    {
        public override void Draw(GameTime gameTime)
        {
            DrawText("This is the lobby screen", 40);

            if(!SimpleGameEngine.Instance.IsConnected)
            {
                DrawText("Press H to host and J to join (localhost test only)", 60);                
            }
            else
            {
                DrawText("Connected. Press D to disconnect", 60);                

                if (SimpleGameEngine.Instance.IsHost)
                {
                    DrawText("Press S to start the game", 80);                    
                }
                else
                {
                    DrawText("Waiting for the host to start the game", 80);                    
                }
            }
        }

        private static void DrawText(string text, int y)
        {
            SimpleGameEngine.Instance.spriteBatch.DrawString(ResourcesHelper.DefaultFont, text, new Vector2(50, y), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                SimpleGameEngine.Instance.ShutDown();
            }


            if (!SimpleGameEngine.Instance.IsConnected)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.H))
                {
                    SimpleGameEngine.Instance.InitializeAsHost();
                }

                if (Keyboard.GetState().IsKeyDown(Keys.J))
                {
                    SimpleGameEngine.Instance.InitializeAsClient();
                }                
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    SimpleGameEngine.Instance.Disconnect();
                }

                if (SimpleGameEngine.Instance.IsHost)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        PlatformGame.Instance.HostStartGame();
                    }  
                }
            }

        }
         
    }
}