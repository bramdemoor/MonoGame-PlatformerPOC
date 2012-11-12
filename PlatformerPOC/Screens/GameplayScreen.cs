using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace PlatformerPOC.Screens
{
    public class GameplayScreen : SimpleScreenBase
    {
        private readonly PlatformGame game;

        public GameplayScreen(PlatformGame game)
        {
            this.game = game;
        }

        public override void Draw(GameTime gameTime)
        {
            game.Level.Draw();

            if (CoreConfig.DebugModeEnabled)
            {
                game.ViewPort.DrawDebug();                
            }
            
            foreach (var gameObject in game.GameObjects)
            {
                if(CoreConfig.DebugModeEnabled)
                {
                    gameObject.DrawDebug();
                }

                gameObject.Draw();
            }

            game.DebugDrawHelper.DrawDebugString("Round: " + game.RoundCounter);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.SwitchScreen(new LobbyScreen(game));                
            }

            game.PlayerManager.HandleGameInput();

            foreach (var gameObject in game.GameObjects)
            {
                gameObject.Update(gameTime);
            }

            game.GeneralUpdate();

            CheckGameState();

            game.DoHouseKeeping();
        }

        private void CheckGameState()
        {
            var alivePlayers = game.PlayerManager.Players.Count(p => p.IsAlive);

            switch (alivePlayers)
            {
                case 0:
                    game.StartNextRound();
                    break;
                case 1:
                    var winner = game.PlayerManager.Players.Single(p => p.IsAlive);

                    // TODO BDM: Do something with winner

                    game.StartNextRound();

                    break;
                default:
                    // continue game
                    break;
            }
        }
    }
}