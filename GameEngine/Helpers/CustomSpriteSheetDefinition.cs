using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Helpers
{
    public class CustomSpriteSheetDefinition
    {
        public int SpriteCount { get; private set; }

        public Texture2D SpriteSheetTexture { get; private set; }

        private readonly List<Rectangle> sprites = new List<Rectangle>();

        public IEnumerable<Rectangle> Sprites { get { return sprites;  } }
        
        // TODO BDM: Implement support for multi-row spritesheets.

        public CustomSpriteSheetDefinition(ContentManager content, string spriteTextureName, Rectangle spriteDimensions, int spriteCount)
        {
            SpriteCount = spriteCount;
            SpriteSheetTexture = content.Load<Texture2D>(spriteTextureName);
            
            for (int i = 0; i < spriteCount; i++)
            {
                sprites.Add(new Rectangle(i * spriteDimensions.Width, 0, spriteDimensions.Width, spriteDimensions.Height));                
            }
        }

        
    }
}