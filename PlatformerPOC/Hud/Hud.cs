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
            const int leftTextStart = 1020;
            const int vTextSpace = 18;

            // Msg (center)
            _game.SpriteBatch.Draw(_game.ResourcesHelper.HudMsg, new Vector2(280, 0), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, LayerDepths.OVERLAY);
            _game.SpriteBatch.DrawString(_game.DefaultFont, "This is just a test message", new Vector2(300, 20), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, -999);


            _game.SpriteBatch.Draw(_game.ResourcesHelper.HudText, new Vector2(1000, 0), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, LayerDepths.OVERLAY);

            const int topSTart = 60;
            var str = "Round: " + _game.RoundCounter;            
            _game.SpriteBatch.DrawString(_game.DefaultFont, str, new Vector2(leftTextStart, topSTart), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, -999);

            _game.SpriteBatch.DrawString(_game.DefaultFont, "10:00", new Vector2(leftTextStart, topSTart + vTextSpace), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, -999);
            _game.SpriteBatch.DrawString(_game.DefaultFont, "Score limit: 10", new Vector2(leftTextStart, topSTart + vTextSpace + vTextSpace), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, -999);
            _game.SpriteBatch.DrawString(_game.DefaultFont, "Map: Forestbridge 1", new Vector2(leftTextStart, topSTart + (vTextSpace * 3)), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, -999);
            _game.SpriteBatch.DrawString(_game.DefaultFont, "Gamemode: Elimination", new Vector2(leftTextStart, topSTart + (vTextSpace * 4)), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, -999);

            const int playersStart = 220;
            // Players section
            for (int index = 0; index < _game.PlayerManager.Players.Count; index++)
            {
                var player = _game.PlayerManager.Players[index];
                _game.SpriteBatch.DrawString(_game.DefaultFont, player.Name, new Vector2(leftTextStart, playersStart + (index * vTextSpace)), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, -999);
                _game.SpriteBatch.DrawString(_game.DefaultFont, player.Wins.ToString(), new Vector2(1170, playersStart + (index * vTextSpace)), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, -999);
                _game.SpriteBatch.DrawString(_game.DefaultFont, player.Deaths.ToString(), new Vector2(1200, playersStart + (index * vTextSpace)), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, -999);
            }

            // healthbar


            // weapon info
        }
    }
}