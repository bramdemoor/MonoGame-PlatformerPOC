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
            var str = "Round: " + _game.RoundCounter;

            _game.SpriteBatch.DrawString(_game.DefaultFont, str, new Vector2(440, 30), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, -999);


            for (int index = 0; index < _game.PlayerManager.Players.Count; index++)
            {
                var player = _game.PlayerManager.Players[index];
                _game.SpriteBatch.DrawString(_game.DefaultFont, player.Name, new Vector2(440, 60 + (index * 30)), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, -999);
            }

            // healthbar


            // weapon info
        }
    }
}