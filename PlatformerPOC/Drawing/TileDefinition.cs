using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.Drawing
{
    public class TileDefinition
    {
        private readonly SimpleGame game;

        public CustomTileSetDefinition TileSet { get; private set; }

        private readonly Rectangle graphicsRectangle;

        public TileDefinition(SimpleGame game, CustomTileSetDefinition tileSet, int x, int y)
        {
            this.game = game;

            TileSet = tileSet;

            graphicsRectangle = tileSet.GetGraphicsRectangle(x, y);
        }

        public void DrawTile(Vector2 pos, float depth)
        {                        
            game.SpriteBatch.Draw(TileSet.TilesetTexture, pos, graphicsRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth);
        }
    }
}