using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.Control;
using PlatformerPOC.Control.AI;
using PlatformerPOC.GameObjects;

namespace PlatformerPOC.Screens
{
    public class GameplayScreen : SimpleScreenBase
    {
        private PlatformGame game;

        public GameplayScreen(PlatformGame game)
        {
            this.game = game;
        }

        public override void Draw(GameTime gameTime)
        {
            game.Level.Draw();

            game.ViewPort.DrawDebug();

            foreach (var gameObject in game.GameObjects)
            {
                // TODO BDM: Debug mode switch!

                gameObject.DrawDebug();

                gameObject.Draw();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.SwitchScreen(new LobbyScreen(game));                
            }

            game.LocalPlayer.HandleInput(new PlayerKeyboardState(Keyboard.GetState()));
            game.LocalPlayer.Update();
            game.DummyPlayer.HandleInput(new DummyAIController());
            game.DummyPlayer.Update();

            foreach (var gameObject in game.GameObjects)
            {
                gameObject.Update(gameTime);
            }

            game.GeneralUpdate();

            game.DoHouseKeeping();
        }
    }
}