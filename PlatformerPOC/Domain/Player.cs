using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Domain
{
    public class Player
    {
        // Same for all players
        private static Texture2D spriteSheetTexture;
        private static SpriteSheet spriteSheet;

        private int animationFrame = 0;
        private Vector2 position = new Vector2(100, 100);

        public static void LoadContent(ContentManager content)
        {
            spriteSheetTexture = content.Load<Texture2D>("player");
            spriteSheet = new SpriteSheet(spriteSheetTexture);

            // Lame way of adding animation frames
            for (int i = 0; i < 8; i++)
            {
                spriteSheet.AddSourceSprite(i, new Rectangle(i * 32, 0, 32, 32));
            }
        }

        public void Update()
        {
            animationFrame++;
            if (animationFrame == 7)
            {
                animationFrame = 0;
            }
        }

        public void MoveLeft()
        {
            position.X -= 5;
        }

        public void MoveRight()
        {
            position.X += 5;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheetTexture, position, spriteSheet[animationFrame], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}