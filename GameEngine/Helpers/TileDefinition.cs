using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.GameObjects
{
    public class TileDefinition
    {
        public CustomTileSetDefinition TileSet { get; private set; }

        private readonly Rectangle graphicsRectangle;

        public TileDefinition(CustomTileSetDefinition tileSet, int x, int y)
        {
            this.TileSet = tileSet;

            graphicsRectangle = tileSet.GetGraphicsRectangle(x, y);
        }


        public void DrawTile(Vector2 pos, float depth)
        {
            SimpleGameEngine.Instance.spriteBatch.Draw(TileSet.TilesetTexture, pos, graphicsRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth);
        }
    }
}