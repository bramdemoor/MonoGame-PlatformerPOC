using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.Domain;

namespace PlatformerPOC.Screens
{
    public class GameplayScreen : SimpleScreen
    {
        private readonly PlatformGame game;

        public GameplayScreen(PlatformGame game)
        {
            this.game = game;
        }

        public override void Draw(GameTime gameTime)
        {
            game.level.Draw(game.spriteBatch);
            game.player.Draw(game.spriteBatch);

            //spriteBatch.DrawString(font, "This is a test", new Vector2(50, 40), Color.Red);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.ShowMenuScreen();
            }

            game.player.HandleInput(new PlayerKeyboardState(Keyboard.GetState()));

            game.player.Update();
        }
    }
}