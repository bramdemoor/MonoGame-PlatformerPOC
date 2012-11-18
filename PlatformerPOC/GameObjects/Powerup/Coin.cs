using Microsoft.Xna.Framework;

namespace PlatformerPOC.GameObjects.Powerup
{
    public class Coin : BasePowerup
    {
        public Coin(PlatformGame game, Vector2 pos) : base(game)
        {
            Position = pos;
        }

        public override void Draw()
        {
            game.SpriteBatch.Draw(game.ResourcesHelper.Coin, PositionRelativeToView, Color.White);
        }
    }
}