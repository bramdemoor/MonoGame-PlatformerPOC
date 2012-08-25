using GameEngine;
using GameEngine.GameObjects;
using GameEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.GameObjects
{
    public class Particle : BaseGameObject 
    {
        private static Texture2D spriteSheetTexture;
        private static SpriteSheet spriteSheet;

        private int animationFrame = 0;

        private readonly int horizontalDirection = 1;

        public Particle(Vector2 position, int horizontalDirection)
        {
            Position = position;
            this.horizontalDirection = horizontalDirection;
        }

        public static void LoadContent(ContentManager content)
        {
            spriteSheetTexture = content.Load<Texture2D>("bullet-impact");
            spriteSheet = new SpriteSheet(spriteSheetTexture);

            // Lame way of adding animation frames
            for (int i = 0; i < 6; i++)
            {
                spriteSheet.AddSourceSprite(i, new Rectangle(i * 42, 0, 42, 29));
            }
        }

        public override void Update(GameTime gameTime)
        {
            animationFrame++;
            if (animationFrame == 5)
            {
                //animationFrame = 0;
                DestroyEntity();
            }
        }

        public override void Draw()
        {
            var drawEffect = horizontalDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            SimpleGameEngine.Instance.spriteBatch.Draw(spriteSheetTexture, Position, spriteSheet[animationFrame], Color.White, 0f, Vector2.Zero, 1f, drawEffect, 0f);
        }
    }
}