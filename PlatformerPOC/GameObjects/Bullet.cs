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

        public Rectangle RectangleCollisionBounds { get { return new Rectangle((int)Position.X, (int)Position.Y, ResourcesHelper.BulletTexture.Bounds.Width, ResourcesHelper.BulletTexture.Bounds.Height); } }

        public Bullet(Player shooter, Vector2 position, int horizontalDirection)
        {
            Position = position;
            _shooter = shooter;
            this.HorizontalDirection = horizontalDirection;
        }

        public override void Draw()
        {
            SimpleGameEngine.Instance.spriteBatch.Draw(ResourcesHelper.BulletTexture, Position, null, Color.White, 0, Vector2.Zero, 1, DrawEffect, 1f);            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!PlatformGame.Instance.Level.IsInBounds(Position))
            {
                DestroyEntity();
                return;
            }

            foreach (var player in PlatformGame.Instance.Players.Where(p => p != _shooter))
            {
                if(CollisionHelper.RectangleCollision(RectangleCollisionBounds, player.RectangleCollisionBounds))
                {
                    PlatformGame.Instance.MarkGameObjectForAdd(new Particle(new Vector2(Position.X + (HorizontalDirection * 40), Position.Y), HorizontalDirection));
                    DestroyEntity();
                    return;
                }
            }

            Position = new Vector2(Position.X + (HorizontalDirection * horizontalMaxSpeed), Position.Y);
        }        
    }
}