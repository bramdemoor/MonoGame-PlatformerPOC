using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace PlatformerPOC.Screens
{
    public class GameplayScreen
    {
        private readonly PlatformGame game;

        private readonly Hud.Hud hud;

        private bool frozen = false;

        public GameplayScreen(PlatformGame game)
        {
            this.game = game;
            hud = new Hud.Hud(game);
        }

        public void Draw(GameTime gameTime)
        {
            game.LevelManager.CurrentLevel.Draw();

            if (Config.DebugModeEnabled)
            {
                game.ViewPort.DrawDebug();                
            }
            
            foreach (var gameObject in game.GameObjects)
            {
                if(gameObject.InView)
                {
                    gameObject.Draw();

                    if (Config.DebugModeEnabled)
                    {
                        gameObject.DrawDebug();
                    }

                }                
            }

            hud.Draw();            
        }
        
        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.ShutDown();                
            }

            if(!frozen)
            {
                game.PlayerManager.HandleGameInput();

                foreach (var gameObject in game.GameObjects)
                {
                    gameObject.Update(gameTime);
                }

                game.GeneralUpdate();

                CheckGameState();

                game.DoHouseKeeping();                
            }

            if(Config.EditMode)
            {
                game.LevelEditor.Update();
            }
        }

        private void CheckGameState()
        {
            switch (game.PlayerManager.AlivePlayers.Count())
            {
                case 0:
                    game.StartNextRound();
                    break;
                case 1:
                    // Only 1 player? Don't do the checks.
                    if (game.PlayerManager.Players.Count() == 1) return;

                    var winner = game.PlayerManager.AlivePlayers.Single();
                    winner.Score.MarkWin();
                    game.StartNextRound();

                    break;
                default:
                    // continue game
                    break;
            }
        }
    }
}