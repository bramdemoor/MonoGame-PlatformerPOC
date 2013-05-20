using System.Linq;
using Microsoft.Xna.Framework;
using PlatformerPOC.Helpers;
using PlatformerPOC.Messages;

namespace PlatformerPOC.Domain.Weapon
{
    public class Bullet : BaseGameObject
    {
        private const int horizontalMaxSpeed = 15;

        private string shooterName;

        public Bullet(PlatformGame game, string shooterName, Vector2 position, int horizontalDirection) : base(game)
        {
            Position = position;
            this.shooterName = shooterName;
            HorizontalDirection = horizontalDirection;

            BoundingBox = new CustomBoundingBox();
            UpdateBoundingBox();
        }

        public void Update(GameTime gameTime)
        {
            if (!game.LevelManager.CurrentLevel.IsPlaceFreeOfWalls(BoundingBox.FullRectangle))
            {
                DestroyEntity();
                return;                
            }

            if (CheckPlayerCollision()) return;

            MoveHorizontal();

            UpdateBoundingBox();
        }

        private void MoveHorizontal()
        {
            Position = new Vector2(Position.X + (HorizontalDirection*horizontalMaxSpeed), Position.Y);
        }

        private void UpdateBoundingBox()
        {
            BoundingBox.SetFullRectangle(Position, game.ResourcePreloader.BulletTexture.Bounds, Velocity);
        }

        private bool CheckPlayerCollision()
        {
            foreach (var player in game.AlivePlayers.Where(p => p.Name != shooterName))
            {
                if (CollisionHelper.RectangleCollision(BoundingBox.FullRectangle, player.BoundingBox.FullRectangle))
                {
                    PlatformGame.eventAggregationManager.SendMessage(new PlayerHitMessage());

                    player.DoDamage(25);

                    DestroyEntity();

                    return true;
                }
            }
            return false;
        }
    }
}