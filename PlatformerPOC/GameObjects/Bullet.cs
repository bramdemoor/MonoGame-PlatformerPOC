using System.Linq;
using GameEngine;
using GameEngine.GameObjects;
using GameEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.GameObjects
{
    public class Bullet : BaseGameObject
    {
        private const int horizontalMaxSpeed = 15;

        private readonly Player _shooter;

        private readonly int horizontalDirection;

        private static Texture2D texture;

        public Rectangle RectangleCollisionBounds { get { return new Rectangle((int) Position.X, (int) Position.Y, texture.Bounds.Width, texture.Bounds.Height); } }

        public Bullet(Player shooter, Vector2 position, int horizontalDirection)
        {
            Position = position;
            _shooter = shooter;
            this.horizontalDirection = horizontalDirection;
        }

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("bullet");
        }

        public override void Draw()
        {
            var drawEffect = horizontalDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            SimpleGameEngine.Instance.spriteBatch.Draw(texture, Position, null, Color.White, 0, Vector2.Zero, 1, drawEffect, 1f);            
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
                    DestroyEntity();
                    return;
                }
            }



            Position = new Vector2(Position.X + (horizontalDirection * horizontalMaxSpeed), Position.Y);
        }        
    }
}