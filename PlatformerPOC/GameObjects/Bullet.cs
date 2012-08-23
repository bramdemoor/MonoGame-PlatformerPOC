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
        private const int horizontalMaxSpeed = 10;

        public int HorizontalDirection { get; set; }

        private static Texture2D texture;

        public Bullet(Vector2 position, int horizontalDirection)
        {
            Position = position;
            HorizontalDirection = horizontalDirection;
        }

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("bullet");
        }

        public override void Draw()
        {
            SimpleGameEngine.Instance.spriteBatch.Draw(texture, Position, Color.White);            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Position = new Vector2(Position.X + (HorizontalDirection * horizontalMaxSpeed), Position.Y);
        }

   
    }
}