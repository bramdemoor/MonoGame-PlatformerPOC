using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.Hud
{
    public class Hud
    {
        private readonly PlatformGame _game;

        public Hud(PlatformGame game)
        {
            _game = game;
        }

        public void Draw()
        {
            var str = string.Format("Round: {0}", _game.RoundCounter);
            string modeAndLevelText = string.Format("{0} in {1}", "Elimination", _game.LevelManager.CurrentLevel.Name);
            string limitsText = "Score limit: 10 | Time limit: 10:00";

            const int leftTextStart = 1020;
            const int vTextSpace = 18;

            // Msg (center)
            _game.SpriteBatch.Draw(_game.ResourcesHelper.HudMsg, new Vector2(280, 0), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, LayerDepths.OVERLAY);
            _game.SpriteBatch.DrawString(_game.DefaultFont, "This is just a test message", new Vector2(300, 20), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, LayerDepths.OVERLAY);


            _game.SpriteBatch.Draw(_game.ResourcesHelper.HudText, new Vector2(1000, 0), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, LayerDepths.OVERLAY);

            const int topSTart = 60;

            _game.SpriteBatch.DrawString(_game.DefaultFont, str, new Vector2(leftTextStart, topSTart), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, LayerDepths.OVERLAY);
            _game.SpriteBatch.DrawString(_game.DefaultFont, modeAndLevelText, new Vector2(leftTextStart, topSTart + vTextSpace), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, LayerDepths.OVERLAY);
            _game.SpriteBatch.DrawString(_game.DefaultFont, limitsText, new Vector2(leftTextStart, topSTart + vTextSpace + vTextSpace), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, LayerDepths.OVERLAY);            

            const int playersStart = 220;
            // Players section
            for (int index = 0; index < _game.PlayerManager.Players.Count; index++)
            {
                var player = _game.PlayerManager.Players[index];
                _game.SpriteBatch.DrawString(_game.DefaultFont, player.Name, new Vector2(leftTextStart, playersStart + (index * vTextSpace)), player.TextColor, 0, Vector2.Zero, 0.6f, SpriteEffects.None, LayerDepths.OVERLAY);
                _game.SpriteBatch.DrawString(_game.DefaultFont, player.Score.ToString(), new Vector2(1170, playersStart + (index * vTextSpace)), player.TextColor, 0, Vector2.Zero, 0.6f, SpriteEffects.None, LayerDepths.OVERLAY);                
            }

            // player info section
            _game.SpriteBatch.DrawString(_game.DefaultFont, "Player 1", new Vector2(leftTextStart, 550), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, LayerDepths.OVERLAY);
            _game.SpriteBatch.Draw(_game.ResourcesHelper.Pistol, new Vector2(1068, 662), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, LayerDepths.OVERLAY);
            //_game.SpriteBatch.Draw(_game.ResourcesHelper.PlayerAvatar, new Vector2(1058, 582), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, LayerDepths.OVERLAY);
        }
    }
}