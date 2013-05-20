using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.Drawing
{
    public class TileDefinition
    {
        private readonly PlatformGame game;

        public CustomTileSetDefinition TileSet { get; private set; }

        public readonly Rectangle graphicsRectangle;

        public TileDefinition(PlatformGame game, CustomTileSetDefinition tileSet, int x, int y)
        {
            this.game = game;

            TileSet = tileSet;

            graphicsRectangle = tileSet.GetGraphicsRectangle(x, y);
        }
    }
}