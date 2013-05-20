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

        public void Update(GameTime gameTime)
        {            
            UpdateBoundingBox();

            foreach (var player in game.AlivePlayers)
            {
                if (CollisionHelper.RectangleCollision(BoundingBox.FullRectangle, player.BoundingBox.FullRectangle))
                {
                    DestroyEntity();
                }
            }
        }

        private void UpdateBoundingBox()
        {
            BoundingBox.SetFullRectangle(Position, new Rectangle(0,0,16,16), Velocity);
        }
    }
}