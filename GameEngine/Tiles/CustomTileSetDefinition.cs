using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Tiles
{
    public class CustomTileSetDefinition
    {
        public Rectangle TileSize { get; private set; }
        public Texture2D TilesetTexture { get; private set; }

        public CustomTileSetDefinition(ContentManager content, string resourceName, Rectangle tileSize)
        {
            TileSize = tileSize;
            TilesetTexture = content.Load<Texture2D>(resourceName);
        }

        public Rectangle GetGraphicsRectangle(int x, int y)
        {
            return new Rectangle(x * TileSize.Width, y * TileSize.Height, TileSize.Width, TileSize.Height);
        }
    }
}