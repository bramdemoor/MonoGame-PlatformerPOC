using Microsoft.Xna.Framework;

namespace PlatformerPOC.Domain.Powerup
{
    public class Coin : BaseGameObject
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