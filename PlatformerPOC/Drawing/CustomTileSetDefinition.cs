using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.Drawing
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

    public class TileDefinition
    {
        public CustomTileSetDefinition TileSet { get; private set; }

        public readonly Rectangle graphicsRectangle;

        public TileDefinition(CustomTileSetDefinition tileSet, int x, int y)
        {
            TileSet = tileSet;

            graphicsRectangle = tileSet.GetGraphicsRectangle(x, y);
        }
    }
}