using System.Linq;
using GameEngine;
using GameEngine.Helpers;
using Microsoft.Xna.Framework;

namespace PlatformerPOC.GameObjects
{
    public class Bullet : PlatformGameObject
    {
        private const int horizontalMaxSpeed = 15;

        private readonly Player _shooter;

        public Bullet(Player shooter, Vector2 position, int horizontalDirection)
        {
            Position = position;
            _shooter = shooter;
            this.HorizontalDirection = horizontalDirection;

            BoundingBox = new CustomBoundingBox();
            UpdateBoundingBox();
        }

        public override void Update(GameTime gameTime)
        {
            if(!PlatformGame.Instance.Level.IsPlaceFree(BoundingBox.FullRectangle))
            {
                DestroyEntity();
                return;                
            }

            if (CheckPlayerCollision()) return;

            Position = new Vector2(Position.X + (HorizontalDirection * horizontalMaxSpeed), Position.Y);

            UpdateBoundingBox();
        }

        private void UpdateBoundingBox()
        {
            BoundingBox.SetFullRectangle(Position, ResourcesHelper.BulletTexture.Bounds, Velocity);
        }

        private bool CheckPlayerCollision()
        {
            foreach (var player in PlatformGame.Instance.Players.Where(p => p != _shooter))
            {
                if (CollisionHelper.RectangleCollision(BoundingBox.FullRectangle, player.BoundingBox.FullRectangle))
                {
                    PlatformGame.Instance.AddObject(new Particle(new Vector2(Position.X + (HorizontalDirection*40), Position.Y), HorizontalDirection));

                    player.DoDamage(25);

                    DestroyEntity();

                    return true;
                }
            }
            return false;
        }

        public override void Draw()
        {
            if (ViewPort.IsObjectInArea(BoundingBox.FullRectangle))
            {
                SpriteBatch.Draw(ResourcesHelper.BulletTexture, ViewPort.GetRelativeCoords(Position), null, Color.White, 0, Vector2.Zero, 1, DrawEffect, 1f);            
            }            
        }

        public override void DrawDebug()
        {
            var rel = ViewPort.GetRelativeCoords(BoundingBox.FullRectangle);

            PlatformGame.Instance.DebugDrawHelper.DrawBorder(SpriteBatch, rel, 1, Color.Lime);
        }
    }
}