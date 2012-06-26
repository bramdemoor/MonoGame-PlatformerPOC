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
            var spriteBatch = SimpleGameEngine.Instance.spriteBatch;

            //if(game.is)

            spriteBatch.DrawString(PlatformGame.Instance.font, "This is the lobby screen", new Vector2(50, 40), Color.Red);

            if(!SimpleGameEngine.Instance.IsConnected)
            {
                spriteBatch.DrawString(PlatformGame.Instance.font, "Press H to host and J to join (localhost test only)", new Vector2(50, 60), Color.Red);    
            }
            else
            {
                spriteBatch.DrawString(PlatformGame.Instance.font, "Connected. Press D to disconnect", new Vector2(50, 60), Color.Red);

                if (SimpleGameEngine.Instance.IsHost)
                {
                    spriteBatch.DrawString(PlatformGame.Instance.font, "Press S to start the game", new Vector2(50, 80), Color.Red);    
                }
                else
                {
                    spriteBatch.DrawString(PlatformGame.Instance.font, "Waiting for the host to start the game", new Vector2(50, 80), Color.Red);    
                }
            }         
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