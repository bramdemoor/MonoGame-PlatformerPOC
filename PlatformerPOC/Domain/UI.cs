using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.Domain
{
    /// <summary>
    /// Handles menu + UI during game
    /// </summary>
    public class UI
    {
        private static SpriteFont font;

        public static void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("spriteFont1");
        }

        public static SpriteFont GetFont()
        {
            return font;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "This is a test", new Vector2(50, 40), Color.Red);
        }
    }
}