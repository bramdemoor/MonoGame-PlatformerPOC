using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PlatformerPOC.Screens
{
    public class LobbyScreen : SimpleScreen
    {
        private PlatformGame game;

        public LobbyScreen(PlatformGame game)
        {
            this.game = game;
        }

        public override void Draw(GameTime gameTime)
        {
            //if(game.is)

            game.spriteBatch.DrawString(game.font, "This is the lobby screen", new Vector2(50, 40), Color.Red);

            if(!game.IsConnected)
            {
                game.spriteBatch.DrawString(game.font, "Press H to host and J to join (localhost test only)", new Vector2(50, 60), Color.Red);    
            }
            else
            {
                game.spriteBatch.DrawString(game.font, "Connected. Press D to disconnect", new Vector2(50, 60), Color.Red);    

                if(game.IsHost)
                {
                    game.spriteBatch.DrawString(game.font, "Press S to start the game", new Vector2(50, 80), Color.Red);    
                }
                else
                {
                    game.spriteBatch.DrawString(game.font, "Waiting for the host to start the game", new Vector2(50, 80), Color.Red);    
                }
            }         
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.ShutDown();
            }


            if(!game.IsConnected)
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

                if(game.IsHost)
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