using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.Helpers
{
    public class CustomSpriteSheetDefinition
    {
        public int SpriteCount { get; private set; }

        public Texture2D SpriteSheetTexture { get; private set; }

        private readonly List<Rectangle> sprites = new List<Rectangle>();

        public IEnumerable<Rectangle> Sprites { get { return sprites;  } }

        public Rectangle SpriteDimensions { get; set; }

        public int Y { get; set; }
        
        public CustomSpriteSheetDefinition(ContentManager content, string spriteTextureName, Rectangle spriteDimensions, int spriteCount)
        {
            SpriteCount = spriteCount;
            SpriteSheetTexture = content.Load<Texture2D>(spriteTextureName);
            SpriteDimensions = spriteDimensions;
            
            for (int i = 0; i < spriteCount; i++)
            {
                sprites.Add(new Rectangle(i * spriteDimensions.Width, Y * spriteDimensions.Height, spriteDimensions.Width, spriteDimensions.Height));                
            }
        }
    }
}