using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.GameObjects
{
    public class Level
    {
        private static Texture2D bgLayer1Texture;
        private static Texture2D bgLayer2Texture;
        private static Texture2D tilesetTexture;

        public static void LoadContent(ContentManager content)
        {
            bgLayer1Texture = content.Load<Texture2D>("parallax-layer1");
            bgLayer2Texture = content.Load<Texture2D>("parallax-layer2");
            tilesetTexture = content.Load<Texture2D>("tileset");
        }

        public void Draw()
        {
            var spriteBatch = SimpleGameEngine.Instance.spriteBatch;

            spriteBatch.Draw(bgLayer1Texture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(bgLayer2Texture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Draw 1 tile
            spriteBatch.Draw(tilesetTexture, new Vector2(200, 200), new Rectangle(0, 0, 32, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}