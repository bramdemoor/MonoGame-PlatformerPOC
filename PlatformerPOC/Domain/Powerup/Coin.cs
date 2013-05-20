using Microsoft.Xna.Framework;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Domain.Powerup
{
    public class Coin : BaseGameObject
    {
        public Coin(PlatformGame game, Vector2 pos) : base(game)
        {
            Position = pos;

            BoundingBox = new CustomBoundingBox();
            UpdateBoundingBox();
        }

        public override void Draw()
        {
            game.SpriteBatch.Draw(game.ResourcePreloader.Coin, PositionRelativeToView, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateBoundingBox();

            foreach (var player in game.PlayerManager.AlivePlayers)
            {
                if (CollisionHelper.RectangleCollision(BoundingBox.FullRectangle, player.BoundingBox.FullRectangle))
                {
                    DestroyEntity();
                }
            }
        }

        private void UpdateBoundingBox()
        {
            BoundingBox.SetFullRectangle(Position, game.ResourcePreloader.Coin.Bounds, Velocity);
        }
    }
}