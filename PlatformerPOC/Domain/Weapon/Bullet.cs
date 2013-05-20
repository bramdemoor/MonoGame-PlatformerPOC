using System.Linq;
using Microsoft.Xna.Framework;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Domain.Weapon
{
    public class Bullet : BaseGameObject
    {
        private const int horizontalMaxSpeed = 15;

        private readonly Player shooter;

        public Bullet(PlatformGame game, Player shooter, Vector2 position, int horizontalDirection) : base(game)
        {
            Position = position;
            this.shooter = shooter;
            HorizontalDirection = horizontalDirection;

            BoundingBox = new CustomBoundingBox();
            UpdateBoundingBox();
        }

        public override void Update(GameTime gameTime)
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
            foreach (var player in game.PlayerManager.AlivePlayers.Where(p => p != shooter))
            {
                if (CollisionHelper.RectangleCollision(BoundingBox.FullRectangle, player.BoundingBox.FullRectangle))
                {
                    SpawnBloodParticle();

                    player.DoDamage(25);

                    DestroyEntity();

                    return true;
                }
            }
            return false;
        }

        private void SpawnBloodParticle()
        {
            game.AddObject(new Particle(game, new Vector2(Position.X + (HorizontalDirection*40), Position.Y), HorizontalDirection));
        }

        public override void Draw()
        {
            SpriteBatch.Draw(game.ResourcePreloader.BulletTexture, PositionRelativeToView, null, Color.White, 0, Vector2.Zero, 1, DrawEffect, 1f);                    
        }

        public override void DrawDebug()
        {
            var rel = ViewPort.GetRelativeCoords(BoundingBox.FullRectangle);
            game.DebugDrawHelper.DrawBorder(rel, 1, Color.Lime);
        }
    }
}