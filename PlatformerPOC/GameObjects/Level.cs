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
            SimpleGameEngine.Instance.spriteBatch.Draw(bgLayer1Texture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            SimpleGameEngine.Instance.spriteBatch.Draw(bgLayer2Texture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Hardcoded map tiles!

            for (int y = 0; y < 14; y++) DrawTile(new Vector2(0, y), 0, 1);        // Left border
            for (int y = 0; y < 14; y++) DrawTile(new Vector2(19, y), 0, 1);        // Right border
            for (int x = 0; x < 20; x++) DrawTile(new Vector2(x, 14), 0, 3);        // Floor

        }

        private static void DrawTile(Vector2 pos, int xtile, int ytile)
        {
            const int squareSize = 32;
            SimpleGameEngine.Instance.spriteBatch.Draw(tilesetTexture, pos * squareSize, new Rectangle(xtile * squareSize, ytile * squareSize, squareSize, squareSize), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}